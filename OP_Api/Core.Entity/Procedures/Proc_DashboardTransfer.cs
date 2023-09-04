using System;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using Core.Entity.Abstract;

namespace Core.Entity.Procedures
{
    public class Proc_DashboardTransfer : IEntityProcView
    {
        public const string ProcName = "Proc_DashboardTransfer";

        [Key]
        public int? TotalReadyToTransfer { get; set; }
        public int? TotalAssignEmployeeTransfer { get; set; }
        public int? Transferring { get; set; }
        public int? TotalDelivering { get; set; }


        public Proc_DashboardTransfer()
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