using System;
using System.Data.SqlClient;
using Core.Entity.Abstract;

namespace Core.Entity.Procedures
{
    public class Proc_ReportByCustomer : IEntityProcView
    {
        public const string ProcName = "Proc_ReportByCustomer";

        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string TaxCode { get; set; }
        public int CustomerTypeId { get; set; }
        public string CustomerTypeName { get; set; }
        public int TotalShipment { get; set; }
        public int TotalShipmentReturn { get; set; }
        public double TotalCODPaid { get; set; }
        public double TotalCODUnpaid { get; set; }
        public double TotalCODUnpaidUnRecived { get; set; }
        public double TotalCODUnpaidRecived { get; set; }
        public double TotalPricePaid { get; set; }
        public double TotalPriceUnpaid { get; set; }
        public int TotalShipmentDeliveryComplete { get; set; }

        public Proc_ReportByCustomer()
        {
        }

        public static IEntityProc GetEntityProc(int? type = null,int ? customerId = null, DateTime? dateFrom = null, DateTime? dateTo = null, int? hubId = null)
        {
            SqlParameter parameter1 = new SqlParameter(
            "@CustomerId", customerId);
            if (!customerId.HasValue)
                parameter1.Value = DBNull.Value;
            SqlParameter parameter2 = new SqlParameter(
    "@DateFrom", dateFrom);
            if (!dateFrom.HasValue)
                parameter2.Value = DBNull.Value;
            SqlParameter parameter3 = new SqlParameter(
    "@DateTo", dateTo);
            if (!dateTo.HasValue)
                parameter3.Value = DBNull.Value;
            SqlParameter parameter4 = new SqlParameter(
    "@Type", type);
            if (!type.HasValue)
                parameter4.Value = DBNull.Value;

        SqlParameter HubId = new SqlParameter("@HubId", hubId);
            if (!hubId.HasValue) HubId.Value = DBNull.Value;

            return new EntityProc(
                $"{ProcName} @CustomerId, @DateFrom, @DateTo, @Type, @HubId",
                new SqlParameter[] {
                parameter1,
                parameter2,
                parameter3,
                parameter4,
                HubId
                }
            );
        }
    }
}
