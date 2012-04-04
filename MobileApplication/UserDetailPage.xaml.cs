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

using MetrocamPan.Models;

using Microsoft.Phone.Tasks;
using Microsoft.Phone.Shell;
using MobileClientLibrary.Models;

namespace MetrocamPan
{
    public partial class UserDetailPage : PhoneApplicationPage
    {
        public static bool isFollowing = true;
        public User user;

        public UserDetailPage()
        {
            InitializeComponent();
            UpdateAppBar();

            Loaded += new RoutedEventHandler(UserDetailPage_Loaded);
        }

        void UserDetailPage_Loaded(object sender, RoutedEventArgs e)
        {
            // Testing Data binding
            user = new User();
            user.Username = "JoeM";
            user.Biography = "I'm a Computer Science Senior studying at Purdue University. I'm a software engineer and I develop Windows Phone applications! By the way, I am a XAML god and yes you may worship me =P.";
            user.EmailAddress = "joemartella@purdue.edu";
            //user.CreatedDate = System.DateTime.Today;
            user.Name = "Joe Martella";
            user.Location = "West Lafayette, Indiana";

            // Set DataContexts
            usernameGrid.DataContext = user;
            bioGrid.DataContext = user;
            emailGrid.DataContext = user;
            activeSinceGrid.DataContext = user;
            fullName.DataContext = user;
            hometown.DataContext = user;
        }

        private void SendEmail_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            EmailComposeTask em = new EmailComposeTask();
            em.To = emailTextBlock.Text;

            em.Show();
        }

        private void UpdateAppBar()
        {
            ApplicationBar.Buttons.Clear();

            if (isFollowing)
            {
                followingStatus.Text = "Following";
                followingStatus.Margin = new Thickness(60, 17, 0, 0);
                followingRec.Fill = new SolidColorBrush(Colors.Green);

                ApplicationBarIconButton unfollow = new ApplicationBarIconButton(new Uri("Images/appbar.arrow.down.png", UriKind.Relative));
                unfollow.Text = "unfollow";
                unfollow.Click += new EventHandler(Unfollow);
                ApplicationBar.Buttons.Add(unfollow);
            }
            else
            {
                followingStatus.Text = "Not Following";
                followingStatus.Margin = new Thickness(44, 17, 0, 0);
                followingRec.Fill = new SolidColorBrush(Colors.Red);

                ApplicationBarIconButton follow = new ApplicationBarIconButton(new Uri("Images/appbar.arrow.up.png", UriKind.Relative));
                follow.Text = "follow";
                follow.Click += new EventHandler(Follow);
                ApplicationBar.Buttons.Add(follow);
            }
        }

        public static HubTile selectedPicture;
        private void hubTilePictureTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            /*
            selectedPicture = (HubTile)sender;
            NavigationService.Navigate(new Uri("/PictureView.xaml", UriKind.Relative));
             */
        }

        private void Follow(object sender, EventArgs e)
        {
            isFollowing = true;
            UpdateAppBar();
        }

        private void Unfollow(object sender, EventArgs e)
        {
            isFollowing = false;
            UpdateAppBar();
        }
    }
}