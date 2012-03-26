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
    public partial class Page1 : PhoneApplicationPage
    {
        public Page1()
        {
            InitializeComponent();
        }

        public static ObservableCollection<User> SearchResults = new ObservableCollection<User>();
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            SearchResults.Clear();

            User u = new User();
            u.Name = "Joe Martella";
            u.Username = "martellaj";
            SearchResults.Add(u);

            User u2 = new User();
            u2.Name = "Matt McCormick";
            u2.Username = "mbmccormick";
            SearchResults.Add(u2);

            User u3 = new User();
            u3.Name = "James Ma";
            u3.Username = "jimmymama";
            SearchResults.Add(u3);

            User u4 = new User();
            u4.Name = "Josh William";
            u4.Username = "jdawg";
            SearchResults.Add(u4);

            User u5 = new User();
            u5.Name = "Jeremiah Theurer";
            u5.Username = "jerryThizzle";
            SearchResults.Add(u5);

            searchResults.ItemsSource = SearchResults;
        }
    }
}