using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using Core.Entity.Abstract;

namespace Core.Entity.Procedures
{
    public class Proc_GetListCustomer : IEntityProcView
    {
        public const string ProcName = "Proc_GetListCustomer";

        [Key]
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string AddressNote { get; set; }
        public string PhoneNumber { get; set; }
        public string CompanyName { get; set; }
        public string UserName { get; set; }
        public int? ProvinceId { get; set; }
        public string ProvinceName { get; set; }
        public int? DistrictId { get; set; }
        public string DistrictName { get; set; }
        public int? WardId { get; set; }
        public string WardName { get; set; }
        public double? Lat { get; set; }
        public double? Lng { get; set; }
        public int? HubId { get; set; }
        public string HubName { get; set; }
        public int? PaymentTypeId { get; set; }
        public string PaymentTypeName { get; set; }
        public bool? IsShowPrice { get; set; }
        public string Email { get; set; }
        public string Fax { get; set; }
        public string Notes { get; set; }
        public string ParentCustomerName { get; set; }
        public int? ParentCustomerId { get; set; }
        public string VSEOracleCode { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string SignName { get; set; }
        public string SignRole { get; set; }
        public string CompanyPhone { get; set; }
        public string CompanyEmail { get; set; }
        public string AddressCompany { get; set; }
        public double? CommissionCus { get; set; }
        public string Professions { get; set; }
        public int? SalesUserId { get; set; }
        public string SalesUserName { get; set; }
        public int? SupportUserId { get; set; }
        public string SupportUserName { get; set; }
        public int? AccountingUserId { get; set; }
        public string AccountingUserName { get; set; }
        public int? PickupUserId { get; set; }
        public string PickupUserName { get; set; }
        public int? CustomerTypeId { get; set; }
        public string CustomerTypeName { get; set; }
        public string SecurityStamp { get; set; }
        public Int64? RowNum { get; set; }
        public int? TotalCount { get; set; }

        public Proc_GetListCustomer()
        {
        }

        public static IEntityProc GetEntityProc(int? customerId, string searchText, int? provinceId, bool? isAccept, int? pageSize, int? pageNumber, int? companyId)
        {
            SqlParameter CustomerId = new SqlParameter("@CustomerId", customerId);
            if (!customerId.HasValue)
                CustomerId.Value = DBNull.Value;

            SqlParameter SearchText = new SqlParameter("@SearchText", searchText);
            if (string.IsNullOrEmpty(searchText))
                SearchText.Value = DBNull.Value;

            SqlParameter ProvinceId = new SqlParameter("@ProvinceId", provinceId);
            if (!provinceId.HasValue)
                ProvinceId.Value = DBNull.Value;

            SqlParameter IsAccept = new SqlParameter("@IsAccept", isAccept);
            if (!isAccept.HasValue)
                IsAccept.Value = DBNull.Value;

            SqlParameter PageSize = new SqlParameter("@PageSize", pageSize);
            if (!pageSize.HasValue)
                PageSize.Value = DBNull.Value;

            SqlParameter PageNumber = new SqlParameter("@PageNumber", pageNumber);
            if (!pageNumber.HasValue)
                PageNumber.Value = DBNull.Value;

            SqlParameter CompanyId = new SqlParameter("@CompanyId", companyId);
            if (!companyId.HasValue)
                CompanyId.Value = DBNull.Value;

            return new EntityProc(
                $"{ProcName} @CustomerId, @SearchText, @ProvinceId, @IsAccept, @PageSize, @PageNumber, @CompanyId",
                new SqlParameter[] {
                    CustomerId,
                    SearchText,
                    ProvinceId,
                    IsAccept,
                    PageSize,
                    PageNumber,
                    CompanyId
                }
            );
        }
    }
}
