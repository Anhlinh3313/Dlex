using Core.Entity.Abstract;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace Core.Entity.Procedures
{
    public class Proc_UpdateCenterHub : IEntityProcView
    {
        public const string ProcName = "Proc_UpdateCenterHub";
        [Key]
        public Boolean IsSuccess { get; set; }
        public string Message { get; set; }

        public Proc_UpdateCenterHub() { }
        public static IEntityProc GetEntityProc(int centerHubId)
        {
            SqlParameter CenterHubId = new SqlParameter("@CenterHubId", centerHubId);
            return new EntityProc(
                $"{ProcName} @CenterHubId",
                new SqlParameter[] {
                    CenterHubId
                }
            );
        }
    }
}
