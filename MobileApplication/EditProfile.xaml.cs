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
using JeffWilcox.FourthAndMayor;

namespace MetrocamPan
{
    public partial class EditProfile : PhoneApplicationPage
    {
        private bool isUpdating = false;

        public EditProfile()
        {
            InitializeComponent();

            Loaded += new RoutedEventHandler(EditProfile_Loaded);
        }

        void EditProfile_Loaded(object sender, RoutedEventArgs e)
        {
            this.ContentPanel.DataContext = App.MetrocamService.CurrentUser;
        }

        private void Accept_Click(object sender, EventArgs e)
        {
            if (isUpdating)
                return;

            // Load in the data into currentUser
            App.MetrocamService.CurrentUser.Name = this.UsernameInput.Text;
            App.MetrocamService.CurrentUser.EmailAddress = this.EmailInput.Text;
            App.MetrocamService.CurrentUser.Location = this.LocationInput.Text;
            App.MetrocamService.CurrentUser.Biography = this.BiographyInput.Text;

            // construct User object to pass to web service
            User updatedData = new User();
            updatedData.CreatedDate = App.MetrocamService.CurrentUser.CreatedDate;
            updatedData.FriendlyCreatedDate = App.MetrocamService.CurrentUser.FriendlyCreatedDate;
            updatedData.ID = App.MetrocamService.CurrentUser.ID;
            updatedData.Username = App.MetrocamService.CurrentUser.Username;
            updatedData.Password = App.MetrocamService.HashPassword(Settings.password.Value);
            updatedData.ProfilePictureID = App.MetrocamService.CurrentUser.ProfilePicture.ID;

            updatedData.Name = App.MetrocamService.CurrentUser.Name;
            updatedData.EmailAddress = App.MetrocamService.CurrentUser.EmailAddress;
            updatedData.Location = App.MetrocamService.CurrentUser.Location;
            updatedData.Biography = App.MetrocamService.CurrentUser.Biography;

            GlobalLoading.Instance.IsLoading = true;
            App.MetrocamService.UpdateUserCompleted +=new RequestCompletedEventHandler(MetrocamService_UpdateUserCompleted);
            App.MetrocamService.UpdateUser(updatedData);            
        }

        void MetrocamService_UpdateUserCompleted(object sender, RequestCompletedEventArgs e)
        {
            App.MetrocamService.UpdateUserCompleted -= MetrocamService_UpdateUserCompleted;
            GlobalLoading.Instance.IsLoading = false;
            MessageBox.Show("Your profile has been updated.");
            NavigationService.GoBack();
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