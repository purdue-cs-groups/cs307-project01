using System;
using System.Collections.Generic;
using System.Linq;
using MobileClientLibrary.Common;

namespace MobileClientLibrary.Models
{
    public class UserStats
    {
        public int Followers
        {
            get;
            set;
        }

        public int Following
        {
            get;
            set;
        }

        public int Pictures
        {
            get;
            set;
        }
    }
}