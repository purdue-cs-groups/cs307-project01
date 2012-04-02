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
using System.Windows.Media.Imaging;

using PictureEffects.Effects;
using MobileClientLibrary;
using System.IO;
using MobileClientLibrary.Common;
using MobileClientLibrary.Models;

namespace MetrocamPan
{
    public partial class UploadPage : PhoneApplicationPage
    {
        public UploadPage()
        {
            InitializeComponent();
        }

        private void captionKey(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.Focus();
            }
        }

        private Boolean isUploading = false;
        private void Upload_Click(object sender, EventArgs e)
        {
            if (!isUploading)
                isUploading = true;
            else
                return;

            captionBox.IsReadOnly = true;

            ProgressBar bar = new ProgressBar();
            bar.IsIndeterminate = true;

            this.progress.Children.Add(bar);

            // login with user's credentials
            App.MetrocamService.AuthenticateCompleted += new RequestCompletedEventHandler(client_AuthenticateCompleted);
            App.MetrocamService.Authenticate(Settings.username.Value, Settings.password.Value);
        }

        private void client_AuthenticateCompleted(object sender, RequestCompletedEventArgs e)
        {
            // unregister previous event handler
            App.MetrocamService.AuthenticateCompleted -= client_AuthenticateCompleted;

            WriteableBitmap bitmap = new WriteableBitmap((BitmapSource) EditPicture.editedPicture.Source);

            var width = bitmap.PixelWidth*2;
            var height = bitmap.PixelHeight*2;
            //var resultPixels = effect.Process(bitmap.Pixels, width, height);

            MemoryStream ms = new MemoryStream();
            bitmap.SaveJpeg(ms, width, height, 0, 100);
            ms.Seek(0, SeekOrigin.Begin);

            // upload the image
            App.MetrocamService.UploadPictureCompleted += new RequestCompletedEventHandler(client_UploadPictureCompleted);
            App.MetrocamService.UploadPicture(ms);
        }

        private void client_UploadPictureCompleted(object sender, RequestCompletedEventArgs e)
        {
            // unregister previous event handler
            App.MetrocamService.UploadPictureCompleted -= client_UploadPictureCompleted;

            // extract response
            PictureURL result = e.Data as PictureURL;

            // create new picture
            Picture data = new Picture();

            Dispatcher.BeginInvoke(() =>
            {
                data.Caption = this.captionBox.Text;

                if (Settings.locationService.Value)
                {
                    data.Latitude = Convert.ToDecimal(MainPage.lat);
                    data.Longitude = Convert.ToDecimal(MainPage.lng);
                }
                else
                {
                    data.Latitude  = Convert.ToDecimal(0.00);
                    data.Longitude = Convert.ToDecimal(0.00);
                }

                data.LargeURL = result.LargeURL;
                data.MediumURL = result.MediumURL;
                data.SmallURL = result.SmallURL;

                // upload the picture object
                App.MetrocamService.CreatePictureCompleted += new RequestCompletedEventHandler(client_CreatePictureCompleted);
                App.MetrocamService.CreatePicture(data);
            });
        }

        private void client_CreatePictureCompleted(object sender, RequestCompletedEventArgs e)
        {
            isUploading = false;

            // unregister previous event handler
            App.MetrocamService.CreatePictureCompleted -= client_CreatePictureCompleted;

            Dispatcher.BeginInvoke(() =>
            {
                MessageBox.Show("Your picture was uploaded successfully!");
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            });
        }

        private void SavePhotosToLibrary()
        {
        }
    }
}