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
    public partial class EditPicture : PhoneApplicationPage
    {
        public EditPicture()
        {
            InitializeComponent();
        }

        public static Image editedPicture = new Image();
        private void capturedImage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!MainPage.isLandscape)
            {
                capturedImage.Source = CropPage.cropped.Source;
            }
            else
            {
                capturedImage.Source = CropPageLandscape.cropped.Source;
            }
        }

        private void Check_Click(object sender, EventArgs e)
        {
            editedPicture.Source = capturedImage.Source;
            NavigationService.Navigate(new Uri("/UploadPage.xaml", UriKind.Relative));
        }
    }
}