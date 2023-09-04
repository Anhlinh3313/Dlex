using System;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using Core.Entity.Abstract;

namespace Core.Entity.Procedures
{
    public class Proc_CheckByShipmentNumbers : IEntityProcView
    {
        public const string ProcName = "Proc_CheckByShipmentNumbers";
        [Key]
        public int Id { get; set; }
        public string ShipmentStatusName { get; set; }
        public int ShipmentStatusId { get; set; }
        public int TotalBox { get; set; }
        public int? RequestShipmentId { get; set; }
        public string ShipmentNumber { get; set; }

        public Proc_CheckByShipmentNumbers()
        {

        }

        public static IEntityProc GetEntityProc(string shipmentNumber)
        {
            SqlParameter ShipmentNumber = new SqlParameter("@ShipmentNumber", shipmentNumber);
            if (string.IsNullOrWhiteSpace(shipmentNumber)) ShipmentNumber.Value = DBNull.Value;

            return new EntityProc(
                $"{ProcName} @ShipmentNumber",
                new SqlParameter[] {
                    ShipmentNumber
                }
            );
        }
    }
}
