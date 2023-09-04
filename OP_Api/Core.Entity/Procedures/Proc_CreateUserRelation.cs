using Core.Entity.Abstract;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace Core.Entity.Procedures
{
	public class Proc_CreateUserRelation : IEntityProcView
	{
		public const string ProcName = "Proc_CreateUserRelation";
		[Key]
		public bool Result { get; set; }


		public Proc_CreateUserRelation() { }
		public static IEntityProc GetEntityProc(string code, string name, int? userId, int? userRelationId, int? companyId)
		{
			SqlParameter sqlParameter1 = new SqlParameter("@Code", code);
			if (string.IsNullOrWhiteSpace(code)) sqlParameter1.Value = DBNull.Value;
			SqlParameter sqlParameter2 = new SqlParameter("@Name", name);
			if (string.IsNullOrWhiteSpace(name)) sqlParameter2.Value = DBNull.Value;
			SqlParameter sqlParameter3 = new SqlParameter("@UserId", userId);
			SqlParameter sqlParameter4 = new SqlParameter("@UserRelationId", userRelationId);
			SqlParameter sqlParameter5 = new SqlParameter("@CompanyId", companyId);

			return new EntityProc(
				$"{ProcName} @Code, @Name, @UserId, @UserRelationId, @CompanyId",
				new SqlParameter[] {
					sqlParameter1,
					sqlParameter2,
					sqlParameter3,
					sqlParameter4,
					sqlParameter5
				}
			);
		}
	}
}
