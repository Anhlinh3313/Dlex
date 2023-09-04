using System;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using Core.Entity.Abstract;

namespace Core.Entity.Procedures
{
    public class Proc_GetExportDataReportSumary : IEntityProcView
    {
        public const string ProcName = "Proc_GetExportDataReportSumary";

        public int Id { get; set; }
        public string ConcurrencyStamp { get; set; }
        public virtual bool IsEnabled { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? CreatedWhen { get; set; }
        public string ShipmentNumber { get; set; }
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
        //
        public string SenderCode { get; set; }
        public string StructureName { get; set; }
        public string Content { get; set; }
        public double FuelPrice { get; set; }
        public double TotalDVGT { get; set; }
        public double VATPrice { get; set; }
        public string CurrentHubName { get; set; }
        public string PickUserFullName { get; set; }
        public DateTime? StartPickTime { get; set; }
        public bool? IsReturn { get; set; }
        public bool? IsCreditTransfer { get; set; }
        public string TransferUserFullName { get; set; }
        public string DeliverUserFullName { get; set; }
        public DateTime? EndReturnTime { get; set; }
        public bool? IsPaidCODToCustomer { get; set; }
        public bool? IsPaidPrice { get; set; }
        public int? ListCustomerPaymentTotalPriceId { get; set; }
        public int? ListCustomerPaymentCODId { get; set; }
        public string DeliveryNote { get; set; }

        public Proc_GetExportDataReportSumary()
        {
        }

        public static IEntityProc GetEntityProc(string shipmentIds)
        {
            return new EntityProc(
                $"{ProcName} @ShipmentIds",
                new SqlParameter[] {
                new SqlParameter("@ShipmentIds", shipmentIds)
                }
            );
        }
    }
}
