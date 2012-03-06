using System;
using System.Collections.Generic;
using System.Linq;

namespace MobileClientLibrary.Common
{
    public class UserCredentials
    {
        public UserCredentials(string username, string password)
        {
            Username = username;
            Password = password;
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