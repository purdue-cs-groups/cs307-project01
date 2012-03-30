using System;
using System.Windows.Media.Imaging;

namespace PictureEffects.Effects
{
    /// <summary>
    /// Adds a round vignette (gets darker to the edges).
    /// </summary>
    public class VignetteEffect : IEffect
    {
        public string Name { get { return "Vignette"; } }

        /// <summary>
        /// Should be in the range [0, 1].
        /// </summary>
        public float Size { get; set; }

        public VignetteEffect()
        {
            Size = 0.5f;
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
            // Prepare some variables
            var resultPixels = new int[inputPixels.Length];
            var ratio = width > height ? height * 32768 / width : width * 32768 / height;

            // Calculate center, min and max
            var cx = width >> 1;
            var cy = height >> 1;
            var max = cx * cx + cy * cy;
            var min = (int)(max * (1 - Size));
            var diff = max - min;

            var index = 0;
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var c = inputPixels[index];

                    // Extract color components
                    var a = (byte)(c >> 24);
                    var r = (byte)(c >> 16);
                    var g = (byte)(c >> 8);
                    var b = (byte)(c);

                    // Calculate distance to center and adapt aspect ratio
                    var dx = cx - x;
                    var dy = cy - y;
                    if (width > height)
                    {
                        dx = (dx * ratio) >> 15;
                    }
                    else
                    {
                        dy = (dy * ratio) >> 15;
                    }
                    var distSq = dx * dx + dy * dy;

                    if (distSq > min)
                    {
                        // Calculate vignette
                        var v = ((max - distSq) << 8) / diff;
                        v *= v;

                        // Apply vignette
                        var ri = (r * v) >> 16;
                        var gi = (g * v) >> 16;
                        var bi = (b * v) >> 16;

                        // Check bounds
                        r = (byte)(ri > 255 ? 255 : (ri < 0 ? 0 : ri));
                        g = (byte)(gi > 255 ? 255 : (gi < 0 ? 0 : gi));
                        b = (byte)(bi > 255 ? 255 : (bi < 0 ? 0 : bi));

                        // Combine components
                        c = (a << 24) | (r << 16) | (g << 8) | b;
                    }

                    resultPixels[index] = c;
                    index++;
                }
            }

            return resultPixels;
        }
    }
}