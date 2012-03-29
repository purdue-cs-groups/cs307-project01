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

namespace SampleApplication
{
    public partial class MainPage : PhoneApplicationPage
    {
        WebServiceClient client = null;

        public MainPage()
        {
            InitializeComponent();

            client = new WebServiceClient("4f5685ce5ad9850e545bb48d");

            // login with user's credentials
            client.AuthenticateCompleted += new RequestCompletedEventHandler(client_AuthenticateCompleted);
            client.Authenticate("mbmccormick", "password");
        }

        private void client_AuthenticateCompleted(object sender, RequestCompletedEventArgs e)
        {
            StreamResourceInfo image = Application.GetResourceStream(new Uri("/SampleApplication;component/sample.jpg", UriKind.Relative)); 
            
            // upload the image
            client.UploadPictureCompleted += new RequestCompletedEventHandler(client_UploadPictureCompleted);
            client.UploadPicture(image.Stream);
        }

        private void client_UploadPictureCompleted(object sender, RequestCompletedEventArgs e)
        {
            // extract response
            PictureURL result = e.Data as PictureURL;

            // create new picture
            Picture data = new Picture();

            data.Caption = "This is the first Winstagram picture ever!";
            data.Latitude = Convert.ToDecimal(40.446980);
            data.Longitude = Convert.ToDecimal(-86.944189);
            data.LargeURL = result.LargeURL;
            data.MediumURL = result.MediumURL;
            data.SmallURL = result.SmallURL;

            // upload the picture object
            client.CreatePictureCompleted += new RequestCompletedEventHandler(client_CreatePictureCompleted);
            client.CreatePicture(data);
        }

        private void client_CreatePictureCompleted(object sender, RequestCompletedEventArgs e)
        {
            // your picture was uploaded successfully!
        }
    }
}