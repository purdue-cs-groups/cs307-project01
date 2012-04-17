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
using MetrocamPan.Helpers;
using Coding4Fun.Phone.Controls;

namespace MetrocamPan
{
    public partial class UserDetailPage : PhoneApplicationPage
    {
        public Boolean AppBarSet = false;
        public UserInfo userInfo;
        public Relationship r = null;

        // ToastPrompt for message display
        private ToastPrompt toastDisplay;

        public static ObservableCollection<PictureInfo> ContinuedUserPictures = new ObservableCollection<PictureInfo>();

        public UserDetailPage()
        {
            InitializeComponent();

            DataContext = new RecentViewModel();
        }

        PictureInfo SelectedPicture = null;
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (App.isFromEditProfile)
            {
                toastDisplay = GlobalToastPrompt.CreateToastPrompt(
                "Success!",
                "Your profile has been updated.");

                toastDisplay.Show();

                App.isFromEditProfile = false;
            }

            if (NavigationContext.QueryString["userid"].Equals(App.MetrocamService.CurrentUser.ID))
            {
                // User navigates to his own profile
                SetCurrentUserProfile();

                if (this.UserPictures.ItemsSource == null)
                {
                    App.MetrocamService.FetchUserPicturesCompleted += new MobileClientLibrary.RequestCompletedEventHandler(MetrocamService_FetchUserPicturesCompleted);
                    GlobalLoading.Instance.IsLoading = true;
                    App.MetrocamService.FetchUserPictures(App.MetrocamService.CurrentUser.ID);
                }
                else if (App.pictureIsDeleted == true)
                {
                    // User has deleted a picture and is navigated back to this page
                    App.pictureIsDeleted = false;
                    App.MetrocamService.FetchUserPicturesCompleted += new MobileClientLibrary.RequestCompletedEventHandler(MetrocamService_FetchUserPicturesCompleted);
                    GlobalLoading.Instance.IsLoading = true;
                    App.MetrocamService.FetchUserPictures(App.MetrocamService.CurrentUser.ID);
                }

                return;
            }
            else if (this.UserPictures.ItemsSource != null)
            {
                return;
            }

            if (NavigationContext.QueryString["type"].Equals("popular"))
            {
                // User navigated here from Popular Pivot
                SelectedPicture = (from pic in App.PopularPictures where pic.ID.Equals(NavigationContext.QueryString["id"]) select pic).First<PictureInfo>();
            }
            else if (NavigationContext.QueryString["type"].Equals("recent"))
            {
                // User navigated here from Recent Pivot
                SelectedPicture = (from pic in App.RecentPictures where pic.ID.Equals(NavigationContext.QueryString["id"]) select pic).First<PictureInfo>();
            }
            else if (NavigationContext.QueryString["type"].Equals("search"))
            {
                // User navigated here from Search Menu
                App.MetrocamService.FetchUserCompleted += new MobileClientLibrary.RequestCompletedEventHandler(MetrocamService_FetchUserCompleted);
                App.MetrocamService.FetchUser(NavigationContext.QueryString["id"]);
                return;
            }

            // Save into userInfo object
            this.userInfo = SelectedPicture.User;

            // pivot name
            this.PivotRoot.Title = SelectedPicture.User.Username;

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

            PictureLabel.Text = SelectedPicture.User.Pictures.ToString();
            FollowingLabel.Text = SelectedPicture.User.Following.ToString();
            FollowerLabel.Text = SelectedPicture.User.Followers.ToString();

            App.MetrocamService.FetchUserPicturesCompleted += new MobileClientLibrary.RequestCompletedEventHandler(MetrocamService_FetchUserPicturesCompleted);
            GlobalLoading.Instance.IsLoading = true;
            App.MetrocamService.FetchUserPictures(userInfo.ID);

            if (SelectedPicture.User.IsFollowing == false)
            {
                FollowingStatus.Text = "You are not following " + userInfo.Username + ".";
                ConstructAppBar(false, false);
            }
            else
            {
                FollowingStatus.Text = "You are following " + userInfo.Username + ".";
                ConstructAppBar(false, true);
            }
        }

        void MetrocamService_FetchUserCompleted(object sender, MobileClientLibrary.RequestCompletedEventArgs e)
        {
            App.MetrocamService.FetchUserCompleted -= MetrocamService_FetchUserCompleted;
            userInfo = e.Data as UserInfo;

            if (userInfo.ID.Equals(App.MetrocamService.CurrentUser.ID))
            {
                ConstructAppBar(true, true);
            }
            else
            {
                if (userInfo.IsFollowing == false)
                {
                    FollowingStatus.Text = "You are not following " + userInfo.Username + ".";
                    ConstructAppBar(false, false);
                }
                else
                {
                    FollowingStatus.Text = "You are following " + userInfo.Username + ".";
                    ConstructAppBar(false, true);
                }
            }

            // pivot name
            this.PivotRoot.Title = this.userInfo.Username;

            // profile pic
            profilePicture.Source = (new BitmapImage(new Uri(userInfo.ProfilePicture.MediumURL, UriKind.RelativeOrAbsolute)));

            // name
            fullName.Text = userInfo.Name;

            // location
            if (userInfo.Location == null)
                hometown.Text = SignUpPage.DefaultLocation;
            else
                hometown.Text = userInfo.Location;

            // username
            usernameTextBlock.Text = userInfo.Username;

            // bio
            if (userInfo.Biography == null)
                biographyTextBlock.Text = SignUpPage.DefaultBiography;
            else
                biographyTextBlock.Text = userInfo.Biography;

            PictureLabel.Text = SelectedPicture.User.Pictures.ToString();
            FollowingLabel.Text = SelectedPicture.User.Following.ToString();
            FollowerLabel.Text = SelectedPicture.User.Followers.ToString();

            App.MetrocamService.FetchUserPicturesCompleted += new MobileClientLibrary.RequestCompletedEventHandler(MetrocamService_FetchUserPicturesCompleted);
            GlobalLoading.Instance.IsLoading = true;
            App.MetrocamService.FetchUserPictures(userInfo.ID);
        }

        private void SetCurrentUserProfile()
        {
            ConstructAppBar(true, true);

            // pivot name
            this.PivotRoot.Title = App.MetrocamService.CurrentUser.Username;

            // profile pic
            profilePicture.Source = (new BitmapImage(new Uri(App.MetrocamService.CurrentUser.ProfilePicture.MediumURL, UriKind.RelativeOrAbsolute)));

            // name
            fullName.Text = App.MetrocamService.CurrentUser.Name;

            // location
            if (App.MetrocamService.CurrentUser.Location == null)
                hometown.Text = "Earth";
            else
                hometown.Text = App.MetrocamService.CurrentUser.Location;

            // username
            usernameTextBlock.Text = App.MetrocamService.CurrentUser.Username;

            // bio
            if (App.MetrocamService.CurrentUser.Biography == null)
                biographyTextBlock.Text = "Just another Metrocammer!";
            else
                biographyTextBlock.Text = App.MetrocamService.CurrentUser.Biography;

            PictureLabel.Text = App.MetrocamService.CurrentUser.Pictures.ToString();
            FollowingLabel.Text = App.MetrocamService.CurrentUser.Following.ToString();
            FollowerLabel.Text = App.MetrocamService.CurrentUser.Followers.ToString();
        }

        List<PictureInfo> userPictures = null;
        void MetrocamService_FetchUserPicturesCompleted(object sender, MobileClientLibrary.RequestCompletedEventArgs e)
        {
            App.MetrocamService.FetchUserPicturesCompleted -= MetrocamService_FetchUserPicturesCompleted;

            userPictures = e.Data as List<PictureInfo>;          
            userPictures.Reverse();
            App.UserPictures.Clear();

            // If user is still on profilePivot, set loading to false since we have loaded PictureLabel
            if (GlobalLoading.Instance.IsLoading)
                GlobalLoading.Instance.IsLoading = false;

            if (UserPictures.ItemsSource == null)
            {
                this.UserPictures.DataContext = App.UserPictures;
            }

            foreach (PictureInfo p in userPictures)
            {
                p.FriendlyCreatedDate = TimeZoneInfo.ConvertTime(p.FriendlyCreatedDate, TimeZoneInfo.Local);

                if (App.UserPictures.Count < 24)
                {
                    App.UserPictures.Add(p);
                }
                else
                {
                    // Put the rest into ContinuedUserPictures collection
                    ContinuedUserPictures.Add(p);
                }
            }
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

        private void EditButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/EditProfile.xaml", UriKind.Relative));
        }

        private void ConstructAppBar(Boolean isForUser, Boolean isFollowing)
        {
            if (AppBarSet)
                return;

            ApplicationBar.Buttons.Clear();

            if (isForUser)
            {
                ApplicationBarIconButton Edit = new ApplicationBarIconButton();
                Edit.Text = "edit profile";
                Edit.IconUri = new Uri("Images/appbar.edit.rest.png", UriKind.RelativeOrAbsolute);
                Edit.Click += EditButton_Click; 
                ApplicationBar.Buttons.Add(Edit);
            }
            else
            {
                if (isFollowing)
                {
                    ApplicationBarIconButton Unfollow = new ApplicationBarIconButton();
                    Unfollow.Text = "unfollow";
                    Unfollow.IconUri = new Uri("Images/appbar.user.minus.png", UriKind.RelativeOrAbsolute);
                    Unfollow.Click += new EventHandler(Unfollow_Click);
                    ApplicationBar.Buttons.Add(Unfollow);
                }
                else
                {
                    ApplicationBarIconButton Follow = new ApplicationBarIconButton();
                    Follow.Text = "follow";
                    Follow.IconUri = new Uri("Images/appbar.user.add.png", UriKind.RelativeOrAbsolute);
                    Follow.Click += new EventHandler(Follow_Click);
                    ApplicationBar.Buttons.Add(Follow);
                }
            }

            AppBarSet = true;
        }

        Boolean doingWork = false; 
        void Follow_Click(object sender, EventArgs e)
        {
            if (doingWork)
                return;

            Relationship data = new Relationship();
            data.UserID = App.MetrocamService.CurrentUser.ID;
            data.FollowingUserID = userInfo.ID;

            App.MetrocamService.CreateRelationshipCompleted += new MobileClientLibrary.RequestCompletedEventHandler(MetrocamService_CreateRelationshipCompleted);
            doingWork = true;
            App.MetrocamService.CreateRelationship(data);
        }

        void MetrocamService_CreateRelationshipCompleted(object sender, MobileClientLibrary.RequestCompletedEventArgs e)
        {
            App.MetrocamService.CreateRelationshipCompleted -= MetrocamService_CreateRelationshipCompleted;
            r = e.Data as Relationship;

            doingWork = false;
            AppBarSet = false;
            FollowingStatus.Text = "You are following " + userInfo.Username + "."; 
            ConstructAppBar(false, true); 
        }

        void Unfollow_Click(object sender, EventArgs e)
        {
            if (doingWork)
                return;

            App.MetrocamService.DeleteRelationshipCompleted += new MobileClientLibrary.RequestCompletedEventHandler(MetrocamService_DeleteRelationshipCompleted);
            doingWork = true;
            App.MetrocamService.DeleteRelationship(r);
        }

        void MetrocamService_DeleteRelationshipCompleted(object sender, MobileClientLibrary.RequestCompletedEventArgs e)
        {
            App.MetrocamService.DeleteRelationshipCompleted -= MetrocamService_DeleteRelationshipCompleted;

            doingWork = false;
            AppBarSet = false;
            FollowingStatus.Text = "You are not following " + userInfo.Username + ".";
            ConstructAppBar(false, false); 
        }
    }
}