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
            List<PictureInfo> pictures = PictureController.FetchPopularNewsFeed();

            foreach (PictureInfo picture in pictures)
            {
                this.litDatabase.Text += "<div class=\"tile\">\n";
                this.litDatabase.Text += "<a href=\"/p/" + picture.ID + "\">\n";
                this.litDatabase.Text += "<img src=\"" + picture.SmallURL + "\" title=\"" + picture.Caption + "\" />\n";
                this.litDatabase.Text += "</a>\n";
                this.litDatabase.Text += "</div>\n";
            }
        }
    }
}