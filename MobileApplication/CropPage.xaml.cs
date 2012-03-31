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

namespace MetrocamPan
{
    public partial class CropPage : PhoneApplicationPage
    {
        public CropPage()
        {
            InitializeComponent();
            drag = new TranslateTransform();
            cropArea.RenderTransform = drag;
            y1 = (int)cropArea.Margin.Top;
            y1min = y1;
            y1max = y1 + 92;
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            SetPhoto();
        }

        private void SetPhoto()
        {
            originalPhoto.Source = MainPage.captured.Source;
        }

        public static Image cropped = new Image();
        public static int y1 = 0;
        public static int y1min = 0;
        public static int y1max = 0;
        private void CropPhoto()
        {
            int y0 = (int)originalPhoto.Margin.Top;

            int yDisplacement = y0 - y1;

            WriteableBitmap wb = new WriteableBitmap((int)cropArea.Width, (int)cropArea.Height);
            TranslateTransform t = new TranslateTransform();
            t.Y = yDisplacement;
            Point p = new Point((int)cropArea.Width, (int)cropArea.Height);
            t.Transform(p);
            wb.Render(originalPhoto, t);
            wb.Invalidate();
            cropped.Source = wb;

            MainPage.isLandscape = false;
            NavigationService.Navigate(new Uri("/EditPicture.xaml", UriKind.Relative));
        }

        private void Crop_Click(object sender, EventArgs e)
        {
            CropPhoto();
        }

        private TranslateTransform drag;
        private void cropArea_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            drag.Y += e.DeltaManipulation.Translation.Y;
            if (drag.Y < 0)
                drag.Y = 0;
            if (drag.Y > 92)
                drag.Y = 92;

            int temp = y1 + (int)e.DeltaManipulation.Translation.Y;
            if (temp < y1min)
                y1 = y1min;
            else if (temp > y1max)
                y1 = y1max;
            else
                y1 = temp;

        }
    }
}