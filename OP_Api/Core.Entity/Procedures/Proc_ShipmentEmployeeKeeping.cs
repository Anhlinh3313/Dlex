using System;
using System.Data.SqlClient;
using Core.Entity.Abstract;

namespace Core.Entity.Procedures
{
    public class Proc_ShipmentEmployeeKeeping : IEntityProcView
    {
        public const string ProcName = "Proc_ShipmentEmployeeKeeping";

        public int Id { get; set; }
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
        public int? KeepingCODEmpId { get; set; }
        public string KeepingCODEmpFullName { get; set; }
        public string KeepingCODEmpUserName { get; set; }
        public int? KeepingTotalPriceEmpId { get; set; }
        public string KeepingTotalPriceEmpFullName { get; set; }
        public string KeepingTotalPriceEmpUserName { get; set; }
        public string ReceiverName { get; set; }
        public string ShippingAddress { get; set; }
        public DateTime? EndDeliveryTime { get; set; }
        public Proc_ShipmentEmployeeKeeping()
        {

        }

        public static IEntityProc GetEntityProc(int hubId, int? empId = null)
        {
            SqlParameter parameter2= new SqlParameter(
            "@EmpId", empId);
            if (!empId.HasValue)
                parameter2.Value = DBNull.Value;
            return new EntityProc(
                $"{ProcName} @HubId, @EmpId",
                new SqlParameter[] {
                new SqlParameter("@HubId", hubId),
                parameter2
                }
            );
        }
    }
}
