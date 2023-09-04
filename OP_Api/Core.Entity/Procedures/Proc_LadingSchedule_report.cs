using System;
using System.Data.SqlClient;
using Core.Entity.Abstract;

namespace Core.Entity.Procedures
{
    public class Proc_LadingSchedule_report : IEntityProcView
    {
        public const string ProcName = "Proc_LadingSchedule_report";

        public int Id { get; set; }
        public DateTime LadingScheduleCreatedWhen { get; set; }
        public int ShipmentStatusId { get; set; }
        public string ShipmentStatusName { get; set; }
        public int? LadingScheduleHubId { get; set; }
        public string LadingScheduleHubName { get; set; }
        public string Note { get; set; }
        public int ShipmentId { get; set; }
        public string ShipmentNumber { get; set; }
        public string PickingAddress { get; set; }
        public DateTime CreatedWhen { get; set; }
        public int FromHubId { get; set; }
        public string FromCode { get; set; }
        public string FromHubName { get; set; }
        public string FromHubAddress { get; set; }
        public int? FromHubRoutingId { get; set; }
        public string FromHubRoutingName { get; set; }
        public int FromProvinceProvinceId { get; set; }
        public string FromProvinceName { get; set; }
        public int FromDistrictId { get; set; }
        public string FromDistrictName { get; set; }
        public int FromWardId { get; set; }
        public string FromWardName { get; set; }
        public int? ToHubId { get; set; }
        public string ToCode { get; set; }
        public string ToHubName { get; set; }
        public string ToHubAddress { get; set; }
        public int? ToHubRoutingId { get; set; }
        public string ToHubRoutingName { get; set; }
        public int? ToProvinceProvinceId { get; set; }
        public string ToProvinceName { get; set; }
        public int? ToDistrictId { get; set; }
        public string ToDistrictName { get; set; }
        public int? ToWardId { get; set; }
        public string ToWardName { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverPhone { get; set; }
        public string ShippingAddress { get; set; }
        public string RealRecipientName { get; set; }
        public int? PaymentTypeId { get; set; }
        public string PaymentTypeName { get; set; }
        public DateTime? StartPickTime { get; set; }
        public DateTime? EndPickTime { get; set; }
        public DateTime? StartReturnTime { get; set; }
        public DateTime? EndReturnTime { get; set; }
        public string ReturnInfo { get; set; }
        public DateTime? StartDeliveryTime { get; set; }
        public DateTime? EndDeliveryTime { get; set; }
        public DateTime? ExpectedDeliveryTime { get; set; }
        public DateTime? FirstDeliveredTime { get; set; }
        public int? NumPick { get; set; }
        public int? NumDeliver { get; set; }
        public int? NumReturn { get; set; }
        public int? PickUserId { get; set; }
        public string PickupCode { get; set; }
        public string PickupFullName { get; set; }
        public int? DeliverUserId { get; set; }
        public string DeliverCode { get; set; }
        public string DeliverFullName { get; set; }
        public string TransferFullName { get; set; }
        public DateTime? StartTransferTime { get; set; }
        public DateTime? EndTransferTime { get; set; }
        public DateTime? DMTransferCODToHubTime { get; set; }
        public DateTime? HubTransferCODToAccountantTime { get; set; }
        public DateTime? AccountantReceivedCODTime { get; set; }
        public double? Weight { get; set; }
        public double? CalWeight { get; set; }
        public int TotalBox { get; set; }
        public string CusNote { get; set; }
        public string PickupNote { get; set; }
        public string DeliveryNote { get; set; }
        public string ReturnNote { get; set; }
        public string DeliveryCancelNote { get; set; }
        public int? ReturnUserId { get; set; }
        public string ReturnCode { get; set; }
        public string ReturnFullName { get; set; }
        //public int? PackageId { get; set; }
        //public string PackageName { get; set; }

        public Proc_LadingSchedule_report()
        {
        }

        public static IEntityProc GetEntityProc(int type, DateTime fromCreatedWhen, DateTime toCreatedWhen)
        {
            return new EntityProc(
                $"{ProcName} @Type,@FromCreatedWhen,@ToCreatedWhen",
                new SqlParameter[] {
                new SqlParameter("@Type", type),
                new SqlParameter("@FromCreatedWhen", fromCreatedWhen.ToString("yyyy-MM-dd 00:00:00")),
                new SqlParameter("@ToCreatedWhen", toCreatedWhen.ToString("yyyy-MM-dd 23:59:59"))
                }
            );
        }
    }
}
