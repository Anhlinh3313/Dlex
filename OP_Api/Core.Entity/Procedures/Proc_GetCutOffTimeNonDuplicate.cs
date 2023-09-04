using Core.Entity.Abstract;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace Core.Entity.Procedures
{
	public class Proc_GetCutOffTimeNonDuplicate : IEntityProcView
	{
		public const string ProcName = "Proc_GetCutOffTimeNonDuplicate";

		[Key]
		public int? Id { get; set; }
		public string Code { get; set; }
		public string Name { get; set; }
		public DateTime? CutTime1st { get; set; }
		public DateTime? CutTime2nd { get; set; }
		public DateTime? CutTime3rd { get; set; }
		public string DaysOfWeek { get; set; }

		public Proc_GetCutOffTimeNonDuplicate() { }


		public static IEntityProc GetEntityProc(int? cutOffId = null)
		{
			SqlParameter CutOffId = new SqlParameter("@CutOffTimeId", cutOffId);
			if (!cutOffId.HasValue) CutOffId.Value = DBNull.Value;
			return new EntityProc(
				$"{ProcName} @CutOffTimeId",
				new SqlParameter[] {
					CutOffId
				}
			);
		}
	}
}

