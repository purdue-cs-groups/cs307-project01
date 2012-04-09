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

        public static UserInfo FetchInfoByUsername(string username)
        {
            MongoServer server = MongoServer.Create(Global.DatabaseConnectionString);
            MongoDatabase database = server.GetDatabase(Global.DatabaseName);

            MongoCollection<User> users = database.GetCollection<User>("Users");
            var query = new QueryDocument("Username", username);

            User u = users.FindOne(query);

            Picture p = PictureController.Fetch(u.ProfilePictureID);

            return new UserInfo(u, p);
        }

        public static UserInfo FetchInfoByEmailAddress(string emailAddress)
        {
            MongoServer server = MongoServer.Create(Global.DatabaseConnectionString);
            MongoDatabase database = server.GetDatabase(Global.DatabaseName);

            MongoCollection<User> users = database.GetCollection<User>("Users");
            var query = new QueryDocument("EmailAddress", emailAddress);

            User u = users.FindOne(query);

            Picture p = PictureController.Fetch(u.ProfilePictureID);

            return new UserInfo(u, p);
        }

        public static List<UserInfo> FetchAll()
        {
            MongoServer server = MongoServer.Create(Global.DatabaseConnectionString);
            MongoDatabase database = server.GetDatabase(Global.DatabaseName);

            MongoCollection<User> users = database.GetCollection<User>("Users");

            List<UserInfo> list = new List<UserInfo>();
            foreach (User u in users.FindAll().ToList<User>())
            {
                Picture p = PictureController.Fetch(u.ProfilePictureID);
                UserInfo i = new UserInfo(u, p);

                list.Add(i);
            }

            return list;
        }

        public static List<UserInfo> FetchAll(string query)
        {
            MongoServer server = MongoServer.Create(Global.DatabaseConnectionString);
            MongoDatabase database = server.GetDatabase(Global.DatabaseName);

            MongoCollection<User> users = database.GetCollection<User>("Users");

            List<UserInfo> list = new List<UserInfo>();

            foreach (User u in users.Find(new QueryDocument("Name", query)).ToList<User>())
            {
                Picture p = PictureController.Fetch(u.ProfilePictureID);
                UserInfo i = new UserInfo(u, p);

                if (list.Contains(i) == false)
                    list.Add(i);
            } 
            
            foreach (User u in users.Find(new QueryDocument("Username", query)).ToList<User>())
            {
                Picture p = PictureController.Fetch(u.ProfilePictureID);
                UserInfo i = new UserInfo(u, p);

                if (list.Contains(i) == false)
                    list.Add(i);
            }

            foreach (User u in users.Find(new QueryDocument("EmailAddress", query)).ToList<User>())
            {
                Picture p = PictureController.Fetch(u.ProfilePictureID);
                UserInfo i = new UserInfo(u, p);

                if (list.Contains(i) == false)
                    list.Add(i);
            }

            return list;
        }

        public static User Create(User data)
        {
            MongoServer server = MongoServer.Create(Global.DatabaseConnectionString);
            MongoDatabase database = server.GetDatabase(Global.DatabaseName);

            MongoCollection<User> users = database.GetCollection<User>("Users");

            data.CreatedDate = Utilities.ConvertToUnixTime(DateTime.UtcNow);

            users.Insert(data);

            return data;
        }

        public static void Update(User data)
        {
            MongoServer server = MongoServer.Create(Global.DatabaseConnectionString);
            MongoDatabase database = server.GetDatabase(Global.DatabaseName);

            MongoCollection<User> users = database.GetCollection<User>("Users");

            users.Save(data);
        }

        public static void Delete(UserInfo data)
        {
            MongoServer server = MongoServer.Create(Global.DatabaseConnectionString);
            MongoDatabase database = server.GetDatabase(Global.DatabaseName);

            MongoCollection<User> users = database.GetCollection<User>("Users");
            var query = new QueryDocument("_id", data.ID);

            users.FindAndRemove(query, new SortByDocument());
        }
    }
}