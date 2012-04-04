﻿using System;
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

        public static PictureInfo FetchInfo(string id)
        {
            MongoServer server = MongoServer.Create(Global.DatabaseConnectionString);
            MongoDatabase database = server.GetDatabase(Global.DatabaseName);

            MongoCollection<Picture> pictures = database.GetCollection<Picture>("Pictures");
            var query = new QueryDocument("_id", id);

            Picture p = pictures.FindOne(query);

            UserInfo u = UserController.FetchInfo(p.UserID);
            
            return new PictureInfo(p, u);
        }

        public static List<PictureInfo> FetchNewsFeed(User data)
        {
            MongoServer server = MongoServer.Create(Global.DatabaseConnectionString);
            MongoDatabase database = server.GetDatabase(Global.DatabaseName);

            MongoCollection<Picture> pictures = database.GetCollection<Picture>("Pictures");
            var query = Query.GT("CreatedDate", Utilities.ConvertToUnixTime(DateTime.UtcNow.AddDays(-7)));

            // TODO: query the pictures properly

            List<PictureInfo> list = new List<PictureInfo>();
            foreach (Picture p in pictures.Find(query).SetSortOrder(SortBy.Descending("ViewCount")).SetSortOrder(SortBy.Descending("CreatedDate")).SetLimit(25).ToList<Picture>())
            {
                UserInfo u = UserController.FetchInfo(p.UserID);
                PictureInfo i = new PictureInfo(p, u);

                list.Add(i);
            }

            return list;
        }

        public static List<PictureInfo> FetchPopularNewsFeed()
        {
            MongoServer server = MongoServer.Create(Global.DatabaseConnectionString);
            MongoDatabase database = server.GetDatabase(Global.DatabaseName);

            MongoCollection<Picture> pictures = database.GetCollection<Picture>("Pictures");
            var query = Query.GT("CreatedDate", Utilities.ConvertToUnixTime(DateTime.UtcNow.AddDays(-7)));

            List<PictureInfo> list = new List<PictureInfo>();
            foreach (Picture p in pictures.Find(query).SetSortOrder(SortBy.Descending("ViewCount")).SetSortOrder(SortBy.Descending("CreatedDate")).SetLimit(25).ToList<Picture>())
            {
                UserInfo u = UserController.FetchInfo(p.UserID);
                PictureInfo i = new PictureInfo(p, u);

                list.Add(i);
            }

            return list;
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

        public static void Delete(PictureInfo data)
        {
            MongoServer server = MongoServer.Create(Global.DatabaseConnectionString);
            MongoDatabase database = server.GetDatabase(Global.DatabaseName);

            MongoCollection<Picture> pictures = database.GetCollection<Picture>("Pictures");
            var query = new QueryDocument("_id", data.ID);

            pictures.FindAndRemove(query, new SortByDocument());
        }
    }
}