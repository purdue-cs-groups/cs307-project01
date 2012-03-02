using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebService.Models;
using MongoDB.Driver;

namespace WebService.Controllers
{
    public static class UserController
    {
        public static User Fetch(string id)
        {
            MongoServer server = MongoServer.Create(Global.DatabaseConnectionString);
            MongoDatabase database = server.GetDatabase(Global.DatabaseName);

            MongoCollection<User> users = database.GetCollection<User>("Users");
            var query = new QueryDocument("ID", id);

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
            var query = new QueryDocument("ID", data.ID);

            users.FindAndRemove(query, new SortByDocument());
        }
    }
}