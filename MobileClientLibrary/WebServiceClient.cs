using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using MobileClientLibrary.Models;
using MobileClientLibrary.Common;
using System.Text;
using ClientLibrary.Common;
using System.Windows.Media.Imaging;
using System.IO;

namespace MobileClientLibrary
{
    #region RequestCompleted Event Declarations

    public delegate void RequestCompletedEventHandler(object sender, RequestCompletedEventArgs e);

    public class RequestCompletedEventArgs : EventArgs
    {
        public object Data { get; set; }

        public RequestCompletedEventArgs(object data)
            : base()
        {
            Data = data;
        }
    }

    #endregion

    public class WebServiceClient
    {
        private const string _WebServiceEndpoint = "http://metrocam.cloudapp.net/v1/";

        private string _APIKey = null;

        private bool _IsAuthenticated = false;
        private UserCredentials _Credentials = null;
        private string _Token = null;

        private const string defaultProfilePictureID = "4f89bfd1d47cd40ac4721f6d";

        public WebServiceClient(string APIkey)
        {
            _APIKey = APIkey;
        }

        public UserInfo CurrentUser
        {
            get;
            set;
        }

        #region Authentication Methods

        public event RequestCompletedEventHandler AuthenticateCompleted;

        public void Authenticate(string username, string password)
        {
            _Credentials = new UserCredentials(username, this.HashPassword(password));

            var jsonData = JsonConvert.SerializeObject(_Credentials);

            WebClient client = new WebClient();
            client.UploadStringCompleted += new UploadStringCompletedEventHandler(Authenticate_UploadStringCompleted);
            client.UploadStringAsync(new Uri(String.Format(_WebServiceEndpoint + "authenticate?key={0}", _APIKey)), jsonData);
        }

        private void Authenticate_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            if (AuthenticateCompleted != null)
            {
                if (e.Error == null)
                {
                    string stringData = e.Result;

                    var jsonData = JsonConvert.DeserializeObject<AuthenticationToken>(stringData);

                    _IsAuthenticated = true;
                    _Token = jsonData.Token;

                    CurrentUser = jsonData.User;

                    AuthenticateCompleted(sender, new RequestCompletedEventArgs(jsonData));
                }
                else
                {
                    WebException we = (WebException)e.Error;
                    HttpWebResponse response = (System.Net.HttpWebResponse)we.Response;

                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        AuthenticateCompleted(sender, new RequestCompletedEventArgs(new UnauthorizedAccessException()));
                        //throw new UnauthorizedAccessException("The User credentials provided were not valid.");
                    }
                }
            }
        }

        public string HashPassword(string password)
        {
            if (!string.IsNullOrWhiteSpace(password))
            {
                byte[] buffer = Encoding.UTF8.GetBytes(password);

                MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
                provider.ComputeHash(buffer);

                string hashed = string.Empty;
                foreach (byte b in provider.Hash)
                {
                    hashed += b.ToString("X2");
                }

                return hashed.ToLower();
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region Favorited Picture Methods

        public event RequestCompletedEventHandler FetchFavoritedPictureByPictureIDCompleted;

        public void FetchFavoritedPictureByPictureID(string pictureId, string userId)
        {
            if (_IsAuthenticated == false) throw new UnauthorizedAccessException("This method requires User authentication.");

            WebClient client = new WebClient();
            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(FetchFavoritedPictureByPictureID_DownloadStringCompleted);
            client.DownloadStringAsync(new Uri(String.Format(_WebServiceEndpoint + "favorites/pictures/fetch?key={0}&token={1}&pictureid={2}&userid={3}&ticks={4}", _APIKey, _Token, pictureId, userId, DateTime.Now.Ticks)));
        }

        private void FetchFavoritedPictureByPictureID_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (FetchFavoritedPictureByPictureIDCompleted != null)
            {
                if (e.Error == null)
                {
                    string stringData = e.Result;

                    var jsonData = JsonConvert.DeserializeObject<FavoritedPicture>(stringData);

                    FetchFavoritedPictureByPictureIDCompleted(sender, new RequestCompletedEventArgs(jsonData));
                }
                else
                {
                    WebException we = (WebException)e.Error;
                    HttpWebResponse response = (System.Net.HttpWebResponse)we.Response;

                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        throw new UnauthorizedAccessException("The Authentication Token has expired.");
                    }
                    else
                    {
                        throw e.Error;
                    }
                }
            }
        }

        public event RequestCompletedEventHandler FetchFavoritedPictureCompleted;

        public void FetchFavoritedPicture(string id)
        {
            if (_IsAuthenticated == false) throw new UnauthorizedAccessException("This method requires User authentication.");

            WebClient client = new WebClient();
            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(FetchFavoritedPicture_DownloadStringCompleted);
            client.DownloadStringAsync(new Uri(String.Format(_WebServiceEndpoint + "favorites/fetch?key={0}&token={1}&id={2}&ticks={3}", _APIKey, _Token, id, DateTime.Now.Ticks)));
        }

        private void FetchFavoritedPicture_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (FetchFavoritedPictureCompleted != null)
            {
                if (e.Error == null)
                {
                    string stringData = e.Result;

                    var jsonData = JsonConvert.DeserializeObject<FavoritedPicture>(stringData);

                    FetchFavoritedPictureCompleted(sender, new RequestCompletedEventArgs(jsonData));
                }
                else
                {
                    WebException we = (WebException)e.Error;
                    HttpWebResponse response = (System.Net.HttpWebResponse)we.Response;

                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        throw new UnauthorizedAccessException("The Authentication Token has expired.");
                    }
                    else
                    {
                        throw e.Error;
                    }
                }
            }
        }

        public event RequestCompletedEventHandler CreateFavoritedPictureCompleted;

        public void CreateFavoritedPicture(FavoritedPicture data)
        {
            if (_IsAuthenticated == false) throw new UnauthorizedAccessException("This method requires User authentication.");

            var jsonData = JsonConvert.SerializeObject(data);

            WebClient client = new WebClient();
            client.UploadStringCompleted += new UploadStringCompletedEventHandler(CreateFavoritedPicture_UploadStringCompleted);
            client.UploadStringAsync(new Uri(String.Format(_WebServiceEndpoint + "favorites/create?key={0}&token={1}", _APIKey, _Token)), jsonData);
        }

        private void CreateFavoritedPicture_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            if (CreateFavoritedPictureCompleted != null)
            {
                if (e.Error == null)
                {
                    string stringData = e.Result;

                    var jsonData = JsonConvert.DeserializeObject<FavoritedPicture>(stringData);

                    CreateFavoritedPictureCompleted(sender, new RequestCompletedEventArgs(jsonData));
                }
                else
                {
                    WebException we = (WebException)e.Error;
                    HttpWebResponse response = (System.Net.HttpWebResponse)we.Response;

                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        throw new UnauthorizedAccessException("The Authentication Token has expired.");
                    }
                    else
                    {
                        throw e.Error;
                    }
                }
            }
        }

        public event RequestCompletedEventHandler UpdateFavoritedPictureCompleted;

        public void UpdateFavoritedPicture(FavoritedPicture data)
        {
            if (_IsAuthenticated == false) throw new UnauthorizedAccessException("This method requires User authentication.");

            var jsonData = JsonConvert.SerializeObject(data);

            WebClient client = new WebClient();
            client.UploadStringCompleted += new UploadStringCompletedEventHandler(UpdateFavoritedPicture_UploadStringCompleted);
            client.UploadStringAsync(new Uri(String.Format(_WebServiceEndpoint + "favorites/update?key={0}&token={1}", _APIKey, _Token)), jsonData);
        }

        private void UpdateFavoritedPicture_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            if (UpdateFavoritedPictureCompleted != null)
            {
                if (e.Error == null)
                {
                    string stringData = e.Result;

                    var jsonData = JsonConvert.DeserializeObject<FavoritedPicture>(stringData);

                    UpdateFavoritedPictureCompleted(sender, new RequestCompletedEventArgs(jsonData));
                }
                else
                {
                    WebException we = (WebException)e.Error;
                    HttpWebResponse response = (System.Net.HttpWebResponse)we.Response;

                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        throw new UnauthorizedAccessException("The Authentication Token has expired.");
                    }
                    else
                    {
                        throw e.Error;
                    }
                }
            }
        }

        public event RequestCompletedEventHandler DeleteFavoritedPictureCompleted;

        public void DeleteFavoritedPicture(FavoritedPicture data)
        {
            if (_IsAuthenticated == false) throw new UnauthorizedAccessException("This method requires User authentication.");

            WebClient client = new WebClient();
            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(DeleteFavoritedPicture_DownloadStringCompleted);
            client.DownloadStringAsync(new Uri(String.Format(_WebServiceEndpoint + "favorites/delete?key={0}&token={1}&id={2}", _APIKey, _Token, data.ID)));
        }

        private void DeleteFavoritedPicture_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (DeleteFavoritedPictureCompleted != null)
            {
                if (e.Error == null)
                {
                    DeleteFavoritedPictureCompleted(sender, new RequestCompletedEventArgs(null));
                }
                else
                {
                    WebException we = (WebException)e.Error;
                    HttpWebResponse response = (System.Net.HttpWebResponse)we.Response;

                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        throw new UnauthorizedAccessException("The Authentication Token has expired.");
                    }
                    else
                    {
                        throw e.Error;
                    }
                }
            }
        }

        public event RequestCompletedEventHandler DeleteFavoritedPictureByPictureIDCompleted;

        public void DeleteFavoritedPictureByPictureID(string pictureId)
        {
            if (_IsAuthenticated == false) throw new UnauthorizedAccessException("This method requires User authentication.");

            WebClient client = new WebClient();
            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(DeleteFavoritedPictureByPictureID_DownloadStringCompleted);
            client.DownloadStringAsync(new Uri(String.Format(_WebServiceEndpoint + "favorites/deleteByPictureID?key={0}&token={1}&pictureid={2}", _APIKey, _Token, pictureId)));
        }

        private void DeleteFavoritedPictureByPictureID_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (DeleteFavoritedPictureByPictureIDCompleted != null)
            {
                if (e.Error == null)
                {
                    DeleteFavoritedPictureByPictureIDCompleted(sender, new RequestCompletedEventArgs(null));
                }
                else
                {
                    WebException we = (WebException)e.Error;
                    HttpWebResponse response = (System.Net.HttpWebResponse)we.Response;

                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        throw new UnauthorizedAccessException("The Authentication Token has expired.");
                    }
                    else
                    {
                        throw e.Error;
                    }
                }
            }
        }

        #region Favorite Picture

        public event RequestCompletedEventHandler FavoritePictureCompleted;

        public void FavoritePicture(string id)
        {
            FavoritedPicture data = new FavoritedPicture();
            data.PictureID = id;

            this.CreateFavoritedPictureCompleted += new RequestCompletedEventHandler(WebServiceClient_CreateFavoritedPictureCompleted);
            this.CreateFavoritedPicture(data);
        }

        private void WebServiceClient_CreateFavoritedPictureCompleted(object sender, RequestCompletedEventArgs e)
        {
            this.CreateFavoritedPictureCompleted -= WebServiceClient_CreateFavoritedPictureCompleted;

            if (FavoritePictureCompleted != null)
            {
                FavoritePictureCompleted(sender, new RequestCompletedEventArgs(null));
            }
        }

        #endregion

        #endregion

        #region Flagged Picture Methods

        public event RequestCompletedEventHandler FetchFlaggedPictureCompleted;

        public void FetchFlaggedPicture(string id)
        {
            if (_IsAuthenticated == false) throw new UnauthorizedAccessException("This method requires User authentication.");

            WebClient client = new WebClient();
            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(FetchFlaggedPicture_DownloadStringCompleted);
            client.DownloadStringAsync(new Uri(String.Format(_WebServiceEndpoint + "flags/fetch?key={0}&token={1}&id={2}", _APIKey, _Token, id)));
        }

        private void FetchFlaggedPicture_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (FetchFlaggedPictureCompleted != null)
            {
                if (e.Error == null)
                {
                    string stringData = e.Result;

                    var jsonData = JsonConvert.DeserializeObject<FlaggedPicture>(stringData);

                    FetchFlaggedPictureCompleted(sender, new RequestCompletedEventArgs(jsonData));
                }
                else
                {
                    WebException we = (WebException)e.Error;
                    HttpWebResponse response = (System.Net.HttpWebResponse)we.Response;

                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        throw new UnauthorizedAccessException("The Authentication Token has expired.");
                    }
                    else
                    {
                        throw e.Error;
                    }
                }
            }
        }

        public event RequestCompletedEventHandler CreateFlaggedPictureCompleted;

        public void CreateFlaggedPicture(FlaggedPicture data)
        {
            if (_IsAuthenticated == false) throw new UnauthorizedAccessException("This method requires User authentication.");

            var jsonData = JsonConvert.SerializeObject(data);

            WebClient client = new WebClient();
            client.UploadStringCompleted += new UploadStringCompletedEventHandler(CreateFlaggedPicture_UploadStringCompleted);
            client.UploadStringAsync(new Uri(String.Format(_WebServiceEndpoint + "flags/create?key={0}&token={1}", _APIKey, _Token)), jsonData);
        }

        private void CreateFlaggedPicture_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            if (CreateFlaggedPictureCompleted != null)
            {
                if (e.Error == null)
                {
                    string stringData = e.Result;

                    var jsonData = JsonConvert.DeserializeObject<FlaggedPicture>(stringData);

                    CreateFlaggedPictureCompleted(sender, new RequestCompletedEventArgs(jsonData));
                }
                else
                {
                    WebException we = (WebException)e.Error;
                    HttpWebResponse response = (System.Net.HttpWebResponse)we.Response;

                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        throw new UnauthorizedAccessException("The Authentication Token has expired.");
                    }
                    else
                    {
                        throw e.Error;
                    }
                }
            }
        }

        public event RequestCompletedEventHandler UpdateFlaggedPictureCompleted;

        public void UpdateFlaggedPicture(FlaggedPicture data)
        {
            if (_IsAuthenticated == false) throw new UnauthorizedAccessException("This method requires User authentication.");

            var jsonData = JsonConvert.SerializeObject(data);

            WebClient client = new WebClient();
            client.UploadStringCompleted += new UploadStringCompletedEventHandler(UpdateFlaggedPicture_UploadStringCompleted);
            client.UploadStringAsync(new Uri(String.Format(_WebServiceEndpoint + "flags/update?key={0}&token={1}", _APIKey, _Token)), jsonData);
        }

        private void UpdateFlaggedPicture_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            if (UpdateFlaggedPictureCompleted != null)
            {
                if (e.Error == null)
                {
                    string stringData = e.Result;

                    var jsonData = JsonConvert.DeserializeObject<FlaggedPicture>(stringData);

                    UpdateFlaggedPictureCompleted(sender, new RequestCompletedEventArgs(jsonData));
                }
                else
                {
                    WebException we = (WebException)e.Error;
                    HttpWebResponse response = (System.Net.HttpWebResponse)we.Response;

                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        throw new UnauthorizedAccessException("The Authentication Token has expired.");
                    }
                    else
                    {
                        throw e.Error;
                    }
                }
            }
        }

        public event RequestCompletedEventHandler DeleteFlaggedPictureCompleted;

        public void DeleteFlaggedPicture()
        {
            if (_IsAuthenticated == false) throw new UnauthorizedAccessException("This method requires User authentication.");

            WebClient client = new WebClient();
            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(DeleteFlaggedPicture_DownloadStringCompleted);
            client.DownloadStringAsync(new Uri(String.Format(_WebServiceEndpoint + "flags/delete?key={0}&token={1}", _APIKey, _Token)));
        }

        private void DeleteFlaggedPicture_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (DeleteFlaggedPictureCompleted != null)
            {
                if (e.Error == null)
                {
                    DeleteFlaggedPictureCompleted(sender, new RequestCompletedEventArgs(null));
                }
                else
                {
                    WebException we = (WebException)e.Error;
                    HttpWebResponse response = (System.Net.HttpWebResponse)we.Response;

                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        throw new UnauthorizedAccessException("The Authentication Token has expired.");
                    }
                    else
                    {
                        throw e.Error;
                    }
                }
            }
        }

        #region Flag Picture

        public event RequestCompletedEventHandler FlagPictureCompleted;

        public void FlagPicture(string id)
        {
            FlaggedPicture data = new FlaggedPicture();
            data.PictureID = id;

            this.CreateFlaggedPictureCompleted += new RequestCompletedEventHandler(WebServiceClient_CreateFlaggedPictureCompleted);
            this.CreateFlaggedPicture(data);
        }

        private void WebServiceClient_CreateFlaggedPictureCompleted(object sender, RequestCompletedEventArgs e)
        {
            this.CreateFlaggedPictureCompleted -= WebServiceClient_CreateFlaggedPictureCompleted;

            if (FlagPictureCompleted != null)
            {
                FlagPictureCompleted(sender, new RequestCompletedEventArgs(null));
            }
        }

        #endregion

        #endregion

        #region Picture Methods

        #region Upload Picture

        public event RequestCompletedEventHandler UploadPictureCompleted;

        private Stream ImageUploadStream;

        public void UploadPicture(Stream image)
        {
            if (_IsAuthenticated == false) throw new UnauthorizedAccessException("This method requires User authentication.");

            var request = HttpWebRequest.Create(new Uri(String.Format(_WebServiceEndpoint + "pictures/upload?key={0}&token={1}", _APIKey, _Token)));
            request.Method = "POST";

            var result = (IAsyncResult)request.BeginGetRequestStream(new AsyncCallback(UploadPicture_GetRequestStreamCallback), request);

            ImageUploadStream = image;
        }

        private void UploadPicture_GetRequestStreamCallback(IAsyncResult asyncResult)
        {
            HttpWebRequest request = (HttpWebRequest)asyncResult.AsyncState;
            Stream postStream = request.EndGetRequestStream(asyncResult);

            postStream.Write(ReadToEnd(ImageUploadStream), 0, (int)ImageUploadStream.Length);
            postStream.Close();

            request.BeginGetResponse(new AsyncCallback(UploadPicture_GetResponseCallback), request);
        }

        void UploadPicture_GetResponseCallback(IAsyncResult asyncResult)
        {
            if (UploadPictureCompleted != null)
            {
                HttpWebRequest r = (HttpWebRequest)asyncResult.AsyncState;
                HttpWebResponse response = (HttpWebResponse)r.EndGetResponse(asyncResult);

                var stringData = "";
                using (Stream responseStream = response.GetResponseStream())
                {
                    responseStream.Seek(0, SeekOrigin.Begin);
                    StreamReader reader = new StreamReader(responseStream);

                    stringData = reader.ReadToEnd();
                }

                var jsonData = JsonConvert.DeserializeObject<PictureURL>(stringData);

                UploadPictureCompleted(this, new RequestCompletedEventArgs(jsonData));
            }
        }

        private byte[] ReadToEnd(Stream data)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = data.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }

                return ms.ToArray();
            }
        }

        #endregion

        #region Fetch Picture

        public event RequestCompletedEventHandler FetchPictureCompleted;

        public void FetchPicture(string id)
        {
            WebClient client = new WebClient();
            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(FetchPicture_DownloadStringCompleted);
            if (_Token != null)
                client.DownloadStringAsync(new Uri(String.Format(_WebServiceEndpoint + "pictures/fetch?key={0}&token={1}&id={2}", _APIKey, _Token, id)));
            else
                client.DownloadStringAsync(new Uri(String.Format(_WebServiceEndpoint + "pictures/fetch?key={0}&id={2}", _APIKey, id)));
        }

        private void FetchPicture_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (FetchPictureCompleted != null)
            {
                if (e.Error == null)
                {
                    string stringData = e.Result;

                    var jsonData = JsonConvert.DeserializeObject<PictureInfo>(stringData);

                    FetchPictureCompleted(sender, new RequestCompletedEventArgs(jsonData));
                }
                else
                {
                    WebException we = (WebException)e.Error;
                    HttpWebResponse response = (System.Net.HttpWebResponse)we.Response;

                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        throw new UnauthorizedAccessException("The Authentication Token has expired.");
                    }
                    else
                    {
                        throw e.Error;
                    }
                }
            }
        }

        #endregion

        #region Fetch News Feed

        public event RequestCompletedEventHandler FetchNewsFeedCompleted;

        public void FetchNewsFeed()
        {
            if (_IsAuthenticated == false) throw new UnauthorizedAccessException("This method requires User authentication.");

            WebClient client = new WebClient();
            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(FetchNewsFeed_DownloadStringCompleted);
            client.DownloadStringAsync(new Uri(String.Format(_WebServiceEndpoint + "pictures/fetch?key={0}&token={1}&ticks={2}", _APIKey, _Token, DateTime.Now.Ticks)));
        }

        private void FetchNewsFeed_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (FetchNewsFeedCompleted != null)
            {
                if (e.Error == null)
                {
                    string stringData = e.Result;

                    var jsonData = JsonConvert.DeserializeObject<List<PictureInfo>>(stringData);

                    FetchNewsFeedCompleted(sender, new RequestCompletedEventArgs(jsonData));
                }
                else
                {
                    WebException we = (WebException)e.Error;
                    HttpWebResponse response = (System.Net.HttpWebResponse)we.Response;

                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        throw new UnauthorizedAccessException("The Authentication Token has expired.");
                    }
                    else
                    {
                        throw e.Error;
                    }
                }
            }
        }

        #endregion

        #region Fetch Popular News Feed

        public event RequestCompletedEventHandler FetchPopularNewsFeedCompleted;

        public void FetchPopularNewsFeed()
        {
            WebClient client = new WebClient();
            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(FetchPopularNewsFeed_DownloadStringCompleted);
            if (_Token != null)
                client.DownloadStringAsync(new Uri(String.Format(_WebServiceEndpoint + "pictures/popular/fetch?key={0}&token={1}&ticks={2}", _APIKey, _Token, DateTime.Now.Ticks)));
            else
                client.DownloadStringAsync(new Uri(String.Format(_WebServiceEndpoint + "pictures/popular/fetch?key={0}&ticks={1}", _APIKey, DateTime.Now.Ticks)));
        }

        private void FetchPopularNewsFeed_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (FetchPopularNewsFeedCompleted != null)
            {
                if (e.Error == null)
                {
                    string stringData = e.Result;

                    var jsonData = JsonConvert.DeserializeObject<List<PictureInfo>>(stringData);

                    FetchPopularNewsFeedCompleted(sender, new RequestCompletedEventArgs(jsonData));
                }
                else
                {
                    WebException we = (WebException)e.Error;
                    HttpWebResponse response = (System.Net.HttpWebResponse)we.Response;

                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        throw new UnauthorizedAccessException("The Authentication Token has expired.");
                    }
                    else
                    {
                        throw e.Error;
                    }
                }
            }
        }

        #endregion

        #region Fetch User Pictures

        public event RequestCompletedEventHandler FetchUserPicturesCompleted;

        public void FetchUserPictures(string userId)
        {
            WebClient client = new WebClient();
            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(FetchUserPictures_DownloadStringCompleted);
            if (_Token != null)
                client.DownloadStringAsync(new Uri(String.Format(_WebServiceEndpoint + "pictures/user/fetch?key={0}&token={1}&userid={2}&ticks={3}", _APIKey, _Token, userId, DateTime.Now.Ticks)));
            else
                client.DownloadStringAsync(new Uri(String.Format(_WebServiceEndpoint + "pictures/user/fetch?key={0}&userid={1}&ticks={2}", _APIKey, userId, DateTime.Now.Ticks)));
        }

        private void FetchUserPictures_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (FetchUserPicturesCompleted != null)
            {
                if (e.Error == null)
                {
                    string stringData = e.Result;

                    var jsonData = JsonConvert.DeserializeObject<List<PictureInfo>>(stringData);

                    FetchUserPicturesCompleted(sender, new RequestCompletedEventArgs(jsonData));
                }
                else
                {
                    WebException we = (WebException)e.Error;
                    HttpWebResponse response = (System.Net.HttpWebResponse)we.Response;

                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        throw new UnauthorizedAccessException("The Authentication Token has expired.");
                    }
                    else
                    {
                        throw e.Error;
                    }
                }
            }
        }

        #endregion

        #region Fetch User Favorited Pictures

        public event RequestCompletedEventHandler FetchUserFavoritedPicturesCompleted;

        public void FetchUserFavoritedPictures(string userId)
        {
            WebClient client = new WebClient();
            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(FetchUserFavoritedPictures_DownloadStringCompleted);

            if (_Token != null)
                client.DownloadStringAsync(new Uri(String.Format(_WebServiceEndpoint + "pictures/user/favorites/fetch?key={0}&token={1}&userid={2}&ticks={3}", _APIKey, _Token, userId, DateTime.Now.Ticks)));
            else
                client.DownloadStringAsync(new Uri(String.Format(_WebServiceEndpoint + "pictures/user/favorites/fetch?key={0}&userid={1}&ticks={2}", _APIKey, userId, DateTime.Now.Ticks)));
        }

        private void FetchUserFavoritedPictures_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (FetchUserFavoritedPicturesCompleted != null)
            {
                if (e.Error == null)
                {
                    string stringData = e.Result;

                    var jsonData = JsonConvert.DeserializeObject<List<PictureInfo>>(stringData);

                    FetchUserFavoritedPicturesCompleted(sender, new RequestCompletedEventArgs(jsonData));
                }
                else
                {
                    WebException we = (WebException)e.Error;
                    HttpWebResponse response = (System.Net.HttpWebResponse)we.Response;

                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        throw new UnauthorizedAccessException("The Authentication Token has expired.");
                    }
                    else
                    {
                        throw e.Error;
                    }
                }
            }
        }

        #endregion

        #region Create Picture

        public event RequestCompletedEventHandler CreatePictureCompleted;

        public void CreatePicture(Picture data)
        {
            var jsonData = JsonConvert.SerializeObject(data);

            WebClient client = new WebClient();
            client.UploadStringCompleted += new UploadStringCompletedEventHandler(CreatePicture_UploadStringCompleted);
            client.UploadStringAsync(new Uri(String.Format(_WebServiceEndpoint + "pictures/create?key={0}&token={1}", _APIKey, _Token)), jsonData);
        }

        private void CreatePicture_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            if (CreatePictureCompleted != null)
            {
                if (e.Error == null)
                {
                    string stringData = e.Result;

                    var jsonData = JsonConvert.DeserializeObject<Picture>(stringData);

                    CreatePictureCompleted(sender, new RequestCompletedEventArgs(jsonData));
                }
                else
                {
                    WebException we = (WebException)e.Error;
                    HttpWebResponse response = (System.Net.HttpWebResponse)we.Response;

                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        throw new UnauthorizedAccessException("The Authentication Token has expired.");
                    }
                    else
                    {
                        throw e.Error;
                    }
                }
            }
        }

        #endregion

        #region Update Picture

        public event RequestCompletedEventHandler UpdatePictureCompleted;

        public void UpdatePicture(Picture data)
        {
            if (_IsAuthenticated == false) throw new UnauthorizedAccessException("This method requires User authentication.");

            var jsonData = JsonConvert.SerializeObject(data);

            WebClient client = new WebClient();
            client.UploadStringCompleted += new UploadStringCompletedEventHandler(UpdatePicture_UploadStringCompleted);
            client.UploadStringAsync(new Uri(String.Format(_WebServiceEndpoint + "pictures/update?key={0}&token={1}", _APIKey, _Token)), jsonData);
        }

        private void UpdatePicture_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            if (UpdatePictureCompleted != null)
            {
                if (e.Error == null)
                {
                    string stringData = e.Result;

                    var jsonData = JsonConvert.DeserializeObject<Picture>(stringData);

                    UpdatePictureCompleted(sender, new RequestCompletedEventArgs(jsonData));
                }
                else
                {
                    WebException we = (WebException)e.Error;
                    HttpWebResponse response = (System.Net.HttpWebResponse)we.Response;

                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        throw new UnauthorizedAccessException("The Authentication Token has expired.");
                    }
                    else
                    {
                        throw e.Error;
                    }
                }
            }
        }

        #endregion

        #region Delete Picture

        public event RequestCompletedEventHandler DeletePictureCompleted;

        public void DeletePicture(string id)
        {
            if (_IsAuthenticated == false) throw new UnauthorizedAccessException("This method requires User authentication.");

            WebClient client = new WebClient();
            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(DeletePicture_DownloadStringCompleted);
            client.DownloadStringAsync(new Uri(String.Format(_WebServiceEndpoint + "pictures/delete?key={0}&token={1}&id={2}", _APIKey, _Token, id)), id);
        }

        private void DeletePicture_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (DeletePictureCompleted != null)
            {
                if (e.Error == null)
                {
                    DeletePictureCompleted(sender, new RequestCompletedEventArgs(null));
                }
                else
                {
                    WebException we = (WebException)e.Error;
                    HttpWebResponse response = (System.Net.HttpWebResponse)we.Response;

                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        throw new UnauthorizedAccessException("The Authentication Token has expired.");
                    }
                    else
                    {
                        throw e.Error;
                    }
                }
            }
        }

        #endregion

        #endregion

        #region Relationship Methods

        public event RequestCompletedEventHandler FetchRelationshipCompleted;

        public void FetchRelationship(string id)
        {
            if (_IsAuthenticated == false) throw new UnauthorizedAccessException("This method requires User authentication.");

            WebClient client = new WebClient();
            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(FetchRelationship_DownloadStringCompleted);
            client.DownloadStringAsync(new Uri(String.Format(_WebServiceEndpoint + "relationships/fetch?key={0}&token={1}&id={2}", _APIKey, _Token, id)));
        }

        private void FetchRelationship_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (FetchRelationshipCompleted != null)
            {
                if (e.Error == null)
                {
                    string stringData = e.Result;

                    var jsonData = JsonConvert.DeserializeObject<Relationship>(stringData);

                    FetchRelationshipCompleted(sender, new RequestCompletedEventArgs(jsonData));
                }
                else
                {
                    WebException we = (WebException)e.Error;
                    HttpWebResponse response = (System.Net.HttpWebResponse)we.Response;

                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        throw new UnauthorizedAccessException("The Authentication Token has expired.");
                    }
                    else
                    {
                        throw e.Error;
                    }
                }
            }
        }

        public event RequestCompletedEventHandler CreateRelationshipCompleted;

        public void CreateRelationship(Relationship data)
        {
            if (_IsAuthenticated == false) throw new UnauthorizedAccessException("This method requires User authentication.");

            var jsonData = JsonConvert.SerializeObject(data);

            WebClient client = new WebClient();
            client.UploadStringCompleted += new UploadStringCompletedEventHandler(CreateRelationship_UploadStringCompleted);
            client.UploadStringAsync(new Uri(String.Format(_WebServiceEndpoint + "relationships/create?key={0}&token={1}", _APIKey, _Token)), jsonData);
        }

        private void CreateRelationship_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            if (CreateRelationshipCompleted != null)
            {
                if (e.Error == null)
                {
                    string stringData = e.Result;

                    var jsonData = JsonConvert.DeserializeObject<Relationship>(stringData);

                    CreateRelationshipCompleted(sender, new RequestCompletedEventArgs(jsonData));
                }
                else
                {
                    WebException we = (WebException)e.Error;
                    HttpWebResponse response = (System.Net.HttpWebResponse)we.Response;

                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        throw new UnauthorizedAccessException("The Authentication Token has expired.");
                    }
                    else
                    {
                        throw e.Error;
                    }
                }
            }
        }

        public event RequestCompletedEventHandler UpdateRelationshipCompleted;

        public void UpdateRelationship(Relationship data)
        {
            if (_IsAuthenticated == false) throw new UnauthorizedAccessException("This method requires User authentication.");

            var jsonData = JsonConvert.SerializeObject(data);

            WebClient client = new WebClient();
            client.UploadStringCompleted += new UploadStringCompletedEventHandler(UpdateRelationship_UploadStringCompleted);
            client.UploadStringAsync(new Uri(String.Format(_WebServiceEndpoint + "relationships/update?key={0}&token={1}", _APIKey, _Token)), jsonData);
        }

        private void UpdateRelationship_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            if (UpdateRelationshipCompleted != null)
            {
                if (e.Error == null)
                {
                    string stringData = e.Result;

                    var jsonData = JsonConvert.DeserializeObject<Relationship>(stringData);

                    UpdateRelationshipCompleted(sender, new RequestCompletedEventArgs(jsonData));
                }
                else
                {
                    WebException we = (WebException)e.Error;
                    HttpWebResponse response = (System.Net.HttpWebResponse)we.Response;

                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        throw new UnauthorizedAccessException("The Authentication Token has expired.");
                    }
                    else
                    {
                        throw e.Error;
                    }
                }
            }
        }

        public event RequestCompletedEventHandler DeleteRelationshipCompleted;

        public void DeleteRelationship(Relationship data)
        {
            if (_IsAuthenticated == false) throw new UnauthorizedAccessException("This method requires User authentication.");

            WebClient client = new WebClient();
            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(DeleteRelationship_DownloadStringCompleted);
            client.DownloadStringAsync(new Uri(String.Format(_WebServiceEndpoint + "relationships/delete?key={0}&token={1}&id={2}", _APIKey, _Token, data.ID)));
        }

        private void DeleteRelationship_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (DeleteRelationshipCompleted != null)
            {
                if (e.Error == null)
                {
                    DeleteRelationshipCompleted(sender, new RequestCompletedEventArgs(null));
                }
                else
                {
                    WebException we = (WebException)e.Error;
                    HttpWebResponse response = (System.Net.HttpWebResponse)we.Response;

                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        throw new UnauthorizedAccessException("The Authentication Token has expired.");
                    }
                    else
                    {
                        throw e.Error;
                    }
                }
            }
        }

        public event RequestCompletedEventHandler DeleteRelationshipByUserIDCompleted;

        public void DeleteRelationshipByUserID(string userId)
        {
            if (_IsAuthenticated == false) throw new UnauthorizedAccessException("This method requires User authentication.");

            WebClient client = new WebClient();
            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(DeleteRelationshipByUserID_DownloadStringCompleted);
            client.DownloadStringAsync(new Uri(String.Format(_WebServiceEndpoint + "relationships/deleteByUserID?key={0}&token={1}&userid={2}", _APIKey, _Token, userId)));
        }

        private void DeleteRelationshipByUserID_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (DeleteRelationshipByUserIDCompleted != null)
            {
                if (e.Error == null)
                {
                    DeleteRelationshipByUserIDCompleted(sender, new RequestCompletedEventArgs(null));
                }
                else
                {
                    WebException we = (WebException)e.Error;
                    HttpWebResponse response = (System.Net.HttpWebResponse)we.Response;

                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        throw new UnauthorizedAccessException("The Authentication Token has expired.");
                    }
                    else
                    {
                        throw e.Error;
                    }
                }
            }
        }

        #endregion

        #region User Methods

        public event RequestCompletedEventHandler FetchUserCompleted;

        public void FetchUser(string id)
        {
            if (_IsAuthenticated == false) throw new UnauthorizedAccessException("This method requires User authentication.");

            WebClient client = new WebClient();
            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(FetchUser_DownloadStringCompleted);
            client.DownloadStringAsync(new Uri(String.Format(_WebServiceEndpoint + "users/fetch?key={0}&token={1}&id={2}", _APIKey, _Token, id)));
        }

        private void FetchUser_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (FetchUserCompleted != null)
            {
                if (e.Error == null)
                {
                    string stringData = e.Result;

                    var jsonData = JsonConvert.DeserializeObject<UserInfo>(stringData);

                    if (jsonData.ID == CurrentUser.ID)
                        CurrentUser = jsonData;

                    FetchUserCompleted(sender, new RequestCompletedEventArgs(jsonData));
                }
                else
                {
                    WebException we = (WebException)e.Error;
                    HttpWebResponse response = (System.Net.HttpWebResponse)we.Response;

                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        throw new UnauthorizedAccessException("The Authentication Token has expired.");
                    }
                    else
                    {
                        throw e.Error;
                    }
                }
            }
        }

        public event RequestCompletedEventHandler FetchAllUsersCompleted;

        public void FetchAllUsers()
        {
            if (_IsAuthenticated == false) throw new UnauthorizedAccessException("This method requires User authentication.");

            WebClient client = new WebClient();
            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(FetchAllUsers_DownloadStringCompleted);
            client.DownloadStringAsync(new Uri(String.Format(_WebServiceEndpoint + "users/fetch?key={0}&token={1}", _APIKey, _Token)));
        }

        private void FetchAllUsers_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (FetchAllUsersCompleted != null)
            {
                if (e.Error == null)
                {
                    string stringData = e.Result;

                    var jsonData = JsonConvert.DeserializeObject<List<UserInfo>>(stringData);

                    FetchAllUsersCompleted(sender, new RequestCompletedEventArgs(jsonData));
                }
                else
                {
                    WebException we = (WebException)e.Error;
                    HttpWebResponse response = (System.Net.HttpWebResponse)we.Response;

                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        throw new UnauthorizedAccessException("The Authentication Token has expired.");
                    }
                    else
                    {
                        throw e.Error;
                    }
                }
            }
        }

        public event RequestCompletedEventHandler SearchUsersCompleted;

        public void SearchUsers(string query)
        {
            if (_IsAuthenticated == false) throw new UnauthorizedAccessException("This method requires User authentication.");

            WebClient client = new WebClient();
            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(SearchUsers_DownloadStringCompleted);
            client.DownloadStringAsync(new Uri(String.Format(_WebServiceEndpoint + "users/search?key={0}&token={1}&query={2}", _APIKey, _Token, query)));
        }

        private void SearchUsers_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (SearchUsersCompleted != null)
            {
                if (e.Error == null)
                {
                    string stringData = e.Result;

                    var jsonData = JsonConvert.DeserializeObject<List<UserInfo>>(stringData);

                    SearchUsersCompleted(sender, new RequestCompletedEventArgs(jsonData));
                }
                else
                {
                    WebException we = (WebException)e.Error;
                    HttpWebResponse response = (System.Net.HttpWebResponse)we.Response;

                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        throw new UnauthorizedAccessException("The Authentication Token has expired.");
                    }
                    else
                    {
                        throw e.Error;
                    }
                }
            }
        }

        public event RequestCompletedEventHandler CreateUserCompleted;

        public void CreateUser(User data)
        {
            // force password hashing
            data.Password = this.HashPassword(data.Password);
            data.ProfilePictureID = defaultProfilePictureID;

            var jsonData = JsonConvert.SerializeObject(data);

            WebClient client = new WebClient();
            client.UploadStringCompleted += new UploadStringCompletedEventHandler(CreateUser_UploadStringCompleted);
            client.UploadStringAsync(new Uri(String.Format(_WebServiceEndpoint + "users/create?key={0}", _APIKey)), jsonData);
        }

        private void CreateUser_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            if (CreateUserCompleted != null)
            {
                if (e.Error == null)
                {
                    string stringData = e.Result;

                    var jsonData = JsonConvert.DeserializeObject<User>(stringData);

                    CreateUserCompleted(sender, new RequestCompletedEventArgs(jsonData));
                }
                else
                {
                    WebException we = (WebException)e.Error;
                    HttpWebResponse response = (System.Net.HttpWebResponse)we.Response;

                    CreateUserCompleted(sender, new RequestCompletedEventArgs(new UnauthorizedAccessException()));
                }
            }
        }

        public event RequestCompletedEventHandler UpdateUserCompleted;

        public void UpdateUser(User data)
        {
            if (_IsAuthenticated == false) throw new UnauthorizedAccessException("This method requires User authentication.");

            var jsonData = JsonConvert.SerializeObject(data);

            WebClient client = new WebClient();
            client.UploadStringCompleted += new UploadStringCompletedEventHandler(UpdateUser_UploadStringCompleted);
            client.UploadStringAsync(new Uri(String.Format(_WebServiceEndpoint + "users/update?key={0}&token={1}", _APIKey, _Token)), jsonData);
        }

        private void UpdateUser_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            if (UpdateUserCompleted != null)
            {
                if (e.Error == null)
                {
                    string stringData = e.Result;

                    var jsonData = JsonConvert.DeserializeObject<User>(stringData);

                    if (jsonData.ID == CurrentUser.ID)
                        FetchUser(CurrentUser.ID);

                    UpdateUserCompleted(sender, new RequestCompletedEventArgs(jsonData));
                }
                else
                {
                    WebException we = (WebException)e.Error;
                    HttpWebResponse response = (System.Net.HttpWebResponse)we.Response;

                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        throw new UnauthorizedAccessException("The Authentication Token has expired.");
                    }
                    else
                    {
                        throw e.Error;
                    }
                }
            }
        }

        public event RequestCompletedEventHandler DeleteUserCompleted;

        public void DeleteUser()
        {
            if (_IsAuthenticated == false) throw new UnauthorizedAccessException("This method requires User authentication.");

            WebClient client = new WebClient();
            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(DeleteUser_DownloadStringCompleted);
            client.DownloadStringAsync(new Uri(String.Format(_WebServiceEndpoint + "users/delete?key={0}&token={1}", _APIKey, _Token)));
        }

        private void DeleteUser_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (DeleteUserCompleted != null)
            {
                if (e.Error == null)
                {
                    DeleteUserCompleted(sender, new RequestCompletedEventArgs(null));
                }
                else
                {
                    WebException we = (WebException)e.Error;
                    HttpWebResponse response = (System.Net.HttpWebResponse)we.Response;

                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        throw new UnauthorizedAccessException("The Authentication Token has expired.");
                    }
                    else
                    {
                        throw e.Error;
                    }
                }
            }
        }

        #endregion

        #region UserConnectedAccount Methods

        public event RequestCompletedEventHandler FetchUserConnectedAccountCompleted;

        public void FetchUserConnectedAccount(string id)
        {
            if (_IsAuthenticated == false) throw new UnauthorizedAccessException("This method requires User authentication.");

            WebClient client = new WebClient();
            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(FetchUserConnectedAccount_DownloadStringCompleted);
            client.DownloadStringAsync(new Uri(String.Format(_WebServiceEndpoint + "users/connections/fetch?key={0}&token={1}&id={2}", _APIKey, _Token, id)));
        }

        private void FetchUserConnectedAccount_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (FetchUserConnectedAccountCompleted != null)
            {
                if (e.Error == null)
                {
                    string stringData = e.Result;

                    var jsonData = JsonConvert.DeserializeObject<UserConnectedAccount>(stringData);

                    FetchUserConnectedAccountCompleted(sender, new RequestCompletedEventArgs(jsonData));
                }
                else
                {
                    WebException we = (WebException)e.Error;
                    HttpWebResponse response = (System.Net.HttpWebResponse)we.Response;

                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        throw new UnauthorizedAccessException("The Authentication Token has expired.");
                    }
                    else
                    {
                        throw e.Error;
                    }
                }
            }
        }

        public event RequestCompletedEventHandler FetchUserConnectedAccountsByUserIDCompleted;

        public void FetchUserConnectedAccountsByUserID(string userId)
        {
            if (_IsAuthenticated == false) throw new UnauthorizedAccessException("This method requires User authentication.");

            WebClient client = new WebClient();
            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(FetchUserConnectedAccountsByUserID_DownloadStringCompleted);
            client.DownloadStringAsync(new Uri(String.Format(_WebServiceEndpoint + "users/connections/fetchByUserID?key={0}&token={1}&userid={2}", _APIKey, _Token, userId)));
        }

        private void FetchUserConnectedAccountsByUserID_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (FetchUserConnectedAccountsByUserIDCompleted != null)
            {
                if (e.Error == null)
                {
                    string stringData = e.Result;

                    var jsonData = JsonConvert.DeserializeObject<List<UserConnectedAccount>>(stringData);

                    FetchUserConnectedAccountsByUserIDCompleted(sender, new RequestCompletedEventArgs(jsonData));
                }
                else
                {
                    WebException we = (WebException)e.Error;
                    HttpWebResponse response = (System.Net.HttpWebResponse)we.Response;

                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        throw new UnauthorizedAccessException("The Authentication Token has expired.");
                    }
                    else
                    {
                        throw e.Error;
                    }
                }
            }
        }

        public event RequestCompletedEventHandler CreateUserConnectedAccountCompleted;

        public void CreateUserConnectedAccount(UserConnectedAccount data)
        {
            if (_IsAuthenticated == false) throw new UnauthorizedAccessException("This method requires User authentication.");

            var jsonData = JsonConvert.SerializeObject(data);

            WebClient client = new WebClient();
            client.UploadStringCompleted += new UploadStringCompletedEventHandler(CreateUserConnectedAccount_UploadStringCompleted);
            client.UploadStringAsync(new Uri(String.Format(_WebServiceEndpoint + "users/connections/create?key={0}&token={1}", _APIKey, _Token)), jsonData);
        }

        private void CreateUserConnectedAccount_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            if (CreateUserConnectedAccountCompleted != null)
            {
                if (e.Error == null)
                {
                    string stringData = e.Result;

                    var jsonData = JsonConvert.DeserializeObject<UserConnectedAccount>(stringData);

                    CreateUserConnectedAccountCompleted(sender, new RequestCompletedEventArgs(jsonData));
                }
                else
                {
                    WebException we = (WebException)e.Error;
                    HttpWebResponse response = (System.Net.HttpWebResponse)we.Response;

                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        throw new UnauthorizedAccessException("The Authentication Token has expired.");
                    }
                    else
                    {
                        throw e.Error;
                    }
                }
            }
        }

        public event RequestCompletedEventHandler UpdateUserConnectedAccountCompleted;

        public void UpdateUserConnectedAccount(UserConnectedAccount data)
        {
            if (_IsAuthenticated == false) throw new UnauthorizedAccessException("This method requires User authentication.");

            var jsonData = JsonConvert.SerializeObject(data);

            WebClient client = new WebClient();
            client.UploadStringCompleted += new UploadStringCompletedEventHandler(UpdateUserConnectedAccount_UploadStringCompleted);
            client.UploadStringAsync(new Uri(String.Format(_WebServiceEndpoint + "users/connections/update?key={0}&token={1}", _APIKey, _Token)), jsonData);
        }

        private void UpdateUserConnectedAccount_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            if (UpdateUserConnectedAccountCompleted != null)
            {
                if (e.Error == null)
                {
                    string stringData = e.Result;

                    var jsonData = JsonConvert.DeserializeObject<UserConnectedAccount>(stringData);

                    UpdateUserConnectedAccountCompleted(sender, new RequestCompletedEventArgs(jsonData));
                }
                else
                {
                    WebException we = (WebException)e.Error;
                    HttpWebResponse response = (System.Net.HttpWebResponse)we.Response;

                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        throw new UnauthorizedAccessException("The Authentication Token has expired.");
                    }
                    else
                    {
                        throw e.Error;
                    }
                }
            }
        }

        public event RequestCompletedEventHandler DeleteUserConnectedAccountCompleted;

        public void DeleteUserConnectedAccount(string id)
        {
            if (_IsAuthenticated == false) throw new UnauthorizedAccessException("This method requires User authentication.");

            WebClient client = new WebClient();
            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(DeleteUserConnectedAccount_DownloadStringCompleted);
            client.DownloadStringAsync(new Uri(String.Format(_WebServiceEndpoint + "users/connections/delete?key={0}&token={1}&id={2}", _APIKey, _Token, id)));
        }

        private void DeleteUserConnectedAccount_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (DeleteUserConnectedAccountCompleted != null)
            {
                if (e.Error == null)
                {
                    DeleteUserConnectedAccountCompleted(sender, new RequestCompletedEventArgs(null));
                }
                else
                {
                    WebException we = (WebException)e.Error;
                    HttpWebResponse response = (System.Net.HttpWebResponse)we.Response;

                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        throw new UnauthorizedAccessException("The Authentication Token has expired.");
                    }
                    else
                    {
                        throw e.Error;
                    }
                }
            }
        }

        #endregion
    }
}
