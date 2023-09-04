using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;


namespace Core.Entity.Procedures
{
    public class Proc_GetTotalShipmentBox : IEntityProcView
    {
        public const string ProcName = "Proc_GetTotalShipmentBox";
        [Key]
        public int TotalBox { get; set; }
        public double TotalWeight { get; set; }
        public double TotalCalWeight { get; set; }
        public double TotalCusWeight { get; set; }
        public static IEntityProc GetEntityProc(int shipmentId)
        {
            SqlParameter sqlParameter1 = new SqlParameter("@ShipmentIdRef", shipmentId);
            return new EntityProc(
                $"{ProcName} @ShipmentIdRef",
                new SqlParameter[] {
                    sqlParameter1
                }
            );
        }
    }
}
