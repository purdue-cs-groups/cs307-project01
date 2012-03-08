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
        public SettingsPage()
        {
            InitializeComponent();
        }

        private void loginasnew_Click(object sender, RoutedEventArgs e)
        {
            //do something with the click on the login as new user button
            //link to the login screen
            //must check some state related stuff
                    //can we go back once we got to the login as new user page?
                    //if we go to login as new user, we want to swap the whole user experience
                    //want to be able to go back to cancel the login
                    //once oyu're logged in, everything changes
        }

        private void changepassword_Click(object sender, RoutedEventArgs e)
        {
            //is this somethign we even want to do?
            //have to go to some other page where we can enter the password change requests
                //user control popup?
        }

        private void connectaccounts_Click(object sender, RoutedEventArgs e)
        {
            //enter twitter information
            //user control in conjunction with twitter API
        }

        private void readprivcy_Click(object sender, RoutedEventArgs e)
        {
            //again, user control popup? that just has a text box with the privacy agreement in it
        }

    }
}