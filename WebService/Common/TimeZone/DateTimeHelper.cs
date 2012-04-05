using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace WebService.Common.TimeZone
{
    public class DateTimeHelper
    {
        private DateTimeHelper() { }

        public const string ShortDateFormat = "d";
        public const string ShortTimeFormat = "t";
        public const string ShortDateTimeFormat = "g";

        public static string Format(DateTime dateTime)
        {
            return Format(dateTime, String.Empty, false);
        }

        public static string Format(DateTime dateTime, string format)
        {
            return Format(dateTime, format, false);
        }

        public static string Format(DateTime dateTime, string format, bool showMinValue)
        {
            if (Helper.IsEmpty(dateTime))
            {
                if (!showMinValue)
                    return String.Empty;
            }

            return dateTime.ToString(format);
        }

        public static bool IsTime24(string time)
        {
            if (Helper.IsEmpty(time))
                return false;

            time = time.Trim();

            string pattern = @"^\d{1,2}:\d\d(:\d\d){0,1}$";
            Regex regex = new Regex(pattern);

            Match match = regex.Match(time);

            if (match == null)
                return false;

            // Make sure that numeric values are valid.
            string[] digits = time.Split(':');

            // At the least, we must have hour and minute.
            if (digits.Length < 2)
                return false;

            try
            {
                // Make sure the hour part is between 0 and 23.
                if (int.Parse(digits[0]) > 23)
                    return false;

                // Make sure the minute part is between 0 and 59.
                if (int.Parse(digits[1]) > 59)
                    return false;

                // Make sure the second part is between 0 and 59.
                if (digits.Length == 3 && int.Parse(digits[2]) > 59)
                    return false;
            }
            catch
            {
                return false;
            }

            return true;
        }

        public static bool ParseTime24(string time, out int hour, out int minute, out int second)
        {
            // Initialize the output values.
            hour = minute = second = 0;

            // Make sure that the format of the time string is valid.
            if (!IsTime24(time))
                return false;

            // Split the time parts.
            string[] digits = time.Split(':');

            // At the least, we must have hour and minute.
            if (digits.Length < 2)
                return false;

            int temp;

            try
            {
                // Make sure the hour part is between 0 and 23.
                temp = int.Parse(digits[0]);

                if (temp > 23)
                    return false;
                else
                    hour = temp;

                // Make sure the minute part is between 0 and 59.
                temp = int.Parse(digits[1]);

                if (temp > 59)
                    return false;
                else
                    minute = temp;

                // Make sure the second part is between 0 and 59.
                if (digits.Length == 3)
                {
                    temp = int.Parse(digits[2]);

                    if (temp > 59)
                        return false;
                    else
                        second = temp;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
