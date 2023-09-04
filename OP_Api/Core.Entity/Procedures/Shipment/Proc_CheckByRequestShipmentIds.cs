using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures.Shipment
{
    public class Proc_CheckByRequestShipmentIds : IEntityProcView
    {
        public const string ProcName = "Proc_CheckByRequestShipmentIds";
        [Key]
        public int Id { get; set; }
        public string shipmentStatusName { get; set; }
        public int? ShipmentStatusId { get; set; }
        public int? TotalBox { get; set; }
        public int? RequestShipmentId { get; set; }
        public string ShipmentNumber { get; set; }
        public string SenderName { get; set; }
        public string Note { get; set; }
        public string AddressNoteTo { get; set; }
        public string AddressNoteFrom { get; set; }
        public string ShippingAddress { get; set; }
        public string ReceiverName { get; set; }
        public double? COD { get; set; }
        public double? Weight { get; set; }

        public Proc_CheckByRequestShipmentIds()
        {

        }

        public static IEntityProc GetEntityProc(string shipmentNumber, int? userId, string requestShipmentIds)
        {
            SqlParameter ShipmentNumber = new SqlParameter("@ShipmentNumber", shipmentNumber);
            if (string.IsNullOrWhiteSpace(shipmentNumber))
                ShipmentNumber.Value = DBNull.Value;

            SqlParameter UserId = new SqlParameter("@UserId", userId);
            if (!userId.HasValue)
                UserId.Value = DBNull.Value;

            SqlParameter RequestShipmentIds = new SqlParameter("@RequestShipmentIds", requestShipmentIds);
            if (string.IsNullOrWhiteSpace(requestShipmentIds))
                RequestShipmentIds.Value = DBNull.Value;

            return new EntityProc(
                $"{ProcName} @ShipmentNumber, @UserId, @RequestShipmentIds",
                new SqlParameter[] {
                    ShipmentNumber,UserId,RequestShipmentIds
                }
            );
        }
    }
}
