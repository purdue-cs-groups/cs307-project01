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
using System.Windows.Threading;
using Microsoft.Phone.Controls;
using MobileClientLibrary;
using MobileClientLibrary.Models;
using JeffWilcox.FourthAndMayor;
using System.Windows.Navigation;
using Coding4Fun.Phone.Controls;

namespace MetrocamPan
{
    public partial class LoginScreen : PhoneApplicationPage
    {
        // Timer object to keep track of how long it is taking to authenticate [temporary fix to authenticate not responding atm]
        private DispatcherTimer timer;
        private int timeCount = 0;

        private ToastPrompt toastDisplay;
        private static ToastPrompt GetBasicToast(string title = "Basic")
        {
            return new ToastPrompt
            {
                Title = title,
                Message = "Please enter text here"
            };
        }

        public LoginScreen()
        {
            InitializeComponent();

            // Initialize timer
            timer = new DispatcherTimer();

            this.Loaded += new RoutedEventHandler(LoginScreen_Loaded);
        }

        void LoginScreen_Loaded(object sender, RoutedEventArgs e)
        {
            this.usernameInput.Focus();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            App.isFromLandingPage = true;
        }

        private void Login_Click(object sender, EventArgs e)
        {
            if (!InputValidator.isNotEmpty(this.usernameInput.Text) ||
                !InputValidator.isNotEmpty(this.passwordInput.Password))
            {
                toastDisplay = GetBasicToast("Oops!");
                toastDisplay.Message = "Please check that you have entered all required fields.";
                toastDisplay.MillisecondsUntilHidden = 3000;
                toastDisplay.TextWrapping = TextWrapping.Wrap;
                toastDisplay.Show();
                return;
            }


            App.MetrocamService.AuthenticateCompleted += new MobileClientLibrary.RequestCompletedEventHandler(MetrocamService_AuthenticateCompleted_Login);
            GlobalLoading.Instance.IsLoading = true;

            // Atach timer event handler for tick
            this.timer.Tick += new EventHandler(timer_Tick);
            // Set one second as each tick
            this.timer.Interval = new TimeSpan(0, 0, 1);
            // Start timer
            this.timer.Start();
            
            App.MetrocamService.Authenticate(this.usernameInput.Text, this.passwordInput.Password);
        }

        // This is catched when authenticate takes too long [temporary fix to authenticate not responding atm]
        void timer_Tick(object sender, EventArgs e)
        {
            timeCount++;

            // Has authenticate taken at least 5 seconds?
            if (timeCount >= 5)
            {
                // Detach event handlers
                App.MetrocamService.AuthenticateCompleted -= MetrocamService_AuthenticateCompleted_Login;
                this.timer.Tick -= timer_Tick;

                GlobalLoading.Instance.IsLoading = false;
                toastDisplay = GetBasicToast("Oops!");
                toastDisplay.Message = "The credentials you provided are invalid.";
                toastDisplay.MillisecondsUntilHidden = 3000;
                toastDisplay.TextWrapping = TextWrapping.Wrap;
                toastDisplay.Show();
            }
        }

        #region Authenticate

        private void MetrocamService_AuthenticateCompleted_Login(object sender, RequestCompletedEventArgs e)
        {
            App.MetrocamService.AuthenticateCompleted -= MetrocamService_AuthenticateCompleted_Login;
            GlobalLoading.Instance.IsLoading = false;

            UnauthorizedAccessException err = e.Data as UnauthorizedAccessException;

            if (err != null)
            {
                toastDisplay = GetBasicToast("Oops!");
                toastDisplay.Message = "The credentials you provided are invalid.";
                toastDisplay.MillisecondsUntilHidden = 3000;
                toastDisplay.TextWrapping = TextWrapping.Wrap;
                toastDisplay.Show();
                return;
            }

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

            if (GlobalLoading.Instance.IsLoading)
                GlobalLoading.Instance.IsLoading = false;
        }
    }
}