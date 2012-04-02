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
    public partial class SignupPage : PhoneApplicationPage
    {
        public SignupPage()
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
            //      Then checks email valid?
            //      Then checks password similar?
            //      Then checks password strong?
            if (!InputValidator.isValidUsername(this.usernameInput.Text) ||
                !InputValidator.isValidEmail(this.emailInput.Text) ||
                !InputValidator.isPasswordSame(this.passwordInput.Password, this.confirmPasswordInput.Password) || 
                !InputValidator.isStrongPassword(this.passwordInput.Password))
            {
                // Do nothing
                return;
            }

            User data = new User();
            data.Username = this.usernameInput.Text;
            data.Name = this.fullnameInput.Text;
            data.Password = this.passwordInput.Password;
            data.EmailAddress = this.emailInput.Text;

            try
            {
                // Subscribe event to CreateUserCompleted
                App.MetrocamService.CreateUserCompleted += new MobileClientLibrary.RequestCompletedEventHandler(MetrocamService_CreateUserCompleted);

                // Calls CreateUser to WebService
                App.MetrocamService.CreateUser(data);
            }
            catch (UnauthorizedAccessException ex)
            {
                // TODO: waiting for Matt
            }
        }

        private void MetrocamService_CreateUserCompleted(object sender, RequestCompletedEventArgs e)
        {
            // For now, we set isLoggedIn to true
            Settings.isLoggedIn.Value = true;

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