using PictureEffects.Effects;
using System;
using System.Windows.Media.Imaging;

namespace PictureEffects.CompositeEffects
{
    public class YoshimoEffect : IEffect
    {
        private readonly BrightnessContrastEffect contrastFx;

        private readonly SaturateEffect satFx;

        private readonly VignetteEffect vignetteFx;

        public float BrightnessFactor
        {
            get
            {
                return this.contrastFx.BrightnessFactor;
            }
            set
            {
                this.contrastFx.BrightnessFactor = value;
            }
        }

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
                return "Yoshimo";
            }
        }

        public float SatFactor
        {
            get
            {
                return this.satFx.SatEffect;
            }
            set
            {
                this.satFx.SatEffect = value;
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

        public YoshimoEffect()
        {
            VignetteEffect vignetteEffect = new VignetteEffect();
            vignetteEffect.Size = 1;
            vignetteFx = vignetteEffect;

            SaturateEffect saturateEffect = new SaturateEffect();
            saturateEffect.LightnessFactor = (float)0.07;
            satFx = saturateEffect;

            BrightnessContrastEffect brightnessContrastEffect = new BrightnessContrastEffect();
            brightnessContrastEffect.ContrastFactor = (float)-0.1;
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
            int[] numArray = this.contrastFx.Process(inputPixels, width, height);
            numArray = this.satFx.Process(numArray, width, height);
            numArray = this.vignetteFx.Process(numArray, width, height);
            return numArray;
        }
    }
}