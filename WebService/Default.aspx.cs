using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebService.Models;
using WebService.Controllers;

namespace WebService
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            List<Picture> pictures = PictureController.FetchPopularNewsFeed();

            foreach (Picture picture in pictures)
            {
                this.litDatabase.Text += "<a href=\"http://winstagram.cloudapp.net/p/" + picture.ID + "\">http://winstagram.cloudapp.net/p/" + picture.ID + "</a><br />\n";
                this.litDatabase.Text += "<br />\n";
            }
        }
    }
}