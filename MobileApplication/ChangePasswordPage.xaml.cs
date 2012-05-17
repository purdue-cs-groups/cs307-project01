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
    public partial class ChangePasswordPage : PhoneApplicationPage
    {
        public ChangePasswordPage()
        {
            InitializeComponent();
        }

        private void Current_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Dispatcher.BeginInvoke(() => 
                {
                    Current.Focus();
                    New.Focus();
                });
            }
        }

        private void New_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Dispatcher.BeginInvoke(() =>
                {
                    New.Focus();
                    ConfirmNew.Focus();
                });
            }
        }

        private void ConfirmNew_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Dispatcher.BeginInvoke(() =>
                {
                    ConfirmNew.Focus();
                });
            }
        }
    }
}