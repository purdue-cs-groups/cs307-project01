using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebService.Models;
using MongoDB.Driver;
using WebService.Common;
using MongoDB.Driver.Builders;

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

        public static List<Picture> FetchNewsFeed(User data)
        {
            MongoServer server = MongoServer.Create(Global.DatabaseConnectionString);
            MongoDatabase database = server.GetDatabase(Global.DatabaseName);

            MongoCollection<Picture> pictures = database.GetCollection<Picture>("Pictures");

            // TODO: query the pictures properly
            return pictures.FindAll().SetSortOrder(SortBy.Descending("CreatedDate")).ToList<Picture>();
        }

        public static List<Picture> FetchPopularNewsFeed()
        {
            MongoServer server = MongoServer.Create(Global.DatabaseConnectionString);
            MongoDatabase database = server.GetDatabase(Global.DatabaseName);

            MongoCollection<Picture> pictures = database.GetCollection<Picture>("Pictures");
            var query = Query.GT("CreatedDate", Utilities.ConvertToUnixTime(DateTime.UtcNow.AddDays(-7)));
            
            return pictures.Find(query).SetSortOrder(SortBy.Descending("ViewCount")).SetLimit(25).ToList<Picture>();
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