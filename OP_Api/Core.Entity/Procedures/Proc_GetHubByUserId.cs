using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_GetHubByUserId : IEntityProcView
    {
        public const string ProcName = "Proc_GetHubByUserId";
        [Key]
        public int Id { get; set; }
        public string HubName { get; set; }

        public Proc_GetHubByUserId() { }
        public static IEntityProc GetEntityProc(int? userId)
        {
            SqlParameter UserId = new SqlParameter("@UserId", userId);
            if (!userId.HasValue) UserId.Value = DBNull.Value;


            return new EntityProc(
                $"{ProcName} @UserId",
                new SqlParameter[] {
                    UserId
                }
            );
        }
    }
}
