using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Infrastructure.Helper
{
    public static class ComplainStatusHelper
    {
        public static int New = 1;
        public static int Receive = 2;
        public static int Handling = 3;
        public static int Forward = 4;
        public static int Complate = 5;
    }
}
