using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_GetBoxNumberAuto : IEntityProcView
    {
        public const string ProcName = "Proc_GetBoxNumberAuto";

        [Key]
        public string BoxNumer { get; set; }
        public static IEntityProc GetEntityProc(int shipmentId, int? shipmentIdRef)
        {
            SqlParameter sqlParameter1 = new SqlParameter("@ShipmentId", shipmentId);
            SqlParameter sqlParameter2 = new SqlParameter("@ShipmentIdRef", shipmentIdRef);
            return new EntityProc(
                $"{ProcName} @ShipmentId,@ShipmentIdRef",
                new SqlParameter[] {
                    sqlParameter1,sqlParameter2
                }
            );
        }
    }
}
