using System.Windows.Media.Imaging;

namespace System
{
    /// <summary>
    /// WriteableBitmap extension for WP7 media library
    /// </summary>
    public static class ArrayExtensions
    {

        /// <summary>
        /// Creates a WriteableBitmap from an integer array that is interpreted as ARGB32.
        /// </summary>
        /// <param name="input">The pixels as ARGB32.</param>
        /// <param name="width">The width of the bitmap.</param>
        /// <param name="height">The height of the bitmap.</param>
        /// <returns>The result WriteableBitmap.</returns>
        public static WriteableBitmap ToWriteableBitmap(this int[] input, int width, int height)
        {
            var result = new WriteableBitmap(width, height);
            Buffer.BlockCopy(input, 0, result.Pixels, 0, input.Length * 4);
            return result;
        }
    }
}