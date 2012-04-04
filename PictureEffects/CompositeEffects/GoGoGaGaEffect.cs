using PictureEffects.Effects;
using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PictureEffects.CompositeEffects
{
    public class GoGoGaGaEffect : IEffect
    {
        private readonly TiltShiftEffect tiltShiftFx;

        public GoGoGaGaEffect()
        {
            TiltShiftEffect tiltShiftEffect = new TiltShiftEffect();
            tiltShiftEffect.LowerFallOff = 0.8f;
            tiltShiftEffect.UpperFallOff = 0.2f;
            tiltShiftEffect.Blurriness = 1.35f;
            tiltShiftEffect.ContrastFactor = 0.4f;
            tiltShiftFx = tiltShiftEffect;
        }

        public string Name
        { 
            get 
            { 
                return "GoGoGaGa";
            } 
        }

        public float ContrastFactor
        {
            get
            {
                return this.tiltShiftFx.ContrastFactor;
            }
            set
            {
                this.tiltShiftFx.ContrastFactor = value;
            }
        }

        public float Blurriness
        {
            get
            {
                return this.tiltShiftFx.Blurriness;
            }
            set
            {
                this.tiltShiftFx.Blurriness = value;
            }
        }

        public float LowerFallOff
        {
            get
            {
                return this.tiltShiftFx.LowerFallOff;
            }
            set
            {
                this.tiltShiftFx.LowerFallOff = value;
            }
        }

        public float UpperFallOff
        {
            get
            {
                return this.tiltShiftFx.UpperFallOff;
            }
            set
            {
                this.tiltShiftFx.UpperFallOff = value;
            }
        }

        public WriteableBitmap Process(WriteableBitmap input)
        {
            // Prepare some variables
            var width = input.PixelWidth;
            var height = input.PixelHeight;
            return Process(input.Pixels, width, height).ToWriteableBitmap(width, height);
        }

        public int[] Process(int[] inputPixels, int width, int height)
        {
            inputPixels = this.tiltShiftFx.Process(inputPixels, width, height);

            return inputPixels;
        }
    }
}
