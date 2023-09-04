using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entity.Entities
{
    public class ShipmentVersion : EntityBasic
    {
        public ShipmentVersion()
        {
        }

        public DateTime OrderDate { get; set; }
        public string Version { get; set; }
        public int ShipmentId { get; set; }
        public string ShipmentNumber { get; set; }
        public int SenderId { get; set; }
        public string SenderName { get; set; }
        public string SenderPhone { get; set; }
        public string PickingAddress { get; set; }
        public int? FromHubId { get; set; }
        public int? FromHubRoutingId { get; set; }
        public int? FromWardId { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverPhone { get; set; }
        public string ShippingAddress { get; set; }
        public string RealRecipientName { get; set; }
        public int? ToHubId { get; set; }
        public int? ToHubRoutingId { get; set; }
        public int? ToWardId { get; set; }
        public int? SellerId { get; set; }
        public string SellerCode { get; set; }
        public string ShopCode { get; set; }
        public string SellerName { get; set; }
        public string SellerPhone { get; set; }
        public string SellerAddress { get; set; }
        public int? ServiceId { get; set; }
        public string STRServiceDVGTIds { get; set; }
        public int? PaymentTypeId { get; set; }
        public int? ThirdPLId { get; set; }
        public int? PickUserId { get; set; }
        public int? DeliverUserId { get; set; }
        public int? TransferUserId { get; set; }
        public int? TransferReturnUserId { get; set; }
        public int? ReturnUserId { get; set; }
        public string TransferNote { get; set; }
        public int ShipmentStatusId { get; set; }
        public int NumPick { get; set; }
        public int NumDeliver { get; set; }
        public int NumReturn { get; set; }
        public int NumTransfer { get; set; }
        public int NumTransferReturn { get; set; }
        public string CurrentWarehouseName { get; set; }
        public string PickWarehouseName { get; set; }
        public string DeliveryWarehouseName { get; set; }
        public double Weight { get; set; }
        public double? CalWeight { get; set; }
        public double Length { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public string Content { get; set; }
        public string Note { get; set; }
        public DateTime? PaidDate { get; set; }
        public DateTime? StartPickTime { get; set; }
        public DateTime? EndPickTime { get; set; }
        public DateTime? StartReturnTime { get; set; }
        public DateTime? EndReturnTime { get; set; }
        public string ReturnInfo { get; set; }
        public DateTime? StartDeliveryTime { get; set; }
        public DateTime? EndDeliveryTime { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public DateTime? ExpectedDeliveryTime { get; set; }
        public DateTime? StartTransferTime { get; set; }
        public DateTime? EndTransferTime { get; set; }
        public DateTime? DMTransferCODToHubTime { get; set; }
        public DateTime? HubTransferCODToAccountantTime { get; set; }
        public DateTime? AccountantReceivedCODTime { get; set; }
        public string CusNote { get; set; }
        public string PickupNote { get; set; }
        public string DeliveryNote { get; set; }
        public string ReturnNote { get; set; }
        public string TransferReturnNote { get; set; }
        public DateTime? StartTransferReturnTime { get; set; }
        public DateTime? EndTransferReturnTime { get; set; }
        public DateTime? PickupAppointmentTime { get; set; }
        public DateTime? DeliveryAppointmentTime { get; set; }
        public DateTime? ReturnAppointmentTime { get; set; }
        public string DeliveryImagePath { get; set; }
        public string ReturnImagePath { get; set; }
        public string ChangingDeliveryAddress { get; set; }
        public string ChangingDeliveryAddressTime { get; set; }
        public string ChangingDeliveryAddressNote { get; set; }
        public int? OldDeliveryWardId { get; set; }
        public int? OldDeliveryHubId { get; set; }
        public int? OldDeliveryHubRoutingId { get; set; }
        public int? CurrentHubId { get; set; }
        public int? CreatedHubId { get; set; }
        public double LatFrom { get; set; }
        public double LngFrom { get; set; }
        public double LatTo { get; set; }
        public double LngTo { get; set; }
        public int? CurrentEmpId { get; set; }
        public int? RequestShipmentId { get; set; }
        public double COD { get; set; }
        public double Insured { get; set; }
        public double DefaultPrice { get; set; }
        public double RemoteAreasPrice { get; set; }
        public double FuelPrice { get; set; }
        public double TotalDVGT { get; set; }
        public double OtherPrice { get; set; }
        public double VATPrice { get; set; }
        public double TotalPrice { get; set; }
        public int? PriceListId { get; set; }
        public string CompanyFrom { get; set; }
        public string CompanyTo { get; set; }
        public string AddressNoteFrom { get; set; }
        public string AddressNoteTo { get; set; }
        public int? FromDistrictId { get; set; }
        public int? FromProvinceId { get; set; }
        public int? ToDistrictId { get; set; }
        public int? ToProvinceId { get; set; }
        public int? StructureId { get; set; }
        public int? PackageId { get; set; }
        public int TotalBox { get; set; }
        public int? ReceiveHubId { get; set; }
        public DateTime? FirstPickupTime { get; set; }
        public DateTime? FirstDeliveredTime { get; set; }
        public DateTime? FirstReturnTime { get; set; }
        public DateTime? FirstTransferTime { get; set; }
        public DateTime? FirstReturnTransferTime { get; set; }
        public bool IsPaidPrice { get; set; }
        public bool IsPaidCODToCustomer { get; set; }
        public int? KeepingCODHubId { get; set; }           //Hub đang giữ cod
        public int? KeepingTotalPriceHubId { get; set; }    //Hub đang giữ tổng phí
        public int? KeepingCODEmpId { get; set; }           //NV đang giữ cod
        public int? KeepingTotalPriceEmpId { get; set; }    //NV đang giữ tổng phí
        public int? ListReceiptMoneyCODId { get; set; }     //Bảng kê nộp tiền COD
        public int? ListReceiptMoneyTotalPriceId { get; set; }  //Bảng kê nộp tiền Amount
        public string DataChanged { get; set; }  //Ghi log dữ liệu sửa đổi

        [ForeignKey("RequestShipmentId")]
        public virtual RequestShipment RequestShipment { get; set; }
        [ForeignKey("SenderId")]
        public virtual Customer Sender { get; set; }
        [ForeignKey("FromHubId")]
        public virtual Hub FromHub { get; set; }
        [ForeignKey("FromHubRoutingId")]
        public virtual HubRouting FromHubRouting { get; set; }
        [ForeignKey("ToHubId")]
        public virtual Hub ToHub { get; set; }
        [ForeignKey("ToHubRoutingId")]
        public virtual HubRouting ToHubRouting { get; set; }
        [ForeignKey("FromWardId")]
        public virtual Ward FromWard { get; set; }
        [ForeignKey("ToWardId")]
        public virtual Ward ToWard { get; set; }
        public virtual Service Service { get; set; }
        public virtual Structure Structure { get; set; }
        public virtual Package Package { get; set; }
        public virtual ShipmentStatus ShipmentStatus { get; set; }
        public virtual PaymentType PaymentType { get; set; }
        [ForeignKey("ReceiveHubId")]
        public virtual Hub ReceiveHub { get; set; }
        [ForeignKey("CurrentHubId")]
        public virtual Hub CurrentHub { get; set; }
        public virtual User PickUser { get; set; }
        public virtual User DeliverUser { get; set; }
        public virtual User TransferUser { get; set; }
        public virtual User TransferReturnUser { get; set; }
        public virtual User ReturnUser { get; set; }
        public virtual User CurrentEmp { get; set; }
        [ForeignKey("CreatedBy")]
        public virtual User CreatedByUser { get; set; }
        public virtual IEnumerable<ShipmentServiceDVGTVersion> ShipmentServiceDVGTs { get; set; } = new Collection<ShipmentServiceDVGTVersion>();
    }
}
