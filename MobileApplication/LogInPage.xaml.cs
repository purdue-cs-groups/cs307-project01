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
using JeffWilcox.FourthAndMayor;
using System.Windows.Navigation;

namespace MetrocamPan
{
    public partial class LoginScreen : PhoneApplicationPage
    {
        public LoginScreen()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            App.isFromLandingPage = true;
        }

        private void Login_Click(object sender, EventArgs e)
        {
            // Validate input, prevent buffer overflows
            if (!InputValidator.isValidLength(this.usernameInput.Text, "username", InputValidator.usernameLowerBoundary, InputValidator.usernameUpperBoundary) ||
                !InputValidator.isValidLength(this.passwordInput.Password, "password", InputValidator.passwordLowerBoundary, InputValidator.passwordUpperBoundary))
            {
                return;
            }

            App.MetrocamService.AuthenticateCompleted += new MobileClientLibrary.RequestCompletedEventHandler(MetrocamService_AuthenticateCompleted);
            GlobalLoading.Instance.IsLoading = true;
            App.MetrocamService.Authenticate(this.usernameInput.Text, this.passwordInput.Password);
        }

        #region Authenticate

        private void MetrocamService_AuthenticateCompleted(object sender, RequestCompletedEventArgs e)
        {
            App.MetrocamService.AuthenticateCompleted -= MetrocamService_AuthenticateCompleted;
            GlobalLoading.Instance.IsLoading = false;

            FetchRecentPictures();

            // Obtain UserInfo object from web service
            UserInfo currentUser = App.MetrocamService.CurrentUser;

            // Load user specific settings
            Settings.getUserSpecificSettings(currentUser.Username);

            // Store into isolated storage
            Settings.isLoggedIn.Value = true;
            Settings.username.Value = currentUser.Username;
            Settings.password.Value = this.passwordInput.Password;      // As of now, currentUser.Password returns a hashed password.

            App.isFromLandingPage = true;
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        #endregion

        #region FetchRecent
        private void FetchRecentPictures()
        {
            App.MetrocamService.FetchNewsFeedCompleted += new RequestCompletedEventHandler(MetrocamService_FetchNewsFeedCompleted);

            try
            {
                App.MetrocamService.FetchNewsFeed();
            }
            catch (Exception ex)
            {
                // Do nothing
            }
        }

        void MetrocamService_FetchNewsFeedCompleted(object sender, RequestCompletedEventArgs e)
        {
            App.MetrocamService.FetchNewsFeedCompleted -= MetrocamService_FetchNewsFeedCompleted;
            App.RecentPictures.Clear();

            foreach (PictureInfo p in e.Data as List<PictureInfo>)
            {
                if (App.RecentPictures.Count == 10)
                    break;

                // changes to local time
                p.FriendlyCreatedDate = TimeZoneInfo.ConvertTime(p.FriendlyCreatedDate, TimeZoneInfo.Local);

                App.RecentPictures.Add(p);
            }
        }
        #endregion 

        #region AppBar

        private void About_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/AboutPage.xaml", UriKind.Relative));
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

        #endregion 

        #region Input
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

        #endregion

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            GlobalLoading.Instance.IsLoading = false;
        }
    }
}