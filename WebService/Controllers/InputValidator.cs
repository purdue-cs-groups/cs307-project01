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
        public static int usernameUpperBoundary = 12;
        public static int passwordLowerBoundary = 6;
        public static int passwordUpperBoundary = 12;

        /*
         * Validates that the String is between the range specified
         */
        public static bool isValidLength(String str, String nameOfInput, int lower, int upper)
        {
            if (str == null)
            {
                return false;
            }

            // Check length
            if (str.Length < lower || str.Length > upper)
            {
                MessageBox.Show(nameOfInput + " must be between " + lower + " and " + upper + " characters long", "Invalid Input", MessageBoxButton.OK);
                return false;
            }

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
            String errorMessage =
                    "must be between " + passwordLowerBoundary + " to " + passwordUpperBoundary + " in length"
                    + "\r\n" +
                    "cannot have special characters or spaces"
                    + "\r\n" +
                    "must have at least one number"
                    + "\r\n" +
                    "must have at least one capital letter";

            // Check for null
            if (password == null)
            {
                return false;
            }

            // Minimum and Maximum Length of field - 6 to 12 Characters
            if (password.Length < passwordLowerBoundary || password.Length > passwordUpperBoundary)
            {
                MessageBox.Show(errorMessage, "Invalid Password", MessageBoxButton.OK);
                return false;
            }

            // Special Characters - Not Allowed
            // Spaces - Not Allowed
            if (!(password.All(c => char.IsLetterOrDigit(c))))
            {
                MessageBox.Show(errorMessage, "Invalid Password", MessageBoxButton.OK);
                return false;
            }

            // Numeric Character - At least one character
            if (!password.Any(c => char.IsNumber(c)))
            {
                MessageBox.Show(errorMessage, "Invalid Password", MessageBoxButton.OK);
                return false;
            }

            // At least one Capital Letter
            if (!password.Any(c => char.IsUpper(c)))
            {
                MessageBox.Show(errorMessage, "Invalid Password", MessageBoxButton.OK);
                return false;
            }

            return true;
        }

        /*
         * Validates that both passwords are the same
         */
        public static bool isPasswordSame(String password1, String password2)
        {
            if (!password1.Equals(password2))
            {
                MessageBox.Show("Passwords entered do not match", "Invalid Password", MessageBoxButton.OK);
                return false;
            }
            return true;
        }

        /*
         * Validates that email address is correct format
         */
        public static bool isValidEmail(String email)
        {
            if (email == null)
            {
                return false;
            }

            // Matches regex pattern
            if (!Regex.IsMatch(email, emailPattern))
            {
                MessageBox.Show("Email address is not of valid format", "Invalid Email", MessageBoxButton.OK);
                return false;
            }

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
            String errorMessage =
                "must be between " + usernameLowerBoundary + " to " + usernameUpperBoundary + " in length"
                + "\r\n" +
                "cannot have special characters or spaces";

            // Check for null
            if (username == null)
            {
                return false;
            }

            // Minimum and Maximum Length of field - 4 to 12 Characters
            if (username.Length < usernameLowerBoundary || username.Length > usernameUpperBoundary)
            {
                MessageBox.Show(errorMessage, "Invalid Username", MessageBoxButton.OK);
                return false;
            }

            // Special Characters - Not Allowed
            // Spaces - Not Allowed
            if (!(username.All(c => char.IsLetterOrDigit(c))))
            {
                MessageBox.Show(errorMessage, "Invalid Username", MessageBoxButton.OK);
                return false;
            }

            return true;
        }

        /*
         * Checks whether this string is empty, returns true if empty
         */
        public static bool isNonEmpty(String input, String nameOfInput)
        {
            if (input.Trim().Length == 0)
            {
                MessageBox.Show("Please enter something for " + nameOfInput, "Invalid Input", MessageBoxButton.OK);
                return false;
            }
            return true;
        }
    }
}
