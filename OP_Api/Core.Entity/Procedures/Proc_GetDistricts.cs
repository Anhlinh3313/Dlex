using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_GetDistricts : IEntityProcView
    {
        public const string ProcName = "Proc_GetDistricts";
        [Key]
        public int Id { get; set; }
        public DateTime? CreatedWhen { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedWhen { get; set; }
        public int? ModifiedBy { get; set; }
        public bool IsEnabled { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int? ProvinceId { get; set; }
        public string ProvinceName { get; set; }
        public bool? IsRemote { get; set; }
        public int? KmNumber { get; set; }
        public int? CompanyId { get; set; }
        public string CompanyName { get; set; }
        public Int32? TotalCount { get; set; }

        public Proc_GetDistricts() { }
        public static IEntityProc GetEntityProc(int? provinceId = null, int? pageNumber = null, int? pageSize = null, string searchText = null, bool? isRemote = null, int? companyId = null)
        {
            SqlParameter ProvinceId = new SqlParameter("@ProvinceId", provinceId);
            if (!provinceId.HasValue)
                ProvinceId.Value = DBNull.Value;

            SqlParameter PageNumber = new SqlParameter("@PageNumber", pageNumber);
            if (!pageNumber.HasValue)
                PageNumber.Value = DBNull.Value;

            SqlParameter PageSize = new SqlParameter("@PageSize", pageSize);
            if (!pageSize.HasValue)
                PageSize.Value = DBNull.Value;

            SqlParameter SearchText = new SqlParameter("@SearchText", searchText);
            if (string.IsNullOrWhiteSpace(searchText))
                SearchText.Value = DBNull.Value;

            SqlParameter IsRemote = new SqlParameter("@IsRemote", isRemote);
            if (!isRemote.HasValue)
                IsRemote.Value = DBNull.Value;

            SqlParameter CompanyId = new SqlParameter("@CompanyId", companyId);
            if (!companyId.HasValue)
                CompanyId.Value = DBNull.Value;

            return new EntityProc(
                $"{ProcName} @ProvinceId, @PageNumber, @PageSize, @SearchText, @IsRemote, @CompanyId",
                new SqlParameter[] {
                    ProvinceId, PageNumber, PageSize, SearchText, IsRemote, CompanyId
                }
            );
        }
    }
}
