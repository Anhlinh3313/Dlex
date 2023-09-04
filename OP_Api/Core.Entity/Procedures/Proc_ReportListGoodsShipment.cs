using System;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using Core.Entity.Abstract;

namespace Core.Entity.Procedures
{
    public class Proc_ReportListGoodsShipment : IEntityProcView
    {
        public const string ProcName = "Proc_ReportListGoodsShipment";
        [Key]
        public int Id { get; set; }
        public DateTime? OrderDate { get; set; }
        public string ShipmentNumber { get; set; }
        public string SOENTRY { get; set; }
        public string SenderCode { get; set; }
        public string SenderName { get; set; }
        public string CompanyFrom { get; set; }
        public string SenderPhone { get; set; }
        public string AddressNoteFrom { get; set; }
        public string PickingAddress { get; set; }
        public string ReceiverName { get; set; }
        public string CompanyTo { get; set; }
        public string ReceiverPhone { get; set; }
        public string AddressNoteTo { get; set; }
        public double? COD { get; set; }
        public int? TotalBox { get; set; }
        public double? Weight { get; set; }
        public double? CalWeight { get; set; }
        public string ServiceName { get; set; }
        public double? DefaultPrice { get; set; }
        public double? TotalDVGT { get; set; }
        public double? PriceCOD { get; set; }
        public double? TotalPrice { get; set; }
        public string Content { get; set; }
        public string CusNote { get; set; }
        public string Note { get; set; }
        public string FromProvinceName { get; set; }
        public string FromDistrictName { get; set; }
        public string ToProvinceName { get; set; }
        public string ToDistrictName { get; set; }
        public DateTime? InOutDate { get; set; }
        public Int64 RowNum { get; set; }
        public int TotalCount { get; set; }

        public Proc_ReportListGoodsShipment() { }

        public static IEntityProc GetEntityProc(int? typeId = null, int? createdByHubId = null, int? fromHubId = null, int? toHubId = null,
            int? userId = null, int? statusId = null, int? transportTypeId = null, int? tplId = null, int? senderId = null, DateTime? dateFrom = null,
            DateTime? dateTo = null, string searchText = null, int? pageNumber = 1, int? pageSize = 20)
        {

            SqlParameter _TypeId = new SqlParameter("@TypeId", typeId);
            if (!typeId.HasValue) _TypeId.Value = DBNull.Value;

            SqlParameter _CreatedByHubId = new SqlParameter("@CreateByHubId", createdByHubId);
            if (!createdByHubId.HasValue) _CreatedByHubId.Value = DBNull.Value;

            SqlParameter _FromHubId = new SqlParameter("@FromHubId", fromHubId);
            if (!fromHubId.HasValue) _FromHubId.Value = DBNull.Value;

            SqlParameter _ToHubId = new SqlParameter("@ToHubId", toHubId);
            if (!toHubId.HasValue) _ToHubId.Value = DBNull.Value;

            SqlParameter _UserId = new SqlParameter("@UserId", userId);
            if (!userId.HasValue) _UserId.Value = DBNull.Value;

            SqlParameter _StatusId = new SqlParameter("@StatusId", statusId);
            if (!statusId.HasValue) _StatusId.Value = DBNull.Value;

            SqlParameter _TransportTypeId = new SqlParameter("@TransportTypeId", transportTypeId);
            if (!transportTypeId.HasValue) _TransportTypeId.Value = DBNull.Value;

            SqlParameter _TPLId = new SqlParameter("@TPLId", tplId);
            if (!tplId.HasValue) _TPLId.Value = DBNull.Value;

            SqlParameter _SenderId = new SqlParameter("@Senderid", senderId);
            if (!senderId.HasValue) _SenderId.Value = DBNull.Value;

            SqlParameter _DateFrom = new SqlParameter("@DateFrom", dateFrom);
            if (!dateFrom.HasValue) _DateFrom.Value = DBNull.Value;

            SqlParameter _DateTo = new SqlParameter("@DateTo", dateTo);
            if (!dateTo.HasValue) _DateTo.Value = DBNull.Value;

            SqlParameter _SearchText = new SqlParameter("@SearchText", searchText);
            if (string.IsNullOrWhiteSpace(searchText)) _SearchText.Value = DBNull.Value;

            SqlParameter _PageNumber = new SqlParameter("@PageNumber", pageNumber);
            if (!pageNumber.HasValue) _PageNumber.Value = 1;

            SqlParameter _PageSize = new SqlParameter("@PageSize", pageSize);
            if (!pageSize.HasValue) _PageSize.Value = 20;

            return new EntityProc(
                $"{ProcName} @TypeId, @CreateByHubId, @FromHubId, @ToHubId, @UserId, @StatusId, @TransportTypeId, @TPLId, @SenderId, @DateFrom, @DateTo, @SearchText, @PageNumber, @PageSize",
                new SqlParameter[] {
                _TypeId,
                _CreatedByHubId,
                _FromHubId,
                _ToHubId,
                _UserId,
                _StatusId,
                _TransportTypeId,
                _TPLId,
                _SenderId,
                _DateFrom,
                _DateTo,
                _SearchText,
                _PageNumber,
                _PageSize
                }
            );
        }

    }
}
