using System;
using System.Collections.Generic;
using System.Linq;
using MobileClientLibrary.Common;

namespace ClientLibrary.Models
{
    public class UserConnectedAccount
    {
        public string UID
        {
            get;
            set;
        }

        public string Account
        {
            get;
            set;
        }

        public string Username
        {
            get;
            set;
        }

        public string Password
        {
            get;
            set;
        }
    }
}
