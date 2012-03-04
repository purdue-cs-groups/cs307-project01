using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System.Runtime.Serialization;

namespace WebService.Models
{
    [DataContract]
    public class APIConsumer
    {
        [DataMember]
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string ID
        {
            get;
            set;
        }

        [DataMember]
        public string EmailAddress
        {
            get;
            set;
        }

        [DataMember]
        public string Key
        {
            get
            {
                return this.ID;
            }
        }

        [DataMember]
        public DateTime CreatedDate
        {
            get;
            set;
        }
    }
}