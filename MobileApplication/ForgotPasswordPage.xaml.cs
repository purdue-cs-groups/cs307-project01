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

namespace MetrocamPan
{
    public partial class ForgotPasswordPage : PhoneApplicationPage
    {
        public ForgotPasswordPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            MainPage.isFromLandingPage = true;
        }

        private void usernameInput_KeyUp(object sender, KeyEventArgs e)
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

        private void AboutMenuItem_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/AboutPage.xaml", UriKind.Relative));
        }

        private void Accept_Click(object sender, EventArgs e)
        {
            // Validate input
            if (!InputValidator.isValidLength(this.usernameInput.Text, "username", InputValidator.usernameLowerBoundary, InputValidator.usernameUpperBoundary) ||
                !InputValidator.isValidEmail(this.emailInput.Text))
            {
                return;
            }

            // For now, we set isLoggedIn to true
            Settings.isLoggedIn.Value = true;

            MainPage.isFromLandingPage = true;
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            // GoBack() Automatically clears everything on this PortraitPage
            NavigationService.GoBack();
        }
    }
}