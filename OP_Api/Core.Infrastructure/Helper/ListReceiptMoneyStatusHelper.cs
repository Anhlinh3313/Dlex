using System;
namespace Core.Infrastructure.Helper
{
    public static class ListReceiptMoneyStatusHelper
    {
        public static int CREATED = 1;
        public static int LOCKED = 2;
        public static int CONFIRMED = 3;
        public static int CANCELLED = 4;
        public static int UNLOCKED = 5;
    }
}
