using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_GetListPriceListSetting : IEntityProcView
    {
        public const string ProcName = "Proc_GetListPriceListSetting";
        [Key]
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string PriceListCode { get; set; }
        public string CustomerCode { get; set; }
        public int? CustomerId { get; set; }
        public int? ServiceId { get; set; }
        public int? PriceListId { get; set; }
        public string ConcurrencyStamp { get; set; }
        public double? VATSurcharge { get; set; }
        public double? FuelSurcharge { get; set; }
        public double? VSVXSurcharge { get; set; }
        public double? DIM { get; set; }
        public Int64? RowNum { get; set; }
        public int? TotalCount { get; set; }

        public Proc_GetListPriceListSetting() { }
        public static IEntityProc GetEntityProc(int? pageNumber = null, int? pageSize = null, int? companyId = null, int? customerId = null, int? serviceId = null, int? priceListId = null)
        {
          
            SqlParameter PageNumber = new SqlParameter("@PageNumber", pageNumber);
            if (!pageNumber.HasValue)PageNumber.Value = DBNull.Value;

            SqlParameter PageSize = new SqlParameter("@PageSize", pageSize);
            if (!pageSize.HasValue)PageSize.Value = DBNull.Value;

            SqlParameter CompanyId = new SqlParameter("@CompanyId", companyId);
            if (!companyId.HasValue) CompanyId.Value = DBNull.Value;

            SqlParameter CustomerId = new SqlParameter("@CustomerId", customerId);
            if (!customerId.HasValue) CustomerId.Value = DBNull.Value;

            SqlParameter ServiceId = new SqlParameter("@ServiceId", serviceId);
            if (!serviceId.HasValue) ServiceId.Value = DBNull.Value;

            SqlParameter PriceListId = new SqlParameter("@PriceListId", priceListId);
            if (!priceListId.HasValue) PriceListId.Value = DBNull.Value;

            return new EntityProc(
                $"{ProcName} @PageNumber, @PageSize, @CompanyId, @CustomerId, @ServiceId, @PriceListId",
                new SqlParameter[] {
                    PageNumber,
                    PageSize,
                    CompanyId,
                    CustomerId,
                    ServiceId,
                    PriceListId
                }
            );
        }
    }
}
