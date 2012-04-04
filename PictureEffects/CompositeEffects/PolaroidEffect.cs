using PictureEffects.Effects;
using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PictureEffects.CompositeEffects
{
    /// <summary>
    /// Polaroid-like conversion effect.
    /// </summary>
    public class PolaroidEffect : IEffect
    {
        readonly GaussianBlurEffect blurFx;
        readonly VignetteEffect vignetteFx;
        readonly TintEffect tintFx;
        readonly BitmapMixer mixer;

        public string Name 
        {
            get 
            { 
                return "Polaroid"; 
            } 
        }

        /// <summary>
        /// The blurriness factor. 
        /// Should be in the range [0, 1]. 
        /// Default is 0.15
        /// </summary>
        public float Blurriness
        {
            get { return blurFx.Sigma; }
            set { blurFx.Sigma = value; }
        }

        /// <summary>
        /// The size of the vignette. 
        /// Should be in the range [0, 1]. 
        /// Default is 0.5
        /// </summary>
        public float Vignette
        {
            get { return vignetteFx.Size; }
            set { vignetteFx.Size = value; }
        }

        /// <summary>
        /// The amount of tinting (mix between original and tinted version). 
        /// Should be in the range [0, 1]. 
        /// Default is 0.5
        /// </summary>
        public float Tinting
        {
            get { return mixer.Mixture; }
            set { mixer.Mixture = value; }
        }

        /// <summary>
        /// The tinting color. 
        /// Default is Sepia.
        /// </summary>
        public Color TintColor
        {
            get { return tintFx.Color; }
            set { tintFx.Color = value; }
        }


        public PolaroidEffect()
        {
            blurFx = new GaussianBlurEffect { Sigma = 0.15f };
            vignetteFx = new VignetteEffect();
            tintFx = TintEffect.Sepia;
            mixer = new BitmapMixer { Mixture = 0.5f };
        }

        /// <summary>
        /// Processes a bitmap and returns a new processed WriteabelBitmap.
        /// </summary>
        /// <param name="input">The input bitmap.</param>
        /// <returns>The result of WriteabelBitmap processing.</returns>
        public WriteableBitmap Process(WriteableBitmap input)
        {
            // Prepare some variables
            var width = input.PixelWidth;
            var height = input.PixelHeight;
            return Process(input.Pixels, width, height).ToWriteableBitmap(width, height);
        }

        /// <summary>
        /// Processes an ARGB32 integer bitmap and returns the new processed bitmap data.
        /// </summary>
        /// <param name="inputPixels">The input bitmap as integer array.</param>
        /// <param name="width">The width of the bitmap.</param>
        /// <param name="height">The height of the bitmap.</param>
        /// <returns>The result of the processing.</returns>
        public int[] Process(int[] inputPixels, int width, int height)
        {
            var resultPixels = blurFx.Process(inputPixels, width, height);
            resultPixels = vignetteFx.Process(resultPixels, width, height);
            var tintedPixels = tintFx.Process(resultPixels, width, height);
            return mixer.Mix(resultPixels, tintedPixels, width, height);
        }
    }
}