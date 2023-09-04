using Core.Entity.Abstract;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace Core.Entity.Procedures
{
    public class Proc_UpdatePoHub : IEntityProcView
    {
        public const string ProcName = "Proc_UpdatePoHub";
        [Key]
        public Boolean IsSuccess { get; set; }
        public string Message { get; set; }

        public Proc_UpdatePoHub() { }
        public static IEntityProc GetEntityProc(int poHubId)
        {
            SqlParameter PoHubId = new SqlParameter("@PoHubId", poHubId);
            return new EntityProc(
                $"{ProcName} @PoHubId",
                new SqlParameter[] {
                    PoHubId
                }
            );
        }
    }
}
