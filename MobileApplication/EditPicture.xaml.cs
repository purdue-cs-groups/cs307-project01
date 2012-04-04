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
        // Transparent SolidColorBrush to paint BorderBrush
        SolidColorBrush transparent;

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
            transparent = new SolidColorBrush(Colors.Transparent);
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

        private void Cancel_Click(object sender, EventArgs e)
        {
            // Edit picture cancelled, go back
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }

        // Takes in an UIElementCollection, typecast to Border, and changes to transparent
        private void MakeBorderTransparent()
        {
            UIElementCollection borderCollection = this.BorderWrapper.Children;

            foreach (UIElement element in borderCollection)
            {
                Border b = (Border)element;
                b.BorderBrush = transparent;
            }
        }

        private void RemoveEffect(object sender, System.Windows.Input.GestureEventArgs e)
        {
            capturedImage.Source = bitmap;
            capturedImage.InvalidateArrange();
            capturedImage.InvalidateMeasure();

            // Make all other borderbrushes transparent
            MakeBorderTransparent();

            // Set this borderbrush to a noticable color
            this.OriginalGridBorder.BorderBrush = (System.Windows.Media.Brush) Resources["PhoneAccentBrush"];
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

            // Make all other borderbrushes transparent
            MakeBorderTransparent();

            // Set this borderbrush to a noticable color
            this.AntiqueGridBorder.BorderBrush = (System.Windows.Media.Brush)Resources["PhoneAccentBrush"];
        }

        private void ApplyBetty(object sender, System.Windows.Input.GestureEventArgs e)
        {
            EffectItem item = new EffectItem(new BettyEffect());
            IEffect effect = item.Effect;

            if (bitmap == null) return;

            var width = bitmap.PixelWidth;
            var height = bitmap.PixelHeight;
            var resultPixels = effect.Process(bitmap.Pixels, width, height);

            WriteableBitmap newBitmap = resultPixels.ToWriteableBitmap(width, height);

            capturedImage.Source = newBitmap;
            capturedImage.InvalidateArrange();
            capturedImage.InvalidateMeasure();

            // Make all other borderbrushes transparent
            MakeBorderTransparent();

            // Set this borderbrush to a noticable color
            this.BettyGridBorder.BorderBrush = (System.Windows.Media.Brush)Resources["PhoneAccentBrush"];
        }

        private void ApplyBandW(object sender, System.Windows.Input.GestureEventArgs e)
        {
            EffectItem item = new EffectItem(new BlackWhiteEffect());
            IEffect effect = item.Effect;

            if (bitmap == null) return;

            var width = bitmap.PixelWidth;
            var height = bitmap.PixelHeight;
            var resultPixels = effect.Process(bitmap.Pixels, width, height);

            WriteableBitmap newBitmap = resultPixels.ToWriteableBitmap(width, height);

            capturedImage.Source = newBitmap;
            capturedImage.InvalidateArrange();
            capturedImage.InvalidateMeasure();

            // Make all other borderbrushes transparent
            MakeBorderTransparent();

            // Set this borderbrush to a noticable color
            this.BlackAndWhiteBorder.BorderBrush = (System.Windows.Media.Brush)Resources["PhoneAccentBrush"];
        }

        private void ApplyCyan(object sender, System.Windows.Input.GestureEventArgs e)
        {
            EffectItem item = new EffectItem(new CyanEffect());
            IEffect effect = item.Effect;

            if (bitmap == null) return;

            var width = bitmap.PixelWidth;
            var height = bitmap.PixelHeight;
            var resultPixels = effect.Process(bitmap.Pixels, width, height);

            WriteableBitmap newBitmap = resultPixels.ToWriteableBitmap(width, height);

            capturedImage.Source = newBitmap;
            capturedImage.InvalidateArrange();
            capturedImage.InvalidateMeasure();

            // Make all other borderbrushes transparent
            MakeBorderTransparent();

            // Set this borderbrush to a noticable color
            this.CyanGridBorder.BorderBrush = (System.Windows.Media.Brush)Resources["PhoneAccentBrush"];
        }

        private void ApplyDawn(object sender, System.Windows.Input.GestureEventArgs e)
        {
            EffectItem item = new EffectItem(new DawnEffect());
            IEffect effect = item.Effect;

            if (bitmap == null) return;

            var width = bitmap.PixelWidth;
            var height = bitmap.PixelHeight;
            var resultPixels = effect.Process(bitmap.Pixels, width, height);

            WriteableBitmap newBitmap = resultPixels.ToWriteableBitmap(width, height);

            capturedImage.Source = newBitmap;
            capturedImage.InvalidateArrange();
            capturedImage.InvalidateMeasure();

            // Make all other borderbrushes transparent
            MakeBorderTransparent();

            // Set this borderbrush to a noticable color
            this.DawnGridBorder.BorderBrush = (System.Windows.Media.Brush)Resources["PhoneAccentBrush"];
        }

        private void ApplyPolaroid(object sender, System.Windows.Input.GestureEventArgs e)
        {
            EffectItem item = new EffectItem(new PolaroidEffect());
            IEffect effect = item.Effect;

            if (bitmap == null) return;

            var width = bitmap.PixelWidth;
            var height = bitmap.PixelHeight;
            var resultPixels = effect.Process(bitmap.Pixels, width, height);

            WriteableBitmap newBitmap = resultPixels.ToWriteableBitmap(width, height);

            capturedImage.Source = newBitmap;
            capturedImage.InvalidateArrange();
            capturedImage.InvalidateMeasure();

            // Make all other borderbrushes transparent
            MakeBorderTransparent();

            // Set this borderbrush to a noticable color
            this.PolaroidGridBorder.BorderBrush = (System.Windows.Media.Brush)Resources["PhoneAccentBrush"];
        }

        private void ApplyPop(object sender, System.Windows.Input.GestureEventArgs e)
        {
            EffectItem item = new EffectItem(new PopEffect());
            IEffect effect = item.Effect;

            if (bitmap == null) return;

            var width = bitmap.PixelWidth;
            var height = bitmap.PixelHeight;
            var resultPixels = effect.Process(bitmap.Pixels, width, height);

            WriteableBitmap newBitmap = resultPixels.ToWriteableBitmap(width, height);

            capturedImage.Source = newBitmap;
            capturedImage.InvalidateArrange();
            capturedImage.InvalidateMeasure();

            // Make all other borderbrushes transparent
            MakeBorderTransparent();

            // Set this borderbrush to a noticable color
            this.PopGridBorder.BorderBrush = (System.Windows.Media.Brush)Resources["PhoneAccentBrush"];
        }

        private void ApplyRustic(object sender, System.Windows.Input.GestureEventArgs e)
        {
            EffectItem item = new EffectItem(new RusticEffect());
            IEffect effect = item.Effect;

            if (bitmap == null) return;

            var width = bitmap.PixelWidth;
            var height = bitmap.PixelHeight;
            var resultPixels = effect.Process(bitmap.Pixels, width, height);

            WriteableBitmap newBitmap = resultPixels.ToWriteableBitmap(width, height);

            capturedImage.Source = newBitmap;
            capturedImage.InvalidateArrange();
            capturedImage.InvalidateMeasure();

            // Make all other borderbrushes transparent
            MakeBorderTransparent();

            // Set this borderbrush to a noticable color
            this.RusticGridBorder.BorderBrush = (System.Windows.Media.Brush)Resources["PhoneAccentBrush"];
        }

        private void ApplyGrunge(object sender, System.Windows.Input.GestureEventArgs e)
        {
            EffectItem item = new EffectItem(new GrungeEffect());
            IEffect effect = item.Effect;

            if (bitmap == null) return;

            var width = bitmap.PixelWidth;
            var height = bitmap.PixelHeight;
            var resultPixels = effect.Process(bitmap.Pixels, width, height);

            WriteableBitmap newBitmap = resultPixels.ToWriteableBitmap(width, height);

            capturedImage.Source = newBitmap;
            capturedImage.InvalidateArrange();
            capturedImage.InvalidateMeasure();

            // Make all other borderbrushes transparent
            MakeBorderTransparent();

            // Set this borderbrush to a noticable color
            this.GrungeGridBorder.BorderBrush = (System.Windows.Media.Brush)Resources["PhoneAccentBrush"];
        }

        private void ApplyStorm(object sender, System.Windows.Input.GestureEventArgs e)
        {
            EffectItem item = new EffectItem(new StormEffect());
            IEffect effect = item.Effect;

            if (bitmap == null) return;

            var width = bitmap.PixelWidth;
            var height = bitmap.PixelHeight;
            var resultPixels = effect.Process(bitmap.Pixels, width, height);

            WriteableBitmap newBitmap = resultPixels.ToWriteableBitmap(width, height);

            capturedImage.Source = newBitmap;
            capturedImage.InvalidateArrange();
            capturedImage.InvalidateMeasure();

            // Make all other borderbrushes transparent
            MakeBorderTransparent();

            // Set this borderbrush to a noticable color
            this.StormGridBorder.BorderBrush = (System.Windows.Media.Brush)Resources["PhoneAccentBrush"];
        }

        private void ApplyWasabi(object sender, System.Windows.Input.GestureEventArgs e)
        {
            EffectItem item = new EffectItem(new WasabiEffect());
            IEffect effect = item.Effect;

            if (bitmap == null) return;

            var width = bitmap.PixelWidth;
            var height = bitmap.PixelHeight;
            var resultPixels = effect.Process(bitmap.Pixels, width, height);

            WriteableBitmap newBitmap = resultPixels.ToWriteableBitmap(width, height);

            capturedImage.Source = newBitmap;
            capturedImage.InvalidateArrange();
            capturedImage.InvalidateMeasure();

            // Make all other borderbrushes transparent
            MakeBorderTransparent();

            // Set this borderbrush to a noticable color
            this.WasabiGridBorder.BorderBrush = (System.Windows.Media.Brush)Resources["PhoneAccentBrush"];
        }

        private void ApplyTiltShift(object sender, System.Windows.Input.GestureEventArgs e)
        {
            EffectItem item = new EffectItem(new TiltShiftEffect());
            IEffect effect = item.Effect;

            if (bitmap == null) return;

            var width = bitmap.PixelWidth;
            var height = bitmap.PixelHeight;
            var resultPixels = effect.Process(bitmap.Pixels, width, height);

            WriteableBitmap newBitmap = resultPixels.ToWriteableBitmap(width, height);

            capturedImage.Source = newBitmap;
            capturedImage.InvalidateArrange();
            capturedImage.InvalidateMeasure();

            // Make all other borderbrushes transparent
            MakeBorderTransparent();

            // Set this borderbrush to a noticable color
            this.TiltShiftGridBorder.BorderBrush = (System.Windows.Media.Brush)Resources["PhoneAccentBrush"];
        }

        private void ApplyYoshimoto(object sender, System.Windows.Input.GestureEventArgs e)
        {
            EffectItem item = new EffectItem(new YoshimotoEffect());
            IEffect effect = item.Effect;

            if (bitmap == null) return;

            var width = bitmap.PixelWidth;
            var height = bitmap.PixelHeight;
            var resultPixels = effect.Process(bitmap.Pixels, width, height);

            WriteableBitmap newBitmap = resultPixels.ToWriteableBitmap(width, height);

            capturedImage.Source = newBitmap;
            capturedImage.InvalidateArrange();
            capturedImage.InvalidateMeasure();

            // Make all other borderbrushes transparent
            MakeBorderTransparent();

            // Set this borderbrush to a noticable color
            this.YoshimotoGridBorder.BorderBrush = (System.Windows.Media.Brush)Resources["PhoneAccentBrush"];
        }

        private void ApplyGoGoGaGa(object sender, System.Windows.Input.GestureEventArgs e)
        {
            EffectItem item = new EffectItem(new GoGoGaGaEffect());
            IEffect effect = item.Effect;

            if (bitmap == null) return;

            var width = bitmap.PixelWidth;
            var height = bitmap.PixelHeight;
            var resultPixels = effect.Process(bitmap.Pixels, width, height);

            WriteableBitmap newBitmap = resultPixels.ToWriteableBitmap(width, height);

            capturedImage.Source = newBitmap;
            capturedImage.InvalidateArrange();
            capturedImage.InvalidateMeasure();

            // Make all other borderbrushes transparent
            MakeBorderTransparent();

            // Set this borderbrush to a noticable color
            this.GoGoGaGaGridBorder.BorderBrush = (System.Windows.Media.Brush)Resources["PhoneAccentBrush"];
        }
    }
}