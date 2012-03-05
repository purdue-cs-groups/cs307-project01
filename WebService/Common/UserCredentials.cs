using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace WebService.Common
{
    [DataContract]
    public class UserCredentials
    {
        [DataMember]
        public string Username
        {
            get;
            set;
        }

        [DataMember]
        public string Password
        {
            get;
            set;
        }
    }
}