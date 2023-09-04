using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
	public class Proc_GetHolidayByYear : IEntityProcView
	{
		public const string ProcName = "Proc_GetHolidayByYear";
		[Key]
		public int Id { get; set; }
		public string Code { get; set; }
		public string Name { get; set; }
		public DateTime? Date { get; set; }
		public DateTime CreatedWhen { get; set; }
		public string NotHoliday { get; set; }
		public Int64? RowNum { get; set; }
		public int? TotalCount { get; set; }


		public Proc_GetHolidayByYear() { }
		public static IEntityProc GetEntityProc(int? year, int? pageNumber = null, int? pageSize = null, int? companyId = null)
		{
			SqlParameter Year = new SqlParameter("@Year", year);
			if (year == null) { Year.Value = DBNull.Value;}

			SqlParameter PageNumber = new SqlParameter("@PageNumber", pageNumber);
			if (!pageNumber.HasValue) PageNumber.Value = DBNull.Value;

			SqlParameter PageSize = new SqlParameter("@PageSize", pageSize);
			if (!pageSize.HasValue) PageSize.Value = DBNull.Value;

			SqlParameter CompanyId = new SqlParameter("@CompanyId", companyId);
			if (!companyId.HasValue) CompanyId.Value = DBNull.Value;

			return new EntityProc(
				$"{ProcName} @Year, @PageNumber, @PageSize, @CompanyId",
				new SqlParameter[] {
					Year,
					PageNumber,
					PageSize,
					CompanyId
				}
			);
		}
	}
}
