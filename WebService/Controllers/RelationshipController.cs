﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebService.Models;
using MongoDB.Driver;
using WebService.Common;
using MongoDB.Bson;
using MongoDB.Driver.Builders;

namespace WebService.Controllers
{
    public class RelationshipController
    {
        public static Relationship Fetch(string id)
        {
            MongoServer server = MongoServer.Create(Global.DatabaseConnectionString);
            MongoDatabase database = server.GetDatabase(Global.DatabaseName);

            MongoCollection<Relationship> relationships = database.GetCollection<Relationship>("Relationships");
            var query = new QueryDocument("_id", id);

            return relationships.FindOne(query);
        }

        public static List<Relationship> FetchByUserID(string userId)
        {
            MongoServer server = MongoServer.Create(Global.DatabaseConnectionString);
            MongoDatabase database = server.GetDatabase(Global.DatabaseName);

            MongoCollection<Relationship> relationships = database.GetCollection<Relationship>("Relationships");
            var query = new QueryDocument("UserID", userId);

            return relationships.Find(query).ToList<Relationship>();
        }

        public static Relationship FetchByFollowingUserID(string userId, string followingUserId)
        {
            MongoServer server = MongoServer.Create(Global.DatabaseConnectionString);
            MongoDatabase database = server.GetDatabase(Global.DatabaseName);

            MongoCollection<Relationship> relationships = database.GetCollection<Relationship>("Relationships");
            var query = new QueryDocument("UserID", userId);

            foreach (Relationship r in relationships.Find(query).ToList<Relationship>())
            {
                if (r.FollowingUserID == followingUserId)
                    return r;
            }

            return null;
        }

        public static bool IsFollowing(string userId, string followingUserId)
        {
            return RelationshipController.FetchByUserID(userId).Where(r => r.FollowingUserID == followingUserId).Count() > 0;
        }

        public static Relationship Create(Relationship data)
        {
            MongoServer server = MongoServer.Create(Global.DatabaseConnectionString);
            MongoDatabase database = server.GetDatabase(Global.DatabaseName);

            MongoCollection<Relationship> relationships = database.GetCollection<Relationship>("Relationships");

            data.CreatedDate = Utilities.ConvertToUnixTime(DateTime.UtcNow);

            relationships.Insert(data);

            return data;
        }

        public static Relationship Update(Relationship data)
        {
            MongoServer server = MongoServer.Create(Global.DatabaseConnectionString);
            MongoDatabase database = server.GetDatabase(Global.DatabaseName);

            MongoCollection<Relationship> relationships = database.GetCollection<Relationship>("Relationships");

            relationships.Save(data);

            return data;
        }

        public static void Delete(Relationship data)
        {
            MongoServer server = MongoServer.Create(Global.DatabaseConnectionString);
            MongoDatabase database = server.GetDatabase(Global.DatabaseName);

            MongoCollection<Relationship> relationships = database.GetCollection<Relationship>("Relationships");
            var query = new QueryDocument("_id", data.ID);

            relationships.FindAndRemove(query, new SortByDocument());
        }
    }
}