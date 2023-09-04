using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_UpdateShipmentNumberAuto : IEntityProcView
    {
        public const string ProcName = "Proc_UpdateShipmentNumberAuto";
        [Key]
        public bool Result { get; set; }

        public Proc_UpdateShipmentNumberAuto() { }

        public static IEntityProc GetEntityProc(int shipmentId,string shipmentNumber)
        {
            SqlParameter parameter1 = new SqlParameter("@ShipmentId", shipmentId);
            //
            SqlParameter parameter2 = new SqlParameter("@ShipmentNumber", shipmentNumber);
            return new EntityProc(
                $"{ProcName} @ShipmentId, @ShipmentNumber",
                new SqlParameter[] {
                    parameter1,
                    parameter2,
                }
            );
        }
    }
}
