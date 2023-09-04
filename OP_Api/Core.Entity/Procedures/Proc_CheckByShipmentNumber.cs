using System;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using Core.Entity.Abstract;

namespace Core.Entity.Procedures
{
    public class Proc_CheckByShipmentNumber : IEntityProcView
    {
        public const string ProcName = "Proc_CheckByShipmentNumber";

        public int Id { get; set; }
        public string ConcurrencyStamp { get; set; }
        public virtual bool IsEnabled { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? CreatedWhen { get; set; }
        public string ShipmentNumber { get; set; }
        public string SOENTRY { get; set; }
        public string SenderName { get; set; }
        public string SenderPhone { get; set; }
        public string PickingAddress { get; set; }
        public string FromHubName { get; set; }
        public string FromHubRoutingName { get; set; }
        public string FromWardName { get; set; }
        public int? CurrentHubId { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverPhone { get; set; }
        public string ShippingAddress { get; set; }
        public string RealRecipientName { get; set; }
        public string ToHubName { get; set; }
        public string ToHubRoutingName { get; set; }
        public string ToWardName { get; set; }
        public string ServiceName { get; set; }
        public string PaymentTypeName { get; set; }
        public string ShipmentStatusName { get; set; }
        public int ShipmentStatusId { get; set; }
        public double? Weight { get; set; }
        public double? CalWeight { get; set; }
        public DateTime? EndDeliveryTime { get; set; }
        public string CusNote { get; set; }
        public string DeliveryImagePath { get; set; }
        public string FromDistrictName { get; set; }
        public string FromProvinceName { get; set; }
        public string ToDistrictName { get; set; }
        public string ToProvinceName { get; set; }
        public double? COD { get; set; }
        public double? Insured { get; set; }
        public double? TotalPrice { get; set; }
        public string AddressNoteFrom { get; set; }
        public string AddressNoteTo { get; set; }
        public int TotalBox { get; set; }
        public string RequestShipmentNumber { get; set; }
        public int? RequestShipmentId { get; set; }
        public int? KeepingCODHubId { get; set; }
        public int? KeepingTotalPriceHubId { get; set; }
        public int? KeepingCODEmpId { get; set; }
        public int? KeepingTotalPriceEmpId { get; set; }
        public string ListGoodsName { get; set; }
        public int? TPLId { get; set; }
        public string TPLNumber { get; set; }
        public bool? IsProcessError { get; set; }
        // use for calculate
        public int? StructureId { get; set; }
        public int? FromDistrictId { get; set; }
        public int? FromWardId { get; set; }
        public int? ServiceId { get; set; }
        public int? ToDistrictId { get; set; }
        public int? TotalItem { get; set; }
        public double? DefaultPrice { get; set; }
        public double? OtherPrice { get; set; }
        public int? DeliverUserId { get; set; }
        public string DeliverUserName { get; set; }
        public string ParrentShipmentNumber { get; set; }
        public bool IsCreatedChild { get; set; }
        public string PickupImagePath { get; set; }
        public string ShipmentTypeName { get; set; }
        public int? TypeId { get; set; }
        public bool IsReturn { get; set; }
        public bool IsPrioritize { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string SenderCode { get; set; }
        public string ServiceDVGTCodes { get; set; }
        public double? PriceCOD { get; set; }
        public string TPLName { get; set; }
        public string PaymentCODCode { get; set; }
        public string ToHubRoutingEmp { get; set; }
        public string Note { get; set; }
        public string Content { get; set; }
        public string StructureName { get; set; }
        public DateTime? InOutDate { get; set; }

        public Proc_CheckByShipmentNumber()
        {

        }

        public static IEntityProc GetEntityProc(string shipmentNumber, int? userId, int? requestShipmentId)
        {
            SqlParameter ShipmentNumber = new SqlParameter("@ShipmentNumber", shipmentNumber);
            if (string.IsNullOrWhiteSpace(shipmentNumber)) ShipmentNumber.Value = DBNull.Value;

            SqlParameter UserId = new SqlParameter("@UserId", userId);
            if (!userId.HasValue) UserId.Value = DBNull.Value;

            SqlParameter RequestShipmentId = new SqlParameter("@RequestShipmentId", requestShipmentId);
            if (!requestShipmentId.HasValue) RequestShipmentId.Value = DBNull.Value;

            return new EntityProc(
                $"{ProcName} @ShipmentNumber, @UserId, @RequestShipmentId",
                new SqlParameter[] {
                    ShipmentNumber,UserId,RequestShipmentId
                }
            );
        }
    }
}
