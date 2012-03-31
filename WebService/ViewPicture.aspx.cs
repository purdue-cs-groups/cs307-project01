using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using System.Configuration;
using WebService.Models;
using WebService.Controllers;
using System.Net;
using System.Web.Helpers;

namespace WebService
{
    public partial class ViewPicture : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string id = Request.QueryString["id"];
            if (id == null)
                id = Page.RouteData.Values["id"].ToString();

            Picture picture = PictureController.Fetch(id);

            User user = UserController.Fetch(picture.UserID);

            // image
            this.imgPicture.ImageUrl = picture.MediumURL;

            // caption
            this.lblCaption.Text = picture.Caption;
            
            // username
            this.lnkUsername.Text = user.Username;
            this.lnkUsername.NavigateUrl = "#";

            // date
            this.lblDate.Text = picture.FriendlyCreatedDate.ToLongDateString() + " at " + picture.FriendlyCreatedDate.ToShortTimeString();

            string location;
            try
            {
                WebClient client = new WebClient();
                
                var jsonResult2 = client.DownloadString("http://maps.googleapis.com/maps/api/geocode/json?address=" + picture.Latitude + "," + picture.Longitude + "&sensor=false");
                var r = Json.Decode(jsonResult2).results[0];

                string city = "";
                string state = "";
                foreach (var t in r.address_components)
                {
                    if (t.types[0] == "locality")
                        city = t.long_name;
                    else if (t.types[0] == "administrative_area_level_1")
                        state = t.short_name;
                }

                location = "<a href=\"http://www.bing.com/maps/default.aspx?lvl=12&cp=" + picture.Latitude + "~" + picture.Longitude + "\" target=\"blank\">" + city + ", " + state + "</a>";
            }
            catch
            {
                location = "<a href=\"http://www.bing.com/maps/default.aspx?lvl=12&cp=" + picture.Latitude + "~" + picture.Longitude + "\" target=\"blank\">(" + picture.Latitude + ", " + picture.Longitude + ")</a>";
            }

            // location
            this.litLocation.Text = location;

            // tweet button
            this.litTweetButton.Text = "<a href=\"https://twitter.com/share?via=metrocam&text=" + picture.Caption + " " + Request.Url.AbsoluteUri + "\" class=\"twitter-share-button\" data-lang=\"en\">Tweet</a>\n";
            this.litTweetButton.Text += "<script>!function(d,s,id){var js,fjs=d.getElementsByTagName(s)[0];if(!d.getElementById(id)){js=d.createElement(s);js.id=id;js.src=\"//platform.twitter.com/widgets.js\";fjs.parentNode.insertBefore(js,fjs);}}(document,\"script\",\"twitter-wjs\");</script>";
        }
    }
}