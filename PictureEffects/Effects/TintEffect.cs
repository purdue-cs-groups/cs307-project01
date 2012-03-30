using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PictureEffects.Effects
{
    /// <summary>
    /// Tinting effect.
    /// </summary>
    public class TintEffect : IEffect
    {
        public static TintEffect White
        {
            get { return new TintEffect { Color = Colors.White }; }
        }

        public static TintEffect Sepia
        {
            get { return new TintEffect { Color = Color.FromArgb(255, 230, 179, 77) }; }
        }

        public string Name { get { return "Tint"; } }

        public Color Color { get; set; }

        public TintEffect()
        {
            Color = Colors.White;
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
            var ta = Color.A;
            var tr = Color.R;
            var tg = Color.G;
            var tb = Color.B;

            for (int i = 0; i < inputPixels.Length; i++)
            {
                // Extract color components
                var c = inputPixels[i];
                var a = (byte)(c >> 24);
                var r = (byte)(c >> 16);
                var g = (byte)(c >> 8);
                var b = (byte)(c);

                // Convert to gray with constant factors 0.2126, 0.7152, 0.0722
                var gray = (r * 6966 + g * 23436 + b * 2366) >> 15;

                // Apply Tint color
                a = (byte)((a * ta) >> 8);
                r = (byte)((gray * tr) >> 8);
                g = (byte)((gray * tg) >> 8);
                b = (byte)((gray * tb) >> 8);

                // Set result color
                resultPixels[i] = (a << 24) | (r << 16) | (g << 8) | b;
            }

            return resultPixels;
        }
    }
}