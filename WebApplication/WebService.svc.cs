using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.ServiceModel.Activation;
using System.Configuration;
using MongoDB.Driver;
using MongoDB.Bson;
using WebService.Models;
using WebService.Controllers;

namespace WebApplication
{
    [ServiceContract]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public partial class WebService
    {
        [OperationContract]
        [WebGet]
        public string HelloWorld(string name)
        {
            return "Hello World! Your name is " + name + ".";
        }

        [OperationContract]
        [WebGet]
        public User GetUser(string id)
        {
            return UserController.Fetch(id);
        }

        [OperationContract]
        [WebGet]
        public List<User> GetUsers()
        {
            return UserController.FetchAll();
        }

        [OperationContract]
        [WebGet]
        public void CreateUser()
        {
            User data = new User();

            data.Username = "mbmccormick";
            data.Password = "password";
            data.Name = "Matt McCormick";
            data.EmailAddress = "mbmccormick@gmail.com";
            data.Biography = "I am awesome at life.";
            data.Location = "Purdue University";
            data.ProfilePictureID = 0;
            data.CreatedDate = DateTime.Now;

            UserController.Create(data);
        }

        [OperationContract]
        [WebGet]
        public void DeleteUser(string id)
        {
            User data = UserController.Fetch(id);
            UserController.Delete(data);
        }
    }
}
