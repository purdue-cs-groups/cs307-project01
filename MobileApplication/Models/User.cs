using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace MetrocamPan.Models
{
    public class User
    {
        private int userID;
        public int UserID
        {
            get
            {
                return userID;
            }
            set
            {
                userID = value;
            }
        }

        private String username;
        public String Username
        {
            get
            {
                return username;
            }
            set
            {
                username = value;
            }
        }

        private String name;
        public String Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        private String emailAddress;
        public String EmailAddress
        {
            get
            {
                return emailAddress;
            }
            set
            {
                emailAddress = value;
            }
        }

        private String password;
        public String Password
        {
            get
            {
                return password;
            }
            set
            {
                password = value;
            }
        }

        private String biography;
        public String Biography
        {
            get
            {
                return biography;
            }
            set
            {
                biography = value;
            }
        }

        private Image profilePicture;
        public Image ProfilePicture
        {
            get
            {
                return profilePicture;
            }
            set
            {
                profilePicture = value;
            }
        }

        private String location;
        public String Location
        {
            get
            {
                return location;
            }
            set
            {
                location = value;
            }
        }

        private DateTime createdDate;
        public DateTime CreatedDate
        {
            get
            {
                return createdDate;
            }
            set
            {
                createdDate = value;
            }
        }
    }
}
