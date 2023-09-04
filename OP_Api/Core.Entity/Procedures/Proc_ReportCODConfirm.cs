using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;


namespace Core.Entity.Procedures
{
    public class Proc_ReportCODConfirm : IEntityProcView
    {
        public const string ProcName = "Proc_ReportCODConfirm";

        [Key]
        public int Id { get; set; }
        public string ShipmentNumber { get; set; }
        public int? RequestShipmentId { get; set; }
        public string CusNote { get; set; }
        public DateTime? OrderDate { get; set; }
        public int? PaymentTypeId { get; set; }
        public string PaymentTypeName { get; set; }
        public int? ServiceId { get; set; }
        public string ServiceName { get; set; }
        public double? DefaultPrice { get; set; }
        public double? FuelPrice { get; set; }
        public double? RemoteAreasPrice { get; set; }
        public double? TotalDVGT { get; set; }
        public double? VATPrice { get; set; }
        public double? TotalPrice { get; set; }
        public int? ShipmentStatusId { get; set; }
        public string ShipmentStatusName { get; set; }
        public int? DeliverUserId { get; set; }
        public string DeliverUserName { get; set; }
        public string SenderName { get; set; }
        public string SenderPhone { get; set; }
        public string CompanyFrom { get; set; }
        public string PickingAddress { get; set; }
        public int? FromProvinceId { get; set; }
        public string FromProvinceName { get; set; }
        public int? FromHubId { get; set; }
        public string FromHubName { get; set; }
        public int? FromHubRoutingId { get; set; }
        public string FromHubRoutingName { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverPhone { get; set; }
        public string CompanyTo { get; set; }
        public string ShippingAddress { get; set; }
        public string RealRecipientName { get; set; }
        public DateTime? EndDeliveryTime { get; set; }
        public int? ToProvinceId { get; set; }
        public string ToProvinceName { get; set; }
        public int? ToHubRoutingId { get; set; }
        public string ToHubRoutingName { get; set; }
        public string ListReceiptMoneyCode { get; set; }
        public double? ListReceiptMoneyGrandTotal { get; set; }
        public string ListReceiptMoneyFullName { get; set; }
        public bool? ListReceiptMoneyIsTransfer { get; set; }
        public double? COD { get; set; }
        public int? TotalCount { get; set; }
        public double? TotalCOD { get; set; }

        public Proc_ReportCODConfirm()
        {
        }

        public static IEntityProc GetEntityProc(
            int? fromHubId = null,
            int? toHubId = null,
            int? currentHubId = null,
            int? serviceId = null,
            int? shipmentStatusId = null,
            int? senderId = null,
            int? currentUserId = null,
            int? accountingAccountId = null,
            int? fromProvinceId = null,
            int? toProvinceId = null,
            DateTime? dateFrom = null,
            DateTime? dateTo = null,
            int? pageNumber = 1,
            int? pageSize = 20,
            string searchText = ""
        )
        {
            SqlParameter FromHubId = new SqlParameter("@FromHubId", fromHubId);
            if (!fromHubId.HasValue)
                FromHubId.Value = DBNull.Value;

            SqlParameter ToHubId = new SqlParameter("@ToHubId", toHubId);
            if (!toHubId.HasValue)
                ToHubId.Value = DBNull.Value;

            SqlParameter CurrentHubId = new SqlParameter("@CurrentHubId", currentHubId);
            if (!currentHubId.HasValue)
                CurrentHubId.Value = DBNull.Value;

            SqlParameter ServiceId = new SqlParameter("@ServiceId", serviceId);
            if (!serviceId.HasValue)
                ServiceId.Value = DBNull.Value;

            SqlParameter ShipmentStatusId = new SqlParameter("@ShipmentStatusId", shipmentStatusId);
            if (!shipmentStatusId.HasValue)
                ShipmentStatusId.Value = DBNull.Value;

            SqlParameter SenderId = new SqlParameter("@SenderId", senderId);
            if (!senderId.HasValue)
                SenderId.Value = DBNull.Value;

            SqlParameter CurrentUserId = new SqlParameter("@CurrentUserId", currentUserId);
            if (!currentUserId.HasValue)
                CurrentUserId.Value = DBNull.Value;

            SqlParameter AccountingAccountId = new SqlParameter("@AccountingAccountId", accountingAccountId);
            if (!accountingAccountId.HasValue)
                AccountingAccountId.Value = DBNull.Value;

            SqlParameter FromProvinceId = new SqlParameter("@FromProvinceId", fromProvinceId);
            if (!fromProvinceId.HasValue)
                FromProvinceId.Value = DBNull.Value;

            SqlParameter ToProvinceId = new SqlParameter("@ToProvinceId", toProvinceId);
            if (!toProvinceId.HasValue)
                ToProvinceId.Value = DBNull.Value;

            SqlParameter DateFrom = new SqlParameter("@DateFrom", dateFrom);
            if (!dateFrom.HasValue)
                DateFrom.Value = DBNull.Value;

            SqlParameter DateTo = new SqlParameter("@DateTo", dateTo);
            if (!dateTo.HasValue)
                DateTo.Value = DBNull.Value;

            SqlParameter PageNumber = new SqlParameter("@PageNumber", pageNumber);
            if (!pageNumber.HasValue)
                PageNumber.Value = DBNull.Value;

            SqlParameter PageSize = new SqlParameter("@PageSize", pageSize);
            if (!pageSize.HasValue)
                PageSize.Value = DBNull.Value;

            SqlParameter SearchText = new SqlParameter("@SearchText", searchText);
            if (string.IsNullOrWhiteSpace(searchText))
                SearchText.Value = DBNull.Value;

            return new EntityProc(
                $"{ProcName} @FromHubId, @ToHubId, @CurrentHubId, @ServiceId, @ShipmentStatusId," +
                $"@SenderId, @CurrentUserId, @AccountingAccountId, @FromProvinceId, @ToProvinceId," + 
                $"@DateFrom, @DateTo, @PageNumber, @PageSize, @SearchText",
                new SqlParameter[] {
                FromHubId, ToHubId, CurrentHubId, ServiceId, ShipmentStatusId,
                SenderId, CurrentUserId, AccountingAccountId, FromProvinceId, ToProvinceId,
                DateFrom, DateTo, PageNumber, PageSize, SearchText
                }
                //@FromHubId INT = NULL,
                //@ToHubId INT = NULL,
                //@CurrentHubId INT = NULL,
                //@ServiceId INT = NULL,
                //@ShipmentStatusId INT = NULL,
                //@SenderId INT = NULL,
                //@CurrentUserId INT = NULL,
                //@AccountingAccountId INT = NULL,
                //@FromProvinceId INT = NULL,
                //@ToProvinceId INT = NULL,
                //@DateFrom DATETIME = NULL,
                //@DateTo DATETIME = NULL,
                //@PageNumber INT = null,
                //@PageSize INT = null,
                //@SearchText NVARCHAR(500) = NULL
            );
        }
    }
}