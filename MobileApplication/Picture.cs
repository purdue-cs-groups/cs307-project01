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

namespace WinstagramPan
{
    public class Picture
    {
        private int pictureID;
        public int PictureID
        {
            get
            {
                return pictureID;
            }
            set
            {
                pictureID = value;
            }
        }

        private int userID;
        public int UserID
        {
            get
            {
                return userID;
            }
            set
            {
                UserID = value;
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

        private Image photo;
        public Image Photo
        {
            get
            {
                return photo;
            }
            set
            {
                photo = value;
            }
        }

        private String caption;
        public String Caption
        {
            get
            {
                return caption;
            }
            set
            {
                caption = value;
            }
        }

        private Decimal latitude;
        public Decimal Latitude
        {
            get
            {
                return latitude;
            }
            set
            {
                latitude = value;
            }
        }

        private Decimal longitude;
        public Decimal Longitude
        {
            get
            {
                return longitude;
            }
            set
            {
                longitude = value;
            }
        }

        private int viewCount;
        public int ViewCount
        {
            get
            {
                return viewCount;
            }
            set
            {
                viewCount = value;
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

        public Picture()
        {
            Photo = new Image();
        }
    }
}
