using System;
using System.Data.SqlClient;
using Core.Entity.Abstract;

namespace Core.Entity.Procedures
{
    public class Proc_ReportPayablesAndReceivablesByCustomer : IEntityProcView
    {
        public const string ProcName = "Proc_ReportPayablesAndReceivablesByCustomer";

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
        public double TotalBefore { get; set; }
        public double TotalPrice { get; set; }
        public double TotalPricePaid { get; set; }
        public double TotalCOD { get; set; }
        public double TotalCODPaid { get; set; }
        public double TotalAfter
        {
            get
            {
                return this.TotalBefore + (TotalCOD - TotalCODPaid - TotalPrice + TotalPricePaid);
            }
        }
        public Proc_ReportPayablesAndReceivablesByCustomer()
        {
        }

        public static IEntityProc GetEntityProc(int? type = null, int? customerId = null, DateTime? dateFrom = null, DateTime? dateTo = null)
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
            return new EntityProc(
                $"{ProcName} @CustomerId, @DateFrom, @DateTo, @Type",
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
