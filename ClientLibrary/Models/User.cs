using System;
using System.Collections.Generic;
using System.Linq;

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