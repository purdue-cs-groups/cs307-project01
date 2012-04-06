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
using MobileClientLibrary;
using MobileClientLibrary.Models;

namespace MetrocamPan
{
    public partial class LoginScreen : PhoneApplicationPage
    {
        public LoginScreen()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, EventArgs e)
        {
            // Validate input, prevent buffer overflows
            if (!InputValidator.isValidLength(this.usernameInput.Text, "username", InputValidator.usernameLowerBoundary, InputValidator.usernameUpperBoundary) ||
                !InputValidator.isValidLength(this.passwordInput.Password, "password", InputValidator.passwordLowerBoundary, InputValidator.passwordUpperBoundary))
            {
                return;
            }

            try
            {
                App.MetrocamService.AuthenticateCompleted += new MobileClientLibrary.RequestCompletedEventHandler(MetrocamService_AuthenticateCompleted);
                
                // Calls authenticate method
                App.MetrocamService.Authenticate(this.usernameInput.Text, this.passwordInput.Password);

                /*Settings.getSettings(this.usernameInput.Text);

                // For now, we set isLoggedIn to true
                Settings.isLoggedIn.Value = true;
                Settings.username.Value = this.usernameInput.Text;
                Settings.password.Value = this.passwordInput.Password;

                MainPage.isFromLogin = true;
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));*/
                
            }
            catch (UnauthorizedAccessException ex)
            {
                // TODO: waiting for Matt
            }
        }

        private void MetrocamService_AuthenticateCompleted(object sender, RequestCompletedEventArgs e)
        {
            App.MetrocamService.AuthenticateCompleted -= new MobileClientLibrary.RequestCompletedEventHandler(MetrocamService_AuthenticateCompleted);

            // Obtain UserInfo object from web service
            UserInfo currentUser = App.MetrocamService.CurrentUser;

            // Load user specific settings
            Settings.getSettings(this.usernameInput.Text);

            Settings.isLoggedIn.Value = true;
            Settings.username.Value = currentUser.Username;
            Settings.password.Value = this.passwordInput.Password;      // As of now, currentUser.Password returns a hashed password.

            MainPage.isFromLogin = true;
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            // GoBack() Automatically clears everything on this PortraitPage
            NavigationService.GoBack();
        }

        private void Forgot_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/ForgotPasswordPage.xaml", UriKind.Relative));
        }

        private void usernameInput_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // Switch focus to next input field
                Dispatcher.BeginInvoke(() =>
                    passwordInput.Focus());
            }
        }

        private void passwordInput_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // Lose Focus on the keyboard
                Dispatcher.BeginInvoke(() =>
                    this.Focus());
            }
        }

        private void About_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/AboutPage.xaml", UriKind.Relative));
        }
    }
}