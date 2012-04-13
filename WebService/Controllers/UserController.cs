using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebService.Models;
using MongoDB.Driver;
using WebService.Common;
using MongoDB.Driver.Builders;
using MongoDB.Bson;

namespace WebService.Controllers
{
    public static class UserController
    {
        public static User Fetch(string id)
        {
            if (id.Length == 0) return null;

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

        public static User Update(User data)
        {
            MongoServer server = MongoServer.Create(Global.DatabaseConnectionString);
            MongoDatabase database = server.GetDatabase(Global.DatabaseName);

            MongoCollection<User> users = database.GetCollection<User>("Users");

            users.Save(data);

            return data;
        }

        public static void Delete(UserInfo data)
        {
            //Set up the server/database connection.
            MongoServer server = MongoServer.Create(Global.DatabaseConnectionString);
            MongoDatabase database = server.GetDatabase(Global.DatabaseName);

            //Get all the collections that need to be updated.
            MongoCollection<User> users = database.GetCollection<User>("Users");
            MongoCollection<Picture> pics = database.GetCollection<Picture>("Pictures");
            MongoCollection<UserConnectedAccount> connAcct = database.GetCollection<UserConnectedAccount>("ConnectedAccounts");
            MongoCollection<Relationship> relate = database.GetCollection<Relationship>("Relationship");
            MongoCollection<FavoritedPicture> fave = database.GetCollection<FavoritedPicture>("FavoritedPictures");
            MongoCollection<FlaggedPicture> flag = database.GetCollection<FlaggedPicture>("FlaggedPictures");
            
            //Build the update for the Relationship collection.
            //I'm not sure if this is right.  If there are bugs when deleting, look here first.
            UpdateBuilder update = new UpdateBuilder();
            BsonValue[] val = { data.ID };
            update.PullAll("FollowingUserID", val);

            //Create the query to delete the user from the system.
            var query = new QueryDocument("UserID", data.ID);

            //Remove the user's documents from the collections.
            pics.Remove(query);
            connAcct.Remove(query);
            relate.Remove(query);
            fave.Remove(query);
            flag.Remove(query);

            //Remove the user from the following lists.
            relate.Update(query, update, UpdateFlags.Multi);

            //Change the query in order to delete the document from the Users collection.
            query = new QueryDocument("_id", data.ID);

            //Finally delete the user from the Users collection.
            users.Remove(query);
        }
    }
}