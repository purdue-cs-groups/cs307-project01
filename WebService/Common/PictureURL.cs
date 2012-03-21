using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace WebService.Common
{
    [DataContract]
    public class PictureURL
    {
        public PictureURL(string url)
        {
            URL = url;
        }

        [DataMember(Name = "URL", Order = 0)]
        public string URL
        {
            get;
            set;
        }
    }
}