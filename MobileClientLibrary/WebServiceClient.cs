﻿using System;
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

        #region Picture Methods

        public event RequestCompletedEventHandler UploadPictureCompleted;

        public void UploadPicture(Stream image)
        {
            if (_IsAuthenticated == false) throw new UnauthorizedAccessException("This method requires User authentication.");

            // convert image stream to byte array string
            string byteArray = ReadToEnd(image).ToString();

            _Client.OpenWriteCompleted += new OpenWriteCompletedEventHandler(UploadPicture_OpenWriteCompleted);
            _Client.OpenWriteAsync(new Uri(String.Format(_WebServiceEndpoint + "pictures/upload?key={0}&token={1}", _APIKey, _Token)), "POST", byteArray);
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

        private void UploadPicture_OpenWriteCompleted(object sender, OpenWriteCompletedEventArgs e)
        {
            if (UploadPictureCompleted != null)
            {
                var stringData = e.Result.ToString();

                var jsonData = JsonConvert.DeserializeObject<PictureURL>(stringData);

                UploadPictureCompleted(sender, new RequestCompletedEventArgs(jsonData));
            }
        }
        
        public event RequestCompletedEventHandler FetchPictureCompleted;

        public void FetchPicture(string id)
        {
            _Client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(FetchPicture_DownloadStringCompleted);
            _Client.DownloadStringAsync(new Uri(String.Format(_WebServiceEndpoint + "pictures/fetch?key={0}&id={2}", _APIKey, _Token, id)));
        }

        private void FetchPicture_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (FetchPictureCompleted != null)
            {
                string stringData = e.Result;

                var jsonData = JsonConvert.DeserializeObject<Picture>(stringData);

                FetchPictureCompleted(sender, new RequestCompletedEventArgs(jsonData));
            }
        }

        public event RequestCompletedEventHandler FetchNewsFeedCompleted;

        public void FetchNewsFeed()
        {
            if (_IsAuthenticated == false) throw new UnauthorizedAccessException("This method requires User authentication.");

            _Client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(FetchNewsFeed_DownloadStringCompleted);
            _Client.DownloadStringAsync(new Uri(String.Format(_WebServiceEndpoint + "pictures/fetch?key={0}&token={1}", _APIKey, _Token)));
        }

        private void FetchNewsFeed_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (FetchNewsFeedCompleted != null)
            {
                string stringData = e.Result;

                var jsonData = JsonConvert.DeserializeObject<List<Picture>>(stringData);

                FetchNewsFeedCompleted(sender, new RequestCompletedEventArgs(jsonData));
            }
        }

        public event RequestCompletedEventHandler FetchPopularNewsFeedCompleted;

        public void FetchPopularNewsFeed()
        {
            _Client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(FetchPopularNewsFeed_DownloadStringCompleted);
            _Client.DownloadStringAsync(new Uri(String.Format(_WebServiceEndpoint + "pictures/popular/fetch?key={0}", _APIKey)));
        }

        private void FetchPopularNewsFeed_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (FetchPopularNewsFeedCompleted != null)
            {
                string stringData = e.Result;

                var jsonData = JsonConvert.DeserializeObject<List<Picture>>(stringData);

                FetchPopularNewsFeedCompleted(sender, new RequestCompletedEventArgs(jsonData));
            }
        }

        public event RequestCompletedEventHandler CreatePictureCompleted;

        public void CreatePicture(Picture data)
        {
            var jsonData = JsonConvert.SerializeObject(data);

            _Client.UploadStringCompleted += new UploadStringCompletedEventHandler(CreatePicture_UploadStringCompleted);
            _Client.UploadStringAsync(new Uri(String.Format(_WebServiceEndpoint + "pictures/create?key={0}&token={1}", _APIKey, _Token)), jsonData);
        }

        private void CreatePicture_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            if (CreatePictureCompleted != null)
            {
                CreatePictureCompleted(sender, new RequestCompletedEventArgs(null));
            }
        }

        public event RequestCompletedEventHandler UpdatePictureCompleted;

        public void UpdatePicture(Picture data)
        {
            if (_IsAuthenticated == false) throw new UnauthorizedAccessException("This method requires User authentication.");

            var jsonData = JsonConvert.SerializeObject(data);

            _Client.UploadStringCompleted += new UploadStringCompletedEventHandler(UpdatePicture_UploadStringCompleted);
            _Client.UploadStringAsync(new Uri(String.Format(_WebServiceEndpoint + "pictures/update?key={0}&token={1}", _APIKey, _Token)), jsonData);
        }

        private void UpdatePicture_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            if (UpdatePictureCompleted != null)
            {
                UpdatePictureCompleted(sender, new RequestCompletedEventArgs(null));
            }
        }

        public event RequestCompletedEventHandler DeletePictureCompleted;

        public void DeletePicture(string id)
        {
            if (_IsAuthenticated == false) throw new UnauthorizedAccessException("This method requires User authentication.");

            _Client.UploadStringCompleted += new UploadStringCompletedEventHandler(DeletePicture_UploadStringCompleted);
            _Client.UploadStringAsync(new Uri(String.Format(_WebServiceEndpoint + "pictures/delete?key={0}&token={1}&id={2}", _APIKey, _Token, id)), null);
        }

        private void DeletePicture_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            if (DeletePictureCompleted != null)
            {
                DeletePictureCompleted(sender, new RequestCompletedEventArgs(null));
            }
        }

        #endregion
    }
}
