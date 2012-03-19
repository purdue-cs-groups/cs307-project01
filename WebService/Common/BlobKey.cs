using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace WebService.Common
{
    [DataContract]
    public class BlobKey
    {
        public BlobKey(string key)
        {
            Key = key;
        }

        [DataMember(Name = "Key", Order = 0)]
        public string Key
        {
            get;
            set;
        }
    }
}