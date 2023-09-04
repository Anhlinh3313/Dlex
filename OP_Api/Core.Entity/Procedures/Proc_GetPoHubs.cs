using Core.Entity.Abstract;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace Core.Entity.Procedures
{
    public class Proc_GetPoHubs : IEntityProcView
    {
        public const string ProcName = "Proc_GetPoHubs";
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int? DistrictId { get; set; }
        public string DistrictName { get; set; }
        public int? WardId { get; set; }
        public string WardName { get; set; }
        public int? POHubId { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string AddressDisplay { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public int? CompanyId { get; set; }
        public int? ProvinceId { get; set; }
        public string ProvinceName { get; set; }
        public bool? HasAirPort { get; set; }
        public string PoHubName { get; set; }
        public int? CenterHubId { get; set; }
        public string ConcurrencyStamp { get; set; }

        public int? TotalCount { get; set; }

        public Proc_GetPoHubs() { }
        public static IEntityProc GetEntityProc(int? pageNumber = null, int? pageSize = null, int? centerHubId = null, string searchText = null)
        {
            SqlParameter PageNumber = new SqlParameter("@PageNumber", pageNumber);
            if (!pageNumber.HasValue)PageNumber.Value = DBNull.Value;

            SqlParameter PageSize = new SqlParameter("@PageSize", pageSize);
            if (!pageSize.HasValue)PageSize.Value = DBNull.Value;

            SqlParameter CenterHubId = new SqlParameter("@CenterHubId", centerHubId);
            if (!centerHubId.HasValue) CenterHubId.Value = DBNull.Value;

            SqlParameter SearchText = new SqlParameter("@SearchText", searchText);
            if (string.IsNullOrWhiteSpace(searchText)) SearchText.Value = DBNull.Value;

            return new EntityProc(
                $"{ProcName} @PageNumber,@PageSize, @CenterHubId, @SearchText",
                new SqlParameter[] {
                    PageNumber,
                    PageSize,
                    CenterHubId,
                    SearchText
                }
            );
        }
    }
}
