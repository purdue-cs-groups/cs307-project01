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
using MobileClientLibrary.Models;
using JeffWilcox.FourthAndMayor;

namespace MetrocamPan
{
    public partial class Page1 : PhoneApplicationPage
    {
        public Page1()
        {
            InitializeComponent();
        }

        protected virtual void OnNavigatedTo(NavigationEventArgs e)
        {
            noresults.Visibility = System.Windows.Visibility.Collapsed;
            searchResults.Visibility = System.Windows.Visibility.Collapsed;
            SearchResults.Clear();
        }

        public static ObservableCollection<UserInfo> SearchResults = new ObservableCollection<UserInfo>();
        public static List<UserInfo> results;
        private void searchbutton_Click(object sender, EventArgs e)
        {
            searchResults.Visibility = System.Windows.Visibility.Collapsed;
            noresults.Visibility = System.Windows.Visibility.Collapsed;
            App.MetrocamService.SearchUsersCompleted += new MobileClientLibrary.RequestCompletedEventHandler(MetrocamService_SearchUsersCompleted);
            GlobalLoading.Instance.IsLoading = true;
            App.MetrocamService.SearchUsers(this.searchterms.Text);
        }


        void MetrocamService_SearchUsersCompleted(object sender, MobileClientLibrary.RequestCompletedEventArgs e)
        {
            App.MetrocamService.SearchUsersCompleted -= MetrocamService_SearchUsersCompleted;
            GlobalLoading.Instance.IsLoading = false;
            SearchResults.Clear();

            results = e.Data as List<UserInfo>;
            foreach (UserInfo u in results)
            {
                if (u.ProfilePicture == null)
                {
                    u.ProfilePicture = new Picture();
                    u.ProfilePicture.MediumURL = "Images/dunsmore.png";
                }

                SearchResults.Add(u);
            }

            if (results.Count == 0)
            {
                noresults.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                searchResults.ItemsSource = SearchResults;
                searchResults.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void searchterms_GotFocus(object sender, RoutedEventArgs e)
        {
            searchResults.Visibility = System.Windows.Visibility.Collapsed;
            noresults.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void ViewUserDetailFromUsername_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            TextBlock username = sender as TextBlock;
            UserInfo info = username.DataContext as UserInfo;

            NavigationService.Navigate(new Uri("/UserDetailPage.xaml?id=" + info.ID + "&type=search", UriKind.Relative));
        }

        private void ViewUserDetailFromPicture_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Image image = sender as Image;
            UserInfo info = image.DataContext as UserInfo;

            NavigationService.Navigate(new Uri("/UserDetailPage.xaml?id=" + info.ID + "&type=search", UriKind.Relative));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            GlobalLoading.Instance.IsLoading = false;
        }
    }
}