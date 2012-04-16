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
using System.Windows.Media.Imaging;

using PictureEffects.Effects;
using MobileClientLibrary;
using System.IO;
using MobileClientLibrary.Common;
using MobileClientLibrary.Models;
using System.IO.IsolatedStorage;

using Newtonsoft.Json;
using Hammock.Authentication.OAuth;
using Hammock;
using Hammock.Web;
using System.Text;
using JeffWilcox.FourthAndMayor;
using Microsoft.Xna.Framework.Media;
using System.Windows.Navigation;
using Hammock.Silverlight.Compat;
using TweetSharp;
using Coding4Fun.Phone.Controls;

namespace MetrocamPan
{
    public partial class UploadPage : PhoneApplicationPage
    {
        public UploadPage()
        {
            InitializeComponent();

            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.RemoveBackEntry();
            });

            if (Settings.twitterAuth.Value)
            {
                twitterSwitch.IsEnabled = true;

                if (Settings.twitterDefault.Value)
                    twitterSwitch.IsChecked = true;
            }
        }

        private void captionKey(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.Focus();
            }
        }

        private Boolean isUploading = false;
        private void Upload_Click(object sender, EventArgs e)
        {
            if (!isUploading)
                isUploading = true;
            else
                return;

            captionBox.IsReadOnly = true;

            GlobalLoading.Instance.IsLoading = true;

            // authenticate with user's credentials
            App.MetrocamService.AuthenticateCompleted += new RequestCompletedEventHandler(client_AuthenticateCompleted);
            App.MetrocamService.Authenticate(Settings.username.Value, Settings.password.Value);
        }

        MemoryStream ms = null;
        private void client_AuthenticateCompleted(object sender, RequestCompletedEventArgs e)
        {
            // unregister previous event handler
            App.MetrocamService.AuthenticateCompleted -= client_AuthenticateCompleted;

            WriteableBitmap bitmap = new WriteableBitmap((BitmapSource)EditPicture.editedPicture.Source);

            var width = bitmap.PixelWidth * 4;
            var height = bitmap.PixelHeight * 4;
            //var resultPixels = effect.Process(bitmap.Pixels, width, height);

            ms = new MemoryStream();
            bitmap.SaveJpeg(ms, width, height, 0, 100);
            ms.Seek(0, SeekOrigin.Begin);

            /******
             * 
             *  save original photo to phone
             * 
             */
            long timestamp = DateTime.Now.ToFileTime();
            String originalFilename = "MetrocamOriginal_" + timestamp.ToString() + ".jpg";

            var myStore = IsolatedStorageFile.GetUserStoreForApplication();

            var lib = new MediaLibrary();

            if (Settings.saveOriginal.Value && MainPage.tookPhoto)
            {
                IsolatedStorageFileStream myFileStream = myStore.CreateFile(originalFilename);
                WriteableBitmap w = new WriteableBitmap((BitmapSource)MainPage.bmp);
                w.SaveJpeg(myFileStream, w.PixelWidth, w.PixelHeight, 0, 100);
                myFileStream.Close();

                myFileStream = myStore.OpenFile(originalFilename, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                lib.SavePictureToCameraRoll(originalFilename, myFileStream);
            }


            // upload the image
            App.MetrocamService.UploadPictureCompleted += new RequestCompletedEventHandler(client_UploadPictureCompleted);
            App.MetrocamService.UploadPicture(ms);
        }

        private void client_UploadPictureCompleted(object sender, RequestCompletedEventArgs e)
        {
            // unregister previous event handler
            App.MetrocamService.UploadPictureCompleted -= client_UploadPictureCompleted;

            // extract response
            PictureURL result = e.Data as PictureURL;

            // create new picture
            MobileClientLibrary.Models.Picture data = new MobileClientLibrary.Models.Picture();

            Dispatcher.BeginInvoke(() =>
            {
                data.Caption = this.captionBox.Text;

                if (Settings.locationService.Value)
                {
                    data.Latitude = Convert.ToDecimal(MainPage.lat);
                    data.Longitude = Convert.ToDecimal(MainPage.lng);
                }
                else
                {
                    data.Latitude = Convert.ToDecimal(0.00);
                    data.Longitude = Convert.ToDecimal(0.00);
                }

                data.LargeURL = result.LargeURL;
                data.MediumURL = result.MediumURL;
                data.SmallURL = result.SmallURL;

                // upload the picture object
                App.MetrocamService.CreatePictureCompleted += new RequestCompletedEventHandler(client_CreatePictureCompleted);
                App.MetrocamService.CreatePicture(data);
            });
        }

        private void PostTweetRequestCallback(RestRequest request, Hammock.RestResponse response, object obj)
        {
            ;
        }

        private void client_CreatePictureCompleted(object sender, RequestCompletedEventArgs e)
        {
            GlobalLoading.Instance.IsLoading = false;
            isUploading = false;

            MobileClientLibrary.Models.Picture data = e.Data as MobileClientLibrary.Models.Picture;

            // unregister previous event handler
            App.MetrocamService.CreatePictureCompleted -= client_CreatePictureCompleted;

            if (twitterSwitch.IsChecked == true)
            {
                TwitterService t = new TwitterService(TwitterSettings.ConsumerKey, TwitterSettings.ConsumerKeySecret, MainPage.TwitterToken, MainPage.TwitterSecret);
                t.SendTweet(data.Caption + " " + "http://metrocam.cloudapp.net/p/" + data.ID, (status, response) =>
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                         // YAY
                    }
                });

            //    var _credentials = new OAuthCredentials
            //    {
            //        Type = OAuthType.AccessToken,
            //        SignatureMethod = OAuthSignatureMethod.HmacSha1,
            //        ParameterHandling = OAuthParameterHandling.HttpAuthorizationHeader,
            //        ConsumerKey = TwitterSettings.ConsumerKey,
            //        ConsumerSecret = TwitterSettings.ConsumerKeySecret,
            //        Token = MainPage.TwitterToken,
            //        TokenSecret = MainPage.TwitterSecret,
            //        Version = TwitterSettings.OAuthVersion,
            //    };

            //    var client = new RestClient
            //    {
            //        Authority = "https://upload.twitter.com",
            //        HasElevatedPermissions = true
            //    };

            //    var requestPath = "/1/statuses/update_with_media.xml";
            //    var request = new RestRequest
            //    {
            //        Credentials = _credentials,
            //        Path = requestPath,
            //        Method = WebMethod.Post
            //    };

            //    request.AddParameter("status", data.Caption);
            //    request.AddFile("media[]", "TweetPhoto.jpg", ms, "image/jpeg");
            //    client.BeginRequest(request, new RestCallback(PhotoTweetCompleted));
            }

            Dispatcher.BeginInvoke(() =>
            {
                // This flag is needed for MainPage to clear back stack
                App.isFromUploadPage = true; 

                NavigationService.Navigate(new Uri("/MainPage.xaml?from=upload", UriKind.Relative));
            });
        }

        //private void PhotoTweetCompleted(RestRequest request, RestResponse response, object userstate)
        //{
        //    if (response.StatusCode == HttpStatusCode.OK)
        //    {
        //    }
        //}

        private void Cancel_Click(object sender, EventArgs e)
        {
            App.isFromUploadPage = true;
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            if (GlobalLoading.Instance.IsLoading)
                GlobalLoading.Instance.IsLoading = false;
        }
    }
}