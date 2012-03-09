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

namespace WinstagramPan
{
    public partial class Page1 : PhoneApplicationPage
    {
        public Page1()
        {
            InitializeComponent();
        }

        private void searchbutton_Click(object sender, RoutedEventArgs e)
        {
            //here we want to take the information from the search box, and search it against a list of users
            //we will finally get the users from the webservice, but initially we will have a local group of users

            //this will eventuall be a search for actual users from the database
            switch (usernameInput.Text) { 
                case "Joe":
                case "Martella":
                case "Joe Martella":
                case "joe martella":
                case "joe":
                case "martella":  
                    //display a basic user page
                    NavigationService.Navigate(new Uri("/UserDetailPage.xaml", UriKind.Relative));
                //display here the results for Joe
                    break;
                default: //display no results found
                    noresults.Visibility = System.Windows.Visibility.Visible;
                    break;

            }
        }

        private void usernameInput_GotFocus(object sender, RoutedEventArgs e) {
            if (usernameInput.Text == "Search") {
                usernameInput.Text = "";
                SolidColorBrush Brush1 = new SolidColorBrush();
                Brush1.Color = Colors.Black;
                usernameInput.Foreground = Brush1;
            }
        }

        private void usernameInput_LostFocus(object sender, RoutedEventArgs e) {
            if (usernameInput.Text == String.Empty)
            {
                usernameInput.Text = "Search";
            }
        }
    }
}