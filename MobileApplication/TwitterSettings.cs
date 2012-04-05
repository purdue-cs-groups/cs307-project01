using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace MetrocamPan
{
    public class TwitterSettings
    {
        // Make sure you set your own ConsumerKey and Secret from dev.twitter.com
        public static string ConsumerKey = "40Ht8py4JbTqlupHwPrQ";
        public static string ConsumerKeySecret = "96yWUojoWG8v05PhiPbGasfLsRC4VofcVfXbiaPeamM";

        public static string RequestTokenUri = "https://api.twitter.com/oauth/request_token";
        public static string OAuthVersion = "1.0";
        public static string CallbackUri = "http://metrocam.cloudapp.net";
        public static string AuthorizeUri = "https://api.twitter.com/oauth/authorize";
        public static string AccessTokenUri = "https://api.twitter.com/oauth/access_token";

        public static string StatusUpdateUrl { get { return "http://api.twitter.com"; } }
    }
}
