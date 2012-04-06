using PictureEffects.Effects;
using System;
using System.Windows.Media.Imaging;

namespace PictureEffects.CompositeEffects
{
    public class OriginalEffect : IEffect
    {
        public string Name
        {
            get
            {
                return "Original";
            }
        }

        public OriginalEffect()
        {

        }

        public WriteableBitmap Process(WriteableBitmap input)
        {
            return input;
        }

        public int[] Process(int[] inputPixels, int width, int height)
        {
            return inputPixels;
        }
    }
}
