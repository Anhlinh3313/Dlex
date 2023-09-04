using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_GetUserByUserRelationId : IEntityProcView
    {
        public const string ProcName = "Proc_GetUserByUserRelationId";
        [Key]
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public Proc_GetUserByUserRelationId() { }
        public static IEntityProc GetEntityProc(int? userId = null)
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
