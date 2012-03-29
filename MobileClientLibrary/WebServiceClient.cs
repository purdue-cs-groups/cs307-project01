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

    public class WebServiceClient
    {
        private const string _WebServiceEndpoint = "http://winstagram.cloudapp.net/v1/";

        private string _APIKey = null;

        private bool _IsAuthenticated = false;
        private UserCredentials _Credentials = null;
        private string _Token = null;

        private int _AuthenticationAttempts = 0;

        public WebServiceClient(string APIkey)
        {
            _APIKey = APIkey;
        }

        #region Authentication Methods

        public event RequestCompletedEventHandler AuthenticateCompleted;

        public void Authenticate(string username, string password)
        {
            _Credentials = new UserCredentials(username, this.HashPassword(password));

            var jsonData = JsonConvert.SerializeObject(_Credentials);

            _AuthenticationAttempts++;

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

                    _AuthenticationAttempts = 0;

                    AuthenticateCompleted(sender, new RequestCompletedEventArgs(null));
                }
                else
                {
                    WebException we = (WebException)e.Error;
                    HttpWebResponse response = (System.Net.HttpWebResponse)we.Response;

                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        throw new UnauthorizedAccessException("The User credentials provided were not valid.");
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

                    var jsonData = JsonConvert.DeserializeObject<User>(stringData);

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

                    var jsonData = JsonConvert.DeserializeObject<List<User>>(stringData);

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

        public event RequestCompletedEventHandler CreateUserCompleted;

        public void CreateUser(User data)
        {
            // force password hashing
            data.Password = this.HashPassword(data.Password);

            var jsonData = JsonConvert.SerializeObject(data);

            WebClient client = new WebClient();
            client.UploadStringCompleted += new UploadStringCompletedEventHandler(CreateUser_UploadStringCompleted);
            client.UploadStringAsync(new Uri(String.Format(_WebServiceEndpoint + "users/create?key={0}&token={1}", _APIKey, _Token)), jsonData);
        }

        private void CreateUser_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            if (CreateUserCompleted != null)
            {
                if (e.Error == null)
                {
                    CreateUserCompleted(sender, new RequestCompletedEventArgs(null));
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
                    UpdateUserCompleted(sender, new RequestCompletedEventArgs(null));
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

                    var jsonData = JsonConvert.DeserializeObject<Picture>(stringData);

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
            client.DownloadStringAsync(new Uri(String.Format(_WebServiceEndpoint + "pictures/fetch?key={0}&token={1}", _APIKey, _Token)));
        }

        private void FetchNewsFeed_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (FetchNewsFeedCompleted != null)
            {
                if (e.Error == null)
                {
                    string stringData = e.Result;

                    var jsonData = JsonConvert.DeserializeObject<List<Picture>>(stringData);

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
            client.DownloadStringAsync(new Uri(String.Format(_WebServiceEndpoint + "pictures/popular/fetch?key={0}", _APIKey)));
        }

        private void FetchPopularNewsFeed_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (FetchPopularNewsFeedCompleted != null)
            {
                if (e.Error == null)
                {
                    string stringData = e.Result;

                    var jsonData = JsonConvert.DeserializeObject<List<Picture>>(stringData);

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
                    CreatePictureCompleted(sender, new RequestCompletedEventArgs(null));
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
                    UpdatePictureCompleted(sender, new RequestCompletedEventArgs(null));
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
            client.UploadStringAsync(new Uri(String.Format(_WebServiceEndpoint + "pictures/delete?key={0}&token={1}&id={2}", _APIKey, _Token, id)), null);
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
    }
}
