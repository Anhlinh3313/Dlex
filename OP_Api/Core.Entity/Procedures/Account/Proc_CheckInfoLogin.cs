using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;


namespace Core.Entity.Procedures
{
    public class Proc_CheckInfoLogin : IEntityProcView
    {
        public const string ProcName = "Proc_CheckInfoLogin";
        [Key]
        public int Id { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public bool? IsBlocked { get; set; }
        public bool? IsPassWordBasic { get; set; }
        public int CompanyId { get; set; }
        public string CompanyCode { get; set; }
        public int TypeUserId { get; set; }

        public Proc_CheckInfoLogin() { }
        public static IEntityProc GetEntityProc(string userName, string companyCode, int typeUserId)
        {
            SqlParameter UserName = new SqlParameter("@UserName", userName);
            if (string.IsNullOrWhiteSpace(userName)) UserName.Value = DBNull.Value;

            SqlParameter CompanyCode = new SqlParameter("@CompanyCode", companyCode);
            if (string.IsNullOrWhiteSpace(companyCode)) CompanyCode.Value = DBNull.Value;

            SqlParameter TypeUserId = new SqlParameter("@TypeUserId", typeUserId);

            return new EntityProc(
                $"{ProcName} @UserName,@CompanyCode,@TypeUserId",
                new SqlParameter[] {
                    UserName,
                    CompanyCode,
                    TypeUserId
                }
            );
        }
    }
}
