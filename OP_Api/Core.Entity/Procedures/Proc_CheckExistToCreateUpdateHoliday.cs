using Core.Entity.Abstract;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace Core.Entity.Procedures
{
	public class Proc_CheckExistToCreateUpdateHoliday : IEntityProcView
	{
		public const string ProcName = "Proc_CheckExistToCreateUpdateHoliday";
		[Key]
		public bool Result { get; set; }
		public string Message { get; set; }


		public Proc_CheckExistToCreateUpdateHoliday() { }
		public static IEntityProc GetEntityProc(int? id, string code, DateTime? date, bool isSa, bool isSu)
		{
			SqlParameter sqlParameter1 = new SqlParameter("@Id", id);
			SqlParameter sqlParameter2 = new SqlParameter("@Code", code);
			if (string.IsNullOrWhiteSpace(code)) sqlParameter2.Value = DBNull.Value;
			SqlParameter sqlParameter3 = new SqlParameter("@Date", date);
			if (date == null) sqlParameter3.Value = DBNull.Value;
			SqlParameter sqlParameter4 = new SqlParameter("@IsSa", isSa);
			SqlParameter sqlParameter5 = new SqlParameter("@IsSu", isSu);

			return new EntityProc(
				$"{ProcName} @Id, @Code, @Date, @IsSa, @IsSu",
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

