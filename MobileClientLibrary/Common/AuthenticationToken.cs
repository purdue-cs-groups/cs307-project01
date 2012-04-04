using System;
using System.Collections.Generic;
using System.Linq;
using MobileClientLibrary.Models;

namespace MobileClientLibrary.Common
{
    public class AuthenticationToken
    {
        public string Token
        {
            get;
            set;
        }

        public UserInfo User
        {
            get;
            set;
        }
    }
}