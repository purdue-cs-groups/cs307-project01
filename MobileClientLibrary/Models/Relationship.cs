using System;
using System.Collections.Generic;
using System.Linq;
using MobileClientLibrary.Common;

namespace ClientLibrary.Models
{
    public class Relationship
    {
        public string UID
        {
            get;
            set;
        }

        public string Followers
        {
            get;
            set;
        }

        public string Following
        {
            get;
            set;
        }
    }
}
