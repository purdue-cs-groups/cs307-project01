using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using MobileClientLibrary.Models;
using MobileClientLibrary.Common;
using System.Text;
using ClientLibrary.Common;

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
        private WebClient _Client = null;
        private const string _WebServiceEndpoint = "http://winstagram.cloudapp.net/v1/";

        private string _APIKey = null;

        private bool _IsAuthenticated = false;
        private UserCredentials _Credentials = null;
        private string _Token = null;

        public WebServiceClient(string APIkey)
        {
            _APIKey = APIkey;

            _Client = new WebClient();
        }

        #region Authentication Methods

        public event RequestCompletedEventHandler AuthenticateCompleted;

        public void Authenticate(string username, string password)
        {
            _Credentials = new UserCredentials(username, this.HashPassword(password));

            var jsonData = JsonConvert.SerializeObject(_Credentials);

            _Client.UploadStringCompleted += new UploadStringCompletedEventHandler(Authenticate_UploadStringCompleted);
            _Client.UploadStringAsync(new Uri(String.Format(_WebServiceEndpoint + "authenticate?key={0}", _APIKey)), jsonData);
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

            _Client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(FetchUser_DownloadStringCompleted);
            _Client.DownloadStringAsync(new Uri(String.Format(_WebServiceEndpoint + "users/fetch?key={0}&token={1}&id={2}", _APIKey, _Token, id)));
        }

        private void FetchUser_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (FetchUserCompleted != null)
            {
                string stringData = e.Result;

                var jsonData = JsonConvert.DeserializeObject<User>(stringData);

                FetchUserCompleted(sender, new RequestCompletedEventArgs(jsonData));
            }
        }

        public event RequestCompletedEventHandler FetchAllUsersCompleted;

        public void FetchAllUsers()
        {
            if (_IsAuthenticated == false) throw new UnauthorizedAccessException("This method requires User authentication.");

            _Client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(FetchAllUsers_DownloadStringCompleted);
            _Client.DownloadStringAsync(new Uri(String.Format(_WebServiceEndpoint + "users/fetch?key={0}&token={1}", _APIKey, _Token)));
        }

        private void FetchAllUsers_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (FetchAllUsersCompleted != null)
            {
                string stringData = e.Result;

                var jsonData = JsonConvert.DeserializeObject<List<User>>(stringData);

                FetchAllUsersCompleted(sender, new RequestCompletedEventArgs(jsonData));
            }
        }

        public event RequestCompletedEventHandler CreateUserCompleted;

        public void CreateUser(User data)
        {
            var jsonData = JsonConvert.SerializeObject(data);

            _Client.UploadStringCompleted += new UploadStringCompletedEventHandler(CreateUser_UploadStringCompleted);
            _Client.UploadStringAsync(new Uri(String.Format(_WebServiceEndpoint + "users/create?key={0}&token={1}", _APIKey, _Token)), jsonData);
        }

        private void CreateUser_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            if (CreateUserCompleted != null)
            {
                CreateUserCompleted(sender, new RequestCompletedEventArgs(null));
            }
        }

        public event RequestCompletedEventHandler UpdateUserCompleted;

        public void UpdateUser(User data)
        {
            if (_IsAuthenticated == false) throw new UnauthorizedAccessException("This method requires User authentication.");

            var jsonData = JsonConvert.SerializeObject(data);

            _Client.UploadStringCompleted += new UploadStringCompletedEventHandler(UpdateUser_UploadStringCompleted);
            _Client.UploadStringAsync(new Uri(String.Format(_WebServiceEndpoint + "users/update?key={0}&token={1}", _APIKey, _Token)), jsonData);
        }

        private void UpdateUser_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            if (UpdateUserCompleted != null)
            {
                UpdateUserCompleted(sender, new RequestCompletedEventArgs(null));
            }
        }

        public event RequestCompletedEventHandler DeleteUserCompleted;

        public void DeleteUser()
        {
            if (_IsAuthenticated == false) throw new UnauthorizedAccessException("This method requires User authentication.");

            _Client.UploadStringCompleted += new UploadStringCompletedEventHandler(DeleteUser_UploadStringCompleted);
            _Client.UploadStringAsync(new Uri(String.Format(_WebServiceEndpoint + "users/delete?key={0}&token={1}", _APIKey, _Token)), null);
        }

        private void DeleteUser_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            if (DeleteUserCompleted != null)
            {
                DeleteUserCompleted(sender, new RequestCompletedEventArgs(null));
            }
        }

        #endregion
    }
}
