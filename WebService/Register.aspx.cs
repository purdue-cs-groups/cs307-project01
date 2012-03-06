using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebService.Models;
using WebService.Controllers;
using System.Net.Mail;
using WebService.Common;

namespace WebService
{
    public partial class Register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            APIConsumer data = new APIConsumer();

            data.EmailAddress = this.txtEmailAddress.Text;
            data.CreatedDate = Utilities.ConvertToUnixTime(DateTime.UtcNow);

            APIConsumerController.Create(data);

            MailMessage message = new MailMessage();

            message.From = new MailAddress("system@winstagram.cloudapp.net");
            message.To.Add(new MailAddress(data.EmailAddress));
            message.Subject = "Your Winstagram API Key";
            message.Body = "Thank you for registering for the Winstagram API. We look forward to seeing the unique and innovative applications that developers like you create with our service. Your API key is listed below. Please let us know if you have any problems.\n\n" + data.Key + "\n\n--\nWinstagram\nhttp://winstagram.cloudapp.net\n";

            SmtpClient client = new SmtpClient();
            client.Send(message);

            Response.Redirect("~/default.aspx");
        }
    }
}