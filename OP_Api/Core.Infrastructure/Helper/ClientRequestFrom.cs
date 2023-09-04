using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Infrastructure.Helper
{
    public static class ClientRequestFrom
    {
        public class WebBrowser
        {
            public const int AllBrowser = 1;
        }
        public class Mobile
        {
            public const int Android = 2;
            public const int IOS = 3;
        }
        public class ToolTesTAPI
        {
            public const int Postman = 4;
            public const int Orther = 5;
        }
    }
}
