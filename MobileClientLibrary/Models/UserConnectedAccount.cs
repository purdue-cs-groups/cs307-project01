using System;
using System.Collections.Generic;
using System.Linq;
using MobileClientLibrary.Common;

namespace MobileClientLibrary.Models
{
    public class UserConnectedAccount
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

        public string AccountName
        {
            get;
            set;
        }

        public string ClientToken
        {
            get;
            set;
        }

        public int ClientSecret
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
