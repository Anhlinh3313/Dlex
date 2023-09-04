using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Infrastructure.Helper
{
    public class StatusNinjaVanHelper
    {
        public StatusNinjaVanHelper() {
            Staging = new List<StatusMapping>();
            this.Staging.Add(new StatusMapping("Staging", "Khởi tạo",StatusHelper.ShipmentStatusId.NewRequest));
            this.Staging.Add(new StatusMapping("Pending Pickup", "Nhân viên đang nhận lấy hàng", StatusHelper.ShipmentStatusId.Delivering));
            this.Staging.Add(new StatusMapping("Van en-route to pickup", "Xe tải đang đến nhận hàng", StatusHelper.ShipmentStatusId.Delivering));
            this.Staging.Add(new StatusMapping("En-route to Sorting Hub", "Hàng đang trung chuyển về kho", StatusHelper.ShipmentStatusId.Delivering));
            this.Staging.Add(new StatusMapping("Arrived at Sorting Hub", "Đã đến kho", StatusHelper.ShipmentStatusId.Delivering));
            this.Staging.Add(new StatusMapping("Arrived at Origin Hub", "Đã đến bưu cục phát", StatusHelper.ShipmentStatusId.Delivering));
            this.Staging.Add(new StatusMapping("On Vehicle for Delivery", "Đang giao hàng", StatusHelper.ShipmentStatusId.Delivering));
            this.Staging.Add(new StatusMapping("Completed", "Giao hàng thành công", StatusHelper.ShipmentStatusId.DeliveryComplete));
            this.Staging.Add(new StatusMapping("Pending Reschedule", "Giao hàng không thành công", StatusHelper.ShipmentStatusId.DeliveryFail));
            this.Staging.Add(new StatusMapping("Pickup fail", "Lấy hàng không thành công", StatusHelper.ShipmentStatusId.Delivering));
            this.Staging.Add(new StatusMapping("Cancelled", "Hủy vận đơn", StatusHelper.ShipmentStatusId.Delivering));
            this.Staging.Add(new StatusMapping("Returned to Sender", "Giao hàng không thành công nhiều lần", StatusHelper.ShipmentStatusId.DeliveryFail));
        }
        public List<StatusMapping> Staging { set; get; }
    }

    public class StatusMapping
    {
        public StatusMapping() { }

        public StatusMapping(string code, string name,int statusId) {
            this.Code = code;
            this.Name = name;
            this.StatusId = statusId;
        }

        public string Code { get; set; }
        public string Name { get; set; }
        public int StatusId { get; set; }
    }
}
