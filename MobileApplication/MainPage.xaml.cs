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

using WinstagramPan.Models;

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
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
        }


        /********************************************************
         * 
         * 
         *                 PRETTY MUCH ALL THE DATA
         * 
         * 
         ********************************************************/
        public static String APIToken;
        public static ObservableCollection<Picture> RecentPictures = new ObservableCollection<Picture>();
        public static ObservableCollection<Picture> PopularPictures = new ObservableCollection<Picture>();


        /********************************************************
         * 
         * 
         *                 POPULAR Pivot Codebehind
         * 
         * 
         ********************************************************/
        public static HubTile selectedPicture;
        private void hubTilePictureTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            PictureView.SenderPage = 1;

            selectedPicture = (HubTile) sender;
            NavigationService.Navigate(new Uri("/PictureView.xaml", UriKind.Relative));
        }

        private void populatePopularPicturesHub()
        {
            String tile;
            String tileName = "popTile";
            int tileNumber = 1;

            foreach (Picture p in PopularPictures)
            {
                // get HubTile object
                tile = tileName + tileNumber;
                HubTile currentHub =  FindName(tile) as HubTile;

                // populate HubTile with popular photo metadata
                currentHub.Source = p.Photo.Source;
            }
        }

        /********************************************************
         * 
         * 
         *               NEWS FEED Pivot Codebehind
         * 
         * 
         ********************************************************/
        private void populateRecentPictures()
        {
            // first pic
            Picture p1 = new Picture();
            p1.Username = "Joe";
            p1.Caption = "It's Joe!";
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
    }
}