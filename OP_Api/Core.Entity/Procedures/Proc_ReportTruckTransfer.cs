using System;
using System.Data.SqlClient;
using Core.Entity.Abstract;

namespace Core.Entity.Procedures
{
    public class Proc_ReportTruckTransfer : IEntityProcView
    {
        public const string ProcName = "Proc_ReportTruckTransfer";

        public int Id { get; set; }
        public Int64 RowNum { get; set; }
        public int TotalCount { get; set; }
        public int? ShipmentId { get; set; }
        public string ShipmentNumber { get; set; }
        public DateTime? IssueDate { get; set; }
        public string IssueHubName { get; set; }
        public string FromProvinceName { get; set; }
        public string ToProvinceName { get; set; }
        public string IssueCode { get; set; }
        public string TruckNumber { get; set; }
        public int? TotalBox { get; set; }
        public double? Weight { get; set; }
        public double? CalWeight { get; set; }
        public double? TotalPriceAll { get; set; }


        public Proc_ReportTruckTransfer()
        {
        }

        public static IEntityProc GetEntityProc(DateTime? fromDate = null, DateTime? toDate = null, int? fromProvinceId = null, int? toProvinceId = null, int? truckId = null, int? pageNumber = 1, int? pageSize = 20)
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

            SqlParameter TruckId = new SqlParameter("@TruckId", truckId);
            if (!truckId.HasValue)
                TruckId.Value = DBNull.Value;

            SqlParameter PageNumber = new SqlParameter("@PageNum", pageNumber);
            if (!pageNumber.HasValue)
                PageNumber.Value = DBNull.Value;

            SqlParameter PageSize = new SqlParameter("@PageSize", pageSize);
            if (!pageSize.HasValue)
                PageSize.Value = DBNull.Value;

            return new EntityProc(
                $"{ProcName} @DateFrom, @DateTo, @FromProvinceId, @ToProvinceId, @TruckId, @PageNum, @PageSize",
                new SqlParameter[] {
                DateFrom,
                DateTo,
                FromProvinceId,
                ToProvinceId,
                TruckId,
                PageNumber,
                PageSize
                }
            );
        }
    }
}
