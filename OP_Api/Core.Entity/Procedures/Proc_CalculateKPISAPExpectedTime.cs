using Core.Entity.Abstract;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace Core.Entity.Procedures
{
    public class Proc_CalculateKPISAPExpectedTime : IEntityProcView
    {
        public const string ProcName = "Proc_CalculateKPISAPExpectedTime";

        [Key]
        public Guid UniqueKey { get; set; }
        public DateTime? COT { get; set; }
        public DateTime? KPIFullLading { get; set; }
        public double? KPIFullLadingDay { get; set; }
        public DateTime? KPIExportSAP { get; set; }
        public DateTime? StartTransferTime { get; set; }
        public DateTime? KPITransfer { get; set; }
        public DateTime? StartDeliveryTime { get; set; }
        public DateTime? KPIDelivery { get; set; }
        public DateTime? KPIPaymentMoney { get; set; }
        public DateTime? KPIConfirmPaymentMoney { get; set; }

        public Proc_CalculateKPISAPExpectedTime() { }


        public static IEntityProc GetEntityProc(DateTime? arDate = null, int? kpiTypeId = null, int? shipmentId = null)
        {

            SqlParameter ARDate = new SqlParameter("@ARDate", arDate);
            if (!arDate.HasValue)
                ARDate.Value = DBNull.Value;

            SqlParameter KPITypeId = new SqlParameter("@KPITypeId", kpiTypeId);
            if (!kpiTypeId.HasValue)
                KPITypeId.Value = DBNull.Value;

            SqlParameter ShipmentId = new SqlParameter("@ShipmentId", shipmentId);
            if (!shipmentId.HasValue)
                ShipmentId.Value = DBNull.Value;

            return new EntityProc(
                $"{ProcName} @ARDate, @KPITypeId, @ShipmentId",
                new SqlParameter[] {
                    ARDate,
                    KPITypeId,
                    ShipmentId
                }
            );
        }
    }
}
