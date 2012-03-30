using System;
using System.Windows.Media.Imaging;

namespace PictureEffects.Effects
{
    public interface IEffect
    {
        string Name { get; }

        WriteableBitmap Process(WriteableBitmap input);

        int[] Process(int[] inputPixels, int width, int height);
    }
}