using Core.Entity.Abstract;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace Core.Entity.Procedures
{
    public class Proc_GetAllPermissionByRoleId : IEntityProcView
    {
        public const string ProcName = "Proc_GetAllPermissionByRoleId";
        [Key]
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int PageId { get; set; }
        public bool IsAccess { get; set; }
        public bool IsAdd { get; set; }
        public bool IsEdit { get; set; }
        public bool IsDelete { get; set; }
        public int? CompanyId { get; set; }
        public int CheckRolePageId { get; set; }

        public Proc_GetAllPermissionByRoleId() { }
        public static IEntityProc GetEntityProc(int roleId)
        {
            SqlParameter RoleId = new SqlParameter("@RoleId", roleId);
            return new EntityProc(
                $"{ProcName} @RoleId",
                new SqlParameter[] {
                    RoleId
                }
            );
        }
    }
}
