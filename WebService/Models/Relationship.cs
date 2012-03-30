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
    public class Relationship
    {
        [DataMember(Order = 0)]
        public string Followers
        {
            get;
            set;
        }

        [DataMember(Order = 1)]
        public string Following
        {
            get;
            set;
        }
    }
}