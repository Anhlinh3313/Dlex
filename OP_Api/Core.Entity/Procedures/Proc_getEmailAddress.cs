using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_GetEmailAddress : IEntityProcView
    {
        public const string ProcName = "Proc_GetEmailAddress";
        [Key]
        public int ShipmentId { get; set; }
        public string EmailAddress { get; set; }
        public string ShipmentNumber { get; set; }
        public string ObjectName { get; set; }
        public bool IsSend { get; set; }

        public Proc_GetEmailAddress() { }
        public static IEntityProc GetEntityProc(string listShipmentId)
        {
            SqlParameter sqlParameter1 = new SqlParameter("@ListShipmentId", listShipmentId);
            return new EntityProc(
                $"{ProcName} @ListShipmentId",
                new SqlParameter[] {
                    sqlParameter1
                }
            );
        }
    }
}
