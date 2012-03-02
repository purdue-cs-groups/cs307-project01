using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebService.Models;
using MongoDB.Driver;

namespace WebService.Controllers
{
    public static class APIConsumerController
    {
        public static APIConsumer Fetch(string id)
        {
            MongoServer server = MongoServer.Create(Global.DatabaseConnectionString);
            MongoDatabase database = server.GetDatabase(Global.DatabaseName);

            MongoCollection<APIConsumer> consumers = database.GetCollection<APIConsumer>("APIConsumers");
            var query = new QueryDocument("ID", id);

            return consumers.FindOne(query);
        }

        public static List<APIConsumer> FetchAll()
        {
            MongoServer server = MongoServer.Create(Global.DatabaseConnectionString);
            MongoDatabase database = server.GetDatabase(Global.DatabaseName);

            MongoCollection<APIConsumer> consumers = database.GetCollection<APIConsumer>("APIConsumers");

            return consumers.FindAll().ToList<APIConsumer>();
        }

        public static void Create(APIConsumer data)
        {
            MongoServer server = MongoServer.Create(Global.DatabaseConnectionString);
            MongoDatabase database = server.GetDatabase(Global.DatabaseName);

            MongoCollection<APIConsumer> consumers = database.GetCollection<APIConsumer>("APIConsumers");

            data.CreatedDate = DateTime.Now;

            consumers.Insert(data);
        }

        public static void Update(APIConsumer data)
        {
            MongoServer server = MongoServer.Create(Global.DatabaseConnectionString);
            MongoDatabase database = server.GetDatabase(Global.DatabaseName);

            MongoCollection<APIConsumer> consumers = database.GetCollection<APIConsumer>("APIConsumers");

            consumers.Save(data);
        }

        public static void Delete(APIConsumer data)
        {
            MongoServer server = MongoServer.Create(Global.DatabaseConnectionString);
            MongoDatabase database = server.GetDatabase(Global.DatabaseName);

            MongoCollection<APIConsumer> consumers = database.GetCollection<APIConsumer>("APIConsumers");
            var query = new QueryDocument("ID", data.ID);

            consumers.FindAndRemove(query, new SortByDocument());
        }
    }
}