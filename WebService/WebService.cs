using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using WebService.Models;
using WebService.Controllers;
using System.IO;
using System.Web.Helpers;
using WebService.Common;
using System.Configuration;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace WebService
{
    [ServiceContract]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class WebService
    {
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "/common/helloWorld?name={name}")]
        public string HelloWorld(string name)
        {
            return "Hello World! Your name is " + name + ".";
        }

        #region Authentication Methods

        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json, UriTemplate = "/authenticate")]
        public Token Authenticate(Stream data)
        {
            StreamReader reader = new StreamReader(data);
            string stringData = reader.ReadToEnd();

            reader.Close();
            reader.Dispose();

            var jsonData = Json.Decode<UserCredentials>(stringData);

            if (AuthenticationManager.IsValidUserCredentials(jsonData))
            {
                AuthenticationToken token = AuthenticationManager.GenerateToken(jsonData.Username);

                Token jsonToken = new Token();
                jsonToken.UniqueIdentifier = token.UniqueIdentifier;

                return jsonToken;
            }
            else
            {
                ErrorResponseHandler.GenerateErrorResponse(OperationContext.Current, ErrorType.InvalidUserCredentials);
                return null;
            }
        }

        #endregion

        #region User Methods

        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "/users/fetch?id={id}")]
        public User FetchUser(string id)
        {
            var token = AuthenticationManager.ValidateToken(OperationContext.Current);

            return UserController.Fetch(id);
        }

        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "/users/fetch")]
        public List<User> FetchAllUsers()
        {
            var token = AuthenticationManager.ValidateToken(OperationContext.Current);

            return UserController.FetchAll();
        }

        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json, UriTemplate = "/users/create")]
        public void CreateUser(Stream data)
        {
            StreamReader reader = new StreamReader(data);
            string stringData = reader.ReadToEnd();

            reader.Close();
            reader.Dispose();

            var jsonData = Json.Decode<User>(stringData);

            // force server-side values
            jsonData.FriendlyCreatedDate = DateTime.UtcNow;

            UserController.Create(jsonData);
        }

        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json, UriTemplate = "/users/update")]
        public void UpdateUser(Stream data)
        {
            var token = AuthenticationManager.ValidateToken(OperationContext.Current);

            StreamReader reader = new StreamReader(data);
            string stringData = reader.ReadToEnd();

            reader.Close();
            reader.Dispose();

            var jsonData = Json.Decode<User>(stringData);

            UserController.Update(jsonData);
        }

        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json, UriTemplate = "/users/delete")]
        public void DeleteUser()
        {
            var token = AuthenticationManager.ValidateToken(OperationContext.Current);

            User data = token.Identity;
            UserController.Delete(data);

            // TODO: delete all user pictures
        }

        #endregion

        #region Picture Methods

        private CloudBlobContainer container;

        private void InitializeBlogStorage()
        {
            // initialize storage account
            StorageCredentialsAccountAndKey storageCredentialsAccountAndKey = new StorageCredentialsAccountAndKey(ConfigurationManager.AppSettings["AzureStorageAccount"], ConfigurationManager.AppSettings["AzureStorageKey"]);
            CloudStorageAccount storageAccount = new CloudStorageAccount(storageCredentialsAccountAndKey, true);

            // initialize blob storage
            CloudBlobClient blobStorage = storageAccount.CreateCloudBlobClient();
            blobStorage.ParallelOperationThreadCount = 1;

            // locate container reference
            container = blobStorage.GetContainerReference("pictures");

            // wait for container to initialize
            bool storageInitialized = false;
            while (!storageInitialized)
            {
                try
                {
                    // check if container exists
                    container.CreateIfNotExist();

                    // set container permissions
                    var permissions = container.GetPermissions();
                    permissions.PublicAccess = BlobContainerPublicAccessType.Container;
                    container.SetPermissions(permissions);

                    storageInitialized = true;
                }
                catch (StorageClientException e)
                {
                    // storage account could not be initialized
                    throw e;
                }
            }
        }

        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json, UriTemplate = "/pictures/upload")]
        public PictureURL UploadPicture(Stream data)
        {
            var token = AuthenticationManager.ValidateToken(OperationContext.Current);

            StreamReader reader = new StreamReader(data);
            string stringData = reader.ReadToEnd();

            // initialize blob storage
            this.InitializeBlogStorage();

            // generate key for this picture
            Guid id = Guid.NewGuid();
            string fileName = String.Format("{0}.jpg", id);

            // locate blob reference for this key
            CloudBlob blob = container.GetBlobReference(fileName);

            // upload image data to blob storage
            reader.BaseStream.Seek(0, SeekOrigin.Begin);
            blob.UploadFromStream(reader.BaseStream);

            // close stream
            reader.Close();
            reader.Dispose();

            // return reference to blob
            return new PictureURL(blob.Uri.AbsoluteUri.ToLower().Replace("https://", "http://"));
        }

        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "/pictures/fetch?id={id}")]
        public Picture FetchPicture(string id)
        {
            return PictureController.Fetch(id);
        }

        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "/pictures/fetch")]
        public List<Picture> FetchAllPictures()
        {
            return PictureController.FetchAll();
        }

        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json, UriTemplate = "/pictures/create")]
        public void CreatePicture(Stream data)
        {
            var token = AuthenticationManager.ValidateToken(OperationContext.Current);

            StreamReader reader = new StreamReader(data);
            string stringData = reader.ReadToEnd();

            reader.Close();
            reader.Dispose();

            var jsonData = Json.Decode<Picture>(stringData);

            // force server-side values
            jsonData.UserID = token.Identity.ID;
            jsonData.FriendlyCreatedDate = DateTime.UtcNow;

            PictureController.Create(jsonData);
        }

        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json, UriTemplate = "/pictures/update")]
        public void UpdatePicture(Stream data)
        {
            var token = AuthenticationManager.ValidateToken(OperationContext.Current);

            StreamReader reader = new StreamReader(data);
            string stringData = reader.ReadToEnd();

            reader.Close();
            reader.Dispose();

            var jsonData = Json.Decode<Picture>(stringData);

            // force server-side values
            jsonData.UserID = token.Identity.ID;

            PictureController.Update(jsonData);
        }

        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json, UriTemplate = "/pictures/delete?id={id}")]
        public void DeletePicture(string id)
        {
            var token = AuthenticationManager.ValidateToken(OperationContext.Current);

            Picture data = PictureController.Fetch(id);
            PictureController.Delete(data);
        }

        #endregion
    }
}