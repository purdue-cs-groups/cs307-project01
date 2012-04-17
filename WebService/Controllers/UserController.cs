using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebService.Models;
using MongoDB.Driver;
using WebService.Common;
using MongoDB.Driver.Builders;
using MongoDB.Bson;
using System.Text.RegularExpressions;

namespace WebService.Controllers
{
    public static class UserController
    {
        public static User Fetch(string id)
        {
            if (id == null) return null;

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

        public static UserStats FetchUserStats(string userId)
        {
            MongoServer server = MongoServer.Create(Global.DatabaseConnectionString);
            MongoDatabase database = server.GetDatabase(Global.DatabaseName);

            MongoCollection<Relationship> relationships = database.GetCollection<Relationship>("Relationships");
            MongoCollection<Picture> pictures = database.GetCollection<Picture>("Pictures");

            var query = Query.EQ("UserID", userId);
            var query2 = Query.EQ("FollowingUserID", userId);
            var query3 = Query.EQ("UserID", userId);

            UserStats stats = new UserStats();
            stats.Following = 0;
            stats.Followers = 0;
            stats.Pictures = 0;

            var following = relationships.Find(query);
            stats.Following = Convert.ToInt16(following.Count());

            var followers = relationships.Find(query2);
            stats.Followers = Convert.ToInt16(followers.Count());

            var pictureCount = pictures.Find(query3);
            stats.Pictures = Convert.ToInt16(pictureCount.Count());

            return stats;
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

            //Regex to find all results with the query string in it.
            var regexQueryUpper = Query.Matches("Name", BsonRegularExpression.Create(new Regex(query, RegexOptions.IgnoreCase)));

            foreach (User u in users.Find(regexQueryUpper).ToList<User>())
            {
                Picture p = PictureController.Fetch(u.ProfilePictureID);
                UserInfo i = new UserInfo(u, p);

                if (list.Contains(i) == false)
                    list.Add(i);
            }

            //Regex to find all results with the query string in it.
            regexQueryUpper = Query.Matches("Username", BsonRegularExpression.Create(new Regex(query, RegexOptions.IgnoreCase)));

            foreach (User u in users.Find(regexQueryUpper).ToList<User>())
            {
                Picture p = PictureController.Fetch(u.ProfilePictureID);
                UserInfo i = new UserInfo(u, p);

                if (list.Contains(i) == false)
                    list.Add(i);
            }

            //Regex to find all results with the query string in it.
            regexQueryUpper = Query.Matches("EmailAddress", BsonRegularExpression.Create(new Regex(query, RegexOptions.IgnoreCase)));

            foreach (User u in users.Find(regexQueryUpper).ToList<User>())
            {
                Picture p = PictureController.Fetch(u.ProfilePictureID);
                UserInfo i = new UserInfo(u, p);

                if (list.Contains(i) == false)
                    list.Add(i);
            }

            return list;
        }

        /*  Create(User data)
         *  Returns user object upon successful creation.
         *  Returns null upon unsucessful creation. 
         */
        public static User Create(User data)
        {
            MongoServer server = MongoServer.Create(Global.DatabaseConnectionString);
            MongoDatabase database = server.GetDatabase(Global.DatabaseName);

            MongoCollection<User> users = database.GetCollection<User>("Users");

            //Checks that all the required fields are not empty.
            if (!InputValidator.isNotEmpty(data.EmailAddress) || !InputValidator.isNotEmpty(data.Name) || !InputValidator.isNotEmpty(data.Username) || !InputValidator.isNotEmpty(data.Password))
            {
                throw new FormatException("Username, Email Address, and Name are all required fields!");
            }

            //Checks that all the required fiels are valid and of the correct length.
            if(!InputValidator.isValidEmail(data.EmailAddress) || !InputValidator.isValidUsername(data.Username) || !InputValidator.isStrongPassword(data.Password) || !InputValidator.isValidLength(data.Password, InputValidator.passwordLowerBoundary, InputValidator.passwordUpperBoundary) || !InputValidator.isValidLength(data.Username, InputValidator.usernameLowerBoundary, InputValidator.usernameUpperBoundary))
            {
                throw new FormatException("Username, Email Address, and Name must be valid fields!");
            }

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