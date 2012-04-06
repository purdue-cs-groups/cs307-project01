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

namespace MetrocamPan
{
    public partial class EditProfile : PhoneApplicationPage
    {
        private UserInfo currentUser;

        public EditProfile()
        {
            InitializeComponent();

            Loaded += new RoutedEventHandler(EditProfile_Loaded);
        }

        void EditProfile_Loaded(object sender, RoutedEventArgs e)
        {
            // Subscribe event handler
            App.MetrocamService.FetchUserCompleted += new MobileClientLibrary.RequestCompletedEventHandler(MetrocamService_FetchUserCompleted);
            // Calls fetch user
            App.MetrocamService.FetchUser(Settings.userid.Value);
        }

        void MetrocamService_FetchUserCompleted(object sender, MobileClientLibrary.RequestCompletedEventArgs e)
        {
            // Unsubscribe
            App.MetrocamService.FetchUserCompleted -= new MobileClientLibrary.RequestCompletedEventHandler(MetrocamService_FetchUserCompleted);

            currentUser = (UserInfo)e.Data;

            this.ContentPanel.DataContext = currentUser;
        }

        private void Accept_Click(object sender, EventArgs e)
        {
            // Load in the data into currentUser
            currentUser.Name = this.UsernameInput.Text;
            currentUser.EmailAddress = this.EmailInput.Text;
            currentUser.Location = this.LocationInput.Text;
            currentUser.Biography = this.BiographyInput.Text;

            // TODO: Matt has to changed UpdateUser(User data) to UserInfo as arguments instead

            MessageBox.Show("Your profile has been updated.");
            NavigationService.GoBack();
        }

        void MetrocamService_UpdateUserCompleted(object sender, RequestCompletedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Your changes have been discarded.");
            NavigationService.GoBack();
        }

        private void UsernameInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Dispatcher.BeginInvoke(() => this.UsernameInput.Focus());
                Dispatcher.BeginInvoke(() => this.EmailInput.Focus());
            }
        }

        private void EmailInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Dispatcher.BeginInvoke(() => this.EmailInput.Focus());
                Dispatcher.BeginInvoke(() => this.LocationInput.Focus());
            }
        }

        private void LocationInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Dispatcher.BeginInvoke(() => this.LocationInput.Focus());
                Dispatcher.BeginInvoke(() => this.BiographyInput.Focus());
            }
        }

        private void BiographyInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.Focus();
            }
        }
    }
}