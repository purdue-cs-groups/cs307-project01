using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebService.Models;
using MongoDB.Driver;
using WebService.Common;

namespace WebService.Controllers
{
    public static class PictureController
    {
        public static Picture Fetch(string id)
        {
            MongoServer server = MongoServer.Create(Global.DatabaseConnectionString);
            MongoDatabase database = server.GetDatabase(Global.DatabaseName);

            MongoCollection<Picture> pictures = database.GetCollection<Picture>("Pictures");
            var query = new QueryDocument("_id", id);

            return pictures.FindOne(query);
        }

        public static List<Picture> FetchAll()
        {
            MongoServer server = MongoServer.Create(Global.DatabaseConnectionString);
            MongoDatabase database = server.GetDatabase(Global.DatabaseName);

            MongoCollection<Picture> pictures = database.GetCollection<Picture>("Pictures");

            return pictures.FindAll().ToList<Picture>();
        }

        public static void Create(Picture data)
        {
            MongoServer server = MongoServer.Create(Global.DatabaseConnectionString);
            MongoDatabase database = server.GetDatabase(Global.DatabaseName);

            MongoCollection<Picture> pictures = database.GetCollection<Picture>("Pictures");

            data.CreatedDate = Utilities.ConvertToUnixTime(DateTime.UtcNow);

            pictures.Insert(data);
        }

        public static void Update(Picture data)
        {
            MongoServer server = MongoServer.Create(Global.DatabaseConnectionString);
            MongoDatabase database = server.GetDatabase(Global.DatabaseName);

            MongoCollection<Picture> pictures = database.GetCollection<Picture>("Pictures");

            pictures.Save(data);
        }

        public static void Delete(Picture data)
        {
            MongoServer server = MongoServer.Create(Global.DatabaseConnectionString);
            MongoDatabase database = server.GetDatabase(Global.DatabaseName);

            MongoCollection<Picture> pictures = database.GetCollection<Picture>("Pictures");
            var query = new QueryDocument("_id", data.ID);

            pictures.FindAndRemove(query, new SortByDocument());
        }
    }
}