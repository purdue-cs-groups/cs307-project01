using System.Windows.Media.Imaging;
using System;

namespace PictureEffects.Effects
{
    /// <summary>
    /// A fast Gaussian Blur implementation. 
    /// Paper: Young, I.T. & van Vliet,L.J, 1995 "Recursive implementation of the Gaussian filter". 
    /// Adapted from http://www.planetmarshall.co.uk/2010/01/silverlight-and-cuda-interop/comment-page-1 by Andrew Marshall.
    /// </summary>
    public class GaussianBlurEffect : IEffect
    {
        const int Padding = 3;
        const int BytesPerPixel = 4;

        public string Name { get { return "Blur"; } }

        /// <summary>
        /// The bluriness factor. 
        /// Should be in the range [0, 40].
        /// </summary>
        public float Sigma { get; set; }

        public GaussianBlurEffect()
        {
            Sigma = 1;
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

            // Copy int pixels to float array
            var srcPixels = ConvertImageWithPadding<int, float>(inputPixels, width, height, Padding, unchecked((int)0xff000000), ArgbIntToFloat);

            // Apply blur
            var destPixels = ApplyBlur(srcPixels, width, height, Sigma);

            // Convert result float back to int
            for (int y = 0; y < height; y++)
            {
                int sourceOffset = (y + Padding) * (width + Padding * 2) + Padding;
                int targetOffset = y * width;

                for (int x = 0; x < width; x++)
                {
                    // calulate index
                    int idx = (sourceOffset + x) * BytesPerPixel;

                    // Preserve alpha
                    var a = inputPixels[targetOffset + x] >> 24;

                    // Set pixels
                    resultPixels[targetOffset + x] =
                        (byte)(a) << 24 |
                        (byte)(destPixels[idx + 1] * 255) << 16 |
                        (byte)(destPixels[idx + 2] * 255) << 8 |
                        (byte)(destPixels[idx + 3] * 255);
                }
            }

            return resultPixels;
        }

        static float[] ApplyBlur(float[] srcPixels, int width, int height, float sigma)
        {
            var destPixels = new float[srcPixels.Length];
            Array.Copy(srcPixels, destPixels, destPixels.Length);

            int w = width + 6;
            int h = height + 6;

            // Calculate the coefficients
            float q = sigma;
            float q2 = q * q;
            float q3 = q2 * q;

            float b0 = 1.57825f + 2.44413f * q + 1.4281f * q2 + 0.422205f * q3;
            float b1 = 2.44413f * q + 2.85619f * q2 + 1.26661f * q3;
            float b2 = -(1.4281f * q2 + 1.26661f * q3);
            float b3 = 0.422205f * q3;

            float b = 1.0f - ((b1 + b2 + b3) / b0);

            // Apply horizontal pass
            ApplyPass(destPixels, w, h, b0, b1, b2, b3, b);

            // Transpose the array
            var transposedPixels = new float[destPixels.Length];
            Transpose(destPixels, transposedPixels, w, h);

            // Apply vertical pass
            ApplyPass(transposedPixels, h, w, b0, b1, b2, b3, b);

            // transpose back
            Transpose(transposedPixels, destPixels, h, w);

            return destPixels;
        }

        static void ApplyPass(float[] pixels, int width, int height, float b0, float b1, float b2, float b3, float b)
        {
            float ib0 = 1.0f / b0;
            const int bytepadding = Padding * BytesPerPixel;
            int stride = width * BytesPerPixel;
            for (int j = 0; j < height; j++)
            {
                int offset = j * stride;

                // Filter forward
                for (int i = offset + bytepadding; i < offset + stride; i += BytesPerPixel)
                {
                    FilterForward(i + 1, pixels, b, b1, b2, b3, ib0);
                    FilterForward(i + 2, pixels, b, b1, b2, b3, ib0);
                    FilterForward(i + 3, pixels, b, b1, b2, b3, ib0);

                }

                // Filter back
                for (int i = offset + stride - bytepadding - BytesPerPixel; i >= offset; i -= BytesPerPixel)
                {
                    FilterBackward(i + 1, pixels, b, b1, b2, b3, ib0);
                    FilterBackward(i + 2, pixels, b, b1, b2, b3, ib0);
                    FilterBackward(i + 3, pixels, b, b1, b2, b3, ib0);
                }
            }
        }

        static void FilterForward(int i, float[] pixels, float b, float b1, float b2, float b3, float ib0)
        {
            pixels[i] = b * pixels[i] + (b1 * pixels[i - 4] + b2 * pixels[i - 8] + b3 * pixels[i - 12]) * ib0;
        }

        static void FilterBackward(int i, float[] pixels, float b, float b1, float b2, float b3, float ib0)
        {
            pixels[i] = b * pixels[i] + (b1 * pixels[i + 4] + b2 * pixels[i + 8] + b3 * pixels[i + 12]) * ib0;
        }

        #region Helpers

        static void Transpose<T>(T[] input, T[] output, int width, int height)
        {
            for (int j = 0; j < height; ++j)
            {
                int rowOffset = j * width * BytesPerPixel;
                for (int i = 0; i < width; ++i)
                {
                    int colOffset = i * height * BytesPerPixel;
                    output[colOffset + j * BytesPerPixel] = input[rowOffset + i * BytesPerPixel];
                    output[colOffset + j * BytesPerPixel + 1] = input[rowOffset + i * BytesPerPixel + 1];
                    output[colOffset + j * BytesPerPixel + 2] = input[rowOffset + i * BytesPerPixel + 2];
                    output[colOffset + j * BytesPerPixel + 3] = input[rowOffset + i * BytesPerPixel + 3];
                }
            }
        }

        /// <summary>
        /// Copy an image with the required padding using the supplied conversion function, repeating the edge pixels
        /// </summary>
        /// <typeparam name="TS">The source array type</typeparam>
        /// <typeparam name="T">The target array type</typeparam>
        /// <param name="source">The source array</param>
        /// <param name="width">The width of the source image</param>
        /// <param name="height">The height of the source image</param>
        /// <param name="padding">The amount of padding to provide on each side of the image</param>
        /// <param name="paddedValue">The value with which to fill the corner spaces</param>
        /// <param name="decompose">A function decomposing a source value into the target array at the supplied offset</param>
        /// <returns>The target array</returns>
        static T[] ConvertImageWithPadding<TS, T>(TS[] source, int width, int height, int padding, TS paddedValue, Action<TS, T[], int> decompose)
        {
            int targetHeight = height + 2 * padding;
            int targetWidth = width + 2 * padding;
            var target = new T[targetHeight * targetWidth * BytesPerPixel];

            for (int j = 0; j < padding; ++j)
            {
                int targetOffset = targetWidth * j;
                for (int i = 0; i < padding; ++i)
                {
                    decompose(paddedValue, target, (targetOffset + i) * BytesPerPixel);
                }
                for (int i = 0; i < width; ++i)
                {
                    decompose(source[i], target, (targetOffset + i + padding) * BytesPerPixel);
                }
                for (int i = width; i < width + padding; ++i)
                {
                    decompose(paddedValue, target, (targetOffset + i) * BytesPerPixel);
                }
            }

            for (int j = 0; j < height; ++j)
            {
                int targetOffset = targetWidth * (j + padding);
                int sourceOffset = width * j;
                for (int i = 0; i < targetWidth; ++i)
                {
                    if (i < padding)
                    {
                        decompose(source[sourceOffset], target, (i + targetOffset) * BytesPerPixel);

                    }
                    else if (i >= padding + width)
                    {
                        decompose(source[sourceOffset + width - 1], target, (i + targetOffset) * BytesPerPixel);
                    }
                    else
                    {
                        decompose(source[sourceOffset - padding + i], target, (i + targetOffset) * BytesPerPixel);
                    }
                }
            }

            for (int j = 0; j < padding; ++j)
            {
                int targetOffset = targetWidth * (height + padding + j);
                int sourceOffset = width * (height - 1);
                for (int i = 0; i < padding; ++i)
                {
                    decompose(paddedValue, target, (targetOffset + i) * BytesPerPixel);
                }
                for (int i = 0; i < width; ++i)
                {
                    decompose(source[sourceOffset + i], target, (targetOffset + i + padding) * BytesPerPixel);
                }
                for (int i = width; i < width + padding; ++i)
                {
                    decompose(paddedValue, target, (targetOffset + i) * BytesPerPixel);
                }
            }

            return target;
        }

        static void ArgbIntToFloat(int src, float[] tgt, int idx)
        {
            const float n = (float)(1.0 / 255.0);
            tgt[idx] = ((src >> 24) & 0xff) * n;
            tgt[idx + 1] = ((src >> 16) & 0xff) * n;
            tgt[idx + 2] = ((src >> 8) & 0xff) * n;
            tgt[idx + 3] = (src & 0xff) * n;
        }

        #endregion
    }
}