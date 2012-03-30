using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using MobileClientLibrary;
using MobileClientLibrary.Common;
using MobileClientLibrary.Models;
using System.Windows.Resources;
using System.Windows.Media.Imaging;
using System.Threading;
using System.Collections.ObjectModel;
using SampleApplication.Models;
using PictureEffects.Effects;


namespace SampleApplication
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();

            BitmapImage image = new BitmapImage(new Uri("/SampleApplication;component/sample.jpg", UriKind.Relative));
            this.imgPicture.Source = image;
        }

        private void btnFilter_Click(object sender, RoutedEventArgs e)
        {
            BitmapImage image = new BitmapImage(new Uri("/SampleApplication;component/sample.jpg", UriKind.Relative));
            this.imgPicture.Source = image;

            Models.EffectItem item = (Models.EffectItem)this.lstFilters.SelectedItem;
            IEffect effect = item.Effect;

            WriteableBitmap bitmap = new WriteableBitmap(this.imgPicture, null);

            var width = bitmap.PixelWidth;
            var height = bitmap.PixelHeight;
            var resultPixels = effect.Process(bitmap.Pixels, width, height);

            WriteableBitmap newBitmap = resultPixels.ToWriteableBitmap(width, height);
            this.imgPicture.Source = newBitmap;
        }
    }
}