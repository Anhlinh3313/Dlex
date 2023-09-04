using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_GetUsersBySearchCode : IEntityProcView
    {
        public const string ProcName = "Proc_GetUsersBySearchCode";
        [Key]
        public int Id { get; set; }
        public int? TypeUserId { get; set; }
        public int? CompanyId { get; set; }
        public int? RoleId { get; set; }
        public int? HubId { get; set; }
        public string UserName { get; set; }
        public string Code { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public string RoleName { get; set; }
        public string DepartmentName { get; set; }
        public string ManageHubName { get; set; }
        public string HubName { get; set; }
        public int? DepartmentId { get; set; }
        public int? ManageHubId { get; set; }
        public bool? IsBlocked { get; set; }
        public string IdentityCard { get; set; }
        public int? TotalCount { get; set; }

        public Proc_GetUsersBySearchCode() { }
        public static IEntityProc GetEntityProc(int? companyId, int? pageNumber = null, int? pageSize = null, string searchText = null)
        {
            SqlParameter CompanyId = new SqlParameter("@CompanyId", companyId);
            if (!companyId.HasValue) CompanyId.Value = DBNull.Value;

            SqlParameter PageNumber = new SqlParameter("@PageNumber", pageNumber);
            if (!pageNumber.HasValue)PageNumber.Value = DBNull.Value;

            SqlParameter PageSize = new SqlParameter("@PageSize", pageSize);
            if (!pageSize.HasValue)PageSize.Value = DBNull.Value;

            SqlParameter SearchText = new SqlParameter("@SearchText", searchText);
            if (string.IsNullOrWhiteSpace(searchText)) SearchText.Value = DBNull.Value;

            return new EntityProc(
                $"{ProcName} @CompanyId,@PageNumber,@PageSize,@SearchText",
                new SqlParameter[] {
                    CompanyId,
                    PageNumber,
                    PageSize,
                    SearchText
                }
            );
        }
    }
}
