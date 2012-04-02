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

using PictureEffects;
using PictureEffects.CompositeEffects;
using PictureEffects.Effects;
using MetrocamPan.Models;
using System.Windows.Media.Imaging;

namespace MetrocamPan
{
    public partial class EditPicture : PhoneApplicationPage
    {
        private ImageSource capturedSource;
        public ImageSource CapturedSource
        {
            get 
            {
                return capturedSource;
            }
            set
            {
                capturedSource = value;
            }
        }

        public EditPicture()
        {
            InitializeComponent();
        }

        public static WriteableBitmap bitmap = null;
        public static Image editedPicture = new Image();
        private void capturedImage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!MainPage.isLandscape)
            {
                capturedSource = CropPage.cropped.Source;
            }
            else
            {
                capturedSource = CropPageLandscapeOrientation.cropped.Source;
            }

            bitmap = new WriteableBitmap((BitmapSource)capturedSource);
            capturedImage.Source = CapturedSource;
        }

        private void Check_Click(object sender, EventArgs e)
        {
            editedPicture.Source = capturedImage.Source;
            NavigationService.Navigate(new Uri("/UploadPage.xaml", UriKind.Relative));
        }

        private void RemoveEffect(object sender, System.Windows.Input.GestureEventArgs e)
        {
            capturedImage.Source = bitmap;
            capturedImage.InvalidateArrange();
            capturedImage.InvalidateMeasure();
        }

        private void ApplyAntique(object sender, System.Windows.Input.GestureEventArgs e)
        {
            EffectItem item = new EffectItem(new AntiqueEffect());
            IEffect effect = item.Effect;

            if (bitmap == null) return;

            var width = bitmap.PixelWidth;
            var height = bitmap.PixelHeight;
            var resultPixels = effect.Process(bitmap.Pixels, width, height);

            WriteableBitmap newBitmap = resultPixels.ToWriteableBitmap(width, height);

            capturedImage.Source = newBitmap;
            capturedImage.InvalidateArrange();
            capturedImage.InvalidateMeasure();
        }
    }
}