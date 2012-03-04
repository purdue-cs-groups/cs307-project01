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
    public class User
    {
        [DataMember]
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string ID
        {
            get;
            set;
        }

        [DataMember]
        public string Username
        {
            get;
            set;
        }

        [DataMember]
        public string Name
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

        public string Password
        {
            get;
            set;
        }

        [DataMember]
        public string Biography
        {
            get;
            set;
        }

        [DataMember]
        public int ProfilePictureID
        {
            get;
            set;
        }

        [DataMember]
        public string Location
        {
            get;
            set;
        }

        [DataMember]
        public DateTime CreatedDate
        {
            get;
            set;
        }
    }
}