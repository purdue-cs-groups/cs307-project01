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
    public static class FlaggedPictureController
    {
        public static FlaggedPicture Fetch(string id)
        {
            MongoServer server = MongoServer.Create(Global.DatabaseConnectionString);
            MongoDatabase database = server.GetDatabase(Global.DatabaseName);

            MongoCollection<FlaggedPicture> flaggedPictures = database.GetCollection<FlaggedPicture>("FlaggedPictures");
            var query = new QueryDocument("_id", id);

            return flaggedPictures.FindOne(query);
        }

        public static FlaggedPicture Create(FlaggedPicture data)
        {
            MongoServer server = MongoServer.Create(Global.DatabaseConnectionString);
            MongoDatabase database = server.GetDatabase(Global.DatabaseName);

            MongoCollection<FlaggedPicture> flaggedPictures = database.GetCollection<FlaggedPicture>("FlaggedPictures");

            data.CreatedDate = Utilities.ConvertToUnixTime(DateTime.UtcNow);

            flaggedPictures.Insert(data);

            return data;
        }

        public static FlaggedPicture Update(FlaggedPicture data)
        {
            MongoServer server = MongoServer.Create(Global.DatabaseConnectionString);
            MongoDatabase database = server.GetDatabase(Global.DatabaseName);

            MongoCollection<FlaggedPicture> flaggedPictures = database.GetCollection<FlaggedPicture>("FlaggedPictures");

            flaggedPictures.Save(data);

            return data;
        }

        public static void Delete(FlaggedPicture data)
        {
            MongoServer server = MongoServer.Create(Global.DatabaseConnectionString);
            MongoDatabase database = server.GetDatabase(Global.DatabaseName);

            MongoCollection<FlaggedPicture> flaggedPictures = database.GetCollection<FlaggedPicture>("FlaggedPictures");
            var query = new QueryDocument("_id", data.ID);

            flaggedPictures.FindAndRemove(query, new SortByDocument());
        }
    }
}