using System;
using System.Data.SqlClient;
using Core.Entity.Abstract;

namespace Core.Entity.Procedures
{
    public class Proc_ReportDebtPriceDetailByCustomer : IEntityProcView
    {
        public const string ProcName = "Proc_ReportDebtPriceDetailByCustomer";

        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int CustomerTypeId { get; set; }
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
        public Proc_ReportDebtPriceDetailByCustomer()
        {
        }

        public static IEntityProc GetEntityProc(int? type = null, int? customerId = null, DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            SqlParameter parameter1 = new SqlParameter(
            "@Type", type);
            if (!type.HasValue)
                parameter1.Value = DBNull.Value;
            SqlParameter parameter2 = new SqlParameter(
            "@CustomerId", customerId);
            if (!customerId.HasValue)
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
                $"{ProcName}  @Type, @CustomerId, @DateFrom, @DateTo",
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
