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

namespace MetrocamPan.Helpers
{
    public class FriendlierTime
    {
        public static String Convert(DateTime friendly)
        {
            String friendlier = "sometime in the past";
            DateTime now        = DateTime.Now;
            int timeDifference = 0;

            if (now.Year - friendly.Year != 0)
            {
                timeDifference = now.Year - friendly.Year;
                if (timeDifference == 1)
                    friendlier = "a year ago";
                else
                    friendlier = timeDifference + " years ago";
            }
            else if (now.Month - friendly.Month != 0)
            {
                timeDifference = now.Month - friendly.Month;
                if (timeDifference == 1)
                    friendlier = "a month ago";
                else
                    friendlier = timeDifference + " months ago";
            }
            else if (now.Day - friendly.Day != 0)
            {
                timeDifference = now.Day - friendly.Day;
                if (timeDifference == 1)
                    friendlier = "yesterday";
                else
                    friendlier = timeDifference + " days ago";
            }
            else if (now.Hour - friendly.Hour != 0)
            {
                timeDifference = now.Hour - friendly.Hour;
                if (timeDifference == 1)
                    friendlier = "an hour ago";
                else
                    friendlier = timeDifference + " hours ago";
            }
            else if (now.Minute - friendly.Minute != 0)
            {
                timeDifference = now.Minute - friendly.Minute;
                if (timeDifference == 1)
                    friendlier = "a minute ago";
                else
                    friendlier = timeDifference + " minutes ago";
            }
            else if (now.Second - friendly.Second != 0)
            {
                timeDifference = now.Second - friendly.Second;
                if (timeDifference == 1)
                    friendlier = "a second ago";
                else
                    friendlier = timeDifference + " seconds ago";
            }


            return friendlier;
        }
    }
}
