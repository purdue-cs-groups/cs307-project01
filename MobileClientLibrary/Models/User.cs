using System;
using System.Collections.Generic;
using System.Linq;
using MobileClientLibrary.Common;

namespace MobileClientLibrary.Models
{
    public class User
    {
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

        public string ProfilePictureID
        {
            get;
            set;
        }

        public string Location
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