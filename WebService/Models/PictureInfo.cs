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
    [DataContract(Name = "Picture")]
    public class PictureInfo
    {
        public PictureInfo(Picture p, UserInfo u)
        {
            ID = p.ID;
            User = u;
            Caption = p.Caption;
            Latitude = p.Latitude;
            Longitude = p.Longitude;
            ViewCount = p.ViewCount;
            LargeURL = p.LargeURL;
            MediumURL = p.MediumURL;
            SmallURL = p.SmallURL;
            CreatedDate = p.CreatedDate;
        }

        [DataMember(Order = 0)]
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string ID
        {
            get;
            set;
        }

        [DataMember(Order = 1)]
        public UserInfo User
        {
            get;
            set;
        }

        [DataMember(Order = 2)]
        public string Caption
        {
            get;
            set;
        }

        [DataMember(Order = 3)]
        public decimal Latitude
        {
            get;
            set;
        }

        [DataMember(Order = 4)]
        public decimal Longitude
        {
            get;
            set;
        }

        [DataMember(Order = 5)]
        public int ViewCount
        {
            get;
            set;
        }

        [DataMember(Order = 6)]
        public string LargeURL
        {
            get;
            set;
        }

        [DataMember(Order = 7)]
        public string MediumURL
        {
            get;
            set;
        }

        [DataMember(Order = 8)]
        public string SmallURL
        {
            get;
            set;
        }

        [DataMember(Order = 9)]
        public bool IsFavorited
        {
            get;
            set;
        }

        [DataMember(Order = 10)]
        public bool IsFlagged
        {
            get;
            set;
        }

        [DataMember(Order = 11)]
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