using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Collections.Specialized;
using System.IO;
using System.Xml.Linq;
using System.Net;
using WebService.Models;
using WebService.Controllers;

namespace WebService.Common
{
    public class AuthorizationManager : ServiceAuthorizationManager
    {
        public const string API_KEY_LIST = "APIKeyList";
        public const string API_KEY_PARAM = "key";

        public List<string> APIKeyList
        {
            get
            {
                var keys = HttpContext.Current.Cache[API_KEY_LIST] as List<string>;

                if (keys == null)
                    keys = RefreshAPIConsumerKeys();

                return keys;
            }
        }

        private List<string> RefreshAPIConsumerKeys()
        {
            List<string> data = new List<string>();

            foreach (APIConsumer item in APIConsumerController.FetchAll())
            {
                data.Add(item.Key);
            }

            return data;
        }

        protected override bool CheckAccessCore(OperationContext context)
        {
            return IsValidAPIKey(context);
        }

        private bool IsValidAPIKey(OperationContext context)
        {
            if (Global.APIKeyAuthorizationEnabled == false)
                return true;

            string key = ParseAPIKey(context);
            if (APIKeyList.Contains(key))
            {
                return true;
            }
            else
            {
                GenerateErrorResponse(context, key);
                return false;
            }
        }

        private const string APIErrorHTML = @"
<html>
<head>
    <title>Request Error - Missing API Key</title>
</head>
<body>
    <h1>Request Error</h1>
    <p>Wrong API key douchebag! Next time you try to make a request to this web service, try providing the proper credentials.</p>
</body>
</html>
";

        private static void GenerateErrorResponse(OperationContext context, string key)
        {
            using (var sr = new StringReader("<?xml version=\"1.0\" encoding=\"utf-8\"?>" + APIErrorHTML))
            {
                XElement response = XElement.Load(sr);
                using (Message reply = Message.CreateMessage(MessageVersion.None, null, response))
                {
                    HttpResponseMessageProperty responseProperty = new HttpResponseMessageProperty() { StatusCode = HttpStatusCode.Unauthorized, StatusDescription = String.Format("'{0}' is an invalid API key", key) };
                    responseProperty.Headers[HttpResponseHeader.ContentType] = "text/html";
                    reply.Properties[HttpResponseMessageProperty.Name] = responseProperty;

                    context.RequestContext.Reply(reply);
                    context.RequestContext = null;
                }
            }
        }

        public string ParseAPIKey(OperationContext context)
        {
            var request = context.RequestContext.RequestMessage;
            var requestProperty = (HttpRequestMessageProperty)request.Properties[HttpRequestMessageProperty.Name];

            NameValueCollection queryParameters = HttpUtility.ParseQueryString(requestProperty.QueryString);

            return queryParameters[API_KEY_PARAM];
        }
    }
}