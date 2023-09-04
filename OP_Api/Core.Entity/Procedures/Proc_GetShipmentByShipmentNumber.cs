using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_GetShipmentByShipmentNumber : IEntityProcView
    {
        public const string ProcName = "Proc_GetShipmentByShipmentNumber";

        public int Id { get; set; }

        public string ShipmentNumber { get; set; }
        public int ShipmentStatusId { get; set; }
        public int? CurrentHubId { get; set; }
        public int? CurrentEmpId { get; set; }
        public double Weight { get; set; }
        public double CalWeight { get; set; }
        public string Content { get; set; }
        public string Note { get; set; }
        public bool IsReturn { get; set; }
        public string SenderName { get; set; }
        public string SenderPhone { get; set; }
        public string PickingAddress { get; set; }
        public string AddressNoteFrom { get; set; }
        public int? FromWardId { get; set; }
        public int? FromDistrictId { get; set; }
        public int? FromProvinceId { get; set; }
        public int? FromHubId { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverPhone { get; set; }
        public string ShippingAddress { get; set; }
        public string AddressNoteTo { get; set; }
        public int? ToWardId { get; set; }
        public int? ToDistrictId { get; set; }
        public int? ToProvinceId { get; set; }
        public int? ToHubId { get; set; }
        public bool IsEnabled { get; set; }
        public string ShopCode { get; set; }
        public string TPLNumber { get; set; }
        public int? ToHubRoutingId { get; set; }
        public int? PackageId { get; set; }
        public DateTime? ARReturnDate { get; set; }
        public double? PaymentTargetCOD { get; set; }

        public Proc_GetShipmentByShipmentNumber() { }

        public static IEntityProc GetEntityProc(string shipmentNumber)
        {
            SqlParameter parameter1 = new SqlParameter(
            "@ShipmentNumber", shipmentNumber + "");
            return new EntityProc(
                $"{ProcName} @ShipmentNumber",
                new SqlParameter[] {
                parameter1
                }
            );
        }
    }
}
