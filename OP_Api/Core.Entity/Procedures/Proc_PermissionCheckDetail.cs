using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_PermissionCheckDetail : IEntityProcView
    {
        public const string ProcName = "Proc_PermissionCheckDetail";
        [Key]
        public bool Access { get; set; }
        public bool Add { get; set; }
        public bool Edit { get; set; }
        public bool Delete { get; set; }


        public Proc_PermissionCheckDetail() { }
        public static IEntityProc GetEntityProc(int userId, string aliasPath, int moduleId)
        {
            return new EntityProc(
               $"{ProcName} @UserId, @PageAlias, @ModuleId",
               new SqlParameter[] {
                    new SqlParameter("@UserId", userId),
                    new SqlParameter("@PageAlias", aliasPath ?? aliasPath),
                    new SqlParameter("@ModuleId", moduleId)
               }
           );
        }
    }
}
