using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_SearchEntityByValue : IEntityProcView
    {
        public const string ProcName = "Proc_SearchEntityByValue";
        [Key]
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public Proc_SearchEntityByValue() { }
        public static IEntityProc GetEntityProc(string tableName, string value, int? id)
        {
            SqlParameter sqlParameter1 = new SqlParameter("@Value", value);
            if (string.IsNullOrWhiteSpace(value)) sqlParameter1.Value = DBNull.Value;

            SqlParameter sqlParameter2 = new SqlParameter("@Id", id);
            if (id == null) sqlParameter2.Value = DBNull.Value;

            return new EntityProc(
                $"{ProcName} @TableName, @Value, @Id",
                new SqlParameter[] {
                    new SqlParameter("@TableName", tableName),
                    sqlParameter1,
                    sqlParameter2
                }
            );
        }
    }
}
