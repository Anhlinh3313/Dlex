using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_GetListReason : IEntityProcView
    {
        public const string ProcName = "Proc_GetListReason";
        [Key]
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool PickFail { get; set; }
        public bool PickCancel { get; set; }
        public bool PickReject { get; set; }
        public bool DeliverFail { get; set; }
        public bool DeliverCancel { get; set; }
        public bool ReturnFail { get; set; }
        public bool ReturnCancel { get; set; }
        public bool IsDelay { get; set; }
        public bool IsIncidents { get; set; }
        public bool IsPublic { get; set; }
        public bool IsAcceptReturn { get; set; }
        public bool IsMustInput { get; set; }
        public int ItemOrder { get; set; }
        public int? RoleId { get; set; }
        public bool IsUnlockListGood { get; set; }
        public string ConcurrencyStamp { get; set; }
        public Int64? RowNum { get; set; }
        public int? TotalCount { get; set; }

        public Proc_GetListReason() { }
        public static IEntityProc GetEntityProc(int? pageNumber = null, int? pageSize = null, string searchText = null, int? companyId = null)
        {
          
            SqlParameter PageNumber = new SqlParameter("@PageNumber", pageNumber);
            if (!pageNumber.HasValue)PageNumber.Value = DBNull.Value;

            SqlParameter PageSize = new SqlParameter("@PageSize", pageSize);
            if (!pageSize.HasValue)PageSize.Value = DBNull.Value;

            SqlParameter SearchText = new SqlParameter("@SearchText", searchText);
            if (string.IsNullOrWhiteSpace(searchText)) SearchText.Value = DBNull.Value;

            SqlParameter CompanyId = new SqlParameter("@CompanyId", companyId);
            if (!companyId.HasValue) CompanyId.Value = DBNull.Value;

            return new EntityProc(
                $"{ProcName} @PageNumber, @PageSize, @SearchText, @CompanyId",
                new SqlParameter[] {
                    PageNumber,
                    PageSize,
                    SearchText,
                    CompanyId
                }
            );
        }
    }
}
