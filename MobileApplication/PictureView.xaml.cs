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

using WinstagramPan.Models;

using ExifLib;
using System.Windows.Media.Imaging;
using System.IO;

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

        #region Application Bar Codebehind
        /***************************************
         ***** Application Bar Codebehind ******
         ***************************************/

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            CameraCaptureTask cam = new CameraCaptureTask();
            cam.Completed += new EventHandler<PhotoResult>(cameraCaptureTask_Completed);

            cam.Show();
        }

        public static Image captured = new Image();
        private void cameraCaptureTask_Completed(object sender, PhotoResult e)
        {
            // figure out the orientation from EXIF data
            e.ChosenPhoto.Position = 0;
            JpegInfo info = ExifReader.ReadJpeg(e.ChosenPhoto, e.OriginalFileName);

            int _width = info.Width;
            int _height = info.Height;
            var _orientation = info.Orientation;
            int _angle = 0;

            switch (info.Orientation)
            {
                case ExifOrientation.TopLeft:
                case ExifOrientation.Undefined:
                    _angle = 0;
                    break;
                case ExifOrientation.TopRight:
                    _angle = 90;
                    break;
                case ExifOrientation.BottomRight:
                    _angle = 180;
                    break;
                case ExifOrientation.BottomLeft:
                    _angle = 270;
                    break;
            }

            BitmapImage bmp = new BitmapImage();
            if (_angle > 0d)
            {
                bmp.SetSource(RotateStream(e.ChosenPhoto, _angle));
            }
            else
            {
                bmp.SetSource(e.ChosenPhoto);
            }

            captured.Source = bmp;

            // wait til UI thread is done, then navigate
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri("/EditPicture.xaml", UriKind.Relative));
            });
        }

        // thanks, interwebs
        private Stream RotateStream(Stream stream, int angle)
        {
            stream.Position = 0;
            if (angle % 90 != 0 || angle < 0) throw new ArgumentException();
            if (angle % 360 == 0) return stream;

            BitmapImage bitmap = new BitmapImage();
            bitmap.SetSource(stream);
            WriteableBitmap wbSource = new WriteableBitmap(bitmap);

            WriteableBitmap wbTarget = null;
            if (angle % 180 == 0)
            {
                wbTarget = new WriteableBitmap(wbSource.PixelWidth, wbSource.PixelHeight);
            }
            else
            {
                wbTarget = new WriteableBitmap(wbSource.PixelHeight, wbSource.PixelWidth);
            }

            for (int x = 0; x < wbSource.PixelWidth; x++)
            {
                for (int y = 0; y < wbSource.PixelHeight; y++)
                {
                    switch (angle % 360)
                    {
                        case 90:
                            wbTarget.Pixels[(wbSource.PixelHeight - y - 1) + x * wbTarget.PixelWidth] = wbSource.Pixels[x + y * wbSource.PixelWidth];
                            break;
                        case 180:
                            wbTarget.Pixels[(wbSource.PixelWidth - x - 1) + (wbSource.PixelHeight - y - 1) * wbSource.PixelWidth] = wbSource.Pixels[x + y * wbSource.PixelWidth];
                            break;
                        case 270:
                            wbTarget.Pixels[y + (wbSource.PixelWidth - x - 1) * wbTarget.PixelWidth] = wbSource.Pixels[x + y * wbSource.PixelWidth];
                            break;
                    }
                }
            }
            MemoryStream targetStream = new MemoryStream();
            wbTarget.SaveJpeg(targetStream, wbTarget.PixelWidth, wbTarget.PixelHeight, 0, 100);
            return targetStream;
        }

        /**
         * 
         *  Need to fill this out.
         * 
         */
        private void SignoutBarIconButton_Click(object sender, EventArgs e)
        {
            Settings.isLoggedIn.Value = false;
        }

        private void Settings_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/SettingsPage.xaml", UriKind.Relative));
        }
        #endregion Application Bar Codebehind      
    }
}