using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebService.Models;
using MongoDB.Driver;
using WebService.Common;

namespace WebService.Controllers
{
    public static class APIConsumerController
    {
        public static APIConsumer Fetch(string id)
        {
            MongoServer server = MongoServer.Create(Global.DatabaseConnectionString);
            MongoDatabase database = server.GetDatabase(Global.DatabaseName);

            MongoCollection<APIConsumer> consumers = database.GetCollection<APIConsumer>("APIConsumers");
            var query = new QueryDocument("_id", id);

            return consumers.FindOne(query);
        }

        public static List<APIConsumer> FetchAll()
        {
            MongoServer server = MongoServer.Create(Global.DatabaseConnectionString);
            MongoDatabase database = server.GetDatabase(Global.DatabaseName);

            MongoCollection<APIConsumer> consumers = database.GetCollection<APIConsumer>("APIConsumers");

            return consumers.FindAll().ToList<APIConsumer>();
        }

        public static APIConsumer Create(APIConsumer data)
        {
            MongoServer server = MongoServer.Create(Global.DatabaseConnectionString);
            MongoDatabase database = server.GetDatabase(Global.DatabaseName);

            MongoCollection<APIConsumer> consumers = database.GetCollection<APIConsumer>("APIConsumers");

            data.CreatedDate = Utilities.ConvertToUnixTime(DateTime.UtcNow);

            consumers.Insert(data);

            return data;
        }

        public static APIConsumer Update(APIConsumer data)
        {
            MongoServer server = MongoServer.Create(Global.DatabaseConnectionString);
            MongoDatabase database = server.GetDatabase(Global.DatabaseName);

            MongoCollection<APIConsumer> consumers = database.GetCollection<APIConsumer>("APIConsumers");

            consumers.Save(data);

            return data;
        }

        public static void Delete(APIConsumer data)
        {
            MongoServer server = MongoServer.Create(Global.DatabaseConnectionString);
            MongoDatabase database = server.GetDatabase(Global.DatabaseName);

            MongoCollection<APIConsumer> consumers = database.GetCollection<APIConsumer>("APIConsumers");
            var query = new QueryDocument("_id", data.ID);

            consumers.FindAndRemove(query, new SortByDocument());
        }
    }
}