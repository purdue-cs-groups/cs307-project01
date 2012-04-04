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

using System.Collections.ObjectModel;
using PictureEffects.Effects;
using PictureEffects.CompositeEffects;

namespace MetrocamPan.Models
{
    public class EffectItems : ObservableCollection<EffectItem>
    {
        public EffectItems()
        {
            // Sort them in whatever order that will be displayed in EditPicture.xaml
            // Concurrently sorted alphabetically
            Add(new Models.EffectItem(new AntiqueEffect()));
            Add(new Models.EffectItem(new BettyEffect()));
            Add(new Models.EffectItem(new BlackWhiteEffect()));
            Add(new Models.EffectItem(new CyanEffect()));
            Add(new Models.EffectItem(new DawnEffect()));
            Add(new Models.EffectItem(new GoGoGaGaEffect()));
            Add(new Models.EffectItem(new GrungeEffect()));
            Add(new Models.EffectItem(new PolaroidEffect()));
            Add(new Models.EffectItem(new PopEffect()));
            Add(new Models.EffectItem(new RusticEffect()));
            Add(new Models.EffectItem(new StormEffect()));
            Add(new Models.EffectItem(new TiltShiftEffect()));
            Add(new Models.EffectItem(new WasabiEffect()));
            Add(new Models.EffectItem(new YoshimotoEffect()));
        }
    }
}
