using Core.Entity.Abstract;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace Core.Entity.Procedures
{
	public class Proc_CheckExistToCreateUpdateCutOffTime : IEntityProcView
	{
		public const string ProcName = "Proc_CheckExistToCreateUpdateCutOffTime";
		[Key]
		public bool Result { get; set; }
		public string Message { get; set; }


		public Proc_CheckExistToCreateUpdateCutOffTime() { }
		public static IEntityProc GetEntityProc(string code, string listDaysOfWeek, int? id = null)
		{
			SqlParameter sqlParameter1 = new SqlParameter("@Code", code);
			SqlParameter sqlParameter2 = new SqlParameter("@ListDaysOfWeek", listDaysOfWeek);
			SqlParameter sqlParameter3 = new SqlParameter("@Id", id);
			if (!id.HasValue) sqlParameter3.Value = DBNull.Value;

			return new EntityProc(
				$"{ProcName} @Code, @ListDaysOfWeek, @Id",
				new SqlParameter[] {
					sqlParameter1,
					sqlParameter2,
					sqlParameter3
				}
			);
		}
	}
}


