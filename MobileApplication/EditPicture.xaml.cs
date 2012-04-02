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

        private void Cancel_Click(object sender, EventArgs e)
        {
            // Edit picture cancelled, go back
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
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
        }

        private void ApplyGoGoGaGa(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (bitmap == null) return;

            // First apply BrightnessContrastEffect
            BrightnessContrastEffect bcEffect = new BrightnessContrastEffect();
            bcEffect.ContrastFactor = 0.2f;
            EffectItem item1 = new EffectItem(bcEffect);
            IEffect effect1 = item1.Effect;

            var width = bitmap.PixelWidth;
            var height = bitmap.PixelHeight;
            var resultPixels = effect1.Process(bitmap.Pixels, width, height);

            WriteableBitmap newBitmap1 = resultPixels.ToWriteableBitmap(width, height);

            // Second, apply TiltShiftEffect with custom settings
            TiltShiftEffect tsEffect = new TiltShiftEffect();
            tsEffect.ContrastFactor = 0.2f;
            tsEffect.Blurriness = 1.35f;
            tsEffect.UpperFallOff = 0.20f;
            tsEffect.LowerFallOff = 0.80f;

            EffectItem item2 = new EffectItem(tsEffect);
            IEffect effect2 = item2.Effect;

            resultPixels = effect2.Process(newBitmap1.Pixels, width, height);

            WriteableBitmap newBitmap2 = resultPixels.ToWriteableBitmap(width, height);

            // newBitmap2 is final product
            capturedImage.Source = newBitmap2;
            capturedImage.InvalidateArrange();
            capturedImage.InvalidateMeasure();
        }
    }
}