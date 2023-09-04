using Core.Entity.Abstract;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace Core.Entity.Procedures
{
	public class Proc_CheckExistCodeToCreateUpdateTransferTime : IEntityProcView
	{
		public const string ProcName = "Proc_CheckExistCodeToCreateUpdateTransferTime";
		[Key]
		public bool Result { get; set; }
		public string Message { get; set; }


		public Proc_CheckExistCodeToCreateUpdateTransferTime() { }
		public static IEntityProc GetEntityProc(string code, int? id = null)
		{
			SqlParameter sqlParameter1 = new SqlParameter("@Code", code);
			SqlParameter sqlParameter2 = new SqlParameter("@Id", id);
			if (!id.HasValue) sqlParameter2.Value = DBNull.Value;

			return new EntityProc(
				$"{ProcName} @Code, @Id",
				new SqlParameter[] {
					sqlParameter1,
					sqlParameter2
				}
			);
		}
	}
}

