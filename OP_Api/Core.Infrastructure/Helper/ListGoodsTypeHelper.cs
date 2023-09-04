using System;
namespace Core.Infrastructure.Helper
{
    public class ListGoodsTypeHelper
    {
        public ListGoodsTypeHelper()
        {
        }

        public static int BK_CTLH = 1; //BK Lấy hàng
        public static int BK_NKLH = 2; //BL Nhập kho lấy hàng
        public static int BK_CTGH = 3; //BK xuất kho giao hàng
        public static int BK_NKGH = 7; //BK nhập kho báo phát
        public static int BK_CTTT = 8; //BK xuất kho trung chuyển
        public static int BK_NKTT = 9; //BK nhập kho trung chuyển
        public static int BK_CTTH = 10;//BK xuất kho trả hàng
        public static int BK_NKTH = 11;//BK nhập kho trả hàng [trả hàng thất bại]
        public static int BK_NKTTNL = 12; // BK nhập kho trung chuyển nhận bill lỗi
    }
}
