using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_UpdateFastBooking : IEntityProcView
    {
        public const string ProcName = "Proc_UpdateFastBooking";
        [Key]
        public Boolean IsSuccess { get; set; }
        public string Message { get; set; }
        public string ShipmentNumber { get; set; }

        public Proc_UpdateFastBooking() { }
        public static IEntityProc GetEntityProc(int shipmentId)
        {
            SqlParameter ShipmentId = new SqlParameter("@ShipmentId", shipmentId);
            return new EntityProc(
                $"{ProcName} @ShipmentId",
                new SqlParameter[] {
                    ShipmentId
                }
            );
        }
    }
}
