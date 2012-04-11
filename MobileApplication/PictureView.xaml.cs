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
using Microsoft.Phone.Tasks;

using MetrocamPan.Models;

using ExifLib;
using System.Windows.Media.Imaging;
using System.IO;
using MobileClientLibrary.Models;
using MobileClientLibrary;
using MetrocamPan.Helpers;
using Microsoft.Phone.Shell;
using JeffWilcox.FourthAndMayor;
using System.Windows.Navigation;

namespace MetrocamPan
{
    public partial class PictureView : PhoneApplicationPage
    {
        PictureInfo CurrentPicture = null;
        Boolean AddedAppBarButton = false;

        public PictureView()
        {
            InitializeComponent();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (NavigationContext.QueryString["type"].Equals("popular"))
            {
                CurrentPicture = (from pic in App.PopularPictures where pic.ID.Equals(NavigationContext.QueryString["id"]) select pic).First<PictureInfo>();
            }
            else if (NavigationContext.QueryString["type"].Equals("recent"))
            {
                CurrentPicture = (from pic in App.RecentPictures where pic.ID.Equals(NavigationContext.QueryString["id"]) select pic).First<PictureInfo>();
            }
            else if (NavigationContext.QueryString["type"].Equals("user"))
            {
                CurrentPicture = (from pic in App.UserPictures where pic.ID.Equals(NavigationContext.QueryString["id"]) select pic).First<PictureInfo>();
            }

            if (CurrentPicture.User.ProfilePicture != null)
            {
                BitmapImage b = new BitmapImage(new Uri(CurrentPicture.User.ProfilePicture.MediumURL, UriKind.RelativeOrAbsolute));
                pictureOwnerPicture.Source = b;
            }

            pictureView.Source = new BitmapImage(new Uri(CurrentPicture.MediumURL));
            pictureOwnerName.Text = CurrentPicture.User.Username;
            pictureCaption.Text = CurrentPicture.Caption;
            pictureTakenTime.Text = FriendlierTime.Convert(CurrentPicture.FriendlyCreatedDate);

            if (CurrentPicture.User.ID.Equals(App.MetrocamService.CurrentUser.ID) && !AddedAppBarButton)
            {
                AddedAppBarButton = true;

                ApplicationBarMenuItem profilePic = new ApplicationBarMenuItem();
                profilePic.Text = "make profile picture";
                profilePic.Click += new EventHandler(MakeProfilePicture);

                ApplicationBar.MenuItems.Add(profilePic);
            }
            else if (!AddedAppBarButton)
            {
                AddedAppBarButton = true;

                ApplicationBarIconButton favorite = new ApplicationBarIconButton(new Uri("Images/appbar.heart.png", UriKind.Relative));
                favorite.Text = "favorite";
                favorite.Click += new EventHandler(Favorite);

                ApplicationBar.Buttons.Add(favorite);
            }
        }

        private void MakeProfilePicture (object sender, EventArgs e)
        {
            User updatedData = new User();
            UserInfo u = App.MetrocamService.CurrentUser;

            updatedData.Biography = u.Biography;
            updatedData.CreatedDate = u.CreatedDate;
            updatedData.EmailAddress = u.EmailAddress;
            updatedData.FriendlyCreatedDate = u.FriendlyCreatedDate;
            updatedData.ID = u.ID;
            updatedData.Location = u.Location;
            updatedData.Name = u.Name;
            updatedData.Password = App.MetrocamService.HashPassword(Settings.password.Value);
            updatedData.Username = u.Username;

            // update ProfilePicture
            updatedData.ProfilePictureID = CurrentPicture.ID;

            App.MetrocamService.UpdateUserCompleted += new RequestCompletedEventHandler(MetrocamService_UpdateUserCompleted);
            GlobalLoading.Instance.IsLoading = true;
            App.MetrocamService.UpdateUser(updatedData);
        }

        void MetrocamService_UpdateUserCompleted(object sender, RequestCompletedEventArgs e)
        {
            App.MetrocamService.UpdateUserCompleted -= MetrocamService_UpdateUserCompleted;
            GlobalLoading.Instance.IsLoading = false;
            MessageBox.Show("Your profile picture has been updated!");
        }

        private void Favorite (object sender, EventArgs e)
        {

        }

        private void ViewUserDetail_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (!NavigationContext.QueryString["type"].Equals("user"))
            {
                NavigationService.Navigate(new Uri("/UserDetailPage.xaml?id=" + CurrentPicture.ID + "&type=" + NavigationContext.QueryString["type"], UriKind.Relative));
            }
        }

        private void Share(object sender, EventArgs e)
        {
            ShareLinkTask shareLinkTask = new ShareLinkTask();

            shareLinkTask.Title = "Shared via Metrocam";

            // replace with Web Application URL
            shareLinkTask.LinkUri = new Uri("http://metrocam.cloudapp.net/p/" + CurrentPicture.ID, UriKind.Absolute);
            shareLinkTask.Message = pictureCaption.Text;

            shareLinkTask.Show();
        }

        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            HubTileService.UnfreezeGroup("PopularTiles");
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            if (GlobalLoading.Instance.IsLoading)
                GlobalLoading.Instance.IsLoading = false;
        }
    }
}