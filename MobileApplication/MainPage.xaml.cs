using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
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
using JeffWilcox.FourthAndMayor;
using MetrocamPan.ScrollLoaders;
using TweetSharp;
using System.IO.IsolatedStorage;
using Microsoft.Xna.Framework.Media;
using Coding4Fun.Phone.Controls;
using MetrocamPan.Helpers;

namespace MetrocamPan
{
    public partial class MainPage : PhoneApplicationPage
    {
        private DateTime startPop;
        private DateTime endPop;
        private DateTime startRec;
        private DateTime endRec;

        public static String TwitterToken = null;
        public static String TwitterSecret = null;

        public static GeoCoordinateWatcher watcher;
        public static double lat = 0;
        public static double lng = 0;

        // ToastPrompt for message display
        private ToastPrompt toastDisplay;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            SetUpLocation();

            // Calls MainPage_Loaded when this page is loaded
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);

            // This is for dynamic loading of RecentPictures
            DataContext = new RecentViewModel();
        }

        // Load data
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            // Checks if there was an exception during previous run
            LittleWatson.CheckForPreviousException(true);

            if (PopularPictures.ItemsSource == null)
            {
                GlobalLoading.Instance.IsLoading = true;
                Dispatcher.BeginInvoke(() =>
                    PopularPictures.DataContext = App.PopularPictures);
            }
        }

        // Display the loading message dynamically based on time
        private void LoadingMessage_Loaded(object sender, RoutedEventArgs e)
        {
            int currentHour = DateTime.Now.Hour;

            if (currentHour < 12 && currentHour > 4)
                LoadingMessage.Text = "Good morning, " + Settings.username.Value + "!";
            else if (currentHour < 17 && currentHour >= 12)
                LoadingMessage.Text = "Good afternoon, " + Settings.username.Value + "!";
            else
                LoadingMessage.Text = "Good evening, " + Settings.username.Value + "!";
        }

        // When this page becomes the active page in the app
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            startPop = DateTime.Now;

            if (App.isFromUploadPage)
            {
                toastDisplay = GlobalToastPrompt.CreateToastPrompt(
                    "Success!",
                    "Your picture has been uploaded.");

                toastDisplay.Show();

                /**
                 * One to remove the Share page, one to remove the MainPage it came from. If you don't
                 * have the second one and hit Back, you'll load the same page and it looks weird.
                 */
                NavigationService.RemoveBackEntry();
                NavigationService.RemoveBackEntry();

                App.isFromUploadPage = false;
            }
            else if (App.isFromEditProfile)
            {
                toastDisplay = GlobalToastPrompt.CreateToastPrompt(
                "Success!",
                "Your profile has been updated.");

                toastDisplay.Show();

                App.isFromEditProfile = false; 
            }
            
            // Checks if user is already logged in previously
            if (!Settings.isLoggedIn.Value || Settings.isLoggedIn == null)
            {
                // Not logged in, navigate to landing page
                NavigationService.RemoveBackEntry();
                NavigationService.Navigate(new Uri("/LandingPage.xaml", UriKind.Relative));
                return;
            }

            if (App.isFromLandingPage)
            {
                // Clears back stack so user cannot go back to LandingPage(s)
                NavigationService.RemoveBackEntry();
                NavigationService.RemoveBackEntry();
                NavigationService.RemoveBackEntry();

                App.MetrocamService.AuthenticateCompleted += new RequestCompletedEventHandler(MetrocamService_AuthenticateCompleted);
                App.MetrocamService.Authenticate(Settings.username.Value, Settings.password.Value);
            }

            // User is logged in previously, check how this app got to main page
            if (App.isFromAppLaunch || App.isFromAppActivate)
            {
                // App is launched from start. We need to authenticate, then populate Popular, then populate Recent
                App.MetrocamService.AuthenticateCompleted += new RequestCompletedEventHandler(MetrocamService_AuthenticateCompleted);
                App.MetrocamService.Authenticate(Settings.username.Value, Settings.password.Value);
            }
            else if (App.isFromUploadPage)
            {
                // Reset back to false
                App.isFromUploadPage = false;

                // Clears back stack so user cannot go back to LandingPage(s)
                NavigationService.RemoveBackEntry();

                // Flag to refresh Recent
                isRefreshingRecent = true;

                // We need to authenticate, then populate Recent
                App.MetrocamService.AuthenticateCompleted += new RequestCompletedEventHandler(MetrocamService_AuthenticateCompleted);
                App.MetrocamService.Authenticate(Settings.username.Value, Settings.password.Value);
            }
            else if (App.pictureIsDeleted)
            {
                // User is navigated here after deleting a page
                if (MainContent.SelectedIndex == 0)
                {
                    // Refresh popular
                    FetchPopularPictures();
                    App.pictureIsDeleted = false;
                }
                else if (MainContent.SelectedIndex == 1)
                {
                    // Refresh recent
                    FetchRecentPictures();
                    App.pictureIsDeleted = false;
                }
                else
                {
                    // Refresh favourites
                    FetchFavoritedPictures();
                }
            }
            else
            {
                // User is navigated here from a page in the forward stack, so do nothing
                return;
            }
        }

        #region Authenticate

        void MetrocamService_AuthenticateCompleted(object sender, RequestCompletedEventArgs e)
        {
            // Unsubcribe
            App.MetrocamService.AuthenticateCompleted -= new RequestCompletedEventHandler(MetrocamService_AuthenticateCompleted);
            Settings.getUserSpecificSettings(App.MetrocamService.CurrentUser.ID);

            if ((App.isFromAppLaunch || App.isFromLandingPage) && App.MetrocamService.CurrentUser != null)
            {
                FetchPopularPictures();
                FetchRecentPictures();

                DateTime start = DateTime.Now;
                DateTime end = DateTime.Now;

                while ((end.Ticks - start.Ticks) < 50000)
                {
                    end = DateTime.Now;
                }

                GetUserConnectedAccounts();                
            }

            if (isRefreshingRecent)
            {
                FetchRecentPictures();
            }

            // Reset back to false
            App.isFromAppLaunch = false;
            App.isFromAppActivate = false;
            App.isFromLandingPage = false;
            isRefreshingRecent = false;
        }

        #endregion

        #region Popular Pivot Codebehind

        private void PopularPicture_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            //HubTileService.FreezeGroup("PopularTiles");

            Image tile = sender as Image;
            PictureInfo info = tile.DataContext as PictureInfo;

            NavigationService.Navigate(new Uri("/PictureView.xaml?id=" + info.ID + "&type=popular", UriKind.Relative));
        }

        private void PopularPicture_Loaded(object sender, RoutedEventArgs e)
        {
            if (LoadingMessage.Visibility == Visibility.Visible)
            {
                LoadingMessage.Visibility = Visibility.Collapsed;

                Dispatcher.BeginInvoke(() =>
                {
                    foreach (PictureInfo p in App.ContinuedPopularPictures)
                        App.PopularPictures.Add(p);
                });

                GlobalLoading.Instance.IsLoading = false;
            }
        }

        #endregion Popular Pivot Codebehind

        #region News Feed Codebehind

        private void recentPicture_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Image image = sender as Image;
            PictureInfo info = image.DataContext as PictureInfo;

            NavigationService.Navigate(new Uri("/PictureView.xaml?id=" + info.ID + "&type=recent&userid=" + info.User.ID, UriKind.Relative));
        }

        private void ViewUserDetail_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Image image = sender as Image;
            PictureInfo info = image.DataContext as PictureInfo;

            NavigationService.Navigate(new Uri("/UserDetailPage.xaml?id=" + info.ID + "&type=recent&userid=" + info.User.ID, UriKind.Relative));
        }

        private void ViewUserDetailFromUsername_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            TextBlock username = sender as TextBlock;
            PictureInfo info = username.DataContext as PictureInfo;

            NavigationService.Navigate(new Uri("/UserDetailPage.xaml?id=" + info.ID + "&type=recent&userid=" + info.User.ID, UriKind.Relative));
        }

        #endregion News Feed Codebehind

        #region Application Bar Codebehind

        #region Camera Button

        private void CameraButton_Click(object sender, EventArgs e)
        {
            if (GlobalLoading.Instance.IsLoading)
                return;

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

        /**
         * rotates picture appropriately
         */
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

        // Sign out
        private void SignoutBarIconButton_Click(object sender, EventArgs e)
        {
            // Display a confirmation message
            if (MessageBox.Show("Are you sure that you would like to sign out?", "Confirm", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                return;
            }
            
            // Reset all isolate storage Setting objects to default values
            Settings.logoutUser();

            this.DataContext = null;

            // Navigate to landing page
            while (NavigationService.CanGoBack)
            {
                NavigationService.RemoveBackEntry();
            }

            NavigationService.Navigate(new Uri("/LandingPage.xaml", UriKind.Relative));
        }
        #endregion

        #region ChoosePicture Button
        private void ChoosePicture_Click(object sender, EventArgs e)
        {
            if (GlobalLoading.Instance.IsLoading)
                return;

            PhotoChooserTask picker = new PhotoChooserTask();
            picker.ShowCamera = false;
            picker.Completed += new EventHandler<PhotoResult>(picker_Completed);

            picker.Show();
        }

        void picker_Completed(object sender, PhotoResult e)
        {
            tookPhoto = false;

            if (e.ChosenPhoto == null)
                return;

            e.ChosenPhoto.Position = 0;
            JpegInfo info = ExifReader.ReadJpeg(e.ChosenPhoto, e.OriginalFileName);

            /**
             * get image location
             */
            lat = info.GpsLatitude[0];
            lng = info.GpsLongitude[0];

            if (info.GpsLatitudeRef == ExifGpsLatitudeRef.South)
                lat = lat * -1;

            if (info.GpsLongitudeRef == ExifGpsLongitudeRef.West)
                lng = lng * -1;
            ///////////////////////

            bmp.SetSource(e.ChosenPhoto);
            captured.Source = bmp;

            Dispatcher.BeginInvoke(() =>
            {
                /**
                 * determine if picture is Portrait or Landscape
                 */
                if ((Convert.ToDouble(bmp.PixelHeight) / Convert.ToDouble(bmp.PixelWidth)) < 1)
                {
                    NavigationService.Navigate(new Uri("/CropPageLandscapeOrientation.xaml", UriKind.Relative));
                }
                else
                {
                    NavigationService.Navigate(new Uri("/CropPage.xaml", UriKind.Relative));
                }
            });
        }

        #endregion

        public bool isRefreshingRecent = false;
        private void Refresh_Click(object sender, EventArgs e)
        {
            if (GlobalLoading.Instance.IsLoading)
                return;

            if (MainContent.SelectedIndex == 0)
            {
                isRefreshingPopular = true;
                FetchPopularPictures();
                // refresh popular
                /*App.MetrocamService.FetchPopularNewsFeedCompleted += new RequestCompletedEventHandler(MetrocamService_FetchPopularNewsFeedCompleted);
                App.MetrocamService.FetchPopularNewsFeed();*/
            }
            else if (MainContent.SelectedIndex == 1)
            {
                // authenticate and refresh recent
                isRefreshingRecent = true;     
                FetchRecentPictures();
                /*App.MetrocamService.AuthenticateCompleted += new RequestCompletedEventHandler(MetrocamService_AuthenticateCompleted);
                App.MetrocamService.Authenticate(Settings.username.Value, Settings.password.Value);*/
            }
        }

        #region Menu Options

        private void Settings_Click(object sender, EventArgs e)
        {
            if (GlobalLoading.Instance.IsLoading)
                return;

            NavigationService.Navigate(new Uri("/SettingsPage.xaml", UriKind.Relative));
        }

        private void UserSearch_Click(object sender, EventArgs e)
        {
            if (GlobalLoading.Instance.IsLoading)
                return;

            NavigationService.Navigate(new Uri("/UserSearch.xaml", UriKind.Relative));
        }

        private void EditProfile_Click(object sender, EventArgs e)
        {
            if (GlobalLoading.Instance.IsLoading)
                return;

            NavigationService.Navigate(new Uri("/UserDetailPage.xaml?userid=" + App.MetrocamService.CurrentUser.ID + "&type=current&id=blah", UriKind.Relative));
        }

        private void About_Click(object sender, EventArgs e)
        {
            if (GlobalLoading.Instance.IsLoading)
                return;

            NavigationService.Navigate(new Uri("/AboutPage.xaml", UriKind.Relative));
        }

        #endregion

        #endregion Application Bar Codebehind

        #region GeoLocation

        // Set up location
        public static void SetUpLocation()
        {
            if (watcher == null)
            {
                watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.High); // using high accuracy
                watcher.MovementThreshold = 20; // use MovementThreshold to ignore noise in the signal
                watcher.StatusChanged += new EventHandler<GeoPositionStatusChangedEventArgs>(watcher_StatusChanged);
            }
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

        private void MainContent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MainContent.SelectedIndex == 0 && App.MetrocamService.CurrentUser != null)
            {
                // If Recent pivot item is selected
                // isRefreshingPopular = true;
                // FetchPopularPictures();
            }
            else if (MainContent.SelectedIndex == 2 && App.MetrocamService.CurrentUser != null && App.FavoritedUserPictures.Count == 0)
            {
                if (App.FavoritedUserPictures.Count != 0)
                {
                    FavoritesLoadingMessage.Visibility = Visibility.Collapsed;
                }

                // If Favorites pivot item is selected
                GlobalLoading.Instance.IsLoading = true;
                FetchFavoritedPictures();
            }
        }
        #endregion

        #region FetchPopular
        public void FetchPopularPictures()
        {
            App.MetrocamService.FetchPopularNewsFeedCompleted += new RequestCompletedEventHandler(MetrocamService_FetchPopularNewsFeedCompleted);

            if (GlobalLoading.Instance.IsLoading == false)
            {
                GlobalLoading.Instance.IsLoading = true;
            }

            GlobalLoading.Instance.Text = "Fetching pictures...";
            App.MetrocamService.FetchPopularNewsFeed();
        }

        Boolean isRefreshingPopular = false;
        void MetrocamService_FetchPopularNewsFeedCompleted(object sender, MobileClientLibrary.RequestCompletedEventArgs e)
        {
            endPop = DateTime.Now;
            GlobalLoading.Instance.Text = "";
            App.MetrocamService.FetchPopularNewsFeedCompleted -= MetrocamService_FetchPopularNewsFeedCompleted;
            App.PopularPictures.Clear();

            int count = 0;
            foreach (PictureInfo p in e.Data as List<PictureInfo>)
            {
                if (count == 24)
                    break;

                // changes to local time
                p.FriendlyCreatedDate = TimeZoneInfo.ConvertTime(p.FriendlyCreatedDate, TimeZoneInfo.Local);

                if (!isRefreshingPopular)
                {
                    if (App.PopularPictures.Count < 3)
                    {
                        App.PopularPictures.Add(p);
                    }
                    else
                    {
                        App.ContinuedPopularPictures.Add(p);
                    }
                }
                else
                {
                    App.PopularPictures.Add(p);
                }

                count++;
            }

            isRefreshingPopular = false;

            if (GlobalLoading.Instance.IsLoading)
                GlobalLoading.Instance.IsLoading = false;
        }

        #endregion

        #region FetchRecent

        public void FetchRecentPictures()
        {
            App.MetrocamService.FetchNewsFeedCompleted += new RequestCompletedEventHandler(MetrocamService_FetchNewsFeedCompleted);

            if (GlobalLoading.Instance.IsLoading == false) 
                GlobalLoading.Instance.IsLoading = true;

            GlobalLoading.Instance.Text = "Fetching pictures...";

            App.MetrocamService.FetchNewsFeed();
        }

        void MetrocamService_FetchNewsFeedCompleted(object sender, RequestCompletedEventArgs e)
        {
            GlobalLoading.Instance.Text = "";
            endRec = DateTime.Now;
            App.MetrocamService.FetchNewsFeedCompleted -= MetrocamService_FetchNewsFeedCompleted;
            App.RecentPictures.Clear();
            App.ContinuedRecentPictures.Clear();

            PictureInfo firstPicture = null;

            foreach (PictureInfo p in e.Data as List<PictureInfo>)
            {
                // changes to local time
                p.FriendlyCreatedDate = TimeZoneInfo.ConvertTime(p.FriendlyCreatedDate, TimeZoneInfo.Local);

                if (App.RecentPictures.Count < 5)
                {
                    if (App.RecentPictures.Count == 0)
                    {
                        firstPicture = p;
                    }
                    // Put only 10 PictureInfo objects in RecentPictures
                    App.RecentPictures.Add(p);
                }
                else
                {
                    // Put the rest of the PictureInfo objects into ContinuedRecentPictures
                    App.ContinuedRecentPictures.Add(p);
                }
            }

            if (recentPictures.ItemsSource == null)
            {
                Dispatcher.BeginInvoke(() =>
                        recentPictures.DataContext = App.RecentPictures);
            }

            if (GlobalLoading.Instance.IsLoading == true)
                GlobalLoading.Instance.IsLoading = false;
        }

        #endregion

        #region FetchFavorites

        public void FetchFavoritedPictures()
        {
            App.MetrocamService.FetchUserFavoritedPicturesCompleted += new RequestCompletedEventHandler(MetrocamService_FetchUserFavoritedPicturesCompleted);

            if (GlobalLoading.Instance.IsLoading == false)
                GlobalLoading.Instance.IsLoading = true;

            GlobalLoading.Instance.Text = "Fetching your favorite pictures...";
            
            App.MetrocamService.FetchUserFavoritedPictures(App.MetrocamService.CurrentUser.ID);
        }

        void MetrocamService_FetchUserFavoritedPicturesCompleted(object sender, RequestCompletedEventArgs e)
        {
            GlobalLoading.Instance.Text = "";
            App.MetrocamService.FetchUserFavoritedPicturesCompleted -= MetrocamService_FetchUserFavoritedPicturesCompleted;
            App.FavoritedUserPictures.Clear();

            Dispatcher.BeginInvoke(() =>
                        FavoritePictures.DataContext = App.FavoritedUserPictures);

            foreach (PictureInfo p in e.Data as List<PictureInfo>)
            {
                // changes to local time
                p.FriendlyCreatedDate = TimeZoneInfo.ConvertTime(p.FriendlyCreatedDate, TimeZoneInfo.Local);

                App.FavoritedUserPictures.Add(p);
            }

            if (App.FavoritedUserPictures.Count == 0)
                // Set loading message to visible if there are not favorited pictures
                this.FavoritesLoadingMessage.Visibility = Visibility.Visible;
            else
            {
                this.FavoritesLoadingMessage.Visibility = Visibility.Collapsed;
            }

            if (GlobalLoading.Instance.IsLoading == true)
                GlobalLoading.Instance.IsLoading = false;
        }

        private void FavoritedPicture_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Image tile = sender as Image;
            PictureInfo info = tile.DataContext as PictureInfo;

            NavigationService.Navigate(new Uri("/PictureView.xaml?id=" + info.ID + "&type=favorite", UriKind.Relative));
        }

        #endregion

        #region UserConnectedAccounts

        private void GetUserConnectedAccounts()
        {
            App.MetrocamService.FetchUserConnectedAccountsByUserIDCompleted += new RequestCompletedEventHandler(MetrocamService_FetchUserConnectedAccountsByUserIDCompleted);
            App.MetrocamService.FetchUserConnectedAccountsByUserID(App.MetrocamService.CurrentUser.ID);
        }

        private Boolean UcaHasCompleted = false;
        void MetrocamService_FetchUserConnectedAccountsByUserIDCompleted(object sender, RequestCompletedEventArgs e)
        {
            List<UserConnectedAccount> UCAs = e.Data as List<UserConnectedAccount>;

            foreach (UserConnectedAccount uca in UCAs)
            {
                if (uca.AccountName.Equals("Twitter"))
                {
                    TwitterSecret = uca.ClientSecret;
                    TwitterToken = uca.ClientToken;

                    TwitterService twitter = new TwitterService(TwitterSettings.ConsumerKey, TwitterSettings.ConsumerKeySecret);
                    twitter.AuthenticateWith(TwitterToken, TwitterSecret);
                    twitter.VerifyCredentials((user, response) =>
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            // yay! user hasn't revoked our access
                            Settings.twitterAuth.Value = true;
                        }
                        else
                        {
                            Settings.twitterAuth.Value = false;
                            Settings.twitterDefault.Value = false;

                            // Delete this UCA since it's no longer valid
                            //App.MetrocamService.DeleteUserConnectedAccountCompleted += new RequestCompletedEventHandler(MetrocamService_DeleteUserConnectedAccountCompleted);
                            //App.MetrocamService.DeleteUserConnectedAccount();
                        }
                    });
                }
            }
        }

        void MetrocamService_DeleteUserConnectedAccountCompleted(object sender, RequestCompletedEventArgs e)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ContextMenu
        private void Image_Loaded(object sender, RoutedEventArgs e)
        {
            Image image = sender as Image;
            PictureInfo info = image.DataContext as PictureInfo;

            if (info.User.ID.Equals(App.MetrocamService.CurrentUser.ID))
            {
                ContextMenu CM = PopulateMyPictureMenuItems(info);
                ContextMenuService.SetContextMenu(image, CM);
            }
            else
            {
                ContextMenu CM = PopulateNotMyPictureMenuItems();
                ContextMenuService.SetContextMenu(image, CM);
            }
        }

        private ContextMenu PopulateMyPictureMenuItems(PictureInfo info)
        {
            MenuItem ProfilePicture = new MenuItem();
            ProfilePicture.Header = "make profile picture";
            ProfilePicture.Tap += new EventHandler<System.Windows.Input.GestureEventArgs>(ProfilePicture_Tap);

            MenuItem Favorite = new MenuItem();
            Favorite.Header = "favorite";
            Favorite.Tap += new EventHandler<System.Windows.Input.GestureEventArgs>(Favorite_Tap);

            MenuItem Save = new MenuItem();
            Save.Header = "save";
            Save.Tap += new EventHandler<System.Windows.Input.GestureEventArgs>(Save_Tap);

            MenuItem DeletePicture = new MenuItem();
            DeletePicture.Header = "delete";
            DeletePicture.Tap += new EventHandler<System.Windows.Input.GestureEventArgs>(DeletePicture_Tap);

            ContextMenu CM = new ContextMenu();

            //CM.Items.Add(Favorite);
            CM.Items.Add(Save);
            if (!info.ID.Equals(App.MetrocamService.CurrentUser.ProfilePicture.ID))
                CM.Items.Add(ProfilePicture);
            //CM.Items.Add(DeletePicture);

            return CM;
        }

        void Save_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            MenuItem item = sender as MenuItem;
            PictureInfo info = item.DataContext as PictureInfo;

            String file = info.User.Username + info.ID + ".jpg";

            var myStore = IsolatedStorageFile.GetUserStoreForApplication();
            IsolatedStorageFileStream myFileStream = myStore.CreateFile(file);

            BitmapImage b = new BitmapImage(new Uri(info.MediumURL, UriKind.Absolute));
            b.CreateOptions = BitmapCreateOptions.None;

            Image i = new Image();
            i.Source = b;

            WriteableBitmap bitmap = new WriteableBitmap((BitmapSource)i.Source);

            bitmap.SaveJpeg(myFileStream, bitmap.PixelWidth, bitmap.PixelHeight, 0, 100);
            myFileStream.Close();

            myFileStream = myStore.OpenFile(file, System.IO.FileMode.Open, System.IO.FileAccess.Read);

            var lib = new MediaLibrary();
            lib.SavePicture(file, myFileStream);

            toastDisplay = GlobalToastPrompt.CreateToastPrompt(
                "Success!",
                "Picture has been saved to your media library.");

            toastDisplay.Show();
        }

        private ContextMenu PopulateNotMyPictureMenuItems()
        {
            ContextMenu CM = new ContextMenu();

            MenuItem Favorite = new MenuItem();
            Favorite.Header = "favorite";
            Favorite.Tap += new EventHandler<System.Windows.Input.GestureEventArgs>(Favorite_Tap);

            MenuItem Flag = new MenuItem();
            Flag.Header = "flag";
            Flag.Tap += new EventHandler<System.Windows.Input.GestureEventArgs>(Flag_Tap);

            MenuItem Save = new MenuItem();
            Save.Header = "save";
            Save.Tap += new EventHandler<System.Windows.Input.GestureEventArgs>(Save_Tap);

            //CM.Items.Add(Favorite);
            CM.Items.Add(Save);
            //CM.Items.Add(Flag);

            return CM;
        }

        void Cancel_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {

        }

        void Flag_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            toastDisplay = GlobalToastPrompt.CreateToastPrompt(
                "Success!",
                "Picture has been flagged for review.");

            toastDisplay.Show();
        }

        void Favorite_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            MenuItem item = sender as MenuItem;
            PictureInfo info = item.DataContext as PictureInfo;

            FavoritedPicture data = new FavoritedPicture();
            data.PictureID = info.ID;
            data.UserID = App.MetrocamService.CurrentUser.ID;

            if ((from pic in App.FavoritedUserPictures where pic.ID.Equals(info.ID) select pic).SingleOrDefault() != null)
            {
                return;
            }

            App.FavoritedUserPictures.Add(info);

            App.MetrocamService.CreateFavoritedPictureCompleted += new RequestCompletedEventHandler(MetrocamService_CreateFavoritedPictureCompleted);
            App.MetrocamService.CreateFavoritedPicture(data);
        }

        void MetrocamService_CreateFavoritedPictureCompleted(object sender, RequestCompletedEventArgs e)
        {
            App.MetrocamService.CreateFavoritedPictureCompleted -= MetrocamService_CreateFavoritedPictureCompleted;

            toastDisplay = GlobalToastPrompt.CreateToastPrompt(
                "Success!",
                "Picture has been added to your favorites.");

            toastDisplay.Show();
        }

        void DeletePicture_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {

        }

        void ProfilePicture_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            MenuItem cm = sender as MenuItem;
            PictureInfo info = cm.DataContext as PictureInfo;

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
            updatedData.ProfilePictureID = info.ID;

            App.MetrocamService.UpdateUserCompleted += new RequestCompletedEventHandler(MetrocamService_UpdateUserCompleted);

            if (GlobalLoading.Instance.IsLoading == false) 
                GlobalLoading.Instance.IsLoading = true;

            App.MetrocamService.UpdateUser(updatedData);
        }

        void MetrocamService_UpdateUserCompleted(object sender, RequestCompletedEventArgs e)
        {
            App.MetrocamService.UpdateUserCompleted -= MetrocamService_UpdateUserCompleted;

            if (GlobalLoading.Instance.IsLoading == true)
                GlobalLoading.Instance.IsLoading = false;

            toastDisplay = GlobalToastPrompt.CreateToastPrompt(
                "Success!",
                "Your profile picture has been updated.");

            toastDisplay.Show();
        }

        #endregion

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            if (GlobalLoading.Instance.IsLoading)
                GlobalLoading.Instance.IsLoading = false;
        }
    }
}