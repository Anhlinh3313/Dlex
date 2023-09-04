using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_SaveLogReceiptMoneyDetail : IEntityProcView
    {
        public const string ProcName = "Proc_SaveLogReceiptMoneyDetail";
        [Key]
        public bool IsSuccess { get; set; }

        public Proc_SaveLogReceiptMoneyDetail() { }
        public static IEntityProc GetEntityProc(int id, int logId)
        {
            SqlParameter sqlParameter1 = new SqlParameter("@Id", id);
            SqlParameter LogId = new SqlParameter("@LogId", logId);

            return new EntityProc(
                $"{ProcName} @Id, @LogId",
                new SqlParameter[] {
                    sqlParameter1, LogId
                }
            );
        }
    }
}
