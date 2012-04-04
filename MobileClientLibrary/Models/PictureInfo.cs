using System;
using System.Collections.Generic;
using System.Linq;
using MobileClientLibrary.Common;

namespace MobileClientLibrary.Models
{
    public class PictureInfo
    {
        public string ID
        {
            get;
            set;
        }

        public User User
        {
            get;
            set;
        }

        public string Caption
        {
            get;
            set;
        }

        public decimal Latitude
        {
            get;
            set;
        }

        public decimal Longitude
        {
            get;
            set;
        }

        public int ViewCount
        {
            get;
            set;
        }

        public string LargeURL
        {
            get;
            set;
        }

        public string MediumURL
        {
            get;
            set;
        }

        public string SmallURL
        {
            get;
            set;
        }

        public int CreatedDate
        {
            get;
            set;
        }

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