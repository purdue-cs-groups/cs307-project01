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
using MetrocamPan.Helpers;
using JeffWilcox.FourthAndMayor;

namespace MetrocamPan
{
    public partial class App : Application
    {
        public static String APIKey = "4f7124f25ad9850a042a5f2d";
        public static WebServiceClient MetrocamService = null;

        public static ObservableCollection<PictureInfo> PopularPictures = new ObservableCollection<PictureInfo>();
        public static ObservableCollection<PictureInfo> ContinuedPopularPictures = new ObservableCollection<PictureInfo>();

        public static ObservableCollection<PictureInfo> RecentPictures = new ObservableCollection<PictureInfo>();
        public static ObservableCollection<PictureInfo> ContinuedRecentPictures = new ObservableCollection<PictureInfo>();

        public static ObservableCollection<PictureInfo> UserPictures = new ObservableCollection<PictureInfo>();
        public static ObservableCollection<PictureInfo> FavoritedUserPictures = new ObservableCollection<PictureInfo>();

        public static bool isFromLandingPage = false;
        public static bool isFromAppLaunch = false;
        public static bool isFromAppActivate = false;
        public static bool isFromUploadPage = false;
        
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

            // Initialize Web Service Client
            MetrocamService = new WebServiceClient(APIKey);
        }

        // Code to execute when the application is launching (eg, from Start)
        // This code will not execute when the application is reactivated
        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
            isFromAppLaunch = true;
        }

        // Code to execute when the application is activated (brought to foreground)
        // This code will not execute when the application is first launched
        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
            try
            {
                Settings.getUserSpecificSettings(MetrocamService.CurrentUser.ID);
            }
            catch (Exception ex)
            {
                // may get caught if user hasn't authenticated yet
            }

            isFromAppActivate = true;
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

            if (e.ExceptionObject is WebException)
            {
                //MessageBox.Show(e.ExceptionObject.Message, "Error", MessageBoxButton.OK);
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

            GlobalLoading.Instance.Initialize(RootFrame);

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