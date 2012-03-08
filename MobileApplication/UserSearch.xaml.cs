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
        }
    }
}