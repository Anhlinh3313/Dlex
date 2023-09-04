using System;
namespace Core.Infrastructure.Helper
{
    public class ListGoodsStatusHelper
    {
        public ListGoodsStatusHelper()
        {
        }

        public static int READY_TO_TRANSFER = 1;   
        public static int TRANSFERRING = 2;   
        public static int TRANSFER_COMPLETE = 3;  
        public static int TRANSFER_CANCEL = 4;  
    }
}
