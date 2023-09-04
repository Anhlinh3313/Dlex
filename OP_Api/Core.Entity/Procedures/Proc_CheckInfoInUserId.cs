using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_CheckInfoInUserId : IEntityProcView
    {
        public const string ProcName = "Proc_CheckInfoInUserId";

        [Key]
        public int CountUser { get; set; }
        public Proc_CheckInfoInUserId() { }
        public static IEntityProc GetEntityProc(int userId)
        {
            SqlParameter sqlParameter1 = new SqlParameter("@UserId", userId);
            return new EntityProc(
                $"{ProcName} @UserId",
                new SqlParameter[] {
                    sqlParameter1
                }
            );
        }
    }
}
