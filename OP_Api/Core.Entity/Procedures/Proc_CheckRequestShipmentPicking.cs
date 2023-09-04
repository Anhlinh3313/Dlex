using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_CheckRequestShipmentPicking : IEntityProcView
    {
        public const string ProcName = "Proc_CheckRequestShipmentPicking";
        [Key]
        public bool IsSuccess { get; set; }
        public string Message { get; set; }

        public Proc_CheckRequestShipmentPicking() { }

        public static IEntityProc GetEntityProc(int id)
        {
            SqlParameter Id = new SqlParameter("@Id", id);
            return new EntityProc(
                $"{ProcName} @Id",
                new SqlParameter[] {
                    Id
                }
            );
        }
    }
}
