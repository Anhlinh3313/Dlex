using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_GetListPrice : IEntityProcView
    {
        public const string ProcName = "Proc_GetListPrice";
        [Key]
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string PriceListDVGTName { get; set; }
        public string HubName { get; set; }
        public double? FuelSurcharge { get; set; }
        public double? RemoteSurcharge { get; set; }
        public DateTime? PublicDateFrom { get; set; }
        public DateTime? PublicDateTo { get; set; }
        public int? NumOrder { get; set; }
        public bool? IsPublic { get; set; }
        public Int64? RowNum { get; set; }
        public int? TotalCount { get; set; }

        public Proc_GetListPrice() { }
        public static IEntityProc GetEntityProc(int? pageNumber = null, int? pageSize = null, string searchText = null, int? hubId = null,  int? companyId = null)
        {
          
            SqlParameter PageNumber = new SqlParameter("@PageNumber", pageNumber);
            if (!pageNumber.HasValue)PageNumber.Value = DBNull.Value;

            SqlParameter PageSize = new SqlParameter("@PageSize", pageSize);
            if (!pageSize.HasValue)PageSize.Value = DBNull.Value;

            SqlParameter SearchText = new SqlParameter("@SearchText", searchText);
            if (string.IsNullOrWhiteSpace(searchText)) SearchText.Value = DBNull.Value;

            SqlParameter HubId = new SqlParameter("@HubId", hubId);
            if (!hubId.HasValue) HubId.Value = DBNull.Value;

            SqlParameter CompanyId = new SqlParameter("@CompanyId", companyId);
            if (!companyId.HasValue) CompanyId.Value = DBNull.Value;

            return new EntityProc(
                $"{ProcName} @PageNumber, @PageSize, @SearchText, @HubId, @CompanyId",
                new SqlParameter[] {
                    PageNumber,
                    PageSize,
                    SearchText,
                    HubId,
                    CompanyId
                }
            );
        }
    }
}
