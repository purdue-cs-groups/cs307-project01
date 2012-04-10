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
using JeffWilcox.FourthAndMayor;
using System.Windows.Navigation;

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
            if (UserPictures.ItemsSource != null)
                return;

            if (NavigationContext.QueryString["type"].Equals("popular"))
            {
                SelectedPicture = (from pic in App.PopularPictures where pic.ID.Equals(NavigationContext.QueryString["id"]) select pic).First<PictureInfo>();
            }
            else if (NavigationContext.QueryString["type"].Equals("recent"))
            {
                SelectedPicture = (from pic in App.RecentPictures where pic.ID.Equals(NavigationContext.QueryString["id"]) select pic).First<PictureInfo>();
            }
            else if (NavigationContext.QueryString["type"].Equals("search"))
            {
                App.MetrocamService.FetchUserCompleted += new MobileClientLibrary.RequestCompletedEventHandler(MetrocamService_FetchUserCompleted);
                App.MetrocamService.FetchUser(NavigationContext.QueryString["id"]);
                return;
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

            // date
            DateTime activeSince = SelectedPicture.User.FriendlyCreatedDate;
            activeSinceTextBlock.Text = activeSince.ToString();

            App.MetrocamService.FetchUserPicturesCompleted += new MobileClientLibrary.RequestCompletedEventHandler(MetrocamService_FetchUserPicturesCompleted);
            GlobalLoading.Instance.IsLoading = true;
            App.MetrocamService.FetchUserPictures(SelectedPicture.User.ID);
        }

        void MetrocamService_FetchUserCompleted(object sender, MobileClientLibrary.RequestCompletedEventArgs e)
        {
            UserInfo u = e.Data as UserInfo;

            // pivot name
            pivot.Title = u.Name;

            // profile pic
            profilePicture.Source = (new BitmapImage(new Uri(u.ProfilePicture.MediumURL, UriKind.RelativeOrAbsolute)));

            // name
            fullName.Text = u.Name;

            // location
            if (u.Location == null)
                hometown.Text = "Earth";
            else
                hometown.Text = u.Location;

            // username
            usernameTextBlock.Text = u.Username;

            // bio
            if (u.Biography == null)
                biographyTextBlock.Text = "Just another Metrocammer!";
            else
                biographyTextBlock.Text = u.Biography;

            // date
            DateTime activeSince = u.FriendlyCreatedDate;
            activeSinceTextBlock.Text = activeSince.ToString();

            App.MetrocamService.FetchUserPicturesCompleted += new MobileClientLibrary.RequestCompletedEventHandler(MetrocamService_FetchUserPicturesCompleted);
            GlobalLoading.Instance.IsLoading = true;
            App.MetrocamService.FetchUserPictures(u.ID);
        }

        List<PictureInfo> userPictures = null;
        void MetrocamService_FetchUserPicturesCompleted(object sender, MobileClientLibrary.RequestCompletedEventArgs e)
        {
            userPictures = e.Data as List<PictureInfo>;
            userPictures.Reverse();
            App.UserPictures.Clear();

            foreach (PictureInfo p in userPictures)
            {
                p.FriendlyCreatedDate = TimeZoneInfo.ConvertTime(p.FriendlyCreatedDate, TimeZoneInfo.Local);
                if (p.User.ProfilePicture == null)
                {
                    p.User.ProfilePicture = new Picture();
                    p.User.ProfilePicture.MediumURL = "Images/dunsmore.png";
                }

                App.UserPictures.Add(p);
            }

            if (UserPictures.ItemsSource == null)
            {
                Dispatcher.BeginInvoke(() =>
                    UserPictures.DataContext = userPictures.GetRange(0, 24));
            }
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

        private void HubTile_Loaded(object sender, RoutedEventArgs e)
        {
            if (GlobalLoading.Instance.IsLoading)
                GlobalLoading.Instance.IsLoading = false;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            GlobalLoading.Instance.IsLoading = false;
        }

        private void HubTile_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            HubTile image = sender as HubTile;
            PictureInfo info = image.DataContext as PictureInfo;

            NavigationService.Navigate(new Uri("/PictureView.xaml?id=" + info.ID + "&type=user", UriKind.Relative));
        }
    }
}