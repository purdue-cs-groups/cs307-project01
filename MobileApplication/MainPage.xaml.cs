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

using System.Collections;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

using WinstagramPan.Models;
using Microsoft.Phone.Tasks;

using System.IO;
using ExifLib;

namespace WinstagramPan
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);

            // Library call to fetch news feed to populate RecentPictures
            populateRecentPictures();
        }

        // Load data for the ViewModel Items
        public static bool isFromLogin = false;
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }

            if (isFromLogin)
            {
                isFromLogin = false;
                NavigationService.RemoveBackEntry();
                NavigationService.RemoveBackEntry();
                NavigationService.RemoveBackEntry();
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            changeVisibilityOfContent(Settings.isLoggedIn.Value);
        }

        /* 
         * Change visibility of content based on flag
         * true: only panorama MainContent is visible
         * false: only tiled WelcomeScreen is visible 
         */
        private void changeVisibilityOfContent(bool flag)
        {
            if (flag)
            {
                
            }
            else
            {
                NavigationService.Navigate(new Uri("/LandingPage.xaml", UriKind.Relative));
            }
        }

        public static String APIToken;
        public static ObservableCollection<Picture> RecentPictures = new ObservableCollection<Picture>();
        public static ObservableCollection<Picture> PopularPictures = new ObservableCollection<Picture>();
        public static ObservableCollection<Picture> UserPictures = new ObservableCollection<Picture>();

        #region Popular Pivot Codebehind
        /***************************************
         ******* Popular Pivot Codebehind ******
         ***************************************/

        public static HubTile selectedPicture;
        private void hubTilePictureTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            PictureView.SenderPage = 1;

            selectedPicture = (HubTile) sender;
            NavigationService.Navigate(new Uri("/PictureView.xaml", UriKind.Relative));
        }

        #endregion Popular Pivot Codebehind

        #region News Feed Codebehind
        /***************************************
         ***** News Feed Codebehind ************
         ***************************************/
        
        private void populateRecentPictures()
        {
            RecentPictures.Clear();

            // first pic
            Picture p1 = new Picture();
            p1.Username = "Joe";
            p1.Caption = "It's Joe! He is helping to write this awesome Windows Phone 7 application.";
            p1.PictureID = 001;
            p1.Photo.Source = new BitmapImage(new Uri("Images/joe.jpg", UriKind.Relative));

            RecentPictures.Add(p1);

            // second pic
            Picture p2 = new Picture();
            p2.Username = "Matt";
            p2.Caption = "It's Matt!";
            p2.PictureID = 002;
            p2.Photo.Source = new BitmapImage(new Uri("Images/matt.jpg", UriKind.Relative));

            RecentPictures.Add(p2);

            // set ItemSource
            feed.ItemsSource = RecentPictures;
        }

        public static String pictureID;
        private void newsFeedPictureTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            PictureView.SenderPage = 2;

            Image i = (Image) sender;

            // the Tag field of each Image on the News Feed will be the pictureID of the Picture
            // in the database

            pictureID = i.Tag.ToString();  
            NavigationService.Navigate(new Uri("/PictureView.xaml", UriKind.Relative));
        }
        #endregion News Feed Codebehind

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
        public static BitmapImage bmp = new BitmapImage();
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
                NavigationService.Navigate(new Uri("/CropPage.xaml", UriKind.Relative));
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

        private void SignoutBarIconButton_Click(object sender, EventArgs e)
        {
            Settings.isLoggedIn.Value = false;

            changeVisibilityOfContent(Settings.isLoggedIn.Value);
        }

        private void Settings_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/SettingsPage.xaml", UriKind.Relative));
        }

        private void UserSearch_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/UserSearch.xaml", UriKind.Relative));
        }

        #endregion Application Bar Codebehind

        private void ViewUserDetailTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/UserDetailPage.xaml", UriKind.Relative));
        }

        private void EditProfile_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/EditProfile.xaml", UriKind.Relative));
        }

        private void About_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/AboutPage.xaml", UriKind.Relative));
        }
    }
}