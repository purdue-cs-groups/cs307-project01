using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using MobileClientLibrary.Models;

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

        #region User Methods

        public event RequestCompletedEventHandler FetchUserCompleted;

        public void FetchUser(string id)
        {
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
