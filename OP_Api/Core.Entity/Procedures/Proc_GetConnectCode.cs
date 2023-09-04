using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_GetConnectCode : IEntityProcView
    {
        public const string ProcName = "Proc_SearchConnectCode";
        [Key]
        public int Id { get; set; }
        public string CodeConnect { get; set; }
        public Proc_GetConnectCode() { }
        public static IEntityProc GetEntityProc(string value)
        {
            SqlParameter sqlParameter1 = new SqlParameter("@Value", value);
            if (string.IsNullOrWhiteSpace(value)) sqlParameter1.Value = DBNull.Value;

            return new EntityProc(
                $"{ProcName} @Value",
                new SqlParameter[] {
                    sqlParameter1
                }
            );
        }
    }
}
