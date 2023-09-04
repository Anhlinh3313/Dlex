using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_Core_DeleteTable : IEntityProcView
    {
        public const string ProcName = "Proc_Core_DeleteTable";

        [Key]
        public bool IsAllowDelete { get; set; }

        public Proc_Core_DeleteTable()
        {
        }

        public static IEntityProc GetEntityProc(string tableName, int id, int? userId)
        {
            return new EntityProc(
                $"{ProcName} @TableName, @Id, @UserId",
                new SqlParameter[] {
                    new SqlParameter("@TableName", tableName),
                    new SqlParameter("@Id", id),
                    new SqlParameter("@UserId", userId)
                }
            );
        }
    }
}
