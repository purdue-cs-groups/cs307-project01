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
            AuthenticationManager.ValidateToken(OperationContext.Current);

            return UserController.Fetch(id);
        }

        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "/users/fetch")]
        public List<User> FetchAllUsers()
        {
            AuthenticationManager.ValidateToken(OperationContext.Current);

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

            UserController.Create(jsonData);
        }

        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json, UriTemplate = "/users/update")]
        public void UpdateUser(Stream data)
        {
            AuthenticationManager.ValidateToken(OperationContext.Current);

            StreamReader reader = new StreamReader(data);
            string stringData = reader.ReadToEnd();

            reader.Close();
            reader.Dispose();

            var jsonData = Json.Decode<User>(stringData);

            UserController.Update(jsonData);
        }

        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json, UriTemplate = "/users/delete?id={id}")]
        public void DeleteUser(string id)
        {
            AuthenticationManager.ValidateToken(OperationContext.Current);

            User data = UserController.Fetch(id);
            UserController.Delete(data);
        }

        #endregion
    }
}