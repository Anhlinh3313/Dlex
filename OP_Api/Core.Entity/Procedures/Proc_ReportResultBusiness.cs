using System;
using System.Data.SqlClient;
using Core.Entity.Abstract;

namespace Core.Entity.Procedures
{
    public class Proc_ReportResultBusiness : IEntityProcView
    {
        public const string ProcName = "Proc_ReportResultBusiness";

        public int Id { get; set; }
        public string Code { get; set; }
        public string SalerCode { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? CreatedWhen { get; set; }
        public string CompanyName { get; set; }
        public string AddressCompany { get; set; }
        public string TaxCode { get; set; }
        public int? TotalShipment { get; set; }
        public double? TotalVAT { get; set; }
        public double? Discount { get; set; }
        public double? CommissionCus { get; set; }
        public double? Commission { get; set; }


        public Proc_ReportResultBusiness()
        {
        }

        public static IEntityProc GetEntityProc(DateTime? dateFrom = null, DateTime? dateTo = null, int? hubId = null, int? fromProvinceId = null, int? userId = null)
        {
            SqlParameter DateFrom = new SqlParameter("@DateFrom", dateFrom);
            if (!dateFrom.HasValue)
                DateFrom.Value = DBNull.Value;

            SqlParameter DateTo = new SqlParameter("@DateTo", dateTo);
            if (!dateTo.HasValue)
                DateTo.Value = DBNull.Value;

            SqlParameter HubId = new SqlParameter("@HubId", hubId);
            if (!hubId.HasValue)
                HubId.Value = DBNull.Value;

            SqlParameter FromProvinceId = new SqlParameter("@FromProvinceId", fromProvinceId);
            if (!fromProvinceId.HasValue)
                FromProvinceId.Value = DBNull.Value;

            SqlParameter UserId = new SqlParameter("@UserId", userId);
            if (!userId.HasValue)
                UserId.Value = DBNull.Value;

            return new EntityProc(
                $"{ProcName} @DateFrom, @DateTo, @HubId, @FromProvinceId, @UserId",
                new SqlParameter[] {
                DateFrom,
                DateTo,
                HubId,
                FromProvinceId,
                UserId
                }
            );
        }
    }
}
