using System;
using System.Data.SqlClient;
using Core.Entity.Abstract;

namespace Core.Entity.Procedures
{
    public class Proc_ReportEmpReceiptIssue : IEntityProcView
    {
        public const string ProcName = "Proc_ReportEmpReceiptIssue";

        public int Id { get; set; }
        public string EmpCode { get; set; }
        public string FullName { get; set; }
        public string HubCode { get; set; }
        public int? TotalShipment { get; set; }
        public double? TotalWeight { get; set; }
        public double? TotalCalWeight { get; set; }

        public Proc_ReportEmpReceiptIssue()
        {
        }

        public static IEntityProc GetEntityProc(DateTime? fromDate = null, DateTime? toDate = null, int? hubId = null, int? userId = null)
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

            SqlParameter UserId = new SqlParameter("@UserId", userId);
            if (!userId.HasValue)
                UserId.Value = DBNull.Value;

            return new EntityProc(
                $"{ProcName} @DateFrom, @DateTo, @HubId, @UserId",
                new SqlParameter[] {
                DateFrom,
                DateTo,
                HubId,
                UserId
                }
            );
        }
    }
}
