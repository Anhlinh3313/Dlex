using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_ReportBroadcastEmployee: IEntityProcView
    {
        public const string ProcName = "Proc_ReportBroadcastEmployee";

        public Int64 Id { get; set; }
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public int PickupComplete { get; set; }
        public int DeliveryComplete { get; set; }
        public int ReturnComplete { get; set; }
        public Proc_ReportBroadcastEmployee()
        {

        }

        public static IEntityProc GetEntityProc(int? hubId, int? empId = null, DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            SqlParameter parameter1 = new SqlParameter(
           "@HubId", hubId);
            if (!hubId.HasValue)
                parameter1.Value = DBNull.Value;
            SqlParameter parameter2 = new SqlParameter(
            "@EmpId", empId);
            if (!empId.HasValue)
                parameter2.Value = DBNull.Value;
            SqlParameter parameter3 = new SqlParameter(
            "@DateFrom", dateFrom);
            if (!dateFrom.HasValue)
                parameter3.Value = DBNull.Value;
            SqlParameter parameter4 = new SqlParameter(
            "@DateTo", dateTo);
            if (!dateTo.HasValue)
                parameter4.Value = DBNull.Value;

            return new EntityProc(
                $"{ProcName} @HubId, @EmpId, @DateFrom, @DateTo",
                new SqlParameter[] {
                parameter1,
                parameter2,
                parameter3,
                parameter4
                }
            );
        }
    }
}
