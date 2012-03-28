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

using System.Windows.Media.Imaging;
using System.Reflection;
using System.Windows.Resources;

namespace WinstagramPan
{
    public partial class CropPageLandscape : PhoneApplicationPage
    {
        public CropPageLandscape()
        {
            InitializeComponent();
            drag = new TranslateTransform();
            cropArea.RenderTransform = drag;
            x1 = (int)cropArea.Margin.Right;
            x1min = x1;
            x1max = x1 + 92;
        }

        private void SetPhoto()
        {
            originalPhoto.Source = MainPage.captured.Source;
        }

        public static Image cropped = new Image();
        public static int x1 = 0;
        public static int x1min = 0;
        public static int x1max = 0;
        private void CropPhoto()
        {
            int x0 = (int)originalPhoto.Margin.Right;

            int xDisplacement = x0 - x1;

            WriteableBitmap wb = new WriteableBitmap((int)cropArea.Width, (int)cropArea.Height);
            TranslateTransform t = new TranslateTransform();
            t.X = xDisplacement;
            Point p = new Point((int)cropArea.Width, (int)cropArea.Height);
            t.Transform(p);
            wb.Render(originalPhoto, t);
            wb.Invalidate();
            cropped.Source = wb;

            MainPage.isLandscape = true;
            NavigationService.Navigate(new Uri("/EditPicture.xaml", UriKind.Relative));
        }

        private void Crop_Click(object sender, EventArgs e)
        {
            CropPhoto();
        }

        private TranslateTransform drag;
        private void cropArea_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            drag.X += e.DeltaManipulation.Translation.X;
            if (drag.X < 0)
                drag.X = 0;
            if (drag.X > 92)
                drag.X = 92;

            int temp = x1 + (int)e.DeltaManipulation.Translation.X;
            if (temp < x1min)
                x1 = x1min;
            else if (temp > x1max)
                x1 = x1max;
            else
                x1 = temp;

        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            SetPhoto();
        }
    }
}