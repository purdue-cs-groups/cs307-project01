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
        //Fetches a list of all the users following the user with given id.
        public static Relationship FetchRaltions(string uid)
        {
            MongoServer server = MongoServer.Create(Global.DatabaseConnectionString);
            MongoDatabase database = server.GetDatabase(Global.DatabaseName);

            MongoCollection<Relationship> relationship = database.GetCollection<Relationship>("Relationship");
            var query = new QueryDocument("uid", uid);

            return relationship.FindOne(query);
        }

        //Deletes a user from the list of followers for the given id.
        public static Boolean DeleteFollower(string uid, string follower)
        {
            MongoServer server = MongoServer.Create(Global.DatabaseConnectionString);
            MongoDatabase database = server.GetDatabase(Global.DatabaseName);

            MongoCollection<BsonDocument> relationship = database.GetCollection<BsonDocument>("Relationship");
            var query = new QueryDocument
            {
                { "uid", uid}
            };
            var update = new UpdateDocument
            {
                {"$pull", new BsonDocument("followers", follower)}
            };
            SafeModeResult updateRelation = relationship.Update(query, update);
            if (updateRelation.LastErrorMessage == null)
            {
                return true;
            }
            return false;
        }

        //Deletes a user from the list of users following the given id.
        public static Boolean DeleteFollowing(string uid, string following)
        {
            MongoServer server = MongoServer.Create(Global.DatabaseConnectionString);
            MongoDatabase database = server.GetDatabase(Global.DatabaseName);

            MongoCollection<BsonDocument> relationship = database.GetCollection<BsonDocument>("Relationship");
            var query = new QueryDocument
            {
                { "uid", uid}
            };
            var update = new UpdateDocument
            {
                {"$pull", new BsonDocument("following", following)}
            };
            SafeModeResult updateRelation = relationship.Update(query, update);
            if (updateRelation.LastErrorMessage == null)
            {
                return true;
            }
            return false;
        }

        //Adds a user to the list of users following the given id.
        public static Boolean AddFollower(string uid, string follower)
        {
            MongoServer server = MongoServer.Create(Global.DatabaseConnectionString);
            MongoDatabase database = server.GetDatabase(Global.DatabaseName);

            MongoCollection<BsonDocument> relationship = database.GetCollection<BsonDocument>("Relationship");
            var query = new QueryDocument
            {
                { "uid", uid}
            };
            var update = new UpdateDocument
            {
                {"$addToSet", new BsonDocument("followers", follower)}
            };
            SafeModeResult updateRelation = relationship.Update(query, update);
            if (updateRelation.LastErrorMessage == null)
            {
                return true;
            }
            return false;
        }

        //Adds a user to the list of users the given id is following.
        public static Boolean AddFollowing(string uid, string following)
        {
            MongoServer server = MongoServer.Create(Global.DatabaseConnectionString);
            MongoDatabase database = server.GetDatabase(Global.DatabaseName);

            MongoCollection<BsonDocument> relationship = database.GetCollection<BsonDocument>("Relationship");
            var query = new QueryDocument
            {
                { "uid", uid}
            };
            var update = new UpdateDocument
            {
                {"$addToSet", new BsonDocument("following", following)}
            };
            SafeModeResult updateRelation = relationship.Update(query, update);
            if (updateRelation.LastErrorMessage == null)
            {
                return true;
            }
            return false;
        }
    }
}