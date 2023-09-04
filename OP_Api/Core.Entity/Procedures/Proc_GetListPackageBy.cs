using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_GetListPackageBy : IEntityProcView
    {
        public const string ProcName = "Proc_GetListPackageBy";


        [Key]
        public int Id { get; set; }
        public string PackageCode { get; set; }
        public string SealNumber { get; set; }
        public double? CalWeight { get; set; }
        public double? Weight { get; set; }
        public double? Width { get; set; }
        public double? Length { get; set; }
        public double? Height { get; set; }
        public string Content { get; set; }
        public string FromHubCode { get; set; }
        public string FromHubName { get; set; }
        public string ToHubCode { get; set; }
        public string ToHubName { get; set; }
        public string StatusName { get; set; }
        public string CreatedUserCode { get; set; }
        public string CreatedUserFullName { get; set; }
        public DateTime? CreatedWhen { get; set; }
        public int? TotalShipment { get; set; }
        public int? TotalBoxs { get; set; }
        public int? TotalCount { get; set; }

        public Proc_GetListPackageBy()
        {
        }

        public static IEntityProc GetEntityProc(
            DateTime? fromDate = null,
            DateTime? toDate = null,
            int? senderId = null,
            int? paymentTypeId = null,
            int? fromProvinceId = null,
            int? toProvinceId = null,
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
            int? listGoodsId = null)
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

            SqlParameter FromWeight = new SqlParameter("@FromWeight", fromWeight);
            if (!fromWeight.HasValue)
                FromWeight.Value = DBNull.Value;

            SqlParameter ToWeight = new SqlParameter("@ToWeight", toWeight);
            if (!toWeight.HasValue)
                ToWeight.Value = DBNull.Value;

            SqlParameter ServiceId = new SqlParameter("@ServiceId", serviceId);
            if (!serviceId.HasValue)
                ServiceId.Value = DBNull.Value;

            SqlParameter ShipmentNumber = new SqlParameter("@ShipmentNumber", shipmentNumber);
            if (string.IsNullOrWhiteSpace(shipmentNumber))
                ShipmentNumber.Value = DBNull.Value;

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

            return new EntityProc(
                $"{ProcName} @DateFrom, @DateTo, @SenderId, @PaymentTypeId, @FromProvinceId, @ToProvinceId, @FromWeight, @ToWeight, @ServiceId, " +
                $"@ShipmentNumber, @ShopCode, @ReferencesCode, @ReShipmentNumber, @SearchText, @IsShortOfInfo, @IsHasImagePickup, @IsBox, " +
                $"@IsPrintBill, @UploadExcelId, @GroupStatusId, @ShipmentStatusId, @FromHubId, @ToHubId, @CurrentHubId, @CurrentUserId, " +
                $"@DeadlineTypeId, @PageNumber, @PageSize, @DeliveryUserId, @NumIssueDelivery, @IsSuccess, @IsGroupEmp, @ListGoodsId",
                new SqlParameter[] {
                DateFrom,
                DateTo,
                SenderId,
                PaymentTypeId,
                FromProvinceId,
                ToProvinceId,
                FromWeight,
                ToWeight,
                ServiceId,
                ShipmentNumber,
                ShopCode,
                ReferencesCode,
                ReShipmentNumber,
                SearchText,
                IsShortOfInfo,
                IsHasImagePickup,
                IsBox,
                IsPrintBill,
                UploadExcelId,
                GroupStatusId,
                ShipmentStatusId,
                FromHubId,
                ToHubId,
                CurrentHubId,
                CurrentUserId,
                DeadlineTypeId,
                PageNumber,
                PageSize,
                DeliveryUserId,
                NumIssueDelivery,
                IsSuccess,
                IsGroupEmp,
                ListGoodsId
                }
            );
        }
    }
}
