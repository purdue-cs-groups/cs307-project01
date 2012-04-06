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

using MetrocamPan.Models;

using Microsoft.Phone.Tasks;
using Microsoft.Phone.Shell;
using MobileClientLibrary.Models;

namespace MetrocamPan
{
    public partial class UserDetailPage : PhoneApplicationPage
    {
        public static bool isFollowing = true;
        public User user;

        public UserDetailPage()
        {
            InitializeComponent();
            UpdateAppBar();

            Loaded += new RoutedEventHandler(UserDetailPage_Loaded);
        }

        void UserDetailPage_Loaded(object sender, RoutedEventArgs e)
        {
            // Subscribe event handler
            App.MetrocamService.FetchUserCompleted += new MobileClientLibrary.RequestCompletedEventHandler(MetrocamService_FetchUserCompleted);
            // Calls fetch user
            App.MetrocamService.FetchUser(NavigationContext.QueryString["id"]);
        }

        void MetrocamService_FetchUserCompleted(object sender, MobileClientLibrary.RequestCompletedEventArgs e)
        {
            // Always unsubscribe the event handler just in case
            App.MetrocamService.FetchUserCompleted -= new MobileClientLibrary.RequestCompletedEventHandler(MetrocamService_FetchUserCompleted);

            UserInfo fetchedUser = (UserInfo)e.Data;

            this.profilePivot.DataContext = fetchedUser;

            // Set default text
            if (fetchedUser.Biography == null)
                this.biographyTextBlock.Text = "unknown";

            // Set created date (active since)
            DateTime activeSince = fetchedUser.FriendlyCreatedDate;
            this.activeSinceTextBlock.Text = activeSince.ToString();
        }

        private void SendEmail_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            EmailComposeTask em = new EmailComposeTask();
            em.To = emailTextBlock.Text;

            em.Show();
        }

        private void UpdateAppBar()
        {
            ApplicationBar.Buttons.Clear();

            if (isFollowing)
            {
                followingStatus.Text = "Following";
                followingStatus.Margin = new Thickness(60, 17, 0, 0);
                followingRec.Fill = new SolidColorBrush(Colors.Green);

                ApplicationBarIconButton unfollow = new ApplicationBarIconButton(new Uri("Images/appbar.arrow.down.png", UriKind.Relative));
                unfollow.Text = "unfollow";
                unfollow.Click += new EventHandler(Unfollow);
                ApplicationBar.Buttons.Add(unfollow);
            }
            else
            {
                followingStatus.Text = "Not Following";
                followingStatus.Margin = new Thickness(44, 17, 0, 0);
                followingRec.Fill = new SolidColorBrush(Colors.Red);

                ApplicationBarIconButton follow = new ApplicationBarIconButton(new Uri("Images/appbar.arrow.up.png", UriKind.Relative));
                follow.Text = "follow";
                follow.Click += new EventHandler(Follow);
                ApplicationBar.Buttons.Add(follow);
            }
        }

        public static HubTile selectedPicture;
        private void hubTilePictureTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            /*
            selectedPicture = (HubTile)sender;
            NavigationService.Navigate(new Uri("/PictureView.xaml", UriKind.Relative));
             */
        }

        private void Follow(object sender, EventArgs e)
        {
            isFollowing = true;
            UpdateAppBar();
        }

        private void Unfollow(object sender, EventArgs e)
        {
            isFollowing = false;
            UpdateAppBar();
        }
    }
}