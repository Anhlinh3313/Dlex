using System;
using System.Data.SqlClient;
using Core.Entity.Abstract;

namespace Core.Entity.Procedures
{
    public class Proc_ShipmentHubKeeping : IEntityProcView
    {
        public const string ProcName = "Proc_ShipmentHubKeeping";

        public Int32 Id { get; set; }
        public DateTime? CreatedWhen { get; set; }
        public string ShipmentNumber { get; set; }
        public int? ShipmentStatusId { get; set; }
        public string ShipmentStatusName { get; set; }
        public int? PaymentTypeId { get; set; }
        public string PaymentTypeName { get; set; }
        public int? ServiceId { get; set; }
        public string ServiceName { get; set; }
        public int? SenderId { get; set; }
        public string SenderName { get; set; }
        public string SenderPhone { get; set; }
        public string PickingAddress { get; set; }
        public double Weight { get; set; }
        public double COD { get; set; }
        public double TotalPrice { get; set; }
        public double CODKeeping { get; set; }
        public double TotalPriceKeeping { get; set; }
        public string TPLNumber { get; set; }
        public string TPLCode { get; set; }
        public DateTime? ReceiptCODCreatedWhen { get; set; }
        public string TypePay { get; set; }
        public string ReceiptCODCreatedBy { get; set; }
        public string Code { get; set; }
        public DateTime? LockDate { get; set; }
        public double? FeeBank { get; set; }
        public DateTime? ModifiedWhen { get; set; }
        public double? GrandTotal { get; set; }
        public double? GrandTotalReal { get; set; }
        public double? TotalCOD { get; set; }

        public Proc_ShipmentHubKeeping()
        {
        }

        public static IEntityProc GetEntityProc(int hubId, int? otherHubId = null, int? listReceitMoneyTypeId = null)
        {
            SqlParameter parameter2 = new SqlParameter(
            "@OtherHubId", otherHubId);
            if (!otherHubId.HasValue)
                parameter2.Value = DBNull.Value;
            SqlParameter parameter3 = new SqlParameter(
            "@ListReceitMoneyTypeId", listReceitMoneyTypeId);
            if (!listReceitMoneyTypeId.HasValue)
                parameter3.Value = DBNull.Value;
            return new EntityProc(
                $"{ProcName} @HubId, @OtherHubId, @ListReceitMoneyTypeId",
                new SqlParameter[] {
                new SqlParameter("@HubId", hubId),
                parameter2,
                parameter3
                }
            );
        }
    }
}
