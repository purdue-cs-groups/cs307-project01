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

        public static ObservableCollection<User> SearchResults = new ObservableCollection<User>();
        public static List<UserInfo> results;
        private void searchbutton_Click(object sender, EventArgs e)
        {
            searchResults.Visibility = System.Windows.Visibility.Collapsed;
            noresults.Visibility = System.Windows.Visibility.Collapsed;
            App.MetrocamService.SearchUsersCompleted +=new MobileClientLibrary.RequestCompletedEventHandler(MetrocamService_SearchUsersCompleted);
            App.MetrocamService.SearchUsers(this.searchterms.Text);
        }


        void  MetrocamService_SearchUsersCompleted(object sender, MobileClientLibrary.RequestCompletedEventArgs e)
        {
            App.MetrocamService.SearchUsersCompleted -= MetrocamService_SearchUsersCompleted;
            SearchResults.Clear();

            //link to profile
            //profile picture


            results = e.Data as List<UserInfo>;
            foreach (UserInfo uio in results) {
                User result = new User();
                result.Name = uio.Name;
                result.Username = uio.Username;
                SearchResults.Add(result);
            }

            if (results.Count == 0)
            {
                noresults.Visibility = System.Windows.Visibility.Visible;
            }
            else {
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
            MessageBox.Show("This should navigate to the user detail page.  Butttttt it doesn't.  Just FYI.");
         /*   TextBlock username = sender as TextBlock;
            UserInfo info = username.DataContext as UserInfo;

            if (info.ID == null)
            {
                MessageBox.Show("the user info object is somehow wrong\n");
            }
            else
            {
                NavigationService.Navigate(new Uri("/UserDetailPage.xaml?id=" + info.ID + "&type=recent", UriKind.Relative));
            }*/
       }
    }
}