using System;
using System.Collections.Generic;
using System.Linq;
using MobileClientLibrary.Common;

namespace MobileClientLibrary.Models
{
    public class Relationship
    {
        public string ID
        {
            get;
            set;
        }

        public string UserID
        {
            get;
            set;
        }

        public string FollowingUserID
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
