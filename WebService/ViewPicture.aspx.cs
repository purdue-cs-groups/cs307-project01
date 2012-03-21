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

namespace WebService
{
    public partial class ViewPicture : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string id = Request.QueryString["id"];
            Picture data = PictureController.Fetch(id);

            this.imgPicture.ImageUrl = data.URL;
        }
    }
}