using System;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using Core.Entity.Abstract;

namespace Core.Entity.Procedures
{
    public class Proc_GetShipmentToPrint : IEntityProcView
    {
        public const string ProcName = "Proc_GetShipmentToPrint";

        [Key]
        public int Id { get; set; }
        public string ShipmentNumber { get; set; }
        public int ShipmentStatusId { get; set; }
        public string ShopCode { get; set; }
        public DateTime OrderDate { get; set; }
        public int? SenderId { get; set; }
        public string SenderName { get; set; }
        public string SenderCode { get; set; }
        public bool? IsShowPrice { get; set; }
        public string customerNotePrint { get; set; }
        public string AddressNoteFrom { get; set; }
        public string PickingAddress { get; set; }
        public string SenderPhone { get; set; }
        public int? FromHubId { get; set; }
        public string FromHubName { get; set; }
        public string FromHubCode { get; set; }
        public int? FromDistrictId { get; set; }
        public string FromDistrictCode { get; set; }
        public string FromDistrictName { get; set; }
        public int? FromProvinceId { get; set; }
        public string FromProvinceCode { get; set; }
        public string FromProvinceName { get; set; }
        public int? StructureId { get; set; }
        public string StructureName { get; set; }
        public int TotalBox { get; set; }
        public double? Weight { get; set; }
        public double? CalWeight { get; set; }
        public string Content { get; set; }
        public string CusNote { get; set; }
        public int? ServiceId { get; set; }
        public string ServiceCode { get; set; }
        public string ServiceName { get; set; }
        public int? RequestShipmentId { get; set; }
        public string RequestShipmentNumber { get; set; }
        public int? PickUserId { get; set; }
        public string PickUserCode { get; set; }
        public string PickUserFullName { get; set; }
        public string ToHubName { get; set; }
        public string ToHubCode { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverPhone { get; set; }
        public string ShippingAddress { get; set; }
        public string AddressNoteTo { get; set; }
        public int? ToDistrictId { get; set; }
        public string ToDistrictCode { get; set; }
        public string ToDistrictName { get; set; }
        public int? ToProvinceId { get; set; }
        public string ToProvinceCode { get; set; }
        public string ToProvinceName { get; set; }
        public int? PaymentTypeId { get; set; }
        public string PaymentTypeCode { get; set; }
        public string PaymentTypeName { get; set; }
        public double? DefaultPrice { get; set; }
        public double? TotalPrice { get; set; }
        public double? COD { get; set; }
        public double? VATPrice { get; set; }
        public double? Insured { get; set; }
        public double? FuelPrice { get; set; }
        public double? RemoteAreasPrice { get; set; }
        public double? TotalDVGT { get; set; }
        public double? OtherPrice { get; set; }
        public int? TPLId { get; set; }
        public string TPLNumber { get; set; }
        public int? ShipmentRelativeId { get; set; }
        public string ShipmentRelativeNumber { get; set; }
        public bool? IsReturn { get; set; }
        public string STRServiceDVGTIds { get; set; }
        public int? DeliverUserId { get; set; }
        public string DeliveryUserCode { get; set; }
        public string DeliveryUserFullName { get; set; }
        public string ServiceDVGTCodes { get; set; }
        public string ReceiptCode { get; set; }
        public string BusenissUserCode { get; set; }
        public string PaymentCODCode { get; set; }
        public string PaymentCODName { get; set; }
        public double? PriceCOD { get; set; }
        public double? PriceReturn { get; set; }
        public double? Width { get; set; }
        public double? Length { get; set; }
        public double? Height { get; set; }
        public string CODf { get; set; }
        public string Insuredf { get; set; }
        public string DefaultPricef { get; set; }
        public string RemoteAreasPricef { get; set; }
        public string TotalDVGTf { get; set; }
        public string TotalPricef { get; set; }
        public string PriceCODf { get; set; }
        public string OrderDatef { get; set; }
        public string TotalCollect { get; set; }
        public string CompanyTo { get; set; }
        public int? Doc { get; set; }
        public string ListBox { get; set; }
        public string FromWardName { get; set; }
        public string ToWardName { get; set; }

        public Proc_GetShipmentToPrint()
        {
        }

        public static IEntityProc GetEntityProc(string ids)
        {
            return new EntityProc(
                $"{ProcName} @Ids",
                new SqlParameter[] {
                new SqlParameter("@Ids", ids)
                }
            );
        }
    }
}
