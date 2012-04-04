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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using MobileClientLibrary;
using MobileClientLibrary.Models;

using System.Collections;
using System.Collections.ObjectModel;
using System.Windows.Threading;

namespace MetrocamPan
{
    public partial class App : Application
    {
        public static String APIKey = "4f7124f25ad9850a042a5f2d";
        public static WebServiceClient MetrocamService = null;
        
        public PhoneApplicationFrame RootFrame { get; private set; }

        /// <summary>
        /// Constructor for the Application object.
        /// </summary>
        public App()
        {
            // Global handler for uncaught exceptions. 
            UnhandledException += Application_UnhandledException;

            // Standard Silverlight initialization
            InitializeComponent();

            // Phone-specific initialization
            InitializePhoneApplication();

            MetrocamService = new WebServiceClient(APIKey);
        }

        // Code to execute when the application is launching (eg, from Start)
        // This code will not execute when the application is reactivated
        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
            if (Settings.isLoggedIn.Value)
            {
                populatePopularPictures();
                populateRecentPictures();
            }
        }

        #region FetchPopular
        public static ObservableCollection<PictureInfo> PopularPictures = new ObservableCollection<PictureInfo>();

        public void populatePopularPictures()
        {
            App.MetrocamService.FetchPopularNewsFeedCompleted += new MobileClientLibrary.RequestCompletedEventHandler(MetrocamService_FetchPopularNewsFeedCompleted);
            App.MetrocamService.FetchPopularNewsFeed();
        }

        void MetrocamService_FetchPopularNewsFeedCompleted(object sender, MobileClientLibrary.RequestCompletedEventArgs e)
        {
            PopularPictures.Clear();

            foreach (PictureInfo p in e.Data as List<PictureInfo>)
            {
                if (PopularPictures.Count == 24)
                    continue;

                PopularPictures.Add(p);
            }
        }
        #endregion

        #region FetchRecent

        public static ObservableCollection<Picture> RecentPictures = new ObservableCollection<Picture>();

        public void populateRecentPictures()
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
            RecentPictures.Clear();

            foreach (Picture p in e.Data as List<Picture>)
            {
                if (RecentPictures.Count == 10)
                    break;

                RecentPictures.Add(p);
            }
        }

        #endregion

        // Code to execute when the application is activated (brought to foreground)
        // This code will not execute when the application is first launched
        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
        }

        // Code to execute when the application is deactivated (sent to background)
        // This code will not execute when the application is closing
        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
        }

        // Code to execute when the application is closing (eg, user hit Back)
        // This code will not execute when the application is deactivated
        private void Application_Closing(object sender, ClosingEventArgs e)
        {
        }

        // Code to execute if a navigation fails
        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // A navigation has failed; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
        }

        // Code to execute on Unhandled Exceptions
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            // Catches this error
            if (e.ExceptionObject is UnauthorizedAccessException)
            {
                MessageBox.Show(e.ExceptionObject.Message, "Error", MessageBoxButton.OK);
                e.Handled = true;
                return;
            }

            if (System.Diagnostics.Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
        }

        #region Phone application initialization

        // Avoid double-initialization
        private bool phoneApplicationInitialized = false;

        // Do not add any additional code to this method
        private void InitializePhoneApplication()
        {
            if (phoneApplicationInitialized)
                return;

            // Create the frame but don't set it as RootVisual yet; this allows the splash
            // screen to remain active until the application is ready to render.
            RootFrame = new TransitionFrame();
            RootFrame.Navigated += CompleteInitializePhoneApplication;

            // Handle navigation failures
            RootFrame.NavigationFailed += RootFrame_NavigationFailed;

            // Ensure we don't initialize again
            phoneApplicationInitialized = true;
        }

        // Do not add any additional code to this method
        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            // Set the root visual to allow the application to render
            if (RootVisual != RootFrame)
                RootVisual = RootFrame;

            // Remove this handler since it is no longer needed
            RootFrame.Navigated -= CompleteInitializePhoneApplication;
        }

        #endregion
    }
}