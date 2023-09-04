using System;
using System.Data.SqlClient;
using Core.Entity.Abstract;


namespace Core.Entity.Procedures
{
    public class Proc_ReportDiscountCustomer : IEntityProcView
    {
        public const string ProcName = "Proc_ReportDiscountCustomer";

        public Int64 Id { get; set; }
        public int CusId { get; set; }
        public string Code { get; set; }
        public string CompanyName { get; set; }
        public string SalerName { get; set; }
        public string HubName { get; set; }
        public double? Discount { get; set; }
        public double? Commission { get; set; }
        public double? CommissionCus { get; set; }
        public double? DiscountCPN { get; set; }
        public double? DiscountHT { get; set; }
        public double? DiscountGR { get; set; }
        public double? DiscountNC { get; set; }
        public double? Discount48h { get; set; }
        public double? DiscountDB { get; set; }
        public double? Discount72h { get; set; }
        public double? DiscountTK { get; set; }


        public Proc_ReportDiscountCustomer()
        {
        }

        public static IEntityProc GetEntityProc(DateTime? dateFrom = null, DateTime? dateTo = null, int? senderId = null, int? businessUserId = null)
        {
            SqlParameter DateFrom = new SqlParameter("@DateFrom", dateFrom);
            if (!dateFrom.HasValue)
                DateFrom.Value = DBNull.Value;

            SqlParameter DateTo = new SqlParameter("@DateTo", dateTo);
            if (!dateTo.HasValue)
                DateTo.Value = DBNull.Value;

            SqlParameter SenderId = new SqlParameter("@SenderId", senderId);
            if (!senderId.HasValue)
                SenderId.Value = DBNull.Value;

            SqlParameter BusinessUserId = new SqlParameter("@BusinessUserId", businessUserId);
            if (!businessUserId.HasValue)
                BusinessUserId.Value = DBNull.Value;

            return new EntityProc(
                $"{ProcName} @DateFrom, @DateTo, @SenderId, @BusinessUserId",
                new SqlParameter[] {
                DateFrom,
                DateTo,
                SenderId,
                BusinessUserId
                }
            );
        }
    }
}
