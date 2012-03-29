using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Routing;
using System.ServiceModel.Activation;
using System.Diagnostics;

namespace WebService
{
    public class Global : System.Web.HttpApplication
    {
        public const string DatabaseConnectionString = "mongodb://pu307user:Password01@ds029847.mongolab.com:29847";
        public const string DatabaseName = "pu307dev";

        public const bool APIKeyAuthorizationEnabled = true;

        protected void Application_Start(object sender, EventArgs e)
        {
            RegisterRoutes();
        }

        private void RegisterRoutes()
        {
            WebServiceHostFactory factory = new WebServiceHostFactory();
            
            RouteTable.Routes.Add(new ServiceRoute("v1", factory, typeof(WebService)));
            RouteTable.Routes.Add("Picture", new Route("p/{id}", new PageRouteHandler("~/ViewPicture.aspx")));
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            Debug.WriteLine(Context.Request.Url.AbsoluteUri);
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}