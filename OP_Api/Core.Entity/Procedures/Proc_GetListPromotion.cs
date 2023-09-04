using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_GetListPromotion : IEntityProcView
    {
        public const string ProcName = "Proc_GetListPromotion";
        [Key]
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int? PromotionTypeId { get; set; }
        public string PromotionTypeName { get; set; }
        public string PromotionNot { get; set; }
        public double? TotalPromotion { get; set; }
        public double? TotalCode { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public bool? IsPublic { get; set; }
        public bool? IsHidden { get; set; }
        public string ConcurrencyStamp { get; set; }
        public Int64? RowNum { get; set; }
        public int? TotalCount { get; set; }

        public Proc_GetListPromotion() { }
        public static IEntityProc GetEntityProc(int? pageNumber = null, int? pageSize = null, string searchText = null, DateTime? fromDate = null, DateTime? toDate = null, int? promotionTypeId = null,
           bool? isPublic = null, bool? isHidden = null, int? companyId = null)
        {
            SqlParameter PageNumber = new SqlParameter("@PageNumber", pageNumber);
            if (!pageNumber.HasValue)PageNumber.Value = DBNull.Value;

            SqlParameter PageSize = new SqlParameter("@PageSize", pageSize);
            if (!pageSize.HasValue)PageSize.Value = DBNull.Value;

            SqlParameter SearchText = new SqlParameter("@SearchText", searchText);
            if (string.IsNullOrWhiteSpace(searchText)) SearchText.Value = DBNull.Value;

            SqlParameter FromDate = new SqlParameter("@FromDate", fromDate);
            if (!fromDate.HasValue) FromDate.Value = DBNull.Value;
            //
            SqlParameter ToDate = new SqlParameter("@ToDate", toDate);
            if (!toDate.HasValue) ToDate.Value = DBNull.Value;

            SqlParameter PromotionTypeId = new SqlParameter("@PromotionTypeId", promotionTypeId);
            if (!promotionTypeId.HasValue) PromotionTypeId.Value = DBNull.Value;

            SqlParameter IsPublic = new SqlParameter("@IsPublic", isPublic);
            if (!isPublic.HasValue) IsPublic.Value = DBNull.Value;

            SqlParameter IsHidden = new SqlParameter("@IsHidden", isHidden);
            if (!isHidden.HasValue) IsHidden.Value = DBNull.Value;

            SqlParameter CompanyId = new SqlParameter("@CompanyId", companyId);
            if (!companyId.HasValue) CompanyId.Value = DBNull.Value;

            return new EntityProc(
                $"{ProcName} @PageNumber, @PageSize, @SearchText, @FromDate, @ToDate, @PromotionTypeId, @IsPublic, @IsHidden, @CompanyId",
                new SqlParameter[] {
                    PageNumber,
                    PageSize,
                    SearchText,
                    FromDate,
                    ToDate,
                    PromotionTypeId,
                    IsPublic,
                    IsHidden,
                    CompanyId
                }
            );
        }
    }
}
