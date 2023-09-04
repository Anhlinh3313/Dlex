using Core.Entity.Abstract;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace Core.Entity.Procedures
{
	public class Proc_DeleteHolidayById : IEntityProcView
	{
		public const string ProcName = "Proc_DeleteHolidayById";
		[Key]
		public bool Result { get; set; }


		public Proc_DeleteHolidayById() { }
		public static IEntityProc GetEntityProc(int id)
		{
			SqlParameter sqlParameter1 = new SqlParameter("@Id", id);

			return new EntityProc(
				$"{ProcName} @Id",
				new SqlParameter[] {
					sqlParameter1
				}
			);
		}
	}
}

