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
using System.Collections;
using System.Collections.ObjectModel;
using MetrocamPan.ScrollLoaders;

namespace MetrocamPan
{
    public partial class UserDetailPage : PhoneApplicationPage
    {
        public static bool isFollowing = true;
        public UserInfo userInfo;
        public static ObservableCollection<PictureInfo> ContinuedUserPictures = new ObservableCollection<PictureInfo>();

        public UserDetailPage()
        {
            InitializeComponent();
            UpdateAppBar();

            DataContext = new RecentViewModel();

            Loaded += new RoutedEventHandler(UserDetailPage_Loaded);
        }

        PictureInfo SelectedPicture = null;
        void UserDetailPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.UserPictures.ItemsSource != null)
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

            // save into userInfo object
            this.userInfo = SelectedPicture.User;

            // pivot name
            this.PivotRoot.Title = SelectedPicture.User.Name;

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
            activeSinceTextBlock.Text = SelectedPicture.User.FriendlyCreatedDate.ToShortDateString();
        }

        void MetrocamService_FetchUserCompleted(object sender, MobileClientLibrary.RequestCompletedEventArgs e)
        {
            userInfo = e.Data as UserInfo;

            // pivot name
            this.PivotRoot.Title = userInfo.Name;

            // profile pic
            profilePicture.Source = (new BitmapImage(new Uri(userInfo.ProfilePicture.MediumURL, UriKind.RelativeOrAbsolute)));

            // name
            fullName.Text = userInfo.Name;

            // location
            if (userInfo.Location == null)
                hometown.Text = "Earth";
            else
                hometown.Text = userInfo.Location;

            // username
            usernameTextBlock.Text = userInfo.Username;

            // bio
            if (userInfo.Biography == null)
                biographyTextBlock.Text = "Just another Metrocammer!";
            else
                biographyTextBlock.Text = userInfo.Biography;

            // date
            DateTime activeSince = userInfo.FriendlyCreatedDate;
            activeSinceTextBlock.Text = activeSince.ToString();
        }

        List<PictureInfo> userPictures = null;
        void MetrocamService_FetchUserPicturesCompleted(object sender, MobileClientLibrary.RequestCompletedEventArgs e)
        {
            userPictures = e.Data as List<PictureInfo>;
            userPictures.Reverse();
            App.UserPictures.Clear();

            if (UserPictures.ItemsSource == null)
            {
                this.UserPictures.DataContext = App.UserPictures;
            }

            foreach (PictureInfo p in userPictures)
            {
                p.FriendlyCreatedDate = TimeZoneInfo.ConvertTime(p.FriendlyCreatedDate, TimeZoneInfo.Local);

                if (App.UserPictures.Count < 24)
                {
                    // Put only 24 PictureInfo objects into App.UserPictures collection
                    if (p.User.ProfilePicture == null)
                    {
                        p.User.ProfilePicture = new Picture();
                        // Set default picture
                        p.User.ProfilePicture.MediumURL = "Images/dunsmore.png";
                    }

                    App.UserPictures.Add(p);
                }
                else
                {
                    // Put the rest into this.ContinuedUserPictures collection
                    ContinuedUserPictures.Add(p);
                }
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

        private void PictureTile_Loaded(object sender, RoutedEventArgs e)
        {
            if (GlobalLoading.Instance.IsLoading)
                GlobalLoading.Instance.IsLoading = false;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            if (GlobalLoading.Instance.IsLoading)
                GlobalLoading.Instance.IsLoading = false;
        }

        private void PictureTile_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Image image = sender as Image;
            PictureInfo info = image.DataContext as PictureInfo;

            NavigationService.Navigate(new Uri("/PictureView.xaml?id=" + info.ID + "&type=user", UriKind.Relative));
        }

        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PivotRoot.SelectedIndex == 1)
            {
                // Pictures pivot is selected, we fetch user pictures using user ID
                App.MetrocamService.FetchUserPicturesCompleted += new MobileClientLibrary.RequestCompletedEventHandler(MetrocamService_FetchUserPicturesCompleted);
                GlobalLoading.Instance.IsLoading = true;
                App.MetrocamService.FetchUserPictures(userInfo.ID);
            }
        }
    }
}