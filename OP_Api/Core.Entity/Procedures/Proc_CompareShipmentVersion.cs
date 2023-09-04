using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_CompareShipmentVersion : IEntityProcView
    {
        public const string ProcName = "Proc_CompareShipmentVersion";

        [Key]
        public int Id { get; set; }
        public int ShipmentId { get; set; }
        public string ReShipmentNumber { get; set; }
        public string ShopCode { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverPhone { get; set; }
        public string CompanyTo { get; set; }
        public string ShippingAddress { get; set; }
        public string AddressNoteTo { get; set; }
        public DateTime CreatedWhen { get; set; }
        public string CreatedByName { get; set; }
        public string Version { get; set; }
        public string ToProvinceName { get; set; }
        public string ToDistrictName { get; set; }
        public string ToWardName { get; set; }
        public string ToHubName { get; set; }
        public string CurrentHubName { get; set; }
        public double? Weight { get; set; }
        public double? CalWeight { get; set; }
        public int TotalBox { get; set; }
        public double Length { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public string SenderName { get; set; }
        public string SenderPhone { get; set; }
        public int SenderId { get; set; }
        public string CompanyFrom { get; set; }
        public string PickingAddress { get; set; }
        public string AddressNoteFrom { get; set; }
        public string FromProvinceName { get; set; }
        public string FromDistrictName { get; set; }
        public string FromWardName { get; set; }
        public string FromHubName { get; set; }
        public string ServiceName { get; set; }
        public string StructureName { get; set; }
        public string PaymentTypeName { get; set; }
        public string CurrentUserName { get; set; }
        public string ShipmentStatusName { get; set; }
        public double FuelPrice { get; set; }
        public double TotalDVGT { get; set; }
        public double OtherPrice { get; set; }
        public double VATPrice { get; set; }
        public double DefaultPrice { get; set; }
        public double TotalPrice { get; set; }
        public double RemoteAreasPrice { get; set; }
        public double Insured { get; set; }
        public double COD { get; set; }
        public string Note { get; set; }

        public Proc_CompareShipmentVersion()
        {
        }

        public static IEntityProc GetEntityProc(
            int? shipmentId, 
            int? shipmentVersionId)
        {
            SqlParameter ShipmentId = new SqlParameter("@ShipmentId", shipmentId);
            if (!shipmentId.HasValue)
                ShipmentId.Value = DBNull.Value;

            SqlParameter ShipmentVersionId = new SqlParameter("@ShipmentVersionId", shipmentVersionId);
            if (!shipmentVersionId.HasValue)
                ShipmentVersionId.Value = DBNull.Value;

            return new EntityProc(
                $"{ProcName} @ShipmentId,@ShipmentVersionId",
                new SqlParameter[] {
                    ShipmentId,
                    ShipmentVersionId,
                }
            );
        }
    }
}
