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

        private void searchbutton_Click(object sender, RoutedEventArgs e)
        {
            //here we want to take the information from the search box, and search it against a list of users
            //we will finally get the users from the webservice, but initially we will have a local group of users

            //this will eventually be a search for actual users from the database...right now it's fake!
            switch (usernameInput.Text) { 
                case "jo":
                case "Jo":
                    usernameInput.Text = "Search";
                    JoePic.Visibility = System.Windows.Visibility.Visible;
                    joeusername.Visibility = System.Windows.Visibility.Visible;
                    JoshPic.Visibility = System.Windows.Visibility.Visible;
                    joshusername.Visibility = System.Windows.Visibility.Visible;
                break;
                case "Joe":
                case "Martella":
                case "Joe Martella":
                case "joe martella":
                case "joe":
                case "martella":  
                    //display a basic user page
                    usernameInput.Text = "Search";
                    JoePic.Visibility = System.Windows.Visibility.Visible;
                    joeusername.Visibility = System.Windows.Visibility.Visible;
                     //display here the results for Joe
                    break;
                default: //display no results found
                    noresults.Visibility = System.Windows.Visibility.Visible;
                    usernameInput.Text = "Search";
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
            noresults.Visibility = System.Windows.Visibility.Collapsed;
            JoePic.Visibility = System.Windows.Visibility.Collapsed;
            joeusername.Visibility = System.Windows.Visibility.Collapsed;
            JoshPic.Visibility = System.Windows.Visibility.Collapsed;
            joshusername.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void usernameInput_LostFocus(object sender, RoutedEventArgs e) {
            if (usernameInput.Text == String.Empty)
            {
                SolidColorBrush Brush2 = new SolidColorBrush();
                Brush2.Color = Colors.Gray;
                usernameInput.Foreground = Brush2;
                        
                usernameInput.Text = "Search";
                noresults.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void ViewUserDetailTap(object sender, System.Windows.Input.GestureEventArgs e) {
            JoePic.Visibility = System.Windows.Visibility.Collapsed;
            joeusername.Visibility = System.Windows.Visibility.Collapsed;
            JoshPic.Visibility = System.Windows.Visibility.Collapsed;
            joshusername.Visibility = System.Windows.Visibility.Collapsed;
            NavigationService.Navigate(new Uri("/UserDetailPage.xaml", UriKind.Relative));
        }
    }
}