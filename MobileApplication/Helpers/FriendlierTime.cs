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
using System.Windows.Data;

namespace MetrocamPan.Helpers
{
    public class FriendlierTime : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            // datetime to convert
            DateTime friendly = (DateTime)value;

            var ts = new TimeSpan(DateTime.Now.Ticks - friendly.Ticks);
            double delta = Math.Abs(ts.TotalSeconds);

            if (delta < 30)
            {
                return "shared just now";
            }
            if (delta < 60)
            {
                return ts.Seconds == 1 ? "shared a second ago" : "shared " + ts.Seconds + " seconds ago";
            }
            if (delta < 120)
            {
                return "shared " + "a minute ago";
            }
            if (delta < 2700) // 45 * 60
            {
                return "shared " + ts.Minutes + " minutes ago";
            }
            if (delta < 5400) // 90 * 60
            {
                return "shared " + "an hour ago";
            }
            if (delta < 86400) // 24 * 60 * 60
            {
                if (ts.Hours == 1)
                {
                    return "shared two hours ago";
                }

                return "shared " + ts.Hours + " hours ago";
            }
            if (delta < 172800) // 48 * 60 * 60
            {
                return "shared " + "yesterday";
            }
            if (delta < 2592000) // 30 * 24 * 60 * 60
            {
                if (ts.Days == 1)
                {
                    return "shared yesterday";
                }

                return "shared " + ts.Days + " days ago";
            }
            if (delta < 31104000) // 12 * 30 * 24 * 60 * 60
            {
                int months = System.Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                return months <= 1 ? "shared " + "a month ago" : "shared " + months + " months ago";
            }

            int years = System.Convert.ToInt32(Math.Floor((double)ts.Days / 365));
            return years <= 1 ? "shared " + "a year ago" : "shared " + years + " years ago";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public static String Convert(DateTime friendly)
        {
            var ts = new TimeSpan(DateTime.Now.Ticks - friendly.Ticks);
            double delta = Math.Abs(ts.TotalSeconds);

            if (delta < 30)
            {
                return "shared just now";
            }
            if (delta < 60)
            {
                return ts.Seconds == 1 ? "shared a second ago" : "shared " + ts.Seconds + " seconds ago";
            }
            if (delta < 120)
            {
                return "shared " + "a minute ago";
            }
            if (delta < 2700) // 45 * 60
            {
                return "shared " + ts.Minutes + " minutes ago";
            }
            if (delta < 5400) // 90 * 60
            {
                return "shared " + "an hour ago";
            }
            if (delta < 86400) // 24 * 60 * 60
            {
                if (ts.Hours == 1)
                {
                    return "shared two hours ago";
                }

                return "shared " + ts.Hours + " hours ago";
            }
            if (delta < 172800) // 48 * 60 * 60
            {
                return "shared " + "yesterday";
            }
            if (delta < 2592000) // 30 * 24 * 60 * 60
            {
                if (ts.Days == 1)
                {
                    return "shared yesterday";
                }

                return "shared " + ts.Days + " days ago";
            }
            if (delta < 31104000) // 12 * 30 * 24 * 60 * 60
            {
                int months = System.Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                return months <= 1 ? "shared " + "a month ago" : "shared " + months + " months ago";
            }

            int years = System.Convert.ToInt32(Math.Floor((double)ts.Days / 365));
            return years <= 1 ? "shared " + "a year ago" : "shared " + years + " years ago";
        }
    }
}
