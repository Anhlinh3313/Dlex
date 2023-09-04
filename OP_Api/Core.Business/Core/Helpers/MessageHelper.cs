using System;
namespace Core.Business.Core.Helpers
{
    public static class MessageHelper
    {
        public const string REQUEST_TO_PICKUP = "Có {0} yêu cầu cần xử lý lấy hàng";
        public const string REQUEST_TO_DELIVERY = "Có {0} yêu cầu cần xử lý giao hàng";
        public const string REQUEST_TO_TRANSIT = "Có {0} yêu cầu cần xử lý trung chuyển";
        public const string REQUEST_CANCEL = "Yêu cầu {0} đã bị huỷ";
    }
}
