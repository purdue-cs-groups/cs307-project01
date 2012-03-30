using System.Windows.Media;
using System.Windows.Media.Imaging;
using PictureEffects.Effects;

namespace SampleApplication.Models
{
    /// <summary>
    /// A generic effect item.
    /// </summary>
    public class EffectItem
    {
        public IEffect Effect { get; private set; }
        public string Name { get; private set; }
        
        public EffectItem(IEffect effect)
        {
            Effect = effect;
            Name = effect.Name;
        }
    }
}