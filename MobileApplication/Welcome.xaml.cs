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

using System.Windows.Controls.Primitives;

namespace WinstagramPan
{
    public partial class Welcome : PhoneApplicationPage
    {
        public Welcome()
        {
            InitializeComponent();
        }

        public static String Username;
        public static String Password;

        /**
         *
         * Fires a pop up that allows user to enter credentials.
         * 
         */
        private void logInTileTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Popup popup = new Popup();
            popup.VerticalAlignment = System.Windows.VerticalAlignment.Top;

            LogInPopUp control = new LogInPopUp();
            popup.Child = control;
            popup.IsOpen = true;

            this.LayoutRoot.IsHitTestVisible = false;

            control.ConfirmButtom.Click += (s, args) =>
            {
                popup.IsOpen = false;
                this.LayoutRoot.IsHitTestVisible = true;

                Username = control.usernameInput.Text;
                Password = control.passwordInput.Password;

                // authenticate

                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            };

            control.CancelButton.Click += (s, args) =>
            {
                popup.IsOpen = false;
                this.LayoutRoot.IsHitTestVisible = true;
            };
        }
    }
}