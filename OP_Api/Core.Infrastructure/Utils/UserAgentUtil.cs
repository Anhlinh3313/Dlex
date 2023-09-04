using Core.Infrastructure.Helper;
using System;
namespace Core.Infrastructure.Utils
{
    public static class UserAgentUtil
    {
        public static int GetTypeUserAgentHeader(string userAgent)
        {
            int typeRequest;
            if (String.IsNullOrWhiteSpace(userAgent))
            {
                typeRequest = 0;
            }
            else
            {
                if (userAgent.ToLower().Contains("android"))
                {
                    typeRequest = ClientRequestFrom.Mobile.Android;
                } else if (userAgent.ToLower().Contains("iphone"))
                {
                    typeRequest = ClientRequestFrom.Mobile.IOS;
                }
                else if (userAgent.ToLower().Contains("postman"))
                {
                    typeRequest = ClientRequestFrom.ToolTesTAPI.Postman;
                } 
                else
                {
                    typeRequest = ClientRequestFrom.WebBrowser.AllBrowser;
                }
            }
            return typeRequest;
        }

    }
}
