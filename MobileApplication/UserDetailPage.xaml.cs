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
using System.Windows.Media.Imaging;

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

        PictureInfo SelectedPicture = null;
        void UserDetailPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (NavigationContext.QueryString["type"].Equals("popular"))
            {
                SelectedPicture = (from pic in App.PopularPictures where pic.ID.Equals(NavigationContext.QueryString["id"]) select pic).First<PictureInfo>();
            }
            else if (NavigationContext.QueryString["type"].Equals("recent"))
            {
                SelectedPicture = (from pic in App.RecentPictures where pic.ID.Equals(NavigationContext.QueryString["id"]) select pic).First<PictureInfo>();
            }

            // pivot name
            pivot.Title = SelectedPicture.User.Name;

            // profile pic
            profilePicture.Source = (new BitmapImage(new Uri(SelectedPicture.User.ProfilePicture.MediumURL, UriKind.RelativeOrAbsolute))); 

            // name
            fullName.Text = SelectedPicture.User.Name;

            // location
            if (SelectedPicture.User.Location == null)
                hometown.Text = "Earth";
            else
                hometown.Text = SelectedPicture.User.Location;

            // username
            usernameTextBlock.Text = SelectedPicture.User.Username;

            // bio
            if (SelectedPicture.User.Biography == null)
                biographyTextBlock.Text = "Just another Metrocammer!";
            else
                biographyTextBlock.Text = SelectedPicture.User.Biography;            

            // email
            emailTextBlock.Text = SelectedPicture.User.EmailAddress;

            // date
            DateTime activeSince = SelectedPicture.User.FriendlyCreatedDate;
            activeSinceTextBlock.Text = activeSince.ToString();

        }

        void MetrocamService_FetchUserCompleted(object sender, MobileClientLibrary.RequestCompletedEventArgs e)
        {
            // Always unsubscribe the event handler just in case
            App.MetrocamService.FetchUserCompleted -= new MobileClientLibrary.RequestCompletedEventHandler(MetrocamService_FetchUserCompleted);

            UserInfo fetchedUser = (UserInfo)e.Data;

            pivot.Name = fetchedUser.Name;
            this.profilePicture.Source = (new BitmapImage(new Uri(fetchedUser.ProfilePicture.MediumURL)));
            this.profilePivot.DataContext = fetchedUser;

            // Set default text
            if (fetchedUser.Biography == null)
                this.biographyTextBlock.Text = "Just another Metrocammer!";
            else
                biographyTextBlock.Text = fetchedUser.Biography;

            if (fetchedUser.Location == null)
                this.hometown.Text = "Earth";
            else
                hometown.Text = fetchedUser.Location;

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