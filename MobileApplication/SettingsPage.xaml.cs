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
            //again, user control popup? that just has a text box with the privacy agreement in it
            popup = new System.Windows.Controls.Primitives.Popup();
            popup.Height = 300;
            popup.Width = 400;
            popup.VerticalOffset = 100;
            PopupControl control = new PopupControl();

            popup.Child = control;
            popup.IsOpen = true;

            control.okbtn.Click += (s, args) =>
            {
                popup.IsOpen = false;

            };
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            if (popup.IsOpen == true) {
                popup.IsOpen = false;

            }
        }

    }
}