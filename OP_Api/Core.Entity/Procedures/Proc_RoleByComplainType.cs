using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_RoleByComplainType : IEntityProcView
    {
        public const string ProcName = "Proc_RoleByComplainType";

        [Key]
        public bool IsAcceptReturn { get; set; }

        public Proc_RoleByComplainType() { }

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
