using System;
using System.Windows.Media.Imaging;

namespace PictureEffects.Effects
{
    /// <summary>
    /// Tinting effect.
    /// </summary>
    public class BitmapMixer
    {
        /// <summary>
        /// The mixture range should be [0, 1]. 
        /// Where 0 means input1 is fully visible and 1 that input2 is shown. Everything in between means a mix of both.
        /// </summary>
        public float Mixture { get; set; }

        public BitmapMixer()
        {
            Mixture = 0.5f;
        }

        /// <summary>
        /// Mixes two bitmaps with the same size (!) and returns a new mixed WriteabelBitmap.
        /// </summary>
        /// <param name="input1">The first WriteableBitmap.</param>
        /// <param name="input2">The second WriteableBitmap.</param>
        /// <returns>The result of WriteabelBitmap mixing.</returns>
        public WriteableBitmap Mix(WriteableBitmap input1, WriteableBitmap input2)
        {
            // Prepare some variables
            var width = input1.PixelWidth;
            var height = input1.PixelHeight;
            return Mix(input1.Pixels, input2.Pixels, width, height).ToWriteableBitmap(width, height);
        }

        /// <summary>
        /// Mixes two ARGB32 integer bitmaps with the same size (!) and returns the new mixed bitmap data.
        /// </summary>
        /// <param name="inputPixels1">The first input bitmap as integer array.</param>
        /// <param name="inputPixels2">The second input bitmap as integer array.</param>
        /// <param name="width">The width of the bitmap.</param>
        /// <param name="height">The height of the bitmap.</param>
        /// <returns>The result of the mixing.</returns>
        public int[] Mix(int[] inputPixels1, int[] inputPixels2, int width, int height)
        {
            // Prepare some variables
            var resultPixels = new int[inputPixels1.Length];
            var m = Mixture;
            var mi = 1 - m;

            for (var i = 0; i < inputPixels1.Length; i++)
            {
                // Extract color components
                var c1 = inputPixels1[i];
                var a1 = (byte)(c1 >> 24);
                var r1 = (byte)(c1 >> 16);
                var g1 = (byte)(c1 >> 8);
                var b1 = (byte)(c1);

                var c2 = inputPixels2[i];
                var a2 = (byte)(c2 >> 24);
                var r2 = (byte)(c2 >> 16);
                var g2 = (byte)(c2 >> 8);
                var b2 = (byte)(c2);

                // Mix it!
                var d = ((byte)(a1 * mi + a2 * m) << 24) |
                        ((byte)(r1 * mi + r2 * m) << 16) |
                        ((byte)(g1 * mi + g2 * m) << 8) |
                        ((byte)(b1 * mi + b2 * m));

                // Set result color
                resultPixels[i] = d;
            }

            return resultPixels;
        }
    }
}