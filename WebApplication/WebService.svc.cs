using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.ServiceModel.Activation;
using System.Configuration;
using MongoDB.Driver;
using MongoDB.Bson;

namespace WebApplication
{
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public partial class WebService
    {
        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, UriTemplate = "common/helloWorld?name={name}")]
        public string HelloWorld(string name)
        {
            return "Hello World! Your name is " + name + ".";
        }

        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, UriTemplate = "database/getDocuments")]
        public string[] GetDocuments()
        {
            string connectionString = "mongodb://pu307user:Password01@ds029847.mongolab.com:29847";
            MongoServer server = MongoServer.Create(connectionString);

            MongoDatabase database = server.GetDatabase("pu307dev");

            return database.GetCollectionNames().ToArray<string>();
        }

        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, UriTemplate = "database/getDocumentData?name={name}")]
        public List<string> GetDocumentData(string name)
        {
            string connectionString = "mongodb://pu307user:Password01@ds029847.mongolab.com:29847";
            MongoServer server = MongoServer.Create(connectionString);

            MongoDatabase database = server.GetDatabase("pu307dev");

            List<string> result = new List<string>();

            MongoCollection collection = database.GetCollection(name);
            foreach (BsonDocument document in collection.FindAllAs<BsonDocument>())
            {
                result.Add(document.AsString);
            }

            return result;
        }
    }
}
