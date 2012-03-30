using System;
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
    public class UserConnectedAccount
    {
        [DataMember(Order = 0)]
        public string UID
        {
            get;
            set;
        }

        [DataMember(Order = 1)]
        public string Account
        {
            get;
            set;
        }

        [DataMember(Order = 2)]
        public string Username
        {
            get;
            set;
        }

        [DataMember(Order = 3)]
        public string Password
        {
            get;
            set;
        }


    }
}