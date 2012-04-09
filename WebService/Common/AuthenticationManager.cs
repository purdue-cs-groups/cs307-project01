using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebService.Models;
using WebService.Controllers;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Collections.Specialized;

namespace WebService.Common
{
    public static class AuthenticationManager
    {
        private const string TOKEN_LIST = "TokenList";
        private const string TOKEN_PARAM = "token";

        private static List<AuthenticationToken> TokenList
        {
            get
            {
                var tokens = HttpContext.Current.Cache[TOKEN_LIST] as List<AuthenticationToken>;

                if (tokens == null)
                {
                    tokens = new List<AuthenticationToken>();
                    HttpContext.Current.Cache[TOKEN_LIST] = tokens;
                }

                return tokens;
            }
        }

        public static bool IsValidUserCredentials(UserCredentials credential)
        {
            UserInfo data = UserController.FetchInfoByUsername(credential.Username);

            // if user is null, try looking up by email address
            if (data == null)
                data = UserController.FetchInfoByEmailAddress(credential.Username);

            if (data != null &&
                data.Password == credential.Password)
                return true;
            else
                return false;
        }

        public static AuthenticationToken GenerateToken(string key, string username)
        {
            AuthenticationToken token = null;
            token = TokenList.SingleOrDefault<AuthenticationToken>(t => t.Identity.Username == username &&
                                                                        t.Consumer.Key == key);

            if (token != null)
            {
                token.LastAccessDate = DateTime.UtcNow;
            }
            else
            {
                string uniqueIdentifier = Guid.NewGuid().ToString();
                uniqueIdentifier = uniqueIdentifier.Replace("-", "");

                UserInfo user = UserController.FetchInfoByUsername(username);

                APIConsumer apiConsumer = APIConsumerController.Fetch(key);

                token = new AuthenticationToken(uniqueIdentifier, user, apiConsumer);

                TokenList.Add(token);
            }

            return token;
        }

        public static AuthenticationToken ValidateToken(OperationContext context)
        {
            string uniqueIdentifier = ParseUniqueIdentifider(context);

            if (IsValidAuthenticationToken(uniqueIdentifier))
            {
                AuthenticationToken token = TokenList.SingleOrDefault<AuthenticationToken>(t => t.UniqueIdentifier == uniqueIdentifier);
                token.LastAccessDate = DateTime.UtcNow;

                return token;
            }
            else
            {
                ErrorResponseHandler.GenerateErrorResponse(context, ErrorType.InvalidAuthenticationToken);
                return null;
            }
        }

        private static bool IsValidAuthenticationToken(string uniqueIdentifier)
        {
            if (TokenList.Where(t => t.UniqueIdentifier == uniqueIdentifier &&
                                     t.ExpirationDate > DateTime.UtcNow).Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string ParseUniqueIdentifider(OperationContext context)
        {
            var request = context.RequestContext.RequestMessage;
            var requestProperty = (HttpRequestMessageProperty)request.Properties[HttpRequestMessageProperty.Name];

            NameValueCollection queryParameters = HttpUtility.ParseQueryString(requestProperty.QueryString);

            return queryParameters[TOKEN_PARAM];
        }
    }
}