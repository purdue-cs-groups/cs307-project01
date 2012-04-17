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
    public static class FavoritedPictureController
    {
        public static FavoritedPicture Fetch(string id)
        {
            MongoServer server = MongoServer.Create(Global.DatabaseConnectionString);
            MongoDatabase database = server.GetDatabase(Global.DatabaseName);

            MongoCollection<FavoritedPicture> favoritedPictures = database.GetCollection<FavoritedPicture>("FavoritedPictures");
            var query = new QueryDocument("_id", id);

            return favoritedPictures.FindOne(query);
        }

        public static FavoritedPicture FetchByPictureID(string pictureId, string userId)
        {
            MongoServer server = MongoServer.Create(Global.DatabaseConnectionString);
            MongoDatabase database = server.GetDatabase(Global.DatabaseName);

            MongoCollection<FavoritedPicture> flaggedPictures = database.GetCollection<FavoritedPicture>("FavoritedPictures");
            var query = new QueryDocument("PictureID", pictureId);

            foreach (FavoritedPicture p in flaggedPictures.Find(query).ToList<FavoritedPicture>())
            {
                if (p.UserID == userId)
                    return p;
            }

            return null;
        }

        public static FavoritedPicture Create(FavoritedPicture data)
        {
            MongoServer server = MongoServer.Create(Global.DatabaseConnectionString);
            MongoDatabase database = server.GetDatabase(Global.DatabaseName);

            MongoCollection<FavoritedPicture> favoritedPictures = database.GetCollection<FavoritedPicture>("FavoritedPictures");

            data.CreatedDate = Utilities.ConvertToUnixTime(DateTime.UtcNow);

            favoritedPictures.Insert(data);

            return data;
        }

        public static FavoritedPicture Update(FavoritedPicture data)
        {
            MongoServer server = MongoServer.Create(Global.DatabaseConnectionString);
            MongoDatabase database = server.GetDatabase(Global.DatabaseName);

            MongoCollection<FavoritedPicture> favoritedPictures = database.GetCollection<FavoritedPicture>("FavoritedPictures");

            favoritedPictures.Save(data);

            return data;
        }

        public static void Delete(FavoritedPicture data)
        {
            MongoServer server = MongoServer.Create(Global.DatabaseConnectionString);
            MongoDatabase database = server.GetDatabase(Global.DatabaseName);

            MongoCollection<FavoritedPicture> favoritedPictures = database.GetCollection<FavoritedPicture>("FavoritedPictures");
            var query = new QueryDocument("_id", data.ID);

            favoritedPictures.FindAndRemove(query, new SortByDocument());
        }
    }
}