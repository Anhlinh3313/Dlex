using Core.Business.ViewModels.ExportExcelModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Business.ViewModels
{
    public class ShipmentFilterViewModel
    {
        public string type { get; set; }
        public int? pageSize { get; set; }
        public int? pageNumber { get; set; }
        public string cols { get; set; }
        //public int? Id { get; set; }
        public bool? IsEnabled { get; set; }
        public string ShipmentNumber { get; set; }
        public string SOENTRY { get; set; }
        public string ReShipmentNumber { get; set; }
        public string ShopCode { get; set; }
        public string ReferencesCode { get; set; }
        public List<string> ShipmentNumberListSelect { get; set; }
        public List<string> ShipmentNumberListUnSelect { get; set; }
        public bool? IsSuccess { get; set; }
        public DateTime? OrderDateFrom { get; set; }
        public DateTime? OrderDateTo { get; set; }
        public int? SellerId { get; set; }
        public string SellerName { get; set; }
        public string SellerPhone { get; set; }
        public int? SenderId { get; set; }
        public string SenderName { get; set; }
        public string SenderPhone { get; set; }
        public string PickingAddress { get; set; }
        public int? FromWardId { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverPhone { get; set; }
        public string ShippingAddress { get; set; }
        public int? ToWardId { get; set; }
        public int? ServiceId { get; set; }
        public int? PaymentTypeId { get; set; }
        public string CusNote { get; set; }
        public string Note { get; set; }
        public string Content { get; set; }
        public double? WeightFrom { get; set; }
        public double? WeightTo { get; set; }
        public double? Length { get; set; }
        public double? Height { get; set; }
        public double? Width { get; set; }
        public double? CalWeight { get; set; }
        public bool IsGroupEmp { get; set; }
        public int? CurrentEmpId { get; set; }
        public int? CurrentHubId { get; set; }
        public int? RequestShipmentId { get; set; }
        public double? COD { get; set; }
        public double? Insured { get; set; }
        public double? DefaultPrice { get; set; }
        public double? RemoteAreasPrice { get; set; }
        public double? FuelPrice { get; set; }
        public double? TotalDVGT { get; set; }
        public double? OtherPrice { get; set; }
        public double? VATPrice { get; set; }
        public double? TotalPrice { get; set; }
        public int? PriceListId { get; set; }
        public int? FromDistrictId { get; set; }
        public int? FromProvinceId { get; set; }
        public int? ToDistrictId { get; set; }
        public int? ToProvinceId { get; set; }
        public string ToProvinceIds { get; set; }
        public string CompanyFrom { get; set; }
        public string CompanyTo { get; set; }
        public string AddressNoteFrom { get; set; }
        public string AddressNoteTo { get; set; }
        public int? TotalBox { get; set; }
        public int? StructureId { get; set; }
        public DateTime? FirstPickupTime { get; set; }
        public DateTime? FirstDeliveredTime { get; set; }
        public DateTime? FirstReturnTime { get; set; }
        public DateTime? FirstTransferTime { get; set; }
        public DateTime? FirstReturnTransferTime { get; set; }
        public bool? IsPaidPrice { get; set; }
        public int? FromHubId { get; set; }
        public int? ToHubId { get; set; }
        public int? ReceiveHubId { get; set; }
        public int? ShipmentStatusId { get; set; }
        public int? ListGoodsId { get; set; }
        public int[] ListGoodsIds { get; set; }
        public double CompensationValue { get; set; }
        public List<int> ShipmentStatusIds { get; set; }
        public int? PickUserId { get; set; }
        public int? DeliveryUserId { get; set; }
        public DateTime? ExpectedDeliveryTime { get; set; }
        public DateTime? ExpectedDeliveryTimeSystem { get; set; }
        public DateTime? EndPickTime { get; set; }
        public int[] ServiceDVGTIds { get; set; }
        public string Location { get; set; }
        public int? KeepingTotalPriceEmpId { get; set; }//NV đang giữ tổng phí
        public int? TransferUserId { get; set; }
        //TPL
        public int? TPLCurrentId { get; set; }
        public int? TPLId { get; set; }
        public string TPLNumber { get; set; }
        public DateTime? TPLCreatedWhen { get; set; }
        //
        public string SearchText { get; set; }
        public int? CurrentHubDelivery { get; set; }
        public bool? IsExistInfoDelivery { get; set; }
        public bool? IsExistImagePickup { get; set; }
        public int? DeadlineType { get; set; }
        //
        public int? KeepingCODHubId { get; set; }           //Hub đang giữ cod
        public int? KeepingTotalPriceHubId { get; set; }    //Hub đang giữ tổng phí
        public int? KeepingCODEmpId { get; set; }           //NV đang giữ cod
        public int? ListReceiptMoneyCODId { get; set; }     //Bảng kê nộp tiền COD
        public int? ListReceiptMoneyTotalPriceId { get; set; }  //Bảng kê nộp tiền cước phí
        public int? ListCustomerPaymentCODId { get; set; }     //Bảng kê thanh toán COD khách hàng 
        public int? ListCustomerPaymentTotalPriceId { get; set; }  //Bảng kê thanh toán cước phí khách hàng 
        // times delivery
        public int? startNumDelivery { get; set; }
        public int? endNumDelivery { get; set; }
        public int? HandlingEmpId { get; set; }
        //
        public int? UploadExcelHistoryId { get; set; }
        public bool? IsSortDescending { get; set; } // sort theo thứ tự giảm dần = true, tăng dần = false, mắc định là giảm dần
        public bool? IsPrioritize { get; set; }
        public bool? IsBox { get; set; }
        public int? GroupStatusId { get; set; }
        public bool? IsPrintBill { get; set; }
        public int? DeadlineTypeId { get; set; }
        public int? NumIssueDelivery { get; set; }
        public bool? IsHideInPackage { get; set; }
        public bool? IsNullHubRouting { get; set; }
        public bool? IsAllowRequest { get; set; } = false;
        public string truckIds { get; set; }
        //
        public bool? IsCreateListReceiptCOD { get; set; }
        public int? ListReceiptCODStatusId { get; set; }

        public CustomExportFile CustomExportFile { get; set; }
    }
}
