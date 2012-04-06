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

namespace MetrocamPan
{
    public partial class SignUpPage : PhoneApplicationPage
    {
        private User currentUser;

        public SignUpPage()
        {
            InitializeComponent();
        }

        private void usernameInput_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // Switch focus to next input field
                Dispatcher.BeginInvoke(() =>
                    fullnameInput.Focus());
            }
        }

        private void fullnameInput_KeyUp(object sender, KeyEventArgs e)
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
                // Switch focus to next input field
                Dispatcher.BeginInvoke(() =>
                    confirmPasswordInput.Focus());
            }
        }

        private void confirmPasswordInput_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // Switch focus to next input field
                Dispatcher.BeginInvoke(() =>
                    emailInput.Focus());
            }
        }

        private void emailInput_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // Lose Focus on the keyboard
                Dispatcher.BeginInvoke(() =>
                    this.Focus());
            }
        }

        private void Accept_Click(object sender, EventArgs e)
        {
            // Validate Input
            //      First checks username valid?
            //      Then checks name empty?
            //      Then checks password similar?
            //      Then checks password strong?
            //      Then checks email valid?
            if (!InputValidator.isValidUsername(this.usernameInput.Text) ||
                !InputValidator.isNonEmpty(this.fullnameInput.Text, "full name") ||
                !InputValidator.isPasswordSame(this.passwordInput.Password, this.confirmPasswordInput.Password) || 
                !InputValidator.isStrongPassword(this.passwordInput.Password) ||
                !InputValidator.isValidEmail(this.emailInput.Text))
            {
                // Do nothing
                return;
            }

            currentUser = new User();
            currentUser.Username = this.usernameInput.Text;
            currentUser.Name = this.fullnameInput.Text;
            currentUser.Password = this.passwordInput.Password;
            currentUser.EmailAddress = this.emailInput.Text;

            // Set default values for fields
            currentUser.Biography = "Just another Metrocammer!";
            currentUser.Location = "Metrocam City";

            try
            {
                // Subscribe event to CreateUserCompleted
                App.MetrocamService.CreateUserCompleted += new MobileClientLibrary.RequestCompletedEventHandler(MetrocamService_CreateUserCompleted);

                // Calls CreateUser to WebService
                App.MetrocamService.CreateUser(currentUser);
            }
            catch (UnauthorizedAccessException ex)
            {
                // TODO: waiting for Matt
            }
        }

        private void MetrocamService_CreateUserCompleted(object sender, RequestCompletedEventArgs e)
        {
            // Unsubscribe
            App.MetrocamService.CreateUserCompleted -= new MobileClientLibrary.RequestCompletedEventHandler(MetrocamService_CreateUserCompleted);

            // Now we authenticate using the username and password
            App.MetrocamService.AuthenticateCompleted += new RequestCompletedEventHandler(MetrocamService_AuthenticateCompleted);

            // Calls Authenticate to obtain UserInfo object
            App.MetrocamService.Authenticate(currentUser.Username, currentUser.Password);
        }

        void MetrocamService_AuthenticateCompleted(object sender, RequestCompletedEventArgs e)
        {
            // Unsubscribe
            App.MetrocamService.AuthenticateCompleted -= new RequestCompletedEventHandler(MetrocamService_AuthenticateCompleted);

            UserInfo obtainedUser = App.MetrocamService.CurrentUser;

            // Load user specific settings
            Settings.getSettings(this.usernameInput.Text);

            Settings.isLoggedIn.Value = true;
            Settings.username.Value = currentUser.Username;
            Settings.password.Value = currentUser.Password;      // As of now, currentUser.Password returns a hashed password.

            MainPage.isFromLogin = true;
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            // Show MessageBox only if something has been entered into at least one input field
            MessageBox.Show("Your information has been discarded.");

            // GoBack() Automatically clears everything on this PortraitPage
            NavigationService.GoBack();
        }

        private void AboutMenuItem_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/AboutPage.xaml", UriKind.Relative));
        }
    }
}