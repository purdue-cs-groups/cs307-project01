using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace WebService.Models
{
    public class User
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string ID
        {
            get;
            set;
        }

        public string Username
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

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

        public string Biography
        {
            get;
            set;
        }

        public int ProfilePictureID
        {
            get;
            set;
        }


        public string Location
        {
            get;
            set;
        }

        public DateTime CreatedDate
        {
            get;
            set;
        }
    }
}