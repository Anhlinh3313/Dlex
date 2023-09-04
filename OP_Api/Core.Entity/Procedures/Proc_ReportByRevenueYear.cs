using System;
using System.Data.SqlClient;
using Core.Entity.Abstract;


namespace Core.Entity.Procedures
{
    public class Proc_ReportByRevenueYear : IEntityProcView
    {
        public const string ProcName = "Proc_ReportByRevenueYear";

        public Int64 Id { get; set; }
        public int SenderId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public double? TotalPrice1 { get; set; }
        public double? TotalPrice2 { get; set; }
        public double? TotalPrice3 { get; set; }
        public double? TotalPrice4 { get; set; }
        public double? TotalPrice5 { get; set; }
        public double? TotalPrice6 { get; set; }
        public double? TotalPrice7 { get; set; }
        public double? TotalPrice8 { get; set; }
        public double? TotalPrice9 { get; set; }
        public double? TotalPrice10 { get; set; }
        public double? TotalPrice11 { get; set; }
        public double? TotalPrice12 { get; set; }
        public double? TotalAmount { get; set; }

        public Proc_ReportByRevenueYear()
        {
        }

        public static IEntityProc GetEntityProc(DateTime? dateGet = null)
        {

            SqlParameter DateGet = new SqlParameter("@DateGet", dateGet);
            if (!dateGet.HasValue)
                DateGet.Value = DBNull.Value;

            return new EntityProc(
                $"{ProcName} @DateGet",
                new SqlParameter[] {
                DateGet
                }
            );
        }
    }
}
