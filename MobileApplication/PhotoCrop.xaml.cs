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

namespace WinstagramPan
{
    public partial class PhotoCrop : PhoneApplicationPage
    {
        public PhotoCrop()
        {
            InitializeComponent();

            canvas.Width = LayoutRoot.Width;
            canvas.Height = LayoutRoot.Height;
            SetPicture();
        }

        private void SetPicture()
        {    
            i.Source = MainPage.bmp;
            i.Opacity = 0.8;
            i.ManipulationStarted += new EventHandler<ManipulationStartedEventArgs>(i_ManipulationStarted);
            i.ManipulationDelta += new EventHandler<ManipulationDeltaEventArgs>(i_ManipulationDelta);
        }

        double startingPositionOfImageX = 0;
        double startingPositionOfImageY = 0;
        private void i_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {
            if (e.OriginalSource == i)
            {
                startingPositionOfImageX =
                Convert.ToDouble(i.GetValue(Canvas.LeftProperty));

                startingPositionOfImageY =
                Convert.ToDouble(i.GetValue(Canvas.TopProperty));
            }
        }

        private void i_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            if (e.OriginalSource == i)
            {
                i.SetValue(Canvas.LeftProperty,
                e.CumulativeManipulation.Translation.X + startingPositionOfImageX);

                i.SetValue(Canvas.TopProperty,
                e.CumulativeManipulation.Translation.Y + startingPositionOfImageY);
            }
        }

        double initialAngle;
        double initialScale;
        private void OnPinchStarted(object sender, PinchStartedGestureEventArgs e)
        {
            Point point0 = e.GetPosition(i, 0);
            Point point1 = e.GetPosition(i, 1);
            Point midpoint = new Point((point0.X + point1.X) / 2, (point0.Y + point1.Y) / 2);
            i.RenderTransformOrigin = new Point(midpoint.X / i.ActualWidth, midpoint.Y / i.ActualHeight);
            initialAngle = transform.Rotation;
            initialScale = transform.ScaleX;
            i.Opacity = 0.8;
        }

        private void OnPinchDelta(object sender, PinchGestureEventArgs e)
        {
            transform.Rotation = initialAngle + e.TotalAngleDelta;
            transform.ScaleX = transform.ScaleY = initialScale * e.DistanceRatio;
        } 

        /*
        // these two fully define the zoom state:
        private double TotalImageScale = 1d;
        private Point ImagePosition = new Point(0, 0);

        private Point _oldFinger1;
        private Point _oldFinger2;
        private double _oldScaleFactor;

        
        private void OnPinchStarted(object s, PinchStartedGestureEventArgs e)
        {
            _oldFinger1 = e.GetPosition(i, 0);
            _oldFinger2 = e.GetPosition(i, 1);
            _oldScaleFactor = 1;
        }

        private void OnPinchDelta(object s, PinchGestureEventArgs e)
        {
            var scaleFactor = e.DistanceRatio / _oldScaleFactor;

            var currentFinger1 = e.GetPosition(i, 0);
            var currentFinger2 = e.GetPosition(i, 1);

            var translationDelta = GetTranslationDelta(
                currentFinger1,
                currentFinger2,
                _oldFinger1,
                _oldFinger2,
                ImagePosition,
                scaleFactor);

            _oldFinger1 = currentFinger1;
            _oldFinger2 = currentFinger2;
            _oldScaleFactor = e.DistanceRatio;

            UpdateImage(scaleFactor, translationDelta);
        }

        private void UpdateImage(double scaleFactor, Point delta)
        {
            TotalImageScale *= scaleFactor;
            ImagePosition = new Point(ImagePosition.X + delta.X, ImagePosition.Y + delta.Y);

            var transform = (CompositeTransform)i.RenderTransform;
            transform.ScaleX = TotalImageScale;
            transform.ScaleY = TotalImageScale;
            transform.TranslateX = ImagePosition.X;
            transform.TranslateY = ImagePosition.Y;
        }

        private Point GetTranslationDelta(
            Point currentFinger1, Point currentFinger2,
            Point oldFinger1, Point oldFinger2,
            Point currentPosition, double scaleFactor)
        {
            var newPos1 = new Point(
                currentFinger1.X + (currentPosition.X - oldFinger1.X) * scaleFactor,
                currentFinger1.Y + (currentPosition.Y - oldFinger1.Y) * scaleFactor);

            var newPos2 = new Point(
                currentFinger2.X + (currentPosition.X - oldFinger2.X) * scaleFactor,
                currentFinger2.Y + (currentPosition.Y - oldFinger2.Y) * scaleFactor);

            var newPos = new Point(
                (newPos1.X + newPos2.X) / 2,
                (newPos1.Y + newPos2.Y) / 2);

            return new Point(
                newPos.X - currentPosition.X,
                newPos.Y - currentPosition.Y);
        }
        */
    }
}