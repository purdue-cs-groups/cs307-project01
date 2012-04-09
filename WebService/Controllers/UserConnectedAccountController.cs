using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebService.Models;
using MongoDB.Driver;
using WebService.Common;
using MongoDB.Bson;

namespace WebService.Controllers
{
    public class UserConnectedAccountController
    {
        public static UserConnectedAccount Fetch(string id)
        {
            MongoServer server = MongoServer.Create(Global.DatabaseConnectionString);
            MongoDatabase database = server.GetDatabase(Global.DatabaseName);

            MongoCollection<UserConnectedAccount> userConnectedAccounts = database.GetCollection<UserConnectedAccount>("UserConnectedAccounts");
            var query = new QueryDocument("_id", id);

            return userConnectedAccounts.FindOne(query);
        }

        public static List<UserConnectedAccount> FetchByUserID(string userId)
        {
            MongoServer server = MongoServer.Create(Global.DatabaseConnectionString);
            MongoDatabase database = server.GetDatabase(Global.DatabaseName);

            MongoCollection<UserConnectedAccount> userConnectedAccounts = database.GetCollection<UserConnectedAccount>("UserConnectedAccounts");
            var query = new QueryDocument("UserID", userId);

            return userConnectedAccounts.Find(query).ToList<UserConnectedAccount>();
        }

        public static UserConnectedAccount Create(UserConnectedAccount data)
        {
            MongoServer server = MongoServer.Create(Global.DatabaseConnectionString);
            MongoDatabase database = server.GetDatabase(Global.DatabaseName);

            MongoCollection<UserConnectedAccount> userConnectedAccounts = database.GetCollection<UserConnectedAccount>("UserConnectedAccounts");

            data.CreatedDate = Utilities.ConvertToUnixTime(DateTime.UtcNow);

            userConnectedAccounts.Insert(data);

            return data;
        }

        public static UserConnectedAccount Update(UserConnectedAccount data)
        {
            MongoServer server = MongoServer.Create(Global.DatabaseConnectionString);
            MongoDatabase database = server.GetDatabase(Global.DatabaseName);

            MongoCollection<UserConnectedAccount> userConnectedAccounts = database.GetCollection<UserConnectedAccount>("UserConnectedAccounts");

            userConnectedAccounts.Save(data);

            return data;
        }

        public static void Delete(UserConnectedAccount data)
        {
            MongoServer server = MongoServer.Create(Global.DatabaseConnectionString);
            MongoDatabase database = server.GetDatabase(Global.DatabaseName);

            MongoCollection<UserConnectedAccount> userConnectedAccounts = database.GetCollection<UserConnectedAccount>("UserConnectedAccounts");
            var query = new QueryDocument("_id", data.ID);

            userConnectedAccounts.FindAndRemove(query, new SortByDocument());
        }
    }
}