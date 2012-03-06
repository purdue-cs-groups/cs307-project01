﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System.Runtime.Serialization;
using WebService.Common;

namespace WebService.Models
{
    [DataContract]
    public class User
    {
        [DataMember(Order = 0)]
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string ID
        {
            get;
            set;
        }

        [DataMember(Order = 1)]
        public string Username
        {
            get;
            set;
        }

        [DataMember(Order = 2)]
        public string Name
        {
            get;
            set;
        }

        [DataMember(Order = 3)]
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

        [DataMember(Order = 4)]
        public string Biography
        {
            get;
            set;
        }

        [DataMember(Order = 5)]
        public string ProfilePictureID
        {
            get;
            set;
        }

        [DataMember(Order = 6)]
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

        [DataMember(Name = "CreatedDate", Order = 7)]
        public int JsonCreatedDate
        {
            get
            {
                return Utilities.ConvertToUnixTime(this.CreatedDate);
            }

            set
            {
                this.CreatedDate = Utilities.ConvertFromUnixTime(value);
            }
        }
    }
}