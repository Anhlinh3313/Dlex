using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_GetLadingScheduleCurrent : IEntityProcView
    {
        public const string ProcName = "Proc_GetLadingScheduleCurrent";
        [Key]
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public int? ShipmentId { get; set; }
        public int? LadingScheduleId { get; set; }

        public Proc_GetLadingScheduleCurrent() { }
        public static IEntityProc GetEntityProc(string shipmentNumber)
        {
            SqlParameter sqlParameter1 = new SqlParameter("@ShipmentNumber", shipmentNumber);

            return new EntityProc(
                $"{ProcName} @ShipmentNumber",
                new SqlParameter[] {
                    sqlParameter1
                }
            );
        }
    }
}
