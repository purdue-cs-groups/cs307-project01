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

using JeffWilcox.FourthAndMayor;
using MobileClientLibrary;
using MobileClientLibrary.Models;
using System.Collections;
using System.Collections.ObjectModel;

namespace MetrocamPan
{
    public partial class BrowsePage : PhoneApplicationPage
    {
        // This is the collection for pictures specific to this page
        public static ObservableCollection<PictureInfo> PopularPictures;

        public BrowsePage()
        {
            InitializeComponent();

            PopularPictures = new ObservableCollection<PictureInfo>();

            this.Loaded += new RoutedEventHandler(BrowsePage_Loaded);
        }

        // Called when the page is fully loaded with objects
        void BrowsePage_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.PopularHubTiles.ItemsSource == null)
            {
                GlobalLoading.Instance.IsLoading = true;
                Dispatcher.BeginInvoke(() =>
                    this.PopularHubTiles.DataContext = PopularPictures);
            }
        }

        // Called when this page becomes the active frame
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            this.FetchPopularPictures();
        }

        // Initiates fetching of popular pictures
        private void FetchPopularPictures()
        {
            App.MetrocamService.FetchPopularNewsFeedCompleted += new RequestCompletedEventHandler(MetrocamService_FetchPopularNewsFeedCompleted);
            App.MetrocamService.FetchPopularNewsFeed();
        }

        void MetrocamService_FetchPopularNewsFeedCompleted(object sender, RequestCompletedEventArgs e)
        {
            App.MetrocamService.FetchPopularNewsFeedCompleted -= MetrocamService_FetchPopularNewsFeedCompleted;

            PopularPictures.Clear();

            foreach (PictureInfo p in e.Data as List<PictureInfo>)
            {
                if (PopularPictures.Count == 24)
                    continue;

                PopularPictures.Add(p);
            }
        }

        private void ApplicationBarMenuItem_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/AboutPage.xaml", UriKind.Relative));
        }

        // Called when a hubtile is loaded. We disable loading message and set it as visible
        private void HubTile_Loaded(object sender, RoutedEventArgs e)
        {
            if (LoadingMessage.Visibility == Visibility.Visible)
            {
                GlobalLoading.Instance.IsLoading = false;
                LoadingMessage.Visibility = Visibility.Collapsed;
            }

            HubTile currentTile = sender as HubTile;
            currentTile.Visibility = Visibility.Visible;
        }
    }
}