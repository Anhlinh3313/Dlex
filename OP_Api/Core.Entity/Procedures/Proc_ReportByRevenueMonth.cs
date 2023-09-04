using System;
using System.Data.SqlClient;
using Core.Entity.Abstract;

namespace Core.Entity.Procedures
{
    public class Proc_ReportByRevenueMonth : IEntityProcView
    {
        public const string ProcName = "Proc_ReportByRevenueMonth";

        public Int64 Id { get; set; }
        public int SenderId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int? TotalShipment { get; set; }
        public double? TotalCOD { get; set; }
        public double? TotalInsured { get; set; }
        public double? TotalWeight { get; set; }
        public int? TotalBox { get; set; }
        public double? TotalDefaultPrice { get; set; }
        public double? TotalPriceCOD { get; set; }
        public double? TotalAmount { get; set; }
        public double? TotalRemoteAreaPrice { get; set; }
        public double? TotalPriceDVGT { get; set; }
        public int? TotalShipmentDeliveryComplete { get; set; }
        public int? TotalShipmentReturn { get; set; }
        public double? TotalPricePay { get; set; }

        public Proc_ReportByRevenueMonth()
        {
        }

        public static IEntityProc GetEntityProc(DateTime? dateFrom = null, DateTime? dateTo = null)
        {

            SqlParameter DateFrom = new SqlParameter("@DateFrom", dateFrom);
            if (!dateFrom.HasValue)
                DateFrom.Value = DBNull.Value;

            SqlParameter DateTo = new SqlParameter("@DateTo", dateTo);
            if (!dateTo.HasValue)
                DateTo.Value = DBNull.Value;

            return new EntityProc(
                $"{ProcName} @DateFrom, @DateTo",
                new SqlParameter[] {
                DateFrom,
                DateTo
                }
            );
        }
    }
}
