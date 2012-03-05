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
        private const string API_KEY_LIST = "APIKeyList";
        private const string API_KEY_PARAM = "key";

        private List<string> APIKeyList
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
                ErrorResponseHandler.GenerateErrorResponse(context, ErrorType.InvalidAPIKey);
                return false;
            }
        }

        private string ParseAPIKey(OperationContext context)
        {
            var request = context.RequestContext.RequestMessage;
            var requestProperty = (HttpRequestMessageProperty)request.Properties[HttpRequestMessageProperty.Name];

            NameValueCollection queryParameters = HttpUtility.ParseQueryString(requestProperty.QueryString);

            return queryParameters[API_KEY_PARAM];
        }
    }
}