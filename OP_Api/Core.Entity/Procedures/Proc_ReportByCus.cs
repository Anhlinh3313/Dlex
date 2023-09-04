using System;
using System.Data.SqlClient;
using Core.Entity.Abstract;

namespace Core.Entity.Procedures
{
    public class Proc_ReportByCus : IEntityProcView
    {
        public const string ProcName = "Proc_ReportByCus";

        public Int64 Id { get; set; }
        public int SenderId { get; set; }
        public string Code { get; set; }
        public string ToProvince { get; set; }
        public int? TotalShipment { get; set; }
        public double? TotalCOD { get; set; }
        public double? TotalInsured { get; set; }
        public double? TotalAmount { get; set; }
        public double? TotalDVGT { get; set; }
        public double? TotalWeight { get; set; }
        public int? TotalBox { get; set; }
        public int? TotalShipmentDeliveryComplete { get; set; }
        public int? L1 { get; set; }
        public int? L2 { get; set; }
        public int? L3 { get; set; }
        public int? TotalShipmentReturn { get; set; }

        public Proc_ReportByCus()
        {
        }

        public static IEntityProc GetEntityProc(int? senderId = null,
            DateTime? dateFrom = null, DateTime? dateTo = null, string listProvinceIds = null, string listDeliveryIds = null)
        {

            SqlParameter SenderId = new SqlParameter("@SenderId", senderId);
            if (!senderId.HasValue)
                SenderId.Value = DBNull.Value;

            SqlParameter DateFrom = new SqlParameter("@DateFrom", dateFrom);
            if (!dateFrom.HasValue)
                DateFrom.Value = DBNull.Value;

            SqlParameter DateTo = new SqlParameter("@DateTo", dateTo);
            if (!dateTo.HasValue)
                DateTo.Value = DBNull.Value;


            SqlParameter ListProvinceIds = new SqlParameter("@ListProvinceIds", listProvinceIds);
            if (string.IsNullOrWhiteSpace(listProvinceIds))
                ListProvinceIds.Value = DBNull.Value;

            SqlParameter ListDeliveryIds = new SqlParameter("@ListDeliveryIds", listDeliveryIds);
            if (string.IsNullOrWhiteSpace(listDeliveryIds))
                ListDeliveryIds.Value = DBNull.Value;

            return new EntityProc(
                $"{ProcName} @SenderId, @DateFrom, @DateTo,@ListProvinceIds,@ListDeliveryIds",
                new SqlParameter[] {
                SenderId,
                DateFrom,
                DateTo,
                ListProvinceIds,
                ListDeliveryIds
                }
            );
        }
    }
}
