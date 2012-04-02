﻿using System;
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
        }

        private void connectaccounts_Click(object sender, RoutedEventArgs e)
        {
            //enter twitter information
            //user control in conjunction with twitter API
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

    }
}