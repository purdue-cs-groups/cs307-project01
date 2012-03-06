using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService.Common
{
    public class Utilities
    {
        private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        
        public static int ConvertToUnixTime(DateTime date)
        {
            TimeSpan diff = date - Epoch;
            return Convert.ToInt32(diff.TotalSeconds);
        }

        public static DateTime ConvertFromUnixTime(int ticks)
        {
            double seconds = Convert.ToDouble(ticks);
            return Epoch.AddSeconds(seconds);
        }
    }
}