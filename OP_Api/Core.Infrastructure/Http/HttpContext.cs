using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Core.Infrastructure.Extensions;
using Microsoft.AspNetCore.Http;

namespace Core.Infrastructure.Http
{
    public static class HttpContext
    {
        private static IHttpContextAccessor _contextAccessor;

        public static Microsoft.AspNetCore.Http.HttpContext Current => _contextAccessor.HttpContext;

        public static int CurrentUserId
        {
            get
            {
                var nameIdentifier = Current.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
                var userId = (nameIdentifier != null) ? nameIdentifier.Value : "";
                return userId.ToSafeInt();
            }
        }
        public static int CurrentCompanyId
        {
            get
            {

                ClaimsPrincipal currentUser = Current.User;
                var sId = currentUser.Claims.FirstOrDefault(x => x.Type == "sid");
                return sId.Value.ToSafeInt();
            }
        }

        internal static void Configure(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public static string GetRequestHeaders(string key)
        {
            var header = Current.Request.Headers[key];
            var headerName = (!String.IsNullOrEmpty(header)) ? header.ToString() : "";
            return headerName;
        }
    }
}
