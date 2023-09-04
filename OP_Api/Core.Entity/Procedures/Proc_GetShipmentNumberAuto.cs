using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_GetShipmentNumberAuto : IEntityProcView
    {

        public const string ProcName = "Proc_GetShipmentNumberAuto";

        [Key]
        public int CountNumber { get; set; }
        public static IEntityProc GetEntityProc(int shipmentId, int? provinceId = 0)
        {
            SqlParameter sqlParameter1 = new SqlParameter("@ProvinceId", provinceId);
            SqlParameter sqlParameter2 = new SqlParameter("@ShipmentId", shipmentId);
            return new EntityProc(
                $"{ProcName} @ProvinceId,@ShipmentId",
                new SqlParameter[] {
                    sqlParameter1,sqlParameter2
                }
            );
        }
    }
}
