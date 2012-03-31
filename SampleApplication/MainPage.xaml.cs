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
using System.IO;


namespace SampleApplication
{
    public partial class MainPage : PhoneApplicationPage
    {
        WebServiceClient client = null;
        WriteableBitmap originalImage = null;

        public MainPage()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);

            client = new WebServiceClient("4f5685ce5ad9850e545bb48d");
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (originalImage == null)
            {
                StreamResourceInfo resource = Application.GetResourceStream(new Uri("/SampleApplication;component/sample.jpg", UriKind.Relative));

                BitmapImage source = new BitmapImage();
                source.SetSource(resource.Stream);

                originalImage = new WriteableBitmap(source);

                this.imgPicture.Source = originalImage;
            }
        }

        private void lstFilters_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.imgPicture == null) return;

            Models.EffectItem item = (Models.EffectItem)this.lstFilters.SelectedItem;
            IEffect effect = item.Effect;

            WriteableBitmap bitmap = originalImage;

            if (bitmap == null) return;

            var width = bitmap.PixelWidth;
            var height = bitmap.PixelHeight;
            var resultPixels = effect.Process(bitmap.Pixels, width, height);

            WriteableBitmap newBitmap = resultPixels.ToWriteableBitmap(width, height);

            this.imgPicture.Source = newBitmap;
        }

        private void btnUpload_Click(object sender, RoutedEventArgs e)
        {
            // login with user's credentials
            client.AuthenticateCompleted += new RequestCompletedEventHandler(client_AuthenticateCompleted);
            client.Authenticate("mbmccormick", "password");
        }

        private void client_AuthenticateCompleted(object sender, RequestCompletedEventArgs e)
        {
            // unregister previous event handler
            client.AuthenticateCompleted -= client_AuthenticateCompleted;

            if (this.imgPicture == null) return;

            Models.EffectItem item = (Models.EffectItem)this.lstFilters.SelectedItem;
            IEffect effect = item.Effect;

            WriteableBitmap bitmap = originalImage;

            if (bitmap.PixelWidth == 0 ||
                bitmap.PixelHeight == 0) return;

            var width = bitmap.PixelWidth;
            var height = bitmap.PixelHeight;
            var resultPixels = effect.Process(bitmap.Pixels, width, height);

            WriteableBitmap newBitmap = resultPixels.ToWriteableBitmap(width, height);

            MemoryStream ms = new MemoryStream();
            newBitmap.SaveJpeg(ms, width, height, 0, 100);

            ms.Seek(0, SeekOrigin.Begin);

            // upload the image
            client.UploadPictureCompleted += new RequestCompletedEventHandler(client_UploadPictureCompleted);
            client.UploadPicture(ms);
        }

        private void client_UploadPictureCompleted(object sender, RequestCompletedEventArgs e)
        {
            // unregister previous event handler
            client.UploadPictureCompleted -= client_UploadPictureCompleted;

            // extract response
            PictureURL result = e.Data as PictureURL;

            // create new picture
            Picture data = new Picture();

            Dispatcher.BeginInvoke(() =>
            {
                data.Caption = this.txtCaption.Text;

                data.Latitude = Convert.ToDecimal(40.446980);
                data.Longitude = Convert.ToDecimal(-86.944189);
                data.LargeURL = result.LargeURL;
                data.MediumURL = result.MediumURL;
                data.SmallURL = result.SmallURL;

                // upload the picture object
                client.CreatePictureCompleted += new RequestCompletedEventHandler(client_CreatePictureCompleted);
                client.CreatePicture(data);
            });
        }

        private void client_CreatePictureCompleted(object sender, RequestCompletedEventArgs e)
        {
            // unregister previous event handler
            client.CreatePictureCompleted -= client_CreatePictureCompleted;

            Dispatcher.BeginInvoke(() =>
            {
                MessageBox.Show("Your picture was uploaded successfully!");
            });
        }
    }
}