using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_ReportDebtPriceDetailByCustomerDetail : IEntityProcView
    {
        public const string ProcName = "Proc_ReportDebtPriceDetailByCustomerDetail";

        public int Id { get; set; }
        public string ShipmentNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public double Weight { get; set; }
        public string FromProvinceCode { get; set; }
        public string ToProvinceCode { get; set; }
        public double TotalPrice { get; set; }

        public Proc_ReportDebtPriceDetailByCustomerDetail()
        {

        }

        public static IEntityProc GetEntityProc(int customerId, DateTime? fromDate, DateTime? toDate)
        {
            SqlParameter parameter1 = new SqlParameter("@CustomerId", customerId);
            parameter1.Value = parameter1.Value ?? DBNull.Value;

            SqlParameter parameter2 = new SqlParameter("@DateFrom", fromDate);
            parameter2.Value = parameter2.Value ?? DBNull.Value;

            SqlParameter parameter3 = new SqlParameter("@DateTo", toDate);
            parameter3.Value = parameter3.Value ?? DBNull.Value;

            return new EntityProc(
                $"{ProcName} @CustomerId, @DateFrom, @DateTo",
                new SqlParameter[] {
                parameter1,
                parameter2,
                parameter3
                }
            );
        }
    }
}
