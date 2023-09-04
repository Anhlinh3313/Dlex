using Core.Entity.Abstract;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace Core.Entity.Procedures
{
    public class Proc_CheckUserLogin : IEntityProcView
    {
        public const string ProcName = "Proc_CheckUserLogin";

        [Key]
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int? TypeUserId { get; set; }
        public int? CompanyId { get; set; }
        public string SecurityStamp { get; set; }
        public string PasswordHash { get; set; }
        public string CodeResetPassWord { get; set; }
        public DateTime? ResetPassWordSentat { get; set; }
        public Boolean? IsPassWordBasic { get; set; }


        public Proc_CheckUserLogin() { }
        public static IEntityProc GetEntityProc(string Email)
        {
            SqlParameter sqlParameter1 = new SqlParameter("@Email", Email);
            return new EntityProc(
                $"{ProcName} @Email",
                new SqlParameter[] {
                    sqlParameter1
                }
            );
        }
    }
}
