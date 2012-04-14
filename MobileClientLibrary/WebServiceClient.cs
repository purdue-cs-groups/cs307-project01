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

        public event RequestCompletedEventHandler FetchFavoritedPictureCompleted;

        public void FetchFavoritedPicture(string id)
        {
            if (_IsAuthenticated == false) throw new UnauthorizedAccessException("This method requires User authentication.");

            WebClient client = new WebClient();
            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(FetchFavoritedPicture_DownloadStringCompleted);
            client.DownloadStringAsync(new Uri(String.Format(_WebServiceEndpoint + "favorites/fetch?key={0}&token={1}&id={2}", _APIKey, _Token, id)));
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

        public void DeleteFavoritedPicture()
        {
            if (_IsAuthenticated == false) throw new UnauthorizedAccessException("This method requires User authentication.");

            WebClient client = new WebClient();
            client.UploadStringCompleted += new UploadStringCompletedEventHandler(DeleteFavoritedPicture_UploadStringCompleted);
            client.UploadStringAsync(new Uri(String.Format(_WebServiceEndpoint + "favorites/delete?key={0}&token={1}", _APIKey, _Token)), null);
        }

        private void DeleteFavoritedPicture_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
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
            client.UploadStringCompleted += new UploadStringCompletedEventHandler(DeleteFlaggedPicture_UploadStringCompleted);
            client.UploadStringAsync(new Uri(String.Format(_WebServiceEndpoint + "flags/delete?key={0}&token={1}", _APIKey, _Token)), null);
        }

        private void DeleteFlaggedPicture_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
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
            client.DownloadStringAsync(new Uri(String.Format(_WebServiceEndpoint + "pictures/fetch?key={0}&id={2}", _APIKey, _Token, id)));
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
            client.DownloadStringAsync(new Uri(String.Format(_WebServiceEndpoint + "pictures/user/fetch?key={0}&userid={1}", _APIKey, userId)));
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
            client.DownloadStringAsync(new Uri(String.Format(_WebServiceEndpoint + "pictures/user/favorites/fetch?key={0}&userid={1}", _APIKey, userId)));
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
            client.UploadStringCompleted += new UploadStringCompletedEventHandler(DeletePicture_UploadStringCompleted);
            client.UploadStringAsync(new Uri(String.Format(_WebServiceEndpoint + "pictures/delete?key={0}&token={1}&id={2}", _APIKey, _Token, id)), id);
        }

        private void DeletePicture_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
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

        #region Flag Picture

        public event RequestCompletedEventHandler FlagPictureCompleted;

        public void FlagPicture(string id)
        {
            if (_IsAuthenticated == false) throw new UnauthorizedAccessException("This method requires User authentication.");

            WebClient client = new WebClient();
            client.UploadStringCompleted += new UploadStringCompletedEventHandler(FlagPicture_UploadStringCompleted);
            client.UploadStringAsync(new Uri(String.Format(_WebServiceEndpoint + "pictures/flag?key={0}&token={1}&id={2}", _APIKey, _Token, id)), null);
        }

        private void FlagPicture_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            if (FlagPictureCompleted != null)
            {
                if (e.Error == null)
                {
                    FlagPictureCompleted(sender, new RequestCompletedEventArgs(null));
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

        public event RequestCompletedEventHandler FetchRelationshipByIDsCompleted;

        public void FetchRelationshipByIDs(string userId, string followingId)
        {
            if (_IsAuthenticated == false) throw new UnauthorizedAccessException("This method requires User authentication.");

            WebClient client = new WebClient();
            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(FetchRelationshipByIDs_DownloadStringCompleted);
            client.DownloadStringAsync(new Uri(String.Format(_WebServiceEndpoint + "relationships/user/fetch?key={0}&token={1}&userid={2}&followingid={3}", _APIKey, _Token, userId, followingId)));
        }

        private void FetchRelationshipByIDs_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (FetchRelationshipByIDsCompleted != null)
            {
                if (e.Error == null)
                {
                    string stringData = e.Result;

                    var jsonData = JsonConvert.DeserializeObject<Relationship>(stringData);

                    //if (jsonData == null)
                    //{
                    //    FetchRelationshipByIDsCompleted(sender, null);
                    //}
                    //else
                    //{
                        FetchRelationshipByIDsCompleted(sender, new RequestCompletedEventArgs(jsonData));
                    //}
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

        public void DeleteRelationship()
        {
            if (_IsAuthenticated == false) throw new UnauthorizedAccessException("This method requires User authentication.");

            WebClient client = new WebClient();
            client.UploadStringCompleted += new UploadStringCompletedEventHandler(DeleteRelationship_UploadStringCompleted);
            client.UploadStringAsync(new Uri(String.Format(_WebServiceEndpoint + "relationships/delete?key={0}&token={1}", _APIKey, _Token)), null);
        }

        private void DeleteRelationship_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
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
            client.UploadStringCompleted += new UploadStringCompletedEventHandler(DeleteUser_UploadStringCompleted);
            client.UploadStringAsync(new Uri(String.Format(_WebServiceEndpoint + "users/delete?key={0}&token={1}", _APIKey, _Token)), null);
        }

        private void DeleteUser_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
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
            client.UploadStringCompleted += new UploadStringCompletedEventHandler(DeleteUserConnectedAccount_UploadStringCompleted);
            client.UploadStringAsync(new Uri(String.Format(_WebServiceEndpoint + "users/connections/delete?key={0}&token={1}&id={2}", _APIKey, _Token, id)), null);
        }

        private void DeleteUserConnectedAccount_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
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
