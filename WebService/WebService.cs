using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using WebService.Models;
using WebService.Controllers;

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

        #region User Methods

        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "/users/fetch?id={id}")]
        public User GetUser(string id)
        {
            return UserController.Fetch(id);
        }

        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "/users/fetch")]
        public List<User> GetUsers()
        {
            return UserController.FetchAll();
        }

        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "/users/delete?id={id}")]
        public void DeleteUser(string id)
        {
            User data = UserController.Fetch(id);
            UserController.Delete(data);
        }

        #endregion
    }
}