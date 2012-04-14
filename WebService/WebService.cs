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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ServiceModel.Channels;
using System.Collections.Specialized;

namespace WebService
{
    [ServiceContract]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class WebService
    {
        #region Authentication Methods

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json, UriTemplate = "/authenticate")]
        public Token Authenticate(Stream data)
        {
            string key = AuthorizationManager.ParseAPIKey(OperationContext.Current);

            StreamReader reader = new StreamReader(data);
            string stringData = reader.ReadToEnd();

            reader.Close();
            reader.Dispose();

            var jsonData = Json.Decode<UserCredentials>(stringData);

            if (AuthenticationManager.IsValidUserCredentials(jsonData))
            {
                AuthenticationToken token = AuthenticationManager.GenerateToken(key, jsonData.Username);

                Token jsonToken = new Token();
                jsonToken.UniqueIdentifier = token.UniqueIdentifier;
                jsonToken.User = UserController.FetchInfo(token.Identity.ID);

                return jsonToken;
            }
            else
            {
                ErrorResponseHandler.GenerateErrorResponse(OperationContext.Current, ErrorType.InvalidUserCredentials);
                return null;
            }
        }

        #endregion

        #region Favorited Picture Methods

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "/favorites/fetch?id={id}")]
        public FavoritedPicture FetchFavoritedPicture(string id)
        {
            return FavoritedPictureController.Fetch(id);
        }

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json, UriTemplate = "/favorites/create")]
        public FavoritedPicture CreateFavoritedPicture(Stream data)
        {
            var token = AuthenticationManager.ValidateToken(OperationContext.Current);

            StreamReader reader = new StreamReader(data);
            string stringData = reader.ReadToEnd();

            reader.Close();
            reader.Dispose();

            var jsonData = Json.Decode<FavoritedPicture>(stringData);

            // force server-side values
            jsonData.UserID = token.Identity.ID;
            jsonData.FriendlyCreatedDate = DateTime.UtcNow;

            return FavoritedPictureController.Create(jsonData);
        }

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json, UriTemplate = "/favorites/update")]
        public FavoritedPicture UpdateFavoritedPicture(Stream data)
        {
            var token = AuthenticationManager.ValidateToken(OperationContext.Current);

            StreamReader reader = new StreamReader(data);
            string stringData = reader.ReadToEnd();

            reader.Close();
            reader.Dispose();

            var jsonData = Json.Decode<FavoritedPicture>(stringData);

            // force server-side values
            jsonData.UserID = token.Identity.ID;

            return FavoritedPictureController.Update(jsonData);
        }

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json, UriTemplate = "/favorites/delete?id={id}")]
        public void DeleteFavoritedPicture(string id)
        {
            var token = AuthenticationManager.ValidateToken(OperationContext.Current);

            FavoritedPicture data = FavoritedPictureController.Fetch(id);

            if (data.UserID == token.Identity.ID)
            {
                FavoritedPictureController.Delete(data);
            }
            else
            {
                throw new UnauthorizedAccessException("You are not the owner of that FavoritedPicture.");
            }
        }

        #endregion

        #region Flagged Picture Methods

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "/flags/fetch?id={id}")]
        public FlaggedPicture FetchFlaggedPicture(string id)
        {
            return FlaggedPictureController.Fetch(id);
        }

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json, UriTemplate = "/flags/create")]
        public FlaggedPicture CreateFlaggedPicture(Stream data)
        {
            var token = AuthenticationManager.ValidateToken(OperationContext.Current);

            StreamReader reader = new StreamReader(data);
            string stringData = reader.ReadToEnd();

            reader.Close();
            reader.Dispose();

            var jsonData = Json.Decode<FlaggedPicture>(stringData);

            // force server-side values
            jsonData.UserID = token.Identity.ID;
            jsonData.FriendlyCreatedDate = DateTime.UtcNow;

            return FlaggedPictureController.Create(jsonData);
        }

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json, UriTemplate = "/flags/update")]
        public FlaggedPicture UpdateFlaggedPicture(Stream data)
        {
            var token = AuthenticationManager.ValidateToken(OperationContext.Current);

            StreamReader reader = new StreamReader(data);
            string stringData = reader.ReadToEnd();

            reader.Close();
            reader.Dispose();

            var jsonData = Json.Decode<FlaggedPicture>(stringData);

            // force server-side values
            jsonData.UserID = token.Identity.ID;

            return FlaggedPictureController.Update(jsonData);
        }

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json, UriTemplate = "/flags/delete?id={id}")]
        public void DeleteFlaggedPicture(string id)
        {
            var token = AuthenticationManager.ValidateToken(OperationContext.Current);

            FlaggedPicture data = FlaggedPictureController.Fetch(id);

            if (data.UserID == token.Identity.ID)
            {
                FlaggedPictureController.Delete(data);
            }
            else
            {
                throw new UnauthorizedAccessException("You are not the owner of that FlaggedPicture.");
            }
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

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json, UriTemplate = "/pictures/upload")]
        public PictureURL UploadPicture(Stream data)
        {
            var token = AuthenticationManager.ValidateToken(OperationContext.Current);

            byte[] image = Utilities.ReadToEnd(data);
            MemoryStream imageStream = new MemoryStream(image);

            StreamReader reader = new StreamReader(imageStream);
            string stringData = reader.ReadToEnd();

            // initialize blob storage
            this.InitializeBlogStorage();

            // parse API key
            string apiKey = AuthorizationManager.ParseAPIKey(OperationContext.Current);

            // generate key for this picture
            Guid id = Guid.NewGuid();

            CloudBlob blob;

            #region Upload Image to Blob Storage

            // locate blob reference for this key
            string largeFileName = String.Format("{0}/{1}_l.jpg", apiKey, id);
            blob = container.GetBlobReference(largeFileName);

            reader.BaseStream.Seek(0, SeekOrigin.Begin);
            blob.UploadFromStream(reader.BaseStream);

            string largeURL = blob.Uri.AbsoluteUri.ToLower().Replace("https://", "http://");

            // locate blob reference for this key
            Stream mediumImage = new MemoryStream();
            ResizeImage(500, 500, reader.BaseStream, mediumImage);

            string mediumFileName = String.Format("{0}/{1}_m.jpg", apiKey, id);
            blob = container.GetBlobReference(mediumFileName);

            mediumImage.Seek(0, SeekOrigin.Begin);
            blob.UploadFromStream(mediumImage);

            string mediumURL = blob.Uri.AbsoluteUri.ToLower().Replace("https://", "http://");

            // locate blob reference for this key
            Stream smallImage = new MemoryStream();
            ResizeImage(100, 100, reader.BaseStream, smallImage);

            string smallFileName = String.Format("{0}/{1}_s.jpg", apiKey, id);
            blob = container.GetBlobReference(smallFileName);

            smallImage.Seek(0, SeekOrigin.Begin);
            blob.UploadFromStream(smallImage);

            string smallURL = blob.Uri.AbsoluteUri.ToLower().Replace("https://", "http://");

            #endregion

            // return reference to blob
            reader.Close();
            reader.Dispose();

            return new PictureURL(largeURL, mediumURL, smallURL);
        }

        private void ResizeImage(int newWidth, int newHeight, Stream fromStream, Stream toStream)
        {
            var image = Image.FromStream(fromStream);
            var thumbnailBitmap = new Bitmap(newWidth, newHeight);

            var thumbnailGraph = Graphics.FromImage(thumbnailBitmap);
            thumbnailGraph.CompositingQuality = CompositingQuality.HighQuality;
            thumbnailGraph.SmoothingMode = SmoothingMode.HighQuality;
            thumbnailGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;

            var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);
            thumbnailGraph.DrawImage(image, imageRectangle);

            thumbnailBitmap.Save(toStream, image.RawFormat);

            thumbnailGraph.Dispose();
            thumbnailBitmap.Dispose();
            image.Dispose();
        }

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "/pictures/fetch?id={id}")]
        public PictureInfo FetchPicture(string id)
        {
            return PictureController.FetchInfo(id);
        }

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "/pictures/fetch")]
        public List<PictureInfo> FetchNewsFeed()
        {
            var token = AuthenticationManager.ValidateToken(OperationContext.Current);

            return PictureController.FetchNewsFeed(token.Identity);
        }

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "/pictures/popular/fetch")]
        public List<PictureInfo> FetchPopularNewsFeed()
        {
            return PictureController.FetchPopularNewsFeed();
        }

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "/pictures/user/fetch?userid={userId}")]
        public List<PictureInfo> FetchUserPictures(string userId)
        {
            return PictureController.FetchUserPictures(userId);
        }

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "/pictures/user/favorites/fetch?userid={userId}")]
        public List<PictureInfo> FetchUserFavoritedPictures(string userId)
        {
            return PictureController.FetchUserFavoritedPictures(userId);
        }

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json, UriTemplate = "/pictures/create")]
        public Picture CreatePicture(Stream data)
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

            return PictureController.Create(jsonData);
        }

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json, UriTemplate = "/pictures/update")]
        public Picture UpdatePicture(Stream data)
        {
            var token = AuthenticationManager.ValidateToken(OperationContext.Current);

            StreamReader reader = new StreamReader(data);
            string stringData = reader.ReadToEnd();

            reader.Close();
            reader.Dispose();

            var jsonData = Json.Decode<Picture>(stringData);

            // force server-side values
            jsonData.UserID = token.Identity.ID;

            return PictureController.Update(jsonData);
        }

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json, UriTemplate = "/pictures/delete?id={id}")]
        public void DeletePicture(string id)
        {
            var token = AuthenticationManager.ValidateToken(OperationContext.Current);

            PictureInfo data = PictureController.FetchInfo(id);

            if (data.User.ID == token.Identity.ID)
            {
                PictureController.Delete(data);
            }
            else
            {
                throw new UnauthorizedAccessException("You are not the owner of that Picture.");
            }
        }

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json, UriTemplate = "/pictures/flag?id={id}")]
        public void FlagPicture(string id)
        {
            var token = AuthenticationManager.ValidateToken(OperationContext.Current);

            PictureInfo data = PictureController.FetchInfo(id);

            // TODO: we should log this action
            PictureController.Delete(data);
        }

        #endregion

        #region Relationship Methods

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "/relationships/fetch?id={id}")]
        public Relationship FetchRelationship(string id)
        {
            return RelationshipController.Fetch(id);
        }

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "/relationships/user/fetch?userid={userId}&followingid={followingId}")]
        public Relationship FetchRelationshipByUserID(string userId, string followingId)
        {
            return RelationshipController.FetchUserRelationshipByIDs(userId, followingId); 
        }

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json, UriTemplate = "/relationships/create")]
        public Relationship CreateRelationship(Stream data)
        {
            var token = AuthenticationManager.ValidateToken(OperationContext.Current);

            StreamReader reader = new StreamReader(data);
            string stringData = reader.ReadToEnd();

            reader.Close();
            reader.Dispose();

            var jsonData = Json.Decode<Relationship>(stringData);

            // force server-side values
            jsonData.UserID = token.Identity.ID;
            jsonData.FriendlyCreatedDate = DateTime.UtcNow;

            return RelationshipController.Create(jsonData);
        }

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json, UriTemplate = "/relationships/update")]
        public Relationship UpdateRelationship(Stream data)
        {
            var token = AuthenticationManager.ValidateToken(OperationContext.Current);

            StreamReader reader = new StreamReader(data);
            string stringData = reader.ReadToEnd();

            reader.Close();
            reader.Dispose();

            var jsonData = Json.Decode<Relationship>(stringData);

            // force server-side values
            jsonData.UserID = token.Identity.ID;

            return RelationshipController.Update(jsonData);
        }

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json, UriTemplate = "/relationships/delete?id={id}")]
        public void DeleteRelationship(string id)
        {
            var token = AuthenticationManager.ValidateToken(OperationContext.Current);

            Relationship data = RelationshipController.Fetch(id);

            if (data.UserID == token.Identity.ID)
            {
                RelationshipController.Delete(data);
            }
            else
            {
                throw new UnauthorizedAccessException("You are not the owner of that Relationship.");
            }
        }

        #endregion

        #region User Methods

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "/users/fetch?id={id}")]
        public UserInfo FetchUser(string id)
        {
            var token = AuthenticationManager.ValidateToken(OperationContext.Current);

            return UserController.FetchInfo(id);
        }

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "/users/fetch")]
        public List<UserInfo> FetchAllUsers()
        {
            var token = AuthenticationManager.ValidateToken(OperationContext.Current);

            return UserController.FetchAll();
        }

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "/users/search?query={query}")]
        public List<UserInfo> SearchUsers(string query)
        {
            var token = AuthenticationManager.ValidateToken(OperationContext.Current);

            return UserController.FetchAll(query);
        }

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json, UriTemplate = "/users/create")]
        public User CreateUser(Stream data)
        {
            StreamReader reader = new StreamReader(data);
            string stringData = reader.ReadToEnd();

            reader.Close();
            reader.Dispose();

            var jsonData = Json.Decode<User>(stringData);
            
            if (UserController.FetchByUsername(jsonData.Username) != null ||
                UserController.FetchByEmailAddress(jsonData.EmailAddress) != null)
            {
                throw new Exception("A user with this Username or Email Address already exists!");
            }

            // force server-side values
            jsonData.FriendlyCreatedDate = DateTime.UtcNow;           

            return UserController.Create(jsonData);
        }

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json, UriTemplate = "/users/update")]
        public User UpdateUser(Stream data)
        {
            var token = AuthenticationManager.ValidateToken(OperationContext.Current);

            StreamReader reader = new StreamReader(data);
            string stringData = reader.ReadToEnd();

            reader.Close();
            reader.Dispose();

            var jsonData = Json.Decode<User>(stringData);

            return UserController.Update(jsonData);
        }

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json, UriTemplate = "/users/delete")]
        public void DeleteUser()
        {
            var token = AuthenticationManager.ValidateToken(OperationContext.Current);

            UserInfo data = token.Identity;
            UserController.Delete(data);

            // TODO: delete all user pictures
        }

        #endregion

        #region User Connected Account Methods

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "/users/connections/fetch?id={id}")]
        public UserConnectedAccount FetchUserConnectedAccount(string id)
        {
            return UserConnectedAccountController.Fetch(id);
        }

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "/users/connections/fetchByUserID?userid={userId}")]
        public List<UserConnectedAccount> FetchUserConnectedAccountsByUserID(string userId)
        {
            return UserConnectedAccountController.FetchByUserID(userId);
        }

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json, UriTemplate = "/users/connections/create")]
        public UserConnectedAccount CreateUserConnectedAccount(Stream data)
        {
            var token = AuthenticationManager.ValidateToken(OperationContext.Current);

            StreamReader reader = new StreamReader(data);
            string stringData = reader.ReadToEnd();

            reader.Close();
            reader.Dispose();

            var jsonData = Json.Decode<UserConnectedAccount>(stringData);

            // force server-side values
            jsonData.UserID = token.Identity.ID;
            jsonData.FriendlyCreatedDate = DateTime.UtcNow;

            return UserConnectedAccountController.Create(jsonData);
        }

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json, UriTemplate = "/users/connections/update")]
        public UserConnectedAccount UpdateUserConnectedAccount(Stream data)
        {
            var token = AuthenticationManager.ValidateToken(OperationContext.Current);

            StreamReader reader = new StreamReader(data);
            string stringData = reader.ReadToEnd();

            reader.Close();
            reader.Dispose();

            var jsonData = Json.Decode<UserConnectedAccount>(stringData);

            // force server-side values
            jsonData.UserID = token.Identity.ID;

            return UserConnectedAccountController.Update(jsonData);
        }

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json, UriTemplate = "/users/connections/delete?id={id}")]
        public void DeleteUserConnectedAccount(string id)
        {
            var token = AuthenticationManager.ValidateToken(OperationContext.Current);

            UserConnectedAccount data = UserConnectedAccountController.Fetch(id);

            if (data.UserID == token.Identity.ID)
            {
                UserConnectedAccountController.Delete(data);
            }
            else
            {
                throw new UnauthorizedAccessException("You are not the owner of that UserConnectedAccount.");
            }
        }

        #endregion
    }
}