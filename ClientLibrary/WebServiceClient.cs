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
        private static string _WebServiceEndpoint = "http://winstagram.cloudapp.net/v1/";
        private string _APIKey = null;

        public WebServiceClient(string APIkey)
        {
            _APIKey = APIkey;
        }

        #region Authentication Methods

        public bool IsAuthenticated
        {
            get;
            set;
        }

        private string Token
        {
            get;
            set;
        }

        public event RequestCompletedEventHandler AuthenticateCompleted;

        public void Authenticate(string username, string password)
        {
            var jsonData = JsonConvert.SerializeObject(new UserCredentials(username, this.HashPassword(password)));

            WebClient client = new WebClient();
            client.UploadStringCompleted += new UploadStringCompletedEventHandler(Authenticate_UploadStringCompleted);
            client.UploadStringAsync(new Uri(String.Format(_WebServiceEndpoint + "authenticate?key={0}", _APIKey)), jsonData);
        }

        private void Authenticate_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            if (AuthenticateCompleted != null)
            {
                string stringData = e.Result;

                var jsonData = JsonConvert.DeserializeObject<AuthenticationToken>(stringData);

                this.IsAuthenticated = true;
                this.Token = jsonData.Token;

                AuthenticateCompleted(sender, new RequestCompletedEventArgs(null));
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
            if (this.IsAuthenticated == false) throw new UnauthorizedAccessException("This method requires User authentication.");

            WebClient client = new WebClient();
            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(FetchUser_DownloadStringCompleted);
            client.DownloadStringAsync(new Uri(String.Format(_WebServiceEndpoint + "users/fetch?key={0}&id={1}", _APIKey, id)));
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
            if (this.IsAuthenticated == false) throw new UnauthorizedAccessException("This method requires User authentication.");

            WebClient client = new WebClient();
            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(FetchAllUsers_DownloadStringCompleted);
            client.DownloadStringAsync(new Uri(String.Format(_WebServiceEndpoint + "users/fetch?key={0}", _APIKey)));
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

            WebClient client = new WebClient();
            client.UploadStringCompleted += new UploadStringCompletedEventHandler(CreateUser_UploadStringCompleted);
            client.UploadStringAsync(new Uri(String.Format(_WebServiceEndpoint + "users/create?key={0}", _APIKey)), jsonData);
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
            if (this.IsAuthenticated == false) throw new UnauthorizedAccessException("This method requires User authentication.");

            var jsonData = JsonConvert.SerializeObject(data);

            WebClient client = new WebClient();
            client.UploadStringCompleted += new UploadStringCompletedEventHandler(UpdateUser_UploadStringCompleted);
            client.UploadStringAsync(new Uri(String.Format(_WebServiceEndpoint + "users/update?key={0}", _APIKey)), jsonData);
        }

        private void UpdateUser_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            if (UpdateUserCompleted != null)
            {
                UpdateUserCompleted(sender, new RequestCompletedEventArgs(null));
            }
        }

        public event RequestCompletedEventHandler DeleteUserCompleted;

        public void DeleteUser(string id)
        {
            if (this.IsAuthenticated == false) throw new UnauthorizedAccessException("This method requires User authentication.");

            WebClient client = new WebClient();
            client.UploadStringCompleted += new UploadStringCompletedEventHandler(DeleteUser_UploadStringCompleted);
            client.UploadStringAsync(new Uri(String.Format(_WebServiceEndpoint + "users/update?key={0}&id=", _APIKey, id)), null);
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
