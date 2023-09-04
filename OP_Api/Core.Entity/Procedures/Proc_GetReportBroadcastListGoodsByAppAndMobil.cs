using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_GetReportBroadcastListGoodsByAppAndMobil : IEntityProcView
    {
        public const string ProcName = "Proc_GetReportBroadcastListGoodsByAppAndMobil";

        public int Id { get; set; }
        public string Code { get; set; }
        public DateTime? CreatedWhen { get; set; }
        public string HubName { get; set; }
        public int? HubId { get; set; }
        public string FullName { get; set; }
        public int? EmpId { get; set; }
        public double? Weight { get; set; }
        public int? TotalBill { get; set; }
        public int? TotalBillFinish { get; set; }
        public int? TotalBillFinishByMobile { get; set; }

        public Proc_GetReportBroadcastListGoodsByAppAndMobil()
        {

        }

        public static IEntityProc GetEntityProc(int? hubId, int? empId = null, DateTime? dateFrom = null, DateTime? dateTo = null, string shipmentNumber = null)
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
            SqlParameter parameter5 = new SqlParameter(
            "@ShipmentNumber", shipmentNumber);
            if (String.IsNullOrWhiteSpace(shipmentNumber))
                parameter5.Value = DBNull.Value;

            return new EntityProc(
                $"{ProcName} @HubId, @EmpId, @DateFrom, @DateTo, @ShipmentNumber",
                new SqlParameter[] {
                parameter1,
                parameter2,
                parameter3,
                parameter4,
                parameter5
                }
            );
        }
    }
}
