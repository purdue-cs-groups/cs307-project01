using PictureEffects.Effects;
using System;
using System.Windows.Media.Imaging;

namespace PictureEffects.CompositeEffects
{
    public class RusticEffect : IEffect
    {
        private BlackWhiteEffect zebraEffect;
        private PolaroidEffect polaroidEffect;

        public string Name
        {
            get
            {
                return "Rustic";
            }
        }

        public RusticEffect()
        {
            this.zebraEffect = new BlackWhiteEffect();
            this.polaroidEffect = new PolaroidEffect();
        }

        public WriteableBitmap Process(WriteableBitmap input)
        {
            int pixelWidth = input.PixelWidth;
            int pixelHeight = input.PixelHeight;
            return this.Process(input.Pixels, pixelWidth, pixelHeight).ToWriteableBitmap(pixelWidth, pixelHeight);
        }

        public int[] Process(int[] inputPixels, int width, int height)
        {
            int[] numArray = this.zebraEffect.Process(inputPixels, width, height);
            numArray = this.polaroidEffect.Process(numArray, width, height);
            return numArray;
        }
    }
}