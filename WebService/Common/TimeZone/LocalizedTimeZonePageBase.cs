using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Threading;

namespace WebService.Common.TimeZone
{
    public class LocalizedTimeZonePageBase : Page
    {
        private const string _DEFAULT_TIME_ZONE_OFFSET_COOKIE_NAME = "TimeZoneOffset";

        #region Private properties

        private int _scriptCount = 0;
        private string _scriptNameFormat = "_clientScript{0}";

        private static string _timeZoneOffsetCookieName = null;

        private int _timeZoneOffset = -1;

        #endregion

        #region Protected properties

        #region TimeZoneOffset property

        protected int TimeZoneOffset
        {
            get
            {
                if (_timeZoneOffset != -1)
                    return _timeZoneOffset;

                if (!Helper.IsEmpty(Request.Cookies))
                {
                    HttpCookie cookie = Request.Cookies[TimeZoneOffsetCookieName];

                    if (cookie != null)
                    {
                        lock (this)
                        {
                            try
                            {
                                _timeZoneOffset = Int32.Parse((
                                                cookie.Value.Split('_'))[0]);
                            }
                            catch
                            {
                            }
                        }
                    }
                }

                if (_timeZoneOffset == -1)
                    return 0;

                return _timeZoneOffset;
            }
        }

        #endregion

        #region TimeZoneOffsetTimeoutGMT property

        protected DateTime TimeZoneOffsetTimeoutGMT
        {
            get
            {
                if (Helper.IsEmpty(Request.Cookies))
                    return DateTime.MinValue;

                HttpCookie cookie = Request.Cookies[TimeZoneOffsetCookieName];

                if (cookie == null)
                    return DateTime.MinValue;

                string[] cookieValues = cookie.Value.Split('_');

                if (cookieValues.Length < 2)
                    return DateTime.MinValue;

                try
                {
                    return new DateTime(long.Parse(cookieValues[1]));
                }
                catch
                {
                }
                return DateTime.MinValue;
            }
        }

        #endregion

        #endregion

        #region Public properties

        #region TimeZoneOffsetCookieName property

        public static string TimeZoneOffsetCookieName
        {
            get
            {
                if (Helper.IsEmpty(_timeZoneOffsetCookieName))
                    return _DEFAULT_TIME_ZONE_OFFSET_COOKIE_NAME;
                return _timeZoneOffsetCookieName;
            }
            set
            {
                lock (typeof(LocalizedTimeZonePageBase))
                {
                    _timeZoneOffsetCookieName = value;
                }
            }
        }

        #endregion

        #endregion

        #region Public methods

        #region ConvertLocalTimeToUtc method

        public DateTime ConvertLocalTimeToUtc(DateTime localDateTime)
        {
            if (TimeZoneOffset == 0)
                return localDateTime;

            return localDateTime.AddMinutes((double)(TimeZoneOffset));
        }

        #endregion

        #region ConvertUtcToLocalTime method

        public DateTime ConvertUtcToLocalTime(DateTime utcDateTime)
        {
            if (TimeZoneOffset == 0)
                return utcDateTime;

            return utcDateTime.AddMinutes((double)(0 - TimeZoneOffset));
        }

        #endregion

        #region FormatJavaScriptBlock method

        public static string FormatJavaScriptBlock(string javaScriptCode)
        {
            // If we did not get any JavaScript code, 
            // we should not generate any output.
            if (Helper.IsEmpty(javaScriptCode))
                return String.Empty;

            // Generate HTML for the script.
            return String.Format(
                            "{0}" +
                            "<SCRIPT Language=\"JavaScript\">{0}" +
                            "<!--{0}" +
                            "{1}{0}" +
                            "-->{0}" +
                            "</SCRIPT>{0}",
                            Environment.NewLine,
                            javaScriptCode);
        }

        #endregion

        #region FormatJavaScriptString method

        public static string FormatJavaScriptString(string message, params object[] args)
        {
            // Make sure we have a valid message.
            if (Helper.IsEmpty(message))
                return String.Empty;

            // If we have message parameters, build a formatted string.
            if (Helper.IsEmpty(args))
                message = message.Trim();
            else
                message = String.Format(message, args).Trim();

            // Back slashes, quotes (both single and double),
            // carriage returns, line feeds, and tabs must be 'escaped'.
            return message.Replace(
                "\\", "\\\\").Replace(
                    "'", "\\'").Replace(
                        "\"", "\\\"").Replace(
                            "\r", "\\r").Replace(
                                "\n", "\\n").Replace(
                                        "\t", "\\t");
        }

        #endregion

        #region FormatLocalDate method

        public string FormatLocalDate(DateTime utcDateTime)
        {
            return FormatLocalDateTime(utcDateTime,
                        DateTimeHelper.ShortDateFormat);
        }

        #endregion

        #region FormatLocalDateTime methods

        public string FormatLocalDateTime(object utcDateTime)
        {
            return FormatLocalDateTime(utcDateTime,
                            DateTimeHelper.ShortDateTimeFormat);
        }


        public string FormatLocalDateTime(object utcDateTime, string format)
        {
            if (Helper.IsEmpty(utcDateTime) ||
                utcDateTime.GetType() != typeof(DateTime))
                return String.Empty;

            return FormatLocalDateTime(
                    (DateTime)utcDateTime,
                    format);
        }

        public string FormatLocalDateTime(DateTime utcDateTime)
        {
            return FormatLocalDateTime(utcDateTime,
                            DateTimeHelper.ShortDateTimeFormat);
        }

        public string FormatLocalDateTime(DateTime utcDateTime, string format)
        {
            if (Helper.IsEmpty(utcDateTime))
                return String.Empty;

            return DateTimeHelper.Format(
                    ConvertUtcToLocalTime(utcDateTime),
                    format);
        }

        #endregion

        #region InitializeLocalTime methods

        public bool InitializeLocalTime()
        {
            return InitializeLocalTime(0);
        }

        public bool InitializeLocalTime(int refreshCookieTimeMin)
        {
            return InitializeLocalTime(refreshCookieTimeMin, String.Empty);
        }

        public bool InitializeLocalTime(int refreshCookieTimeMin, string timeZoneOffsetCookiePath)
        {
            return InitializeLocalTime(
                    refreshCookieTimeMin,
                    timeZoneOffsetCookiePath,
                    String.Empty);
        }

        public bool InitializeLocalTime(int refreshCookieTimeMin, string timeZoneOffsetCookiePath, string timeZoneOffsetCookieDomain)
        {
            bool isPostBack = IsPostBack || Request.Form.HasKeys();

            // Check if we already got the cookie and 
            // we do not need to check for timeout.
            if (!Helper.IsEmpty(Request.Cookies[TimeZoneOffsetCookieName])
                &&
                refreshCookieTimeMin <= 0)
                return false;

            // If we got the cookie, but need to check for timeout,
            // see if we reached the timeout.
            if (!Helper.IsEmpty(Request.Cookies[TimeZoneOffsetCookieName])
                &&
                refreshCookieTimeMin > 0
                &&
                DateTime.UtcNow < TimeZoneOffsetTimeoutGMT)
                return false;

            string path = Helper.IsEmpty(timeZoneOffsetCookiePath) ?
                            String.Empty :
                            String.Format(" path={0};",
                                    timeZoneOffsetCookiePath);

            string domain = Helper.IsEmpty(timeZoneOffsetCookieDomain) ?
                            String.Empty :
                            String.Format(" domain={0};",
                                    timeZoneOffsetCookieDomain);

            string timeout = refreshCookieTimeMin <= 0 ?
                            String.Empty :
                            "_" +
                            DateTime.UtcNow.AddMinutes(
                            (double)refreshCookieTimeMin).Ticks.ToString();

            // Generate a unique name of the start-up script.
            string scriptBlockName = String.Format(
                            _scriptNameFormat,
                            Interlocked.Increment(ref _scriptCount));

            // Generate HTML for the script.
            string scriptHtml = String.Format(
                    "document.cookie = \"{0}=\" + " +
                    "(new Date()).getTimezoneOffset() + " +
                                "\"{1};{2}{3}\";",
                    TimeZoneOffsetCookieName,
                    timeout, path, domain);

            // If we are not posting data, reload the page.
            // Before reloading, make sure that the Web browser
            // supports cookies.
            if (!isPostBack)
                scriptHtml += String.Format("{0}" +
                                "if (document.cookie) " +
                                "document.location.reload(true);",
                                Environment.NewLine);

            // Insert script after opening form (<form>) tag.
            ClientScript.RegisterClientScriptBlock(GetType(), scriptBlockName,
                    FormatJavaScriptBlock(scriptHtml));

            return true;
        }

        #endregion

        #region ShowPopup methods

        public void ShowPopup(string message, params object[] args)
        {
            ShowPopup(true, message, args);
        }

        public void ShowPopup(bool showFirst, string message, params object[] args)
        {
            if (Helper.IsEmpty(message))
                return;

            // Build message string which is safe to display in
            // JavaScript code.
            message = FormatJavaScriptString(message, args);

            // If we did not get any message, we should not generate
            // any output.
            if (message.Length == 0)
                return;

            // Generate a unique name of the start-up script.
            string scriptBlockName = String.Format(
                            _scriptNameFormat,
                            Interlocked.Increment(ref _scriptCount));

            // Generate HTML for the script.
            string scriptHtml =
                FormatJavaScriptBlock(
                    String.Format("alert(\"{0}\")", message));

            // Generate script opening a popup with error message.
            if (showFirst)
                ClientScript.RegisterClientScriptBlock(GetType(), scriptBlockName, scriptHtml);
            else
                ClientScript.RegisterStartupScript(GetType(), scriptBlockName, scriptHtml);
        }

        #endregion

        #endregion
    }
}
