﻿using Core.Entity.Abstract;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace Core.Entity.Procedures
{
	public class Proc_UpdateHolidayById : IEntityProcView
	{
		public const string ProcName = "Proc_UpdateHolidayById";
		[Key]
		public bool Result { get; set; }


		public Proc_UpdateHolidayById() { }
		public static IEntityProc GetEntityProc(int id, string code, string name, DateTime? date, bool isSa, bool isSu, bool isFull)
		{
			SqlParameter sqlParameter1 = new SqlParameter("@Id", id);
			SqlParameter sqlParameter2 = new SqlParameter("@Code", code);
			if (string.IsNullOrWhiteSpace(code)) sqlParameter2.Value = DBNull.Value;
			SqlParameter sqlParameter3 = new SqlParameter("@Name", name);
			if (string.IsNullOrWhiteSpace(name)) sqlParameter3.Value = DBNull.Value;
			SqlParameter sqlParameter4 = new SqlParameter("@Date", date);
			if (date == null) sqlParameter4.Value = DBNull.Value;
			SqlParameter sqlParameter5 = new SqlParameter("@IsSa", isSa);
			SqlParameter sqlParameter6 = new SqlParameter("@IsSu", isSu);
			SqlParameter sqlParameter7 = new SqlParameter("@IsFull", isFull);

			return new EntityProc(
				$"{ProcName} @Id, @Code, @Name, @Date, @IsSa, @IsSu, @IsFull",
				new SqlParameter[] {
					sqlParameter1,
					sqlParameter2,
					sqlParameter3,
					sqlParameter4,
					sqlParameter5,
					sqlParameter6,
					sqlParameter7
				}
			);
		}
	}
}

