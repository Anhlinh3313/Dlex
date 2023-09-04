using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_GetRoleAndHubByUser : IEntityProcView
    {
        public const string ProcName = "Proc_GetRoleAndHubByUser";

        [Key]
        public int Id { get; set; }
        public int? HubId { get; set; }
        public int? ComplainTypeId { get; set; }

        public Proc_GetRoleAndHubByUser() { }

        public static IEntityProc GetEntityProc(int id)
        {
            SqlParameter UserId = new SqlParameter("@UserId", id);

            return new EntityProc(
                $"{ProcName} @UserId",
                new SqlParameter[] {
                    UserId
                }
            );
        }
    }
}
