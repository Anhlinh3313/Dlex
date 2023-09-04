using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;


namespace Core.Entity.Procedures
{
    public class Proc_CheckShipmentAcceptReturn : IEntityProcView
    {
        public const string ProcName = "Proc_CheckShipmentAcceptReturn";

        [Key]
        public bool IsWaitingReturn { get; set; }

        public Proc_CheckShipmentAcceptReturn() { }

        public static IEntityProc GetEntityProc(int userId)
        {
            SqlParameter UserId = new SqlParameter("@UserId", userId);

            return new EntityProc(
                $"{ProcName} @UserId",
                new SqlParameter[] {
                    UserId
                }
            );
        }
    }
}
