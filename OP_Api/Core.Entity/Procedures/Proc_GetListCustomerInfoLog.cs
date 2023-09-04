using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_GetListCustomerInfoLog : IEntityProcView
    {
        public const string ProcName = "Proc_GetListCustomerInfoLog";
        [Key]
        public int Id { get; set; }
        public int? SenderId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int? ProvinceId { get; set; }
        public int? DistrictId { get; set; }
        public int? WardId { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string AddressNote { get; set; }
        public string ProvinceName { get; set; }
        public string DistrictName { get; set; }
        public string WardName { get; set; }
        public int? TotalCount { get; set; }

        public Proc_GetListCustomerInfoLog() { }
        public static IEntityProc GetEntityProc(int? senderId = null, int? provinceId = null, int? pageNumber = null, int? pageSize = null, string searchText = null, int? companyId = null)
        {
            SqlParameter SenderId = new SqlParameter("@SenderId", senderId);
            if (!senderId.HasValue) SenderId.Value = DBNull.Value;

            SqlParameter ProvinceId = new SqlParameter("@ProvinceId", provinceId);
            if (!provinceId.HasValue) ProvinceId.Value = DBNull.Value;

            SqlParameter PageNumber = new SqlParameter("@PageNumber", pageNumber);
            if (!pageNumber.HasValue) PageNumber.Value = DBNull.Value;

            SqlParameter PageSize = new SqlParameter("@PageSize", pageSize);
            if (!pageSize.HasValue) PageSize.Value = DBNull.Value;

            SqlParameter SearchText = new SqlParameter("@SearchText", searchText);
            if (string.IsNullOrWhiteSpace(searchText)) SearchText.Value = DBNull.Value;

            SqlParameter CompanyId = new SqlParameter("@CompanyId", companyId);
            if (!companyId.HasValue) CompanyId.Value = DBNull.Value;

            return new EntityProc(
                $"{ProcName} @SenderId, @ProvinceId, @PageNumber, @PageSize, @SearchText, @CompanyId",
                new SqlParameter[] {
                    SenderId,
                    ProvinceId,
                    PageNumber,
                    PageSize,
                    SearchText,
                    CompanyId
                }
            );
        }
    }
}
