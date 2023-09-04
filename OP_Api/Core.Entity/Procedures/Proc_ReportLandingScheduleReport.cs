using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;
namespace Core.Entity.Procedures
{
    public class Proc_ReportLandingScheduleReport : IEntityProcView
    {
        public const string ProcName = "Proc_ReportLandingScheduleReport";


        [Key]
        public int Id { get; set; }
        public string ShipmentNumber { get; set; }
        public string ReShipmentNumber { get; set; }
        public DateTime? OrderDate { get; set; }
        public string SenderCode { get; set; }
        public string SenderName { get; set; }
        public string AddressNoteFrom { get; set; }
        public string PickingAddress { get; set; }
        public string FromWardName { get; set; }
        public string FromDistrictName { get; set; }
        public string FromProvinceName { get; set; }
        public string FromHubRoutingName { get; set; }
        public string ReceiverCode { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverPhone { get; set; }
        public string AddressNoteTo { get; set; }
        public string ShippingAddress { get; set; }
        public string ToWardName { get; set; }
        public string ToProvinceName { get; set; }
        public string ToHubName { get; set; }
        public string ToHubRoutingName { get; set; }
        public double? COD { get; set; }
        public double? Insured { get; set; }
        public double? Weight { get; set; }
        public double? CalWeight { get; set; }
        public double? CusWeight { get; set; }
        public string ServiceName { get; set; }
        public string ServiceDVGTCodes { get; set; }
        public int TotalBox { get; set; }
        public int TotalItem { get; set; }
        public string StructureName { get; set; }
        public double? DefaultPrice { get; set; }
        public double? RemoteAreasPrice { get; set; }
        public double? PriceReturn { get; set; }
        public double? TotalDVGT { get; set; }
        public double? TotalPrice { get; set; }
        public double? TotalPriceSYS { get; set; }
        public string PaymentTypeName { get; set; }
        public double? PriceCOD { get; set; }
        public string PaymentCODTypeName { get; set; }
        public string Content { get; set; }
        public string CusNote { get; set; }
        public string Note { get; set; }
        public bool? IsReturn { get; set; }
        public string ShipmentStatusName { get; set; }
        public string RealRecipientName { get; set; }
        public DateTime? EndDeliveryTime { get; set; }
        public string DeliveryNote { get; set; }
        public bool? ISPrioritize { get; set; }
        public bool? IsIncidents { get; set; }
        public int? SenderId { get; set; }
        public string ReasonName { get; set; }
        public string ToDistrictName { get; set; }
        public int? KmNumberTW { get; set; }
        public int? KmNumberTD { get; set; }
        public string CompanyNameFrom { get; set; }
        public double VATPrice { get; set; }
        public double FuelPrice { get; set; }
        public string PackageCode { get; set; }
        public int? PackageId { get; set; }
        public string UserCode { get; set; }
        public string FullName { get; set; }
        public DateTime? EndReturnTime { get; set; }
        public DateTime? EndPickTime { get; set; }
        public int KmNumber { get; set; }
        public string ReceiverCode2 { get; set; }
        public string ReceiptCODCode { get; set; }
        public string ReceiptPriceCode { get; set; }
        public int? TimeCompare { get; set; }
        public double? TotalPricePay { get; set; }
        public double? Distance { get; set; }
        public string ReturnReason { get; set; }
        public DateTime? DeliveryWhenOne { get; set; }
        public string DeliveryReasonOne { get; set; }
        public DateTime? DeliveryWhenTwo { get; set; }
        public string DeliveryReasonTwo { get; set; }
        public DateTime? DeliveryWhenThree { get; set; }
        public string DeliveryReasonThree { get; set; }
        public DateTime? ReceiptCODCreatedWhen { get; set; }
        public int? CountImageDelivery { get; set; }
        public string AllTimeCompareString { get; set; }
        public string TimeCompareString { get; set; }
        public DateTime? InOutDate { get; set; }
        public DateTime? OrderDate2 { get; set; }
        public Int64? RowNum { get; set; }
        public int? TotalCount { get; set; }
        public int? TotalShipmentInPackage { get; set; }
        public int? TotalPackage { get; set; }
        public int? TotalBoxs { get; set; }
        public double? SumInsured { get; set; }
        public double? SumCOD { get; set; }
        public double? SumPriceCOD { get; set; }
        public double? SumTotalPrice { get; set; }
        public double? SumTotalPricePay { get; set; }
        public double? SumWeight { get; set; }
        //public string TimeCompareEndPickupTime { get; set; }
        //public string TimeCompareEndDeliveryTime { get; set; }
        //public DateTime? DeadlineDeliveryTime { get; set; }
        //public DateTime? TimeCompareReceiptMoney { get; set; }
        public DateTime? SubmitCODDate { get; set; }
        public DateTime? PaymentCODDate { get; set; }
        public string PaymentCode { get; set; }

        public Proc_ReportLandingScheduleReport()
        {
        }

        public static IEntityProc GetEntityProc(
            DateTime? fromDate = null,
            DateTime? toDate = null,
            int? senderId = null,
            int? paymentTypeId = null,
            int? fromProvinceId = null,
            int? toProvinceId = null,
            string toProvinceIds = null,
            double? fromWeight = null,
            double? toWeight = null,
            int? serviceId = null,
            string shipmentNumber = null,
            string shopCode = null,
            string referencesCode = null,
            string reShipmentNumber = null,
            string searchText = null,
            bool? isShortOfInfo = null,
            bool? isHasImagePickup = null,
            bool? isBox = null,
            bool? isPrintBill = null,
            int? uploadExcelId = null,
            int? groupStatusId = null,
            int? shipmentStatusId = null,
            int? fromHubId = null,
            int? toHubId = null,
            int? currentHubId = null,
            int? currentUserId = null,
            int? deadlineTypeId = null,
            int? pageNumber = null,
            int? pageSize = null,
            int? deliveryUserId = null,
            int? numIssueDelivery = null,
            bool? isSuccess = null,
            bool? isGroupEmp = null,
            int? listGoodsId = null,
            string listGoodsIds = null,
            bool? isHideInPackage = null,
            int? parentUserId = null,
            bool? isNullHubRouting = null)
        {

            SqlParameter DateFrom = new SqlParameter("@DateFrom", fromDate);
            if (!fromDate.HasValue)
                DateFrom.Value = DBNull.Value;

            SqlParameter DateTo = new SqlParameter("@DateTo", toDate);
            if (!toDate.HasValue)
                DateTo.Value = DBNull.Value;

            SqlParameter SenderId = new SqlParameter("@SenderId", senderId);
            if (!senderId.HasValue)
                SenderId.Value = DBNull.Value;

            SqlParameter PaymentTypeId = new SqlParameter("@PaymentTypeId", paymentTypeId);
            if (!paymentTypeId.HasValue)
                PaymentTypeId.Value = DBNull.Value;

            SqlParameter FromProvinceId = new SqlParameter("@FromProvinceId", fromProvinceId);
            if (!fromProvinceId.HasValue)
                FromProvinceId.Value = DBNull.Value;

            SqlParameter ToProvinceId = new SqlParameter("@ToProvinceId", toProvinceId);
            if (!toProvinceId.HasValue)
                ToProvinceId.Value = DBNull.Value;

            SqlParameter ToProvinceIds = new SqlParameter("@ToProvinceIds", toProvinceIds);
            if (string.IsNullOrWhiteSpace(toProvinceIds))
                ToProvinceIds.Value = DBNull.Value;

            SqlParameter FromWeight = new SqlParameter("@FromWeight", fromWeight);
            if (!fromWeight.HasValue)
                FromWeight.Value = DBNull.Value;

            SqlParameter ToWeight = new SqlParameter("@ToWeight", toWeight);
            if (!toWeight.HasValue)
                ToWeight.Value = DBNull.Value;

            SqlParameter ServiceId = new SqlParameter("@ServiceId", serviceId);
            if (!serviceId.HasValue)
                ServiceId.Value = DBNull.Value;

            if (!string.IsNullOrWhiteSpace(shipmentNumber))
            {
                shipmentNumber = shipmentNumber.Replace("/r/n", " ");
                shipmentNumber = shipmentNumber.Trim().Replace(" ", ",");
            }
            SqlParameter ShipmentNumber = new SqlParameter("@ShipmentNumber", shipmentNumber);
            if (string.IsNullOrWhiteSpace(shipmentNumber)) ShipmentNumber.Value = DBNull.Value;


            SqlParameter ShopCode = new SqlParameter("@ShopCode", shopCode);
            if (string.IsNullOrWhiteSpace(shopCode))
                ShopCode.Value = DBNull.Value;

            SqlParameter ReferencesCode = new SqlParameter("@ReferencesCode", referencesCode);
            if (string.IsNullOrWhiteSpace(referencesCode))
                ReferencesCode.Value = DBNull.Value;

            SqlParameter ReShipmentNumber = new SqlParameter("@ReShipmentNumber", reShipmentNumber);
            if (string.IsNullOrWhiteSpace(reShipmentNumber))
                ReShipmentNumber.Value = DBNull.Value;

            SqlParameter SearchText = new SqlParameter("@SearchText", searchText);
            if (string.IsNullOrWhiteSpace(searchText))
                SearchText.Value = DBNull.Value;

            SqlParameter IsShortOfInfo = new SqlParameter("@IsShortOfInfo", isShortOfInfo);
            if (!isShortOfInfo.HasValue)
                IsShortOfInfo.Value = DBNull.Value;

            SqlParameter IsHasImagePickup = new SqlParameter("@IsHasImagePickup", isHasImagePickup);
            if (!isHasImagePickup.HasValue)
                IsHasImagePickup.Value = DBNull.Value;

            SqlParameter IsBox = new SqlParameter("@IsBox", isBox);
            if (!isBox.HasValue)
                IsBox.Value = DBNull.Value;

            SqlParameter IsPrintBill = new SqlParameter("@IsPrintBill", isPrintBill);
            if (!isPrintBill.HasValue)
                IsPrintBill.Value = DBNull.Value;

            SqlParameter UploadExcelId = new SqlParameter("@UploadExcelId", uploadExcelId);
            if (!uploadExcelId.HasValue)
                UploadExcelId.Value = DBNull.Value;

            SqlParameter GroupStatusId = new SqlParameter("@GroupStatusId", groupStatusId);
            if (!groupStatusId.HasValue)
                GroupStatusId.Value = DBNull.Value;

            SqlParameter ShipmentStatusId = new SqlParameter("@ShipmentStatusId", shipmentStatusId);
            if (!shipmentStatusId.HasValue)
                ShipmentStatusId.Value = DBNull.Value;

            SqlParameter FromHubId = new SqlParameter("@FromHubId", fromHubId);
            if (!fromHubId.HasValue)
                FromHubId.Value = DBNull.Value;

            SqlParameter ToHubId = new SqlParameter("@ToHubId", toHubId);
            if (!toHubId.HasValue)
                ToHubId.Value = DBNull.Value;

            SqlParameter CurrentHubId = new SqlParameter("@CurrentHubId", currentHubId);
            if (!currentHubId.HasValue)
                CurrentHubId.Value = DBNull.Value;

            SqlParameter CurrentUserId = new SqlParameter("@CurrentUserId", currentUserId);
            if (!currentUserId.HasValue)
                CurrentUserId.Value = DBNull.Value;

            SqlParameter DeadlineTypeId = new SqlParameter("@DeadlineTypeId", deadlineTypeId);
            if (!deadlineTypeId.HasValue)
                DeadlineTypeId.Value = DBNull.Value;

            SqlParameter PageNumber = new SqlParameter("@PageNumber", pageNumber);
            if (!pageNumber.HasValue)
                PageNumber.Value = DBNull.Value;

            SqlParameter PageSize = new SqlParameter("@PageSize", pageSize);
            if (!pageSize.HasValue)
                PageSize.Value = DBNull.Value;

            SqlParameter DeliveryUserId = new SqlParameter("@DeliveryUserId", deliveryUserId);
            if (!deliveryUserId.HasValue)
                DeliveryUserId.Value = DBNull.Value;

            SqlParameter NumIssueDelivery = new SqlParameter("@NumIssueDelivery", numIssueDelivery);
            if (!numIssueDelivery.HasValue)
                NumIssueDelivery.Value = DBNull.Value;

            SqlParameter IsSuccess = new SqlParameter("@IsSuccess", isSuccess);
            if (!isSuccess.HasValue)
                IsSuccess.Value = DBNull.Value;

            SqlParameter IsGroupEmp = new SqlParameter("@IsGroupEmp", isGroupEmp);
            if (!isGroupEmp.HasValue)
                IsGroupEmp.Value = DBNull.Value;

            SqlParameter ListGoodsId = new SqlParameter("@ListGoodsId", listGoodsId);
            if (!listGoodsId.HasValue)
                ListGoodsId.Value = DBNull.Value;

            SqlParameter ListGoodsIds = new SqlParameter("@ListGoodsIds", listGoodsIds);
            if (string.IsNullOrWhiteSpace(listGoodsIds)) ListGoodsIds.Value = DBNull.Value;

            SqlParameter IsHideInPackage = new SqlParameter("@IsHideInPackage", isHideInPackage);
            if (!isHideInPackage.HasValue)
                IsHideInPackage.Value = DBNull.Value;

            SqlParameter ParentUserId = new SqlParameter("@ParentUserId", parentUserId);
            if (!parentUserId.HasValue)
                ParentUserId.Value = DBNull.Value;

            SqlParameter IsNullHubRouting = new SqlParameter("@IsNullHubRouting", isNullHubRouting);
            if (!isNullHubRouting.HasValue)
                IsNullHubRouting.Value = DBNull.Value;

            return new EntityProc(
                $"{ProcName} @DateFrom, @DateTo, @SenderId, @PaymentTypeId, @FromProvinceId, " +
                $"@ToProvinceId, @ToProvinceIds, @FromWeight, @ToWeight, @ServiceId, " +
                $"@ShipmentNumber, @ShopCode, @ReferencesCode, @ReShipmentNumber, @SearchText, " +
                $"@IsShortOfInfo, @IsHasImagePickup, @IsBox, " +
                $"@IsPrintBill, @UploadExcelId, @GroupStatusId, @ShipmentStatusId, @FromHubId, " +
                $"@ToHubId, @CurrentHubId, @CurrentUserId, " +
                $"@DeadlineTypeId, @PageNumber, @PageSize, @DeliveryUserId, @NumIssueDelivery, " +
                $"@IsSuccess, @IsGroupEmp, @ListGoodsId, @ListGoodsIds, @IsHideInPackage, @ParentUserId, @IsNullHubRouting",
                new SqlParameter[] {
                DateFrom, DateTo, SenderId, PaymentTypeId, FromProvinceId,
                ToProvinceId, ToProvinceIds, FromWeight, ToWeight, ServiceId,
                ShipmentNumber, ShopCode, ReferencesCode, ReShipmentNumber, SearchText,
                IsShortOfInfo, IsHasImagePickup, IsBox,
                IsPrintBill, UploadExcelId, GroupStatusId, ShipmentStatusId, FromHubId,
                ToHubId, CurrentHubId, CurrentUserId,
                DeadlineTypeId, PageNumber, PageSize, DeliveryUserId, NumIssueDelivery,
                IsSuccess, IsGroupEmp, ListGoodsId, ListGoodsIds, IsHideInPackage, ParentUserId, IsNullHubRouting
                },
                ProcName
            );
        }


    }
}
