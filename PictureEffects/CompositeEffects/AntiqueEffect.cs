using PictureEffects.Effects;
using System;
using System.Windows.Media.Imaging;

namespace PictureEffects.CompositeEffects
{
    public class AntiqueEffect : IEffect
    {
        private readonly BrightnessContrastEffect contrastFx;

        private readonly TintEffect tintFx;

        private readonly VignetteEffect vignetteFx;

        public float ContrastFactor
        {
            get
            {
                return this.contrastFx.ContrastFactor;
            }
            set
            {
                this.contrastFx.ContrastFactor = value;
            }
        }

        public string Name
        {
            get
            {
                return "Antique";
            }
        }

        public float Vignette
        {
            get
            {
                return this.vignetteFx.Size;
            }
            set
            {
                this.vignetteFx.Size = value;
            }
        }

        public AntiqueEffect()
        {
            this.tintFx = TintEffect.Sepia;

            VignetteEffect vignetteEffect = new VignetteEffect();
            vignetteEffect.Size = (float)1;
            vignetteFx = vignetteEffect;

            BrightnessContrastEffect brightnessContrastEffect = new BrightnessContrastEffect();
            brightnessContrastEffect.ContrastFactor = (float)0.05;
            contrastFx = brightnessContrastEffect;
        }

        public WriteableBitmap Process(WriteableBitmap input)
        {
            int pixelWidth = input.PixelWidth;
            int pixelHeight = input.PixelHeight;
            return this.Process(input.Pixels, pixelWidth, pixelHeight).ToWriteableBitmap(pixelWidth, pixelHeight);
        }

        public int[] Process(int[] inputPixels, int width, int height)
        {
            int[] numArray = this.tintFx.Process(inputPixels, width, height);
            numArray = this.vignetteFx.Process(numArray, width, height);
            numArray = this.contrastFx.Process(numArray, width, height);
            return numArray;
        }
    }
}