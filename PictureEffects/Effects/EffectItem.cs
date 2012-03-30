using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PictureEffects.Effects
{
    public class EffectItem
    {
        public IEffect Effect
        {
            get;
            private set;
        }

        public string Name
        {
            get;
            private set;
        }

        public ImageSource Thumbnail
        {
            get;
            set;
        }

        public EffectItem(IEffect effect)
        {
            this.Effect = effect;
            this.Name = effect.Name;
        }

        public EffectItem(IEffect effect, string thumbnailRelativeResourcePath)
        {
            this.Thumbnail = new WriteableBitmap(0, 0).FromResource(thumbnailRelativeResourcePath);
        }

        public EffectItem(IEffect effect, string thumbnailRelativeResourcePath, string name)
        {
            this.Name = name;
        }
    }
}