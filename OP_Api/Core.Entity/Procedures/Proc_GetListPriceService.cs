using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_GetListPriceService : IEntityProcView
    {
        public const string ProcName = "Proc_GetListPriceService";
        [Key]
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string ServiceName { get; set; }
        public string ServiceCode { get; set; }
        public int? ServiceId { get; set; }
        public int? NumOrder { get; set; }
        public DateTime? PublicDateFrom { get; set; }
        public DateTime? PublicDateTo { get; set; }
        public double? DIM { get; set; }
        public double? VATPercent { get; set; }
        public double? FuelPercent { get; set; }
        public double? RemoteAreasPricePercent { get; set; }
        public bool? IsTwoWay { get; set; }
        public bool? IsPublic { get; set; }
        public Int64? RowNum { get; set; }
        public int? TotalCount { get; set; }

        public Proc_GetListPriceService() { }
        public static IEntityProc GetEntityProc(int? pageNumber = null, int? pageSize = null, string searchText = null, DateTime? fromDate = null, DateTime? toDate = null, int? servicceId = null,
           int? provinceFromId = null, int? provinceToId = null, int? companyId = null)
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

            SqlParameter ServicceId = new SqlParameter("@ServicceId", servicceId);
            if (!servicceId.HasValue) ServicceId.Value = DBNull.Value;

            SqlParameter ProvinceFromId = new SqlParameter("@ProvinceFromId", provinceFromId);
            if (!provinceFromId.HasValue) ProvinceFromId.Value = DBNull.Value;

            SqlParameter ProvinceToId = new SqlParameter("@ProvinceToId", provinceToId);
            if (!provinceToId.HasValue) ProvinceToId.Value = DBNull.Value;

            SqlParameter CompanyId = new SqlParameter("@CompanyId", companyId);
            if (!companyId.HasValue) CompanyId.Value = DBNull.Value;

            return new EntityProc(
                $"{ProcName} @PageNumber, @PageSize, @SearchText, @FromDate, @ToDate, @ServicceId, @ProvinceFromId, @ProvinceToId, @CompanyId",
                new SqlParameter[] {
                    PageNumber,
                    PageSize,
                    SearchText,
                    FromDate,
                    ToDate,
                    ServicceId,
                    ProvinceFromId,
                    ProvinceToId,
                    CompanyId
                }
            );
        }
    }
}
