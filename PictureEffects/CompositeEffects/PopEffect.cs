using PictureEffects.Effects;
using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PictureEffects.CompositeEffects
{
    public class PopEffect : IEffect
    {
        private readonly BrightnessContrastEffect contrastFx;

        private readonly BitmapMixer mixer;

        private readonly SaturateEffect satFx;

        private readonly TintEffect tintFx;

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
                return "Pop";
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

        public Color TintColor
        {
            get
            {
                return this.tintFx.Color;
            }
            set
            {
                this.tintFx.Color = value;
            }
        }

        public float Tinting
        {
            get
            {
                return this.mixer.Mixture;
            }
            set
            {
                this.mixer.Mixture = value;
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

        public PopEffect()
        {
            Color color = Color.FromArgb(255, 208, 21, 133);

            this.tintFx = TintEffect.Sepia;
            this.tintFx.Color = color;

            VignetteEffect vignetteEffect = new VignetteEffect();
            vignetteEffect.Size = (float)1;
            vignetteFx = vignetteEffect;

            SaturateEffect saturateEffect = new SaturateEffect();
            saturateEffect.LightnessFactor = (float)0.1;
            satFx = saturateEffect;

            BitmapMixer bitmapMixer = new BitmapMixer();
            bitmapMixer.Mixture = (float)0.4;
            mixer = bitmapMixer;
        }

        public WriteableBitmap Process(WriteableBitmap input)
        {
            int pixelWidth = input.PixelWidth;
            int pixelHeight = input.PixelHeight;
            return this.Process(input.Pixels, pixelWidth, pixelHeight).ToWriteableBitmap(pixelWidth, pixelHeight);
        }

        public int[] Process(int[] inputPixels, int width, int height)
        {
            int[] numArray1 = this.vignetteFx.Process(inputPixels, width, height);
            int[] numArray2 = this.tintFx.Process(numArray1, width, height);
            numArray1 = this.satFx.Process(numArray1, width, height);
            return this.mixer.Mix(numArray1, numArray2, width, height);
        }
    }
}