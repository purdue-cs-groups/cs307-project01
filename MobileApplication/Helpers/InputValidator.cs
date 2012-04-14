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

using System.Linq;
using System.Text.RegularExpressions;

namespace MetrocamPan
{
    public class InputValidator
    {
        // Regex pattern taken from http://www.rhyous.com/2010/06/15/regular-expressions-in-cincluding-a-new-comprehensive-email-pattern/
        public const String emailPattern = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
            + "@"
            + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$";

        public static int usernameLowerBoundary = 4;
        public static int usernameUpperBoundary = 25;
        public static int passwordLowerBoundary = 6;
        public static int passwordUpperBoundary = 20;

        /*
         * Validates that the String is between the range specified
         */
        public static bool isValidLength(String str, String nameOfInput, int lower, int upper)
        {
            if (str == null)
                return false;

            // Check length
            if (str.Length < lower || str.Length > upper)
                return false;

            return true;
        }

        /*
         * Validates that password is a strong password
         * Conditions:
         *      Special characters not allowed
         *      Spaces not allowed
         *      At least one number character
         *      At least one capital character
         *      Between 6 to 12 characters in length
         */
        public static bool isStrongPassword(String password)
        {
            // Check for null
            if (password == null)
                return false;

            // Minimum and Maximum Length of field - 6 to 12 Characters
            if (password.Length < passwordLowerBoundary || password.Length > passwordUpperBoundary)
                return false;

            // Special Characters - Not Allowed
            // Spaces - Not Allowed
            if (!(password.All(c => char.IsLetterOrDigit(c))))
                return false;

            // Numeric Character - At least one character
            if (!password.Any(c => char.IsNumber(c)))
                return false;

            // At least one Capital Letter
            if (!password.Any(c => char.IsUpper(c)))
                return false;

            return true;
        }

        // Validates that both passwords are the same
        public static bool isPasswordSame(String password1, String password2)
        {
            if (!password1.Equals(password2))
                return false;
            return true;
        }

        // Validates that email address is correct format
        public static bool isValidEmail(String email)
        {
            if (email == null)
            {
                return false;
            }

            // Matches regex pattern
            if (!Regex.IsMatch(email, emailPattern))
                return false;

            return true;
        }

        /*
         * Validates that username is correct format
         * Conditions:
         *      Special characters not allowed
         *      Spaces not allowed
         *      Between 4-12 characters in length
         */
        public static bool isValidUsername(String username)
        {
            // Check for null
            if (username == null)
            {
                return false;
            }

            // Minimum and Maximum Length of field - 4 to 12 Characters
            if (username.Length < usernameLowerBoundary || username.Length > usernameUpperBoundary)
                return false;

            // Special Characters - Not Allowed
            // Spaces - Not Allowed
            if (!(username.All(c => char.IsLetterOrDigit(c))))
                return false;

            return true;
        }

        // Checks whether this string is empty, returns true if empty
        public static bool isNotEmpty(String input)
        {
            if (input.Trim().Length == 0)
                return false;
            return true;
        }
    }
}
