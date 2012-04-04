using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using WebService.Models;

namespace WebService.Common
{
    [DataContract]
    public class Token
    {
        [DataMember(Name = "Token", Order = 0)]
        public string UniqueIdentifier
        {
            get;
            set;
        }

        [DataMember(Order = 1)]
        public UserInfo User
        {
            get;
            set;
        }
    }
}