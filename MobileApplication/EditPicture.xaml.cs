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
using System.IO.IsolatedStorage;
using Microsoft.Xna.Framework.Media;

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

            // set Original image filter to be highlight on default
            ImageFiltersWrapper.SelectedItem = imageFilters.ElementAt(0);
        }

        private void capturedImage_Loaded(object sender, RoutedEventArgs e)
        {
            NavigationService.RemoveBackEntry();

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

        private T FindFirstElementInVisualTree<T>(DependencyObject parentElement) where T : DependencyObject
        {
            var count = VisualTreeHelper.GetChildrenCount(parentElement);
            if (count == 0)
                return null;

            for (int i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(parentElement, i);

                if (child != null && child is T)
                {
                    return (T)child;
                }
                else
                {
                    var result = FindFirstElementInVisualTree<T>(child);
                    if (result != null)
                        return result;

                }
            }
            return null;
        }

        // Apply image filter upon selection changed
        TextBlock old = null;
        TextBlock curr = null;
        private void ImageFiltersWrapper_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {     
            int index = ((sender as ListBox).SelectedIndex);
            ListBoxItem lbi = this.ImageFiltersWrapper.ItemContainerGenerator.ContainerFromIndex(index) as ListBoxItem;

            if (lbi != null)
            {
                if (old == null)
                {
                    ListBoxItem _lbi = this.ImageFiltersWrapper.ItemContainerGenerator.ContainerFromIndex(0) as ListBoxItem;
                    old = FindFirstElementInVisualTree<TextBlock>(_lbi);
                }

                curr = FindFirstElementInVisualTree<TextBlock>(lbi);
                Color accent = (Color)Application.Current.Resources["PhoneAccentColor"];
                curr.Foreground = new SolidColorBrush(accent);
                old.Foreground = new SolidColorBrush(Colors.White);
                old = curr; 
            }
            
            // If original filter is selected, remove filter effect
            if (this.ImageFiltersWrapper.SelectedItem is OriginalEffect)
            {
                this.CapturedImage.Source = bitmap;
                this.CapturedImage.InvalidateArrange();
                this.CapturedImage.InvalidateMeasure();
                return;
            }

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

        private void ImageFiltersWrapper_Loaded(object sender, RoutedEventArgs e)
        {
            ListBoxItem _lbi = this.ImageFiltersWrapper.ItemContainerGenerator.ContainerFromIndex(0) as ListBoxItem;
            TextBlock first = FindFirstElementInVisualTree<TextBlock>(_lbi);

            Color accent = (Color) Application.Current.Resources["PhoneAccentColor"];
            first.Foreground = new SolidColorBrush(accent);
        }
    }
}