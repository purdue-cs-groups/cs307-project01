using System;
using System.Collections.Generic;
using System.Linq;

namespace MobileClientLibrary.Common
{
    public class PictureURL
    {
        public PictureURL(string largeURL, string mediumURL, string smallURL)
        {
            LargeURL = largeURL;
            MediumURL = mediumURL;
            SmallURL = smallURL;
        }

        public string LargeURL
        {
            get;
            set;
        }

        public string MediumURL
        {
            get;
            set;
        }

        public string SmallURL
        {
            get;
            set;
        }
    }
}