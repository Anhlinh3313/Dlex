using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_ReportListGoodsDetail : IEntityProcView
    {
        public const string ProcName = "Proc_ReportListGoodsDetail";
        public Proc_ReportListGoodsDetail() { }

        public int Id { get; set; }
        public string ShipmentNumber { get; set; }
        public string SOENTRY { get; set; }

        public DateTime? OrderDate { get; set; }
        public DateTime? InOutDate { get; set; }
        public string FromProvinceName { get; set; }
        public string FromHubNameISSUE { get; set; }
        public string ToProvinceName { get; set; }
        public string ToHubNameISSUE { get; set; }
        public double? Insured { get; set; }
        public double? COD { get; set; }
        public double? Weight { get; set; }
        public double? CalWeight { get; set; }
        public string ServiceName { get; set; }
        public string SenderName { get; set; }
        public string SenderPhone { get; set; }
        public string PickingAddress { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverPhone { get; set; }
        public string ShippingAddress { get; set; }
        public string Content { get; set; }
        public string CusNote { get; set; }
        public string Note { get; set; }
        public int? TotalBox { get; set; }
        public string FromUserCode { get; set; }
        public string FromUserFullname { get; set; }
        public string ToUserCode { get; set; }
        public string ToUserFullname { get; set; }
        public string ListGoodsCode { get; set; }
        public string SenderCode { get; set; }
        public string AddressNoteFrom { get; set; }
        public string FromWardName { get; set; }
        public string FromDistrictName { get; set; }
        public string FromHubName { get; set; }
        public string ReceiverCode2 { get; set; }
        public string AddressNoteTo { get; set; }
        public string ToWardName { get; set; }
        public string ToDistrictName { get; set; }
        public string ToHubName { get; set; }
        public double? CusWeight { get; set; }
        public int? TotalItem { get; set; }
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
        public bool? IsReturn { get; set; }
        public string ShipmentStatusName { get; set; }
        public string RealRecipientName { get; set; }
        public DateTime? EndDeliveryTime { get; set; }
        public string DeliveryNote { get; set; }
        public bool? IsPrioritize { get; set; }
        public bool? IsIncidents { get; set; }
        public int TotalCount { get; set; }


        public static IEntityProc GetEntityProc(int? typeId = null, int? createByHubId = null, int? fromHubId = null,
            int? toHubId = null, int? userId = null, int? statusId = null, int? transportTypeId = null, int? tplId = null, 
            DateTime? dateFrom = null, DateTime? dateTo = null, string listGoodsCode = null,int? pageNumber = null, int? pageSize = null,
            string listIds = null)
        {
            SqlParameter parameter1 = new SqlParameter(
           "@TypeId", typeId);
            if (!typeId.HasValue)
                parameter1.Value = DBNull.Value;
            SqlParameter parameter2 = new SqlParameter(
           "@CreateByHubId", createByHubId);
            if (!createByHubId.HasValue)
                parameter2.Value = DBNull.Value;
            SqlParameter parameter3 = new SqlParameter(
            "@FromHubId", fromHubId);
            if (!fromHubId.HasValue)
                parameter3.Value = DBNull.Value;
            SqlParameter parameter4 = new SqlParameter(
           "@ToHubId", toHubId);
            if (!toHubId.HasValue)
                parameter4.Value = DBNull.Value;
            SqlParameter parameter5 = new SqlParameter(
           "@UserId", userId);
            if (!userId.HasValue)
                parameter5.Value = DBNull.Value;
            SqlParameter parameter6 = new SqlParameter(
           "@StatusId", statusId);
            if (!statusId.HasValue)
                parameter6.Value = DBNull.Value;
            SqlParameter parameter7 = new SqlParameter(
           "@TransportTypeId", transportTypeId);
            if (!transportTypeId.HasValue)
                parameter7.Value = DBNull.Value;
            SqlParameter parameter8 = new SqlParameter(
            "@TPLId", tplId);
            if (!tplId.HasValue)
                parameter8.Value = DBNull.Value;
            SqlParameter parameter9 = new SqlParameter(
           "@DateFrom", dateFrom);
            if (!dateFrom.HasValue)
                parameter9.Value = DBNull.Value;
            SqlParameter parameter10 = new SqlParameter(
           "@DateTo", dateTo);
            if (!dateTo.HasValue)
                parameter10.Value = DBNull.Value;

            SqlParameter parameter11 = new SqlParameter(
           "@ListGoodsCode", listGoodsCode);
            if (string.IsNullOrWhiteSpace(listGoodsCode))
                parameter11.Value = DBNull.Value;

            SqlParameter PageNumber = new SqlParameter(
           "@PageNumber", pageNumber);
            if (!pageNumber.HasValue) PageNumber.Value = 1;

            SqlParameter PageSize = new SqlParameter(
           "@PageSize", pageSize);
            if (!pageSize.HasValue) PageSize.Value = 20;

            SqlParameter ListIds = new SqlParameter(
          "@ListIds", listIds);
            if (string.IsNullOrWhiteSpace(listIds)) ListIds.Value = DBNull.Value;

            return new EntityProc(
                $"{ProcName} @TypeId, @CreateByHubId, @FromHubId, @ToHubId, @UserId, @StatusId, @TransportTypeId, @TPLId, @DateFrom, @DateTo, @ListGoodsCode, @PageNumber, @PageSize, @ListIds",
                new SqlParameter[] {
                parameter1,
                parameter2,
                parameter3,
                parameter4,
                parameter5,
                parameter6,
                parameter7,
                parameter8,
                parameter9,
                parameter10,
                parameter11,
                PageNumber,
                PageSize,
                ListIds
                }
            );
        }
    }
}
