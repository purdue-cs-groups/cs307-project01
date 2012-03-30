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

        public static void Create(Relationship data)
        {
            MongoServer server = MongoServer.Create(Global.DatabaseConnectionString);
            MongoDatabase database = server.GetDatabase(Global.DatabaseName);

            MongoCollection<Relationship> relationships = database.GetCollection<Relationship>("Relationships");

            data.CreatedDate = Utilities.ConvertToUnixTime(DateTime.UtcNow);

            relationships.Insert(data);
        }

        public static void Update(Relationship data)
        {
            MongoServer server = MongoServer.Create(Global.DatabaseConnectionString);
            MongoDatabase database = server.GetDatabase(Global.DatabaseName);

            MongoCollection<Relationship> relationships = database.GetCollection<Relationship>("Relationships");

            relationships.Save(data);
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