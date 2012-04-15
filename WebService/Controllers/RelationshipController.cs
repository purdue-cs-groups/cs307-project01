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

        public static Relationship FetchUserRelationshipByIDs(string userId, string followingId)
        {
            MongoServer server = MongoServer.Create(Global.DatabaseConnectionString);
            MongoDatabase database = server.GetDatabase(Global.DatabaseName);

            MongoCollection<Relationship> relationships = database.GetCollection<Relationship>("Relationships");
            var query = new QueryDocument("UserID", userId);

            List<Relationship> UserRelationships = new List<Relationship>();

            foreach (Relationship r in relationships.Find(query))
            {
                if (r.FollowingUserID.Equals(followingId))
                    return r;
            }

            return null;
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