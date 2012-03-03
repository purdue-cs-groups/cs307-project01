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

namespace WinstagramPan
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

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (SenderPage == 1)
            {
                String ownerID = MainPage.selectedPicture.Message;
                String ownerName = MainPage.selectedPicture.Title;
                String pictureCaptionText = MainPage.selectedPicture.Notification;
                ImageSource pictureSource = MainPage.selectedPicture.Source;

                pictureOwnerName.Text = ownerName;
                pictureView.Source = pictureSource;
                pictureCaption.Text = pictureCaptionText;
            }
            else if (SenderPage == 2)
            {
                Picture p = (from pic in MainPage.RecentPictures where pic.PictureID == Convert.ToInt16(MainPage.pictureID) select pic).First<Picture>();
                pictureOwnerName.Text = p.Username;
                pictureView.Source = p.Photo.Source;
                pictureCaption.Text = p.Caption;
            }
        }

        private void shareButton_Click(object sender, RoutedEventArgs e)
        {
            ShareLinkTask shareLinkTask = new ShareLinkTask();

            shareLinkTask.Title = pictureCaption.Text;

            // replace with Web Application URL
            shareLinkTask.LinkUri = new Uri("http://img.tgdaily.com/sites/default/files/stock/450teaser/steveballmer.jpg", UriKind.Absolute);
            shareLinkTask.Message = "Shared via Winstagram";

            shareLinkTask.Show();
        }
    }
}