using System;
using System.Data.SqlClient;
using Core.Entity.Abstract;

namespace Core.Entity.Procedures
{
    public class Proc_ReportPrioritize : IEntityProcView
    {
        public const string ProcName = "Proc_ReportPrioritize";

        public int Id { get; set; }
        public int TotalCount { get; set; }
        public string FromProvinceName { get; set; }
        public string ToProvinceName { get; set; }
        public string CustomerTypeName { get; set; }
        public string ShipmentNumber { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? ThoiGianNhapKho { get; set; }
        public DateTime? ThoiGianXuatKho { get; set; }
        public string DeliverUserName { get; set; }
        public string RealRecipientName { get; set; }
        public string EndDeliveryTime { get; set; }
        public bool? IsReturn { get; set; }
        public string ShipmentTypeName { get; set; }
        public string RelativeShipmentNumber { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public int? KPI { get; set; }

        public Proc_ReportPrioritize()
        {
        }

        public static IEntityProc GetEntityProc(DateTime? fromDate = null, DateTime? toDate = null, int? fromProvinceId = null, int? toProvinceId = null, int? fromHubId = null, int? toHubId = null, int? deliveryUserId = null, int? senderId = null, int? pageNumber = 1, int? pageSize = 20)
        {

            SqlParameter DateFrom = new SqlParameter("@DateFrom", fromDate);
            if (!fromDate.HasValue)
                DateFrom.Value = DBNull.Value;

            SqlParameter DateTo = new SqlParameter("@DateTo", toDate);
            if (!toDate.HasValue)
                DateTo.Value = DBNull.Value;

            SqlParameter FromProvinceId = new SqlParameter("@FromProvinceId", fromProvinceId);
            if (!fromProvinceId.HasValue)
                FromProvinceId.Value = DBNull.Value;

            SqlParameter ToProvinceId = new SqlParameter("@ToProvinceId", toProvinceId);
            if (!toProvinceId.HasValue)
                ToProvinceId.Value = DBNull.Value;

            SqlParameter FromHubId = new SqlParameter("@FromHubId", fromHubId);
            if (!fromHubId.HasValue)
                FromHubId.Value = DBNull.Value;

            SqlParameter ToHubId = new SqlParameter("@ToHubId", toHubId);
            if (!toHubId.HasValue)
                ToHubId.Value = DBNull.Value;

            SqlParameter DeliveryUserId = new SqlParameter("@DeliveryUserId", deliveryUserId);
            if (!deliveryUserId.HasValue)
                DeliveryUserId.Value = DBNull.Value;

            SqlParameter SenderId = new SqlParameter("@SenderId", senderId);
            if (!senderId.HasValue)
                SenderId.Value = DBNull.Value;

            SqlParameter PageNumber = new SqlParameter("@PageNumber", pageNumber);
            if (!pageNumber.HasValue)
                PageNumber.Value = DBNull.Value;

            SqlParameter PageSize = new SqlParameter("@PageSize", pageSize);
            if (!pageSize.HasValue)
                PageSize.Value = DBNull.Value;

            return new EntityProc(
                $"{ProcName} @DateFrom, @DateTo, @FromProvinceId, @ToProvinceId, @FromHubId, @ToHubId, @DeliveryUserId, @SenderId,@PageNumber, @PageSize",
                new SqlParameter[] {
                DateFrom,
                DateTo,
                FromProvinceId,
                ToProvinceId,
                FromHubId,
                ToHubId,
                DeliveryUserId,
                SenderId,
                PageNumber,
                PageSize
                }
            );
        }
    }
}
