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
    public partial class ForgotPasswordPage : PhoneApplicationPage
    {
        private string forgotRedirectionLink = "http://metrocam.cloudapp.net/ResetPassword.aspx";

        public ForgotPasswordPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            this.BrowserControl.Source = new Uri(forgotRedirectionLink, UriKind.Absolute);
        }
    }
}