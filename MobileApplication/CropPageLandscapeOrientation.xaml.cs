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
    public partial class CropPageLandscapeOrientation : PhoneApplicationPage
    {
        public static Image cropped = new Image();

        public int min = 0;                      // the smallest value the left margin can be
        public int max = 0;                      // the largest value the left margin can be (i.e. margin that makes it even with originalImage)
        public int current = 0;                  // the current value of the left margin of the cropArea (gray square)

        public CropPageLandscapeOrientation()
        {
            InitializeComponent();
            drag = new TranslateTransform();
            cropArea.RenderTransform = drag;
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            SetPhoto();
        }

        private void SetPhoto()
        {
            double height = MainPage.bmp.PixelHeight;
            double width = MainPage.bmp.PixelWidth;
            double ratio = height / width;

            if (ratio > 3.00 / 4.00)
            {
                originalPhoto.Width = Convert.ToInt32((width * originalPhoto.Height) / height);
            }
            else if (ratio < 3.00 / 4.00)
            {
                originalPhoto.Height = Convert.ToInt32((height * originalPhoto.Width) / width);
                cropArea.Height      = originalPhoto.Height;
                cropArea.Width       = originalPhoto.Height;
            }
            else
                ;

            this.originalPhoto.Source = MainPage.captured.Source;

            min = (int)cropArea.Margin.Right;
            max = (int)originalPhoto.Width - (int)cropArea.Width;
        }

        private void CropPhoto()
        {
            int x0 = (int)originalPhoto.Margin.Right;

            int xDisplacement = x0 - current;

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
            if (drag.X > max)
                drag.X = max;

            int temp = current + (int)e.DeltaManipulation.Translation.X;
            if (temp < min)
                current = min;
            else if (temp > max)
                current = max;
            else
                current = temp;
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }
    }
}