using System;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using Core.Entity.Abstract;

namespace Core.Entity.Procedures
{
    public class Proc_DashboardPickup : IEntityProcView
    {
        public const string ProcName = "Proc_DashboardPickup";

        [Key]
        public int? TotalNewPickup { get; set; }
        public int? TotalPicking { get; set; }
        public int? TotalPickupComplete { get; set; }
        public int? TotalPickCancel { get; set; }

        public Proc_DashboardPickup()
        {
        }

        public static IEntityProc GetEntityProc(DateTime? fromDate = null, DateTime? toDate = null,  int? hubId = null)
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
