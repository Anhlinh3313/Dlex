using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using Core.Entity.Abstract;

namespace Core.Entity.Procedures
{
    public class Proc_UpdatePassWordUser : IEntityProcView
    {
        public const string ProcName = "Proc_UpdatePassWordUser";

        [Key]
        public bool Result { get; set; }

        public Proc_UpdatePassWordUser()
        {
        }

        public static IEntityProc GetEntityProc(int? id = null, string passwordHash = null, string codeResetPassWord = null, DateTime? resetPassWordSentat = null,
            Boolean? isPassWordBasic = null)
        {
            SqlParameter sqlParameter1 = new SqlParameter("@Id", id);
            SqlParameter sqlParameter2 = new SqlParameter("@PasswordHash", passwordHash);
            if (string.IsNullOrWhiteSpace(passwordHash)) sqlParameter2.Value = DBNull.Value;
            SqlParameter sqlParameter3 = new SqlParameter("@CodeResetPassWord", codeResetPassWord);
            if (string.IsNullOrWhiteSpace(codeResetPassWord)) sqlParameter3.Value = DBNull.Value;
            SqlParameter sqlParameter4 = new SqlParameter("@ResetPassWordSentat", resetPassWordSentat);
            SqlParameter sqlParameter5 = new SqlParameter("@IsPassWordBasic", isPassWordBasic);

            return new EntityProc(
                $"{ProcName} @Id, @PasswordHash, @CodeResetPassWord, @ResetPassWordSentat, @IsPassWordBasic",
                new SqlParameter[] {
                    sqlParameter1,
                    sqlParameter2,
                    sqlParameter3,
                    sqlParameter4,
                    sqlParameter5,
                }
            );
        }
    }
}
