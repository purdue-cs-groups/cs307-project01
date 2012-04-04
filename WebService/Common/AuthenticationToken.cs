using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebService.Models;

namespace WebService.Common
{
    public class AuthenticationToken
    {
        public AuthenticationToken(string uniqueIdentifier, UserInfo identity, APIConsumer consumer)
        {
            UniqueIdentifier = uniqueIdentifier;
            Identity = identity;
            Consumer = consumer;

            LastAccessDate = DateTime.UtcNow;
        }

        public string UniqueIdentifier
        {
            get;
            set;
        }

        public UserInfo Identity
        {
            get;
            set;
        }

        public APIConsumer Consumer
        {
            get;
            set;
        }

        public DateTime LastAccessDate
        {
            get;
            set;
        }

        public DateTime ExpirationDate
        {
            get
            {
                return LastAccessDate.AddMinutes(15);
            }
        }
    }
}