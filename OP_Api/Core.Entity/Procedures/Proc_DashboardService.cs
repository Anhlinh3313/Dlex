using System;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using Core.Entity.Abstract;

namespace Core.Entity.Procedures
{
    public class Proc_DashboardService : IEntityProcView
    {
        public const string ProcName = "Proc_DashboardService";

        [Key]
        public int? TotalCPN { get; set; }
        public int? TotalHT { get; set; }
        public int? TotalDB { get; set; }
        public int? Total48 { get; set; }
        public int? TotalGR { get; set; }
        public int? TotalTK { get; set; }
        public int? TotalKhac { get; set; }
        public double? TotalPriceCPN { get; set; }
        public double? TotalPriceHT { get; set; }
        public double? TotalPriceDB { get; set; }
        public double? TotalPrice48 { get; set; }
        public double? TotalPriceGR { get; set; }
        public double? TotalPriceTK { get; set; }
        public double? TotalPriceKhac { get; set; }


        public Proc_DashboardService()
        {
        }

        public static IEntityProc GetEntityProc(DateTime? fromDate = null, DateTime? toDate = null, int? hubId = null)
        {

            SqlParameter DateFrom = new SqlParameter("@DateFrom", fromDate);
            if (!fromDate.HasValue)
                DateFrom.Value = DBNull.Value;

            SqlParameter DateTo = new SqlParameter("@DateTo", toDate);
            if (!toDate.HasValue)
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
