using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_GetBillUpdateInfo : IEntityProcView
    {
        public const string ProcName = "Proc_GetBillUpdateInfo";
        [Key]
        public int Id { get; set; }
        public string ShipmentNumber { get; set; }

        public Proc_GetBillUpdateInfo() { }
        public static IEntityProc GetEntityProc()
        {

            return new EntityProc(
                $"{ProcName} ",
                new SqlParameter[] {
                }
            );
        }
    }
}
