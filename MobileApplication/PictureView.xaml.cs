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

namespace MetrocamPan
{
    public partial class PictureView : PhoneApplicationPage
    {
        PictureInfo CurrentPicture = null;

        public PictureView()
        {
            InitializeComponent();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            CurrentPicture = App.PopularPictures.Where(x => x.ID == NavigationContext.QueryString["id"]).SingleOrDefault<PictureInfo>();

            pictureView.Source = new BitmapImage(new Uri(CurrentPicture.MediumURL));
            pictureOwnerName.Text = CurrentPicture.User.Username;
            pictureCaption.Text = CurrentPicture.Caption;
            pictureTakenTime.Text = "shared " + FriendlierTime.Convert(CurrentPicture.FriendlyCreatedDate);
        }

        private void ViewUserDetail_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/UserDetailPage.xaml?id=" + CurrentPicture.User.ID, UriKind.Relative));
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
    }
}