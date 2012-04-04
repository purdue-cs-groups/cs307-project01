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

namespace MetrocamPan
{
    public partial class PictureView : PhoneApplicationPage
    {
        public PictureView()
        {
            InitializeComponent();
        }

        public static int SenderPage = 0; 
        // 1 = Popular
        // 2 = News Feed

        public static String ownerToGet = null;
        public static PictureInfo p = null;
        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            // change drastically when the time comes
            if (SenderPage == 1)
            {
                p = App.PopularPictures.Where(x => x.ID == NavigationContext.QueryString["id"]).SingleOrDefault<PictureInfo>();

                pictureView.Source = new BitmapImage(new Uri(p.MediumURL));
                pictureOwnerName.Text = p.User.Username;     
                pictureCaption.Text = p.Caption;
                pictureTakenTime.Text = p.FriendlyCreatedDate.ToString();
            }
            else if (SenderPage == 2)
            {
                p = MainPage.selectedNewsFeedPicture;
                pictureView.Source = new BitmapImage(new Uri(p.MediumURL));
                pictureOwnerName.Text = p.User.Username;
                pictureCaption.Text = p.Caption;
                pictureTakenTime.Text = p.FriendlyCreatedDate.ToString();
            }
        }

        #region Application Bar Codebehind
        /***************************************
         ***** Application Bar Codebehind ******
         ***************************************/

        private void SignoutBarIconButton_Click(object sender, EventArgs e)
        {
            Settings.isLoggedIn.Value = false;
        }

        private void Settings_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/SettingsPage.xaml", UriKind.Relative));
        }
        #endregion Application Bar Codebehind 
     
        private void ViewUserDetailTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/UserDetailPage.xaml", UriKind.Relative));
        }

        private void Share(object sender, EventArgs e)
        {
            ShareLinkTask shareLinkTask = new ShareLinkTask();

            shareLinkTask.Title = "Shared via Metrocam";

            // replace with Web Application URL
            shareLinkTask.LinkUri = new Uri("http://metrocam.cloudapp.net/p/" + p.ID, UriKind.Absolute);
            shareLinkTask.Message = pictureCaption.Text; 

            shareLinkTask.Show();
        }

        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            HubTileService.UnfreezeGroup("PopularTiles");
        }
    }
}