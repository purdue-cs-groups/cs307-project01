using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebService.Common;
using WebService.Models;
using WebService.Controllers;
using System.Net.Mail;
using System.Text;

namespace WebService
{
    public partial class ResetPassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string newPassword = Guid.NewGuid().ToString().Replace("-", "");
            string hashedPassword = this.HashPassword(newPassword);

            User data = UserController.FetchByEmailAddress(this.txtEmailAddress.Text);
            data.Password = hashedPassword;

            UserController.Update(data);

            MailMessage message = new MailMessage();

            message.From = new MailAddress("system@metrocam.cloudapp.net");
            message.To.Add(new MailAddress(data.EmailAddress));
            message.Subject = "Your New Metrocam Password";
            message.Body = "Your new Metrocam password is " + newPassword + " . Please change this password in Metrocam after you have logged in successfully.\n\n--\nMetrocam\nhttp://metrocam.cloudapp.net\n";

            SmtpClient client = new SmtpClient();
            client.Send(message);

            Response.Redirect("~/default.aspx");
        }

        private string HashPassword(string password)
        {
            if (!string.IsNullOrWhiteSpace(password))
            {
                byte[] buffer = Encoding.UTF8.GetBytes(password);

                MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
                provider.ComputeHash(buffer);

                string hashed = string.Empty;
                foreach (byte b in provider.Hash)
                {
                    hashed += b.ToString("X2");
                }

                return hashed.ToLower();
            }
            else
            {
                return null;
            }
        }
    }
}