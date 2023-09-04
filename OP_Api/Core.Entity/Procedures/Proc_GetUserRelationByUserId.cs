using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_GetUserRelationByUserId : IEntityProcView
    {
        public const string ProcName = "Proc_GetUserRelationByUserId";
        [Key]
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }
        public int UserRelationId { get; set; }
        public int TotalCount { get; set; }

        public Proc_GetUserRelationByUserId() { }
        public static IEntityProc GetEntityProc(int? pageNumber = null, int? pageSize = null, int? userId = null)
        {
           
            SqlParameter PageNumber = new SqlParameter("@PageNumber", pageNumber);
            if (!pageNumber.HasValue)PageNumber.Value = DBNull.Value;

            SqlParameter PageSize = new SqlParameter("@PageSize", pageSize);
            if (!pageSize.HasValue)PageSize.Value = DBNull.Value;

            SqlParameter UserId = new SqlParameter("@UserId", userId);
            if (!userId.HasValue) UserId.Value = DBNull.Value;

            return new EntityProc(
                $"{ProcName} @UserId, @PageNumber, @PageSize",
                new SqlParameter[] {
                    UserId,
                    PageNumber,
                    PageSize
                }
            );
        }
    }
}
