using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using System.IO;
using System.Xml.Linq;
using System.ServiceModel.Channels;
using System.Net;

namespace WebService.Common
{
    public enum ErrorType
    {
        InvalidAPIKey = 0,
        InvalidUserCredentials = 1,
        InvalidAuthenticationToken = 2
    }

    public static class ErrorResponseHandler
    {
        private const string ErrorResponseHTML = @"
<html>
<head>
    <title>Request Error: {{TITLE}}</title>
</head>
<body>
    <h1>Request Error: {{TITLE}}</h1>
    <p>{{DESCRIPTION}}</p>
</body>
</html>
";

        public static void GenerateErrorResponse(OperationContext context, ErrorType error)
        {
            var errorHTML = ErrorResponseHTML;

            switch (error)
            {
                case ErrorType.InvalidAPIKey:
                    errorHTML = errorHTML.Replace("{{TITLE}}", "Invalid API Key");
                    errorHTML = errorHTML.Replace("{{DESCRIPTION}}", "The API Key that you provided was not valid. Please check your API Key and try your request again later.");
                    break;
                case ErrorType.InvalidUserCredentials:
                    errorHTML = errorHTML.Replace("{{TITLE}}", "Invalid User Credentials");
                    errorHTML = errorHTML.Replace("{{DESCRIPTION}}", "The User Credentials that you provided were not valid. Please check your User Credentials and try your request again later.");
                    break;
                case ErrorType.InvalidAuthenticationToken:
                    errorHTML = errorHTML.Replace("{{TITLE}}", "Invalid Authentication Token");
                    errorHTML = errorHTML.Replace("{{DESCRIPTION}}", "The Authentication Token that you provided was not valid. Please check your Authentication Token and try your request again later.");
                    break;
            }

            using (var sr = new StringReader("<?xml version=\"1.0\" encoding=\"utf-8\"?>" + errorHTML))
            {
                XElement response = XElement.Load(sr);
                using (Message reply = Message.CreateMessage(MessageVersion.None, null, response))
                {
                    HttpResponseMessageProperty responseProperty = null;
                    
                    switch (error)
                    {
                        case ErrorType.InvalidAPIKey:
                            responseProperty = new HttpResponseMessageProperty() { StatusCode = HttpStatusCode.Unauthorized };
                            break;
                        case ErrorType.InvalidUserCredentials:
                            responseProperty = new HttpResponseMessageProperty() { StatusCode = HttpStatusCode.Unauthorized };
                            break;
                        case ErrorType.InvalidAuthenticationToken:
                            responseProperty = new HttpResponseMessageProperty() { StatusCode = HttpStatusCode.Unauthorized };
                            break;
                    }
                    
                    responseProperty.Headers[HttpResponseHeader.ContentType] = "text/html";
                    reply.Properties[HttpResponseMessageProperty.Name] = responseProperty;

                    context.RequestContext.Reply(reply);
                    context.RequestContext = null;
                }
            }
        }

        
    }
}