using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using MobileClientLibrary.Models;
using MobileClientLibrary;
using System.Windows.Navigation;
using JeffWilcox.FourthAndMayor;
using Coding4Fun.Phone.Controls;
using MetrocamPan.Helpers;

namespace MetrocamPan
{
    public partial class SignUpPage : PhoneApplicationPage
    {
        private User currentUser;

        private ToastPrompt toastDisplay;

        public const string DefaultLocation = "Metrocam City";
        public const string DefaultBiography = "A proud Metrocammer!";

        public SignUpPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            App.isFromLandingPage = true;

            this.LocationInput.Hint = "optional";
            this.BiographyInput.Hint = "optional";
        }

        private void UsernameInput_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // Switch focus to the whole frame
                Dispatcher.BeginInvoke(() =>
                    this.Focus());
            }
        }

        private void FullnameInput_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // Switch focus to the whole frame
                Dispatcher.BeginInvoke(() =>
                    this.Focus());
            }
        }

        private void PasswordInput_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // Switch focus to the whole frame
                Dispatcher.BeginInvoke(() =>
                    this.Focus());
            }
        }

        private void ConfirmPasswordInput_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // Switch focus to the whole frame
                Dispatcher.BeginInvoke(() =>
                    this.Focus());
            }
        }

        private void EmailInput_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // Switch focus to the whole frame
                Dispatcher.BeginInvoke(() =>
                    this.Focus());
            }
        }

        private void LocationInput_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // Switch focus to the whole frame
                Dispatcher.BeginInvoke(() =>
                    this.Focus());
            }
        }

        private void BiographyInput_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // Switch focus to the whole frame
                Dispatcher.BeginInvoke(() =>
                    this.Focus());
            }
        }

        // Begin validation sequence
        private string CheckAllInputs()
        {
            // Checks if any of the required fields are empty
            if (!InputValidator.isNotEmpty(this.UsernameInput.Text) ||
                !InputValidator.isNotEmpty(this.FullnameInput.Text) ||
                !InputValidator.isNotEmpty(this.PasswordInput.Password) ||
                !InputValidator.isNotEmpty(this.ConfirmPasswordInput.Password) ||
                !InputValidator.isNotEmpty(this.EmailInput.Text))
            {
                return "Please check that you have entered all required fields.";
            }

            // Checks if username is valid
            if (!InputValidator.isValidUsername(this.UsernameInput.Text))
            {
                return "Please check that your username is valid.";
            }

            // Checks if both passwords are similar
            if (!InputValidator.isPasswordSame(this.PasswordInput.Password, this.ConfirmPasswordInput.Password))
            {
                return "Please check that both passwords match.";
            }

            // Checks if password is valid
            if (!InputValidator.isStrongPassword(this.PasswordInput.Password))
            {
                return "Please check that your password is valid.";
            }

            // Checks if email is valid
            if (!InputValidator.isValidEmail(this.EmailInput.Text))
            {
                return "Please check that your email is valid.";
            }

            // Input validation passed
            return "Valid";
        }

        private void Accept_Click(object sender, EventArgs e)
        {
            string inputValidationMessage = CheckAllInputs();

            if (!inputValidationMessage.Equals("Valid"))
            {
                // Input validation failed
                if (toastDisplay != null)
                    toastDisplay.Hide();

                // Set properties of ToastPrompt
                toastDisplay = GlobalToastPrompt.CreateToastPrompt("Oops",
                    inputValidationMessage,
                    5000);

                toastDisplay.Show();

                return;
            }

            // Input validation passed, proceed with registration
            currentUser = new User();
            currentUser.Username = this.UsernameInput.Text;
            currentUser.Name = this.FullnameInput.Text;
            currentUser.Password = this.PasswordInput.Password;
            currentUser.EmailAddress = this.EmailInput.Text;

            // Set default values for optional fields if empty
            if (this.LocationInput.Text.Length == 0)
                currentUser.Location = DefaultLocation;
            else
                currentUser.Location = this.LocationInput.Text;

            if (this.BiographyInput.Text.Length == 0)
                currentUser.Biography = DefaultBiography;
            else
                currentUser.Biography = this.BiographyInput.Text;

            // Subscribe event to CreateUserCompleted
            App.MetrocamService.CreateUserCompleted += new MobileClientLibrary.RequestCompletedEventHandler(MetrocamService_CreateUserCompleted);

            GlobalLoading.Instance.IsLoading = true;

            // Calls CreateUser to WebService
            App.MetrocamService.CreateUser(currentUser);
        }

        private void MetrocamService_CreateUserCompleted(object sender, RequestCompletedEventArgs e)
        {
            // Unsubscribe
            App.MetrocamService.CreateUserCompleted -= new MobileClientLibrary.RequestCompletedEventHandler(MetrocamService_CreateUserCompleted);

            UnauthorizedAccessException err = e.Data as UnauthorizedAccessException;

            if (err != null)
            {
                if (GlobalLoading.Instance.IsLoading)
                    GlobalLoading.Instance.IsLoading = false;
                // There was an error with CreateUser in the webservice
                toastDisplay = GlobalToastPrompt.CreateToastPrompt(
                    "Signup Failure",
                    "This Username and/or Email is already being registered to another account.",
                    3000);
                toastDisplay.Show();
                return;
            }

            // Now we authenticate using the username and password
            App.MetrocamService.AuthenticateCompleted += new RequestCompletedEventHandler(MetrocamService_AuthenticateCompleted);

            // Calls Authenticate to obtain UserInfo object
            App.MetrocamService.Authenticate(this.UsernameInput.Text, this.PasswordInput.Password);
        }

        void MetrocamService_AuthenticateCompleted(object sender, RequestCompletedEventArgs e)
        {
            // Unsubscribe
            App.MetrocamService.AuthenticateCompleted -= new RequestCompletedEventHandler(MetrocamService_AuthenticateCompleted);

            GlobalLoading.Instance.IsLoading = false;

            // Save UserInfo object from WebClient
            UserInfo obtainedUser = App.MetrocamService.CurrentUser;

            // Load user specific settings
            Settings.getUserSpecificSettings(obtainedUser.Username);

            // Store into isolated storage
            Settings.isLoggedIn.Value = true;
            Settings.username.Value = currentUser.Username;
            Settings.password.Value = this.PasswordInput.Password;      // As of now, currentUser.Password returns a hashed password.

            App.isFromLandingPage = true;
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }

        private void AboutMenuItem_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/AboutPage.xaml", UriKind.Relative));
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            if (GlobalLoading.Instance.IsLoading)
                GlobalLoading.Instance.IsLoading = false;
        }

        private void UsernameInput_GotFocus(object sender, RoutedEventArgs e)
        {
            if (toastDisplay != null)
                toastDisplay.Hide();

            // Set properties of ToastPrompt
            toastDisplay = GlobalToastPrompt.CreateToastPrompt("Username Rules",
                "Must be between 4 and 25 characters.\nNo special characters.\nNo spaces.",
                10000);
            
            toastDisplay.Show();
        }

        private void UsernameInput_LostFocus(object sender, RoutedEventArgs e)
        {
            toastDisplay.Hide();
        }

        private void PasswordInput_GotFocus(object sender, RoutedEventArgs e)
        {
            if (toastDisplay != null)
                toastDisplay.Hide();

            // Set properties of ToastPrompt
            toastDisplay = GlobalToastPrompt.CreateToastPrompt("Password Rules",
                "Must between 6 and 20 characters.\nAt least one capital letter.\nAt least one number.\nNo special characters.\nNo spaces.",
                10000);

            toastDisplay.Show();
        }

        private void PasswordInput_LostFocus(object sender, RoutedEventArgs e)
        {
            toastDisplay.Hide();
        }
    }
}