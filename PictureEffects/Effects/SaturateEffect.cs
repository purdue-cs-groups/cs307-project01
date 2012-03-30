using System;
using System.Windows.Media.Imaging;
using System.Diagnostics;
using HslColorConversion;
using System.Windows.Media;

namespace PictureEffects.Effects
{
    public class SaturateEffect : IEffect
    {
        public float LightnessFactor
        {
            get;
            set;
        }

        public string Name
        {
            get
            {
                return "Saturation";
            }
        }

        public float SatEffect
        {
            get;
            set;
        }

        public SaturateEffect()
        {
            this.SatEffect = 0;
            this.LightnessFactor = 0;
        }

        public WriteableBitmap Process(WriteableBitmap input)
        {
            int pixelWidth = input.PixelWidth;
            int pixelHeight = input.PixelHeight;
            return this.Process(input.Pixels, pixelWidth, pixelHeight).ToWriteableBitmap(pixelWidth, pixelHeight);
        }

        public int[] Process(int[] inputPixels, int width, int height)
        {
            int[] numArray = new int[inputPixels.Length];

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            
            for (int i = 0; i < inputPixels.Length; i++)
            {
                var c = inputPixels[i];

                var a = (byte)(c >> 24);
                var r = (byte)(c >> 16);
                var g = (byte)(c >> 8);
                var b = (byte)(c);

                HslColor hSLAndSaturateAndLighten = HslColor.ConvertToHSLAndSaturateAndLighten(a, r, g, b, (double)this.SatEffect, (double)this.LightnessFactor);
                
                Color color1 = hSLAndSaturateAndLighten.ToColor();
                a = color1.A;

                Color color2 = hSLAndSaturateAndLighten.ToColor();
                r = color2.R;

                Color color3 = hSLAndSaturateAndLighten.ToColor();
                g = color3.G;

                Color color4 = hSLAndSaturateAndLighten.ToColor();
                b = color4.B;

                numArray[i] = (a << 24) | (r << 16) | (g << 8) | b;
            }

            stopwatch.Stop();

            return numArray;
        }
    }
}