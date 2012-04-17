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
        public static Image cropped = new Image();

        public int min = 0;                      // the smallest value the top margin can be
        public int max = 0;                      // the largest value the top margin can be (i.e. margin that makes it even with originalImage)
        public int current = 0;                  // the current value of the top margin of the cropArea (gray square)
   
        public CropPage()
        {
            InitializeComponent();
            drag = new TranslateTransform();
            cropArea.RenderTransform = drag;
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            drag.Y = 0;
            SetPhoto();
        }

        private void SetPhoto()
        {
            double height = MainPage.bmp.PixelHeight;
            double width  = MainPage.bmp.PixelWidth;
            double ratio  = height / width;

            if (ratio < 4.00 / 3.00)
            {
                originalPhoto.Height = Convert.ToInt32((height * originalPhoto.Width) / width);
            }
            else if (ratio > 4.00 / 3.00)
            {
                originalPhoto.Width = Convert.ToInt32((width * originalPhoto.Height) / height);
                cropArea.Height = originalPhoto.Width;
                cropArea.Width = originalPhoto.Width;
            }
            else
            {
                // Do nothing
            }

            originalPhoto.Source = MainPage.captured.Source;

            current = (int)cropArea.Margin.Top;
            min     = (int)cropArea.Margin.Top;
            max     = (int)originalPhoto.Height - (int)cropArea.Height;

            lowerBound.Height  = max;

            upperBound.Width = cropArea.Width;
            lowerBound.Width = cropArea.Width;
        }

        private void CropPhoto()
        {
            int y0 = (int)originalPhoto.Margin.Top;
            int yDisplacement = y0 - current;

            WriteableBitmap wb = new WriteableBitmap((int)cropArea.Width, (int)cropArea.Height);
            TranslateTransform t = new TranslateTransform();
            t.Y = yDisplacement;
            Point p = new Point((int)cropArea.Width / 2, (int)cropArea.Height / 2);
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
            /**
             *  all of this code keeps the picture within the boundaries
             */

            drag.Y += e.DeltaManipulation.Translation.Y;
            if (drag.Y < 0)
                drag.Y = 0;
            if (drag.Y > max)
                drag.Y = max;

            int temp = current + (int)e.DeltaManipulation.Translation.Y;
            if (temp < min)
            {
                lowerBound.Height = originalPhoto.Height - cropArea.Height;
                upperBound.Height = 0;
                current = min;
            }
            else if (temp > max)
            {
                upperBound.Height = originalPhoto.Height - cropArea.Height;
                lowerBound.Height = 0;
                current = max;
            }
            else
            {
                upperBound.Height = temp;
                lowerBound.Height = max - temp;
                current = temp;
            }

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