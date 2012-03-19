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
using MobileClientLibrary.Models;

namespace TestApplication
{
    public partial class MainPage : PhoneApplicationPage
    {
        WebServiceClient client;

        public MainPage()
        {
            InitializeComponent();

            client = new WebServiceClient("4f5685ce5ad9850e545bb48d");

            client.AuthenticateCompleted += new RequestCompletedEventHandler(client_AuthenticateCompleted);
            client.Authenticate("mbmccormick", "password");
        }

        private void client_AuthenticateCompleted(object sender, RequestCompletedEventArgs e)
        {
            client.FetchAllUsersCompleted += new RequestCompletedEventHandler(client_FetchAllUsersCompleted);
            client.FetchAllUsers();
        }

        private void client_FetchAllUsersCompleted(object sender, RequestCompletedEventArgs e)
        {
            List<User> data = e.Data as List<User>;

            foreach (var user in data)
            {
                this.txtOutput.Text = user.Name + "\n";
            }
        }
    }
}