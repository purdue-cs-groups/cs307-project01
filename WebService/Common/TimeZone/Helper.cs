using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;

namespace WebService.Common.TimeZone
{
    public class Helper
    {
        private Helper() { }

        public static bool IsEmpty(object param)
        {
            if (param == null ||

                (param.GetType() == typeof(string) &&
                    IsEmpty(param.ToString())) ||

                (param.GetType() == typeof(DateTime) &&
                    IsEmpty((DateTime)param)) ||

                param == DBNull.Value)
            {
                return true;
            }

            return false;
        }

        public static bool IsEmpty(DateTime dateTime)
        {
            if (dateTime == DateTime.MinValue ||
                (dateTime.Year <= 1 &&
                 dateTime.Month <= 1 &&
                 dateTime.Day <= 1))
                return true;

            return false;
        }

        public static bool IsEmpty(ICollection param)
        {
            return (param == null || param.Count == 0);
        }

        public static bool IsEmpty(Array param)
        {
            return (param == null || param.Length == 0);
        }

        public static bool IsEmpty(string param)
        {
            // To be non-empty, string must be not null and have 
            // non-white-space chars in it.
            return IsEmpty(param, true);
        }

        public static bool IsEmpty(string param, bool ignoreWhiteSpaces)
        {
            if (param == null)
                return true;

            if (ignoreWhiteSpaces)
                return (param.Trim().Length == 0);

            return (param.Length == 0);
        }
    }
}
