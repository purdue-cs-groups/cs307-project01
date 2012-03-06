using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

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
    }
}