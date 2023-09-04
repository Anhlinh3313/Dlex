using System;
using System.Data.SqlClient;
using Core.Entity.Abstract;

namespace Core.Entity.Procedures
{
    public class Proc_ReportKPIBusiness : IEntityProcView
    {
        public const string ProcName = "Proc_ReportKPIBusiness";

        public int Id { get; set; }
        public string Code { get; set; }
        public string FullName { get; set; }
        public int? TotalCustomer { get; set; }
        public double? TotalPriceAll { get; set; }
        public double? PercentComplete { get; set; }
        public double? PercentPayment { get; set; }


        public Proc_ReportKPIBusiness()
        {
        }

        public static IEntityProc GetEntityProc(DateTime? dateFrom = null, DateTime? dateTo = null, int? hubId = null)
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

            return new EntityProc(
                $"{ProcName} @DateFrom, @DateTo, @HubId",
                new SqlParameter[] {
                DateFrom,
                DateTo,
                HubId
                }
            );
        }
    }
}

