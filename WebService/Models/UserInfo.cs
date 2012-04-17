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
    [DataContract(Name = "User")]
    public class UserInfo
    {
        public UserInfo(User u, Picture p)
        {
            ID = u.ID;
            Username = u.Username;
            Name = u.Name;
            EmailAddress = u.EmailAddress;
            Password = u.Password;
            Biography = u.Biography;
            ProfilePicture = p;
            Location = u.Location;
            CreatedDate = u.CreatedDate;
        }

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
        public Picture ProfilePicture
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

        [DataMember(Order = 7)]
        public bool IsFollowing
        {
            get;
            set;
        }

        [DataMember(Order = 8)]
        public int CreatedDate
        {
            get;
            set;
        }

        [BsonIgnore]
        public DateTime FriendlyCreatedDate
        {
            get
            {
                return Utilities.ConvertFromUnixTime(this.CreatedDate);
            }

            set
            {
                this.CreatedDate = Utilities.ConvertToUnixTime(value);
            }
        }
    }
}