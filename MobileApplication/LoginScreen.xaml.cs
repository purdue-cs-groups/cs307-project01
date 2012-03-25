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

namespace WinstagramPan
{
    public partial class LoginScreen : PhoneApplicationPage
    {
        public LoginScreen()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // For now, we set isLoggedIn to true
            Settings.isLoggedIn.Value = true;

            MainPage.isFromLogin = true;
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        private void ForgotPasswordButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void usernameInput_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // TODO: Validate Input!

                // Switch focus to next input field
                Dispatcher.BeginInvoke(() =>
                    passwordInput.Focus());
            }
        }

        private void passwordInput_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // TODO: Validate Input!

                // Lose Focus on the keyboard
                Dispatcher.BeginInvoke(() =>
                    this.Focus());
            }
        }
    }
}