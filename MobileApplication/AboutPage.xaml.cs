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
    public partial class AboutPage : PhoneApplicationPage
    {
        public AboutPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            try
            {
                string newparameter = this.NavigationContext.QueryString["parameter"];
                if (newparameter.Equals("privacy"))
                {
                    AboutPivot.SelectedItem = privacy;
                }
            }
            catch (KeyNotFoundException ex) {
                AboutPivot.SelectedItem = about;
            }
           
        }
    }
}