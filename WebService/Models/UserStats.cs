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
    [DataContract(Name = "Relationship")]
    public class UserStats
    {
        public UserStats()
        {
        }

        [DataMember(Order = 0)]
        public int Followers
        {
            get;
            set;
        }

        [DataMember(Order = 1)]
        public int Following
        {
            get;
            set;
        }

        [DataMember(Order = 2)]
        public int Pictures
        {
            get;
            set;
        }
    }
}