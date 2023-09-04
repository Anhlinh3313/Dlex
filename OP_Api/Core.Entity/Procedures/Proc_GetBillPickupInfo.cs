using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_GetBillPickupInfo : IEntityProcView
    {
        public const string ProcName = "Proc_GetBillPickupInfo";
        [Key]
        public int Id { get; set; }
        public string ShipmentNumber { get; set; }
        public string PickupUserCode { get; set; }
        public string ImagePath { get; set; }
        public DateTime? EndPickTime { get; set; }
        public DateTime? CreatedWhen { get; set; }

        public Proc_GetBillPickupInfo() { }
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
