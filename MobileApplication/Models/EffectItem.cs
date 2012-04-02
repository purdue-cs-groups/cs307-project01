using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using PictureEffects.Effects;

namespace MetrocamPan.Models
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
