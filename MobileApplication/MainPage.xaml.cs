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

using MetrocamPan.Models;
using Microsoft.Phone.Tasks;

using System.IO;
using ExifLib;

using System.Device;
using System.Device.Location;
using MobileClientLibrary.Models;
using MobileClientLibrary;

namespace MetrocamPan
{
    public partial class MainPage : PhoneApplicationPage
    {
        public static GeoCoordinateWatcher watcher;
        public static double lat = 0;
        public static double lng = 0;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);

            setUpLocation();
        }

        public static void setUpLocation()
        {
            // set up location
            if (watcher == null)
            {
                watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.High); // using high accuracy
                watcher.MovementThreshold = 20; // use MovementThreshold to ignore noise in the signal
                watcher.StatusChanged += new EventHandler<GeoPositionStatusChangedEventArgs>(watcher_StatusChanged);
            }
        }

        // Load data for the ViewModel Items
        public static bool isFromLogin = false;
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (isFromLogin)
            {
                isFromLogin = false;
                NavigationService.RemoveBackEntry();
                NavigationService.RemoveBackEntry();
                NavigationService.RemoveBackEntry();
            }

            if (popularHubTiles.ItemsSource == null)
            {            
                Dispatcher.BeginInvoke(() => popularHubTiles.DataContext = App.PopularPictures);       
            }

            if (App.RecentPictures.Count == 0)
            {
                refreshRecentPictures();
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
                Settings.getSettings(Settings.username.Value);
            }
            else
            {
                NavigationService.Navigate(new Uri("/LandingPage.xaml", UriKind.Relative));
            }
        }

        public static ObservableCollection<Picture> UserPictures = new ObservableCollection<Picture>();

        #region Popular Pivot Codebehind
        /***************************************
         ******* Popular Pivot Codebehind ******
         ***************************************/

        public static HubTile selectedPicture;
        private void hubTilePictureTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            PictureView.SenderPage = 1;
 
            HubTileService.FreezeGroup("PopularTiles");

            HubTile tile = sender as HubTile;
            PictureInfo info = tile.DataContext as PictureInfo;

            NavigationService.Navigate(new Uri("/PictureView.xaml?id=" + info.ID, UriKind.Relative));          
        }

        #region refreshPopular
        public void refreshPopularPictures()
        {
            App.MetrocamService.FetchPopularNewsFeedCompleted += new MobileClientLibrary.RequestCompletedEventHandler(MetrocamService_FetchPopularNewsFeedCompleted);
            App.MetrocamService.FetchPopularNewsFeed();
        }

        void MetrocamService_FetchPopularNewsFeedCompleted(object sender, MobileClientLibrary.RequestCompletedEventArgs e)
        {
            App.PopularPictures.Clear();

            foreach (PictureInfo p in e.Data as List<PictureInfo>) 
            {
                if (App.PopularPictures.Count == 24)
                    continue;

                App.PopularPictures.Add(p); 
            }

            popularHubTiles.ItemsSource = App.PopularPictures;
        }
        #endregion 

        #region refreshRecent

        public void refreshRecentPictures()
        {
            // authenticate with user's credentials
            App.MetrocamService.AuthenticateCompleted += new RequestCompletedEventHandler(fetchRecent);
            App.MetrocamService.Authenticate(Settings.username.Value, Settings.password.Value);
        }

        private void fetchRecent(object sender, RequestCompletedEventArgs e)
        {
            App.MetrocamService.FetchNewsFeedCompleted += new RequestCompletedEventHandler(MetrocamService_FetchNewsFeedCompleted);
            App.MetrocamService.FetchNewsFeed();
        }

        void MetrocamService_FetchNewsFeedCompleted(object sender, RequestCompletedEventArgs e)
        {
            App.RecentPictures.Clear();

            foreach (PictureInfo p in e.Data as List<PictureInfo>)
            {
                if (App.RecentPictures.Count == 10)
                    break;

                App.RecentPictures.Add(p);
            }
        }

        #endregion 

        #endregion Popular Pivot Codebehind

        #region News Feed Codebehind
        /***************************************
         ***** News Feed Codebehind ************
         ***************************************/

        public static PictureInfo selectedNewsFeedPicture;
        private void newsFeedPictureTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            int index = recentPictures.SelectedIndex;
            PictureView.SenderPage = 2;

            // the Tag field of each Image on the News Feed will be the pictureID of the Picture
            // in the database

            selectedNewsFeedPicture = App.RecentPictures.ElementAt<PictureInfo>(index);
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

        public static Boolean isLandscape = false;
        public static Image captured = new Image();
        public static BitmapImage bmp = new BitmapImage();
        public static Boolean tookPhoto = false;
        private void cameraCaptureTask_Completed(object sender, PhotoResult e)
        {
            // if no picture was taken
            if (e.ChosenPhoto == null)
                return;

            // collect location data
            watcher.Start();

            tookPhoto = true;

            bool land = false;
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
                    land = true;
                    break;
                case ExifOrientation.TopRight:
                    _angle = 90;
                    break;
                case ExifOrientation.BottomRight:
                    _angle = 180;
                    land = true;
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
                if (land)
                {
                    NavigationService.Navigate(new Uri("/CropPageLandscapeOrientation.xaml", UriKind.Relative));
                }
                else
                {
                    NavigationService.Navigate(new Uri("/CropPage.xaml", UriKind.Relative));
                }
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
            // Reset all isolate storage Setting objects to default values
            Settings.logoutUser();

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

        private void ApplicationBarIconButton_Click_1(object sender, EventArgs e)
        {
            PhotoChooserTask picker = new PhotoChooserTask();
            picker.ShowCamera = false;
            picker.Completed += new EventHandler<PhotoResult>(picker_Completed);

            picker.Show();
        }

        public static int MAX_HEIGHT = 92;
        public static int MAX_WIDTH = 110;
        void picker_Completed(object sender, PhotoResult e)
        {
            if (e.ChosenPhoto == null)
                return;

            tookPhoto = false;

            e.ChosenPhoto.Position = 0;
            JpegInfo info = ExifReader.ReadJpeg(e.ChosenPhoto, e.OriginalFileName);

            lat = info.GpsLatitude[1];
            lng = info.GpsLongitude[1];

            bmp.SetSource(e.ChosenPhoto);
            captured.Source = bmp;

            Dispatcher.BeginInvoke(() =>
            {
                if (bmp.PixelWidth > bmp.PixelHeight)
                {
                    NavigationService.Navigate(new Uri("/CropPageLandscapeOrientation.xaml", UriKind.Relative));
                }
                else
                {
                    double ratio = Convert.ToDouble(Convert.ToDouble(bmp.PixelHeight) / Convert.ToDouble(bmp.PixelWidth));

                    if (ratio > 1.30 && ratio < 1.36)
                    {
                        MAX_HEIGHT = 133;
                        NavigationService.Navigate(new Uri("/CropPage.xaml", UriKind.Relative));
                    }
                    else
                    {

                    }
                }
            });
        }

        // Event handler for the GeoCoordinateWatcher.StatusChanged event.
        public static Boolean hasLocationData = false;
        public static void watcher_StatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            switch (e.Status)
            {
                case GeoPositionStatus.Disabled:
                    // The Location Service is disabled or unsupported.
                    // Check to see whether the user has disabled the Location Service.
                    if (watcher.Permission == GeoPositionPermission.Denied)
                    {
                        // The user has disabled the Location Service on their device.
                        hasLocationData = false;
                    }
                    else
                    {
                        hasLocationData = false;
                    }
                    break;

                case GeoPositionStatus.NoData:
                    // The Location Service is working, but it cannot get location data.
                    // Alert the user and enable the Stop Location button.
                    hasLocationData = false;
                    break;

                case GeoPositionStatus.Ready:
                    // The Location Service is working and is receiving location data.
                    // Show the current position and enable the Stop Location button.
                    lat = watcher.Position.Location.Latitude;
                    lng = watcher.Position.Location.Longitude;
                    hasLocationData = true;
                    watcher.Stop();
                    break;
            }
        }

        private void HubTile_Loaded(object sender, RoutedEventArgs e)
        {
            if (LoadingMessage.Visibility == Visibility.Visible)
            {
                LoadingMessage.Visibility = Visibility.Collapsed;
            }

            HubTile curr = sender as HubTile;
            curr.Visibility = Visibility.Visible;
        }

        private void MainContent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MainContent.SelectedIndex == 1 && recentPictures.ItemsSource == null)
                Dispatcher.BeginInvoke(() => recentPictures.DataContext = App.RecentPictures);
        }
    }
}