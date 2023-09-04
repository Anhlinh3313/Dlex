using Core.Entity.Abstract;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace Core.Entity.Procedures
{
    public class Proc_UpdateWard : IEntityProcView
    {
        public const string ProcName = "Proc_UpdateWard";
        [Key]
        public Boolean IsSuccess { get; set; }
        public string Message { get; set; }

        public Proc_UpdateWard() { }
        public static IEntityProc GetEntityProc(int wardId)
        {
            SqlParameter WardId = new SqlParameter("@WardId", wardId);
            return new EntityProc(
                $"{ProcName} @WardId",
                new SqlParameter[] {
                    WardId
                }
            );
        }
    }
}
