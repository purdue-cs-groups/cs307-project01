using System;
using System.Collections.Generic;
using System.Linq;

namespace MobileClientLibrary.Common
{
    public class PictureURL
    {
        public PictureURL(string url)
        {
            URL = url;
        }

        public string URL
        {
            get;
            set;
        }
    }
}