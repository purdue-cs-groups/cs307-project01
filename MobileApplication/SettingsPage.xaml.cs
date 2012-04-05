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

 

namespace MetrocamPan
{
    public partial class SettingsPage : PhoneApplicationPage
    {
        public SettingsPage()
        {
            InitializeComponent();

            if (Settings.locationService.Value)
                lToggle.IsChecked = true;
            if (Settings.saveOriginal.Value)
                oToggle.IsChecked = true;
            if (Settings.saveEdited.Value)
                eToggle.IsChecked = true;
        }

        private void originalCheck(object sender, RoutedEventArgs e)
        {
            Settings.saveOriginal.Value = true;
        }

        private void originalUncheck(object sender, RoutedEventArgs e)
        {
            Settings.saveOriginal.Value = false;
        }

        private void editedCheck(object sender, RoutedEventArgs e)
        {
            Settings.saveEdited.Value = true;
        }

        private void editedUncheck(object sender, RoutedEventArgs e)
        {
            Settings.saveEdited.Value = false;
        }

        private void locCheck(object sender, RoutedEventArgs e)
        {
            Settings.locationService.Value = true;
            MainPage.watcher.Start();
        }

        private void locUncheck(object sender, RoutedEventArgs e)
        {
            Settings.locationService.Value = false;
            MainPage.lat = 0;
            MainPage.lng = 0;
            MainPage.watcher.Stop();
        }

        private void ConnectToTwitter_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/TwitterAuthorizationPage.xaml", UriKind.Relative));
        }

    }
}