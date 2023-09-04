using System;
using System.Data.SqlClient;
using Core.Entity.Abstract;

namespace Core.Entity.Procedures
{
    public class Proc_ReportLadingSchedule : IEntityProcView
    {
        public const string ProcName = "Proc_ReportLadingSchedule";

        public int Id { get; set; }
        public string FromProvinceName { get; set; }
        public string ToProvinceName { get; set; }
        public string CustomerTypeName { get; set; }
        public string ShipmentNumber { get; set; }
        public DateTime? OrderDate { get; set; } // ????????? Ngày vận đơn
        public DateTime? ThoiGianNhapKho { get; set; }
        public DateTime? ThoiGianXuatKho { get; set; }
        public string DeliverUserName { get; set; }
        public string RealRecipientName { get; set; }
        public DateTime? EndDeliveryTime { get; set; } // ????????? Thời gian phát hàng
        public bool? IsReturn { get; set; }
        public string ShipmentTypeName { get; set; }
        public string RelativeShipmentNumber { get; set; }
        public string MaBangKeTienCuocDaNop { get; set; }
        public string MaBangKeCODDaNop { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public float? KPI { get; set; }
        public int TotalCount { get; set; }


        public Proc_ReportLadingSchedule()
        {
        }

        public static IEntityProc GetEntityProc(int? pageNumber = 1, int? pageSize = 20, DateTime? fromDate = null, DateTime? toDate = null, int? fromProvinceId = null, int? toProvinceId = null, int? fromHubId = null, int? toHubId = null, int? deliveryUserId = null)
        {
            SqlParameter PageNumber = new SqlParameter("@PageNumber", pageNumber);
            if (!pageNumber.HasValue)
                PageNumber.Value = DBNull.Value;

            SqlParameter PageSize = new SqlParameter("@PageSize", pageSize);
            if (!pageSize.HasValue)
                PageSize.Value = DBNull.Value;

            SqlParameter FromDate = new SqlParameter("@FromDate", fromDate);
            if (!fromDate.HasValue)
                FromDate.Value = DBNull.Value;

            SqlParameter ToDate = new SqlParameter("@ToDate", toDate);
            if (!toDate.HasValue)
                ToDate.Value = DBNull.Value;

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

            return new EntityProc(
                $"{ProcName} @FromDate, @ToDate, @FromProvinceId, @ToProvinceId, @FromHubId, @ToHubId, @DeliveryUserId,@PageNumber, @PageSize",
                new SqlParameter[] {
                FromDate,
                ToDate,
                FromProvinceId,
                ToProvinceId,
                FromHubId,
                ToHubId,
                DeliveryUserId,
                PageNumber,
                PageSize
                }
            );
        }
    }
}

