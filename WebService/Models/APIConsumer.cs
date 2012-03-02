using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace WebService.Models
{
    public class APIConsumer
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string ID
        {
            get;
            set;
        }

        public string EmailAddress
        {
            get;
            set;
        }

        public string Key
        {
            get
            {
                return this.ID;
            }
        }

        public DateTime CreatedDate
        {
            get;
            set;
        }
    }
}