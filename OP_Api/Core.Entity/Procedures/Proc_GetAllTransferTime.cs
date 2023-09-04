using Core.Entity.Abstract;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace Core.Entity.Procedures
{
	public class Proc_GetAllTransferTime : IEntityProcView
	{
		public const string ProcName = "Proc_GetAllTransferTime";

		[Key]
		public int? Id { get; set; }
		public string Code { get; set; }
		public string Name { get; set; }
		public int? CutOffId { get; set; }
        public double? ExportTime1st { get; set; }
        public double? ExportTime2nd { get; set; }
        public double? ExportTime3rd { get; set; }
        public DateTime? StartTime1st { get; set; }
		public DateTime? StartTime2nd { get; set; }
		public DateTime? StartTime3rd { get; set; }

		public Proc_GetAllTransferTime() { }


		public static IEntityProc GetEntityProc()
		{
			return new EntityProc(
				$"{ProcName}",
				new SqlParameter[] {
				}
			);
		}
	}
}

