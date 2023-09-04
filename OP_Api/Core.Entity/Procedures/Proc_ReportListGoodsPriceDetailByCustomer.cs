using System;
using System.Data.SqlClient;
using Core.Entity.Abstract;

namespace Core.Entity.Procedures
{
    public class Proc_ReportListGoodsPriceDetailByCustomer : IEntityProcView
    {
        public const string ProcName = "Proc_ReportListGoodsPriceDetailByCustomer";

        public int Id { get; set; }
        public string ShipmentNumber { get; set; }
        public DateTime? OrderDate { get; set; }
        public int? FromProvinceId { get; set; }
        public string FromProvinceCode { get; set; }
        public string FromProvinceName { get; set; }
        public int? ToProvinceId { get; set; }
        public string ToProvinceCode { get; set; }
        public string ToProvinceName { get; set; }
        public int? StructureId { get; set; }
        public string StructureCode { get; set; }
        public string StructureName { get; set; }
        public double Weight { get; set; }
        public double DefaultPrice { get; set; }
        public double TotalPrice { get; set; }
        public double OtherPrice { get; set; }
        public double RemoteAreasPrice { get; set; }
        public double TotalDVGT { get; set; }
        public double FuelPrice { get; set; }
        public double VatPrice { get; set; }
        public int? SenderId { get; set; }
        public string SenderCode { get; set; }
        public string SenderName { get; set; }
        public int? CustomerTypeId { get; set; }
        public string SenderPhone { get; set; }
        public string SenderFax { get; set; }
        public string SenderTaxCode { get; set; }

        public Proc_ReportListGoodsPriceDetailByCustomer()
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
