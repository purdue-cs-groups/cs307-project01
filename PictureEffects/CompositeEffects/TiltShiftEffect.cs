using PictureEffects.Effects;
using System;
using System.Windows.Media.Imaging;

namespace PictureEffects.CompositeEffects
{
    /// <summary>
    /// A miniature faking (tilt shift) effect.
    /// </summary>
    public class TiltShiftEffect : IEffect
    {
        readonly GaussianBlurEffect blurFx;
        readonly BrightnessContrastEffect contrastFx;
        private int[] contrastedPixels;
        private int[] blurredPixels;

        const float MaxFallOffFactor = 0.3f;

        public string Name 
        { 
            get 
            { 
                return "Tilt Shift"; 
            } 
        }

        /// <summary>
        /// The blurriness factor. 
        /// Should be in the range [0, 20]. 
        /// Default is 1.25
        /// </summary>
        public float Blurriness
        {
            get { return blurFx.Sigma; }
            set { blurFx.Sigma = value; }
        }

        /// <summary>
        /// The contrast factor. 
        /// Should be in the range [-1, 1]. 
        /// Default is 0.1
        /// </summary>
        public float ContrastFactor
        {
            get { return contrastFx.ContrastFactor; }
            set { contrastFx.ContrastFactor = value; }
        }

        /// <summary>
        /// The upper fall off factor for blurring. 
        /// Should be in the range [0, 1]. 
        /// Default is 0.25
        /// </summary>
        public float UpperFallOff { get; set; }

        /// <summary>
        /// The upper fall off factor for blurring. 
        /// Should be in the range [0, 1]. 
        /// Default is 0.75
        /// </summary>
        public float LowerFallOff { get; set; }

        public TiltShiftEffect()
        {
            UpperFallOff = 0.3f;
            LowerFallOff = 0.7f;
            blurFx = new GaussianBlurEffect { Sigma = 1.25f };
            contrastFx = new BrightnessContrastEffect { ContrastFactor = 0.1f };
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
            // Increase contrast
            CreateBlurredBitmap(inputPixels, width, height);

            // Mix the fade off
            return ProcessOnlyFocusFadeOff(inputPixels, width, height);
        }

        private void CreateBlurredBitmap(int[] inputPixels, int width, int height)
        {
            // Increase contrast
            contrastedPixels = contrastFx.Process(inputPixels, width, height);

            // Blur 
            blurredPixels = blurFx.Process(contrastedPixels, width, height);
        }

        /// <summary>
        /// Uses the cached saturated and the blurred bitmap and only mixes them together based on the focus fall / fade off values.
        /// The input bitmap is only used if no input was previously cached.
        /// </summary>
        /// <param name="input">The input bitmap.</param>
        /// <returns>The result of WriteabelBitmap processing.</returns>
        public WriteableBitmap ProcessOnlyFocusFadeOff(WriteableBitmap input)
        {
            // Prepare some variables
            var width = input.PixelWidth;
            var height = input.PixelHeight;
            return ProcessOnlyFocusFadeOff(input.Pixels, width, height).ToWriteableBitmap(width, height);
        }

        /// <summary>
        /// Uses the cached saturated and the blurred bitmap and only mixes them together based on the focus fall / fade off values.
        /// The input bitmap is only used if no input was previously cached.
        /// </summary>
        /// <param name="inputPixels">The input bitmap as integer array.</param>
        /// <param name="width">The width of the bitmap.</param>
        /// <param name="height">The height of the bitmap.</param>
        /// <returns>The result of the processing.</returns>
        public int[] ProcessOnlyFocusFadeOff(int[] inputPixels, int width, int height)
        {
            // Check if the cache is empty
            if (contrastedPixels == null || blurredPixels == null)
            {
                CreateBlurredBitmap(inputPixels, width, height);
            }

            var resultPixels = blurredPixels;

            // If not fully blurred?
            if (UpperFallOff < LowerFallOff)
            {
                // Prepare some variables
                resultPixels = new int[inputPixels.Length];

                // Calculate fade area
                var uf = (int)(UpperFallOff * height);
                var lf = (int)(LowerFallOff * height);
                var fo = ((lf - uf) >> 1);
                var mf = uf + fo;
                var mfu = mf;
                var mfl = mf;

                // Limit fall off and calc inverse
                if (fo > height * MaxFallOffFactor)
                {
                    fo = (int)(height * MaxFallOffFactor);
                    mfu = uf + fo;
                    mfl = lf - fo;
                }
                var ifo = 1f / fo;


                // Blend
                var index = 0;
                for (var y = 0; y < height; y++)
                {
                    for (var x = 0; x < width; x++)
                    {
                        var c2 = contrastedPixels[index];

                        // Above or below the fading area
                        if (y < mfu || y > mfl)
                        {
                            var c = blurredPixels[index];

                            // Inside the fading area, but not in the focused area
                            if (y > uf || y < lf)
                            {
                                // Extract color components
                                var a1 = (byte)(c >> 24);
                                var r1 = (byte)(c >> 16);
                                var g1 = (byte)(c >> 8);
                                var b1 = (byte)(c);

                                var a2 = (byte)(c2 >> 24);
                                var r2 = (byte)(c2 >> 16);
                                var g2 = (byte)(c2 >> 8);
                                var b2 = (byte)(c2);

                                // Calculate blending
                                float m = y < mf ? (mfu - y) : (y - mfl);
                                m *= ifo;
                                if (m > 1)
                                {
                                    m = 1f;
                                }
                                var mi = 1 - m;

                                // Mix it!
                                c = ((byte)(a1 * m + a2 * mi) << 24) |
                                    ((byte)(r1 * m + r2 * mi) << 16) |
                                    ((byte)(g1 * m + g2 * mi) << 8) |
                                    ((byte)(b1 * m + b2 * mi));
                            }

                            // Set result color
                            resultPixels[index] = c;
                        }
                        else
                        {
                            resultPixels[index] = c2;
                        }
                        index++;
                    }
                }
            }

            return resultPixels;
        }
    }
}