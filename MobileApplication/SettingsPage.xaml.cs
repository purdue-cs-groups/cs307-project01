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
    public partial class SettingsPage : PhoneApplicationPage
    {

        System.Windows.Controls.Primitives.Popup popup;

        public SettingsPage()
        {
            InitializeComponent();
        }

        private void connectaccounts_Click(object sender, RoutedEventArgs e)
        {
            //enter twitter information
            //user control in conjunction with twitter API
        }

        private void readprivcy_Click(object sender, RoutedEventArgs e)
        {           
            string parameter = "privacy";
            NavigationService.Navigate(new Uri(String.Format("/AboutPage.xaml?parameter={0}",parameter), UriKind.Relative));
        }

        private void About_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/AboutPage.xaml", UriKind.Relative));
        }

        private void Edit_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/EditProfile.xaml", UriKind.Relative));
        }

        private void Find_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/UserSearch.xaml", UriKind.Relative));
        }

        private void SignOut_Click(object sender, EventArgs e)
        {
            Settings.isLoggedIn.Value = false;
            NavigationService.Navigate(new Uri("/LandingPage.xaml", UriKind.Relative));
        }
    }
}