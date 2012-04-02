using PictureEffects.Effects;
using System;
using System.Windows.Media.Imaging;

namespace PictureEffects.CompositeEffects
{
    public class GoGoGaGaEffect : IEffect
    {
        private readonly BrightnessContrastEffect contrastFx;

        private readonly TintEffect tintFx;

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
                return "GoGoGaGa";
            }
        }

        public GoGoGaGaEffect()
        {
            this.tintFx = TintEffect.White;

            BrightnessContrastEffect brightnessContrastEffect = new BrightnessContrastEffect();
            brightnessContrastEffect.ContrastFactor = (float)1.50;
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
            numArray = this.contrastFx.Process(numArray, width, height);
            return numArray;
        }
    }
}
