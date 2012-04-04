using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebService.Models;
using MongoDB.Driver;
using WebService.Common;

namespace WebService.Controllers
{
    public static class UserController
    {
        public static User Fetch(string id)
        {
            MongoServer server = MongoServer.Create(Global.DatabaseConnectionString);
            MongoDatabase database = server.GetDatabase(Global.DatabaseName);

            MongoCollection<User> users = database.GetCollection<User>("Users");
            var query = new QueryDocument("_id", id);

            return users.FindOne(query);
        }

        public static UserInfo FetchInfo(string id)
        {
            MongoServer server = MongoServer.Create(Global.DatabaseConnectionString);
            MongoDatabase database = server.GetDatabase(Global.DatabaseName);

            MongoCollection<User> users = database.GetCollection<User>("Users");
            var query = new QueryDocument("_id", id);

            User u = users.FindOne(query);

            Picture p = PictureController.Fetch(u.ProfilePictureID);

            return new UserInfo(u, p);
        }

        public static User FetchByUsername(string username)
        {
            MongoServer server = MongoServer.Create(Global.DatabaseConnectionString);
            MongoDatabase database = server.GetDatabase(Global.DatabaseName);

            MongoCollection<User> users = database.GetCollection<User>("Users");
            var query = new QueryDocument("Username", username);

            return users.FindOne(query);
        }

        public static User FetchByEmailAddress(string emailAddress)
        {
            MongoServer server = MongoServer.Create(Global.DatabaseConnectionString);
            MongoDatabase database = server.GetDatabase(Global.DatabaseName);

            MongoCollection<User> users = database.GetCollection<User>("Users");
            var query = new QueryDocument("EmailAddress", emailAddress);

            return users.FindOne(query);
        }

        public static List<User> FetchAll()
        {
            MongoServer server = MongoServer.Create(Global.DatabaseConnectionString);
            MongoDatabase database = server.GetDatabase(Global.DatabaseName);

            MongoCollection<User> users = database.GetCollection<User>("Users");

            return users.FindAll().ToList<User>();
        }
        
        public static void Create(User data)
        {
            MongoServer server = MongoServer.Create(Global.DatabaseConnectionString);
            MongoDatabase database = server.GetDatabase(Global.DatabaseName);

            MongoCollection<User> users = database.GetCollection<User>("Users");

            data.CreatedDate = Utilities.ConvertToUnixTime(DateTime.UtcNow);

            users.Insert(data);
        }

        public static void Update(User data)
        {
            MongoServer server = MongoServer.Create(Global.DatabaseConnectionString);
            MongoDatabase database = server.GetDatabase(Global.DatabaseName);

            MongoCollection<User> users = database.GetCollection<User>("Users");

            users.Save(data);
        }

        public static void Delete(User data)
        {
            MongoServer server = MongoServer.Create(Global.DatabaseConnectionString);
            MongoDatabase database = server.GetDatabase(Global.DatabaseName);

            MongoCollection<User> users = database.GetCollection<User>("Users");
            var query = new QueryDocument("_id", data.ID);

            users.FindAndRemove(query, new SortByDocument());
        }
    }
}