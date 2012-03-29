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
        public PictureURL(string largeURL, string mediumURL, string smallURL)
        {
            LargeURL = largeURL;
            MediumURL = mediumURL;
            SmallURL = smallURL;
        }

        [DataMember(Name = "LargeURL", Order = 0)]
        public string LargeURL
        {
            get;
            set;
        }

        [DataMember(Name = "MediumURL", Order = 1)]
        public string MediumURL
        {
            get;
            set;
        }

        [DataMember(Name = "SmallURL", Order = 2)]
        public string SmallURL
        {
            get;
            set;
        }
    }
}