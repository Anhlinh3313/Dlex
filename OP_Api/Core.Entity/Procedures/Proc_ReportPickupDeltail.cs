using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_ReportPickupDeltail : IEntityProcView
    {
        public const string ProcName = "Proc_ReportPickupDeltail";

        public int Id { get; set; }
        public int ShipmentId { get; set; }
        public string ShipmentNumber { get; set; }
        public string SOENTRY { get; set; }
        public DateTime? OrderDate { get; set; }
        public double? TotalPrice { get; set; }
        public double? COD { get; set; }
        public string ServiceName { get; set; }
        public double? Weight { get; set; }
        public double? CalWeight { get; set; }
        public DateTime? EndPickTime { get; set; }
        public string SenderName { get; set; }

        public Proc_ReportPickupDeltail()
        {
        }

        public static IEntityProc GetEntityProc(int? userId = null, DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            SqlParameter parameter1 = new SqlParameter(
            "@UserId", userId);
            if (!userId.HasValue)
                parameter1.Value = DBNull.Value;
            SqlParameter parameter2 = new SqlParameter(
            "@DateFrom", dateFrom);
            if (!dateFrom.HasValue)
                parameter2.Value = DBNull.Value;
            SqlParameter parameter3 = new SqlParameter(
            "@DateTo", dateTo);
            if (!dateTo.HasValue)
                parameter3.Value = DBNull.Value;

            return new EntityProc(
                $"{ProcName} @UserId, @DateFrom, @DateTo",
                new SqlParameter[] {
                parameter1,
                parameter2,
                parameter3
                }
            );
        }
    }
}
