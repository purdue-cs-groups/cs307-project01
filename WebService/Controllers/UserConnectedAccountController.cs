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
    public class UserConnectedAccountController
    {
        //Returns all the accounts that a user has connected.
        public static MongoCursor<BsonDocument> FetchConnectedAccounts(string uid)
        {
            MongoServer server = MongoServer.Create(Global.DatabaseConnectionString);
            MongoDatabase database = server.GetDatabase(Global.DatabaseName);

            MongoCollection<BsonDocument> connectedAccount = database.GetCollection<BsonDocument>("ConnectedAccount");
            var query = new QueryDocument("uid", uid);


            return connectedAccount.Find(query);
        }

        //Adds a new connected account.
        public static Boolean AddConnectedAccount(UserConnectedAccount account)
        {
            MongoServer server = MongoServer.Create(Global.DatabaseConnectionString);
            MongoDatabase database = server.GetDatabase(Global.DatabaseName);

            MongoCollection<BsonDocument> connectedAccount = database.GetCollection<BsonDocument>("ConnectedAccount");

            if (account != null)
            {
                connectedAccount.Insert(account);
                return true;
            }
            return false;
            
        }

        //Deletes an already connected account.
        public static Boolean DeleteConnectedAccount(string uid, string account)
        {
            MongoServer server = MongoServer.Create(Global.DatabaseConnectionString);
            MongoDatabase database = server.GetDatabase(Global.DatabaseName);

            MongoCollection<BsonDocument> connectedAccount = database.GetCollection<BsonDocument>("ConnectedAccount");
            var query = new QueryDocument
            {
                {"uid", uid},
                {"Account", account}
            };

            connectedAccount.Remove(query);
            return true;
        }

        //Updates the username or password of an already connected account.
        public static Boolean UpdateConnectedAccount(string uid, string updatedField, string data)
        {
            MongoServer server = MongoServer.Create(Global.DatabaseConnectionString);
            MongoDatabase database = server.GetDatabase(Global.DatabaseName);

            MongoCollection<BsonDocument> relationship = database.GetCollection<BsonDocument>("Relationship");
            var query = new QueryDocument
            {
                { "uid", uid}
            };
            if(updatedField.Equals("Username"))
            {
                var update = new UpdateDocument
                {
                    {"$set", new BsonDocument("Username", data)}
                };
                SafeModeResult updateRelation = relationship.Update(query, update);
                if (updateRelation.LastErrorMessage == null)
                {
                    return true;
                }
            }
            if(updatedField.Equals("Password"))
            {
                var update = new UpdateDocument
                {
                    {"$set", new BsonDocument("Password", data)}
                };
                SafeModeResult updateRelation = relationship.Update(query, update);
                if (updateRelation.LastErrorMessage == null)
                {
                    return true;
                }
            }            
            return false;
        }
    }
}