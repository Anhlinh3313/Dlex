using System;
using System.Data.SqlClient;
using Core.Entity.Abstract;

namespace Core.Entity.Procedures
{
    public class Proc_ReportKPICustomer : IEntityProcView
    {
        public const string ProcName = "Proc_ReportKPICustomer";

        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string TaxCode { get; set; }
        public string PhoneNumber { get; set; }
        public int TotalShipment { get; set; }
        public int TotalShipmentReal { get; set; }
        public int TotalComplete { get; set; }
        public int TotalReturn { get; set; }
        public int TotalDeliveredOnTime { get; set; }
        public double? TotalPriceAll { get; set; }
        public int TotalIncidents { get; set; }
        public double? TotalCompensationValue { get; set; }
        public int RatioCompleted { get; set; }
        public int RatioOnTime { get; set; }
        public int RatioReturn { get; set; }


        public Proc_ReportKPICustomer()
        {
        }

        public static IEntityProc GetEntityProc(DateTime? dateFrom = null, DateTime? dateTo = null, int? hubId = null, int? customerId = null)
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

            SqlParameter CustomerId = new SqlParameter("@CustomerId", customerId);
            if (!customerId.HasValue)
                CustomerId.Value = DBNull.Value;

            return new EntityProc(
                $"{ProcName} @DateFrom, @DateTo, @HubId, @CustomerId",
                new SqlParameter[] {
                DateFrom,
                DateTo,
                HubId,
                CustomerId
                }
            );
        }
    }
}

