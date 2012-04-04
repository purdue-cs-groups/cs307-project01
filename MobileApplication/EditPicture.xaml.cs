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
        private EffectItems imageFilters;
        public static WriteableBitmap bitmap = null;
        public static Image editedPicture = new Image();
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

            imageFilters = new EffectItems();
            // data bind imageFilters to ListBox
            ImageFiltersWrapper.ItemsSource = imageFilters;
        }

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
            this.CapturedImage.Source = CapturedSource;
        }

        // Apply image filter upon selection changed
        private void ImageFiltersWrapper_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EffectItem item = (EffectItem)this.ImageFiltersWrapper.SelectedItem;
            IEffect effect = item.Effect;

            if (bitmap == null) return;

            var width = bitmap.PixelWidth;
            var height = bitmap.PixelHeight;
            var resultPixels = effect.Process(bitmap.Pixels, width, height);

            WriteableBitmap newBitmap = resultPixels.ToWriteableBitmap(width, height);

            this.CapturedImage.Source = newBitmap;
        }

        private void Check_Click(object sender, EventArgs e)
        {
            editedPicture.Source = this.CapturedImage.Source;
            NavigationService.Navigate(new Uri("/UploadPage.xaml", UriKind.Relative));
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            // Edit picture cancelled, go back
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }
    }
}