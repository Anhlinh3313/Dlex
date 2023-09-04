using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_GetListPackage : IEntityProcView
    {
        public const string ProcName = "Proc_GetListPackage";
        [Key]
        public int Id { get; set; }
        public DateTime? CreatedWhen { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string  Content { get; set; }
        public double? Weight { get; set; }
        public double? CalWeight { get; set; }
        public double? Length { get; set; }
        public double? Width { get; set; }
        public double? Height { get; set; }
        public int? TotalShipment { get; set; }
        public string CreatedHubName { get; set; }
        public string CreatedUserName { get; set; }
        public string ToHubName { get; set; }
        public string StatusName { get; set; }
        public int TotalCount { get; set; }

        public Proc_GetListPackage() { }
        public static IEntityProc GetEntityProc(int? createdHubId = null, string searchText = null, int? statusId = null,
            DateTime? dateFrom = null, DateTime? dateTo = null, int? pageNumber = null, int? pageSize = null)
        {
            SqlParameter CreatedHubId = new SqlParameter("@CreatedHubId", createdHubId);
            if (!createdHubId.HasValue) CreatedHubId.Value = DBNull.Value;
            SqlParameter SearchText = new SqlParameter("@SearchText", searchText);
            if (string.IsNullOrWhiteSpace(searchText)) SearchText.Value = DBNull.Value;
            SqlParameter StatusId = new SqlParameter("@StatusId", statusId);
            if (!statusId.HasValue) StatusId.Value = DBNull.Value;
            SqlParameter DateFrom = new SqlParameter("@DateFrom", dateFrom);
            if (!dateFrom.HasValue) DateFrom.Value = DBNull.Value;
            SqlParameter DateTo = new SqlParameter("@DateTo", dateTo);
            if (!dateTo.HasValue) DateTo.Value = DBNull.Value;
            SqlParameter PageNumber = new SqlParameter("@PageNumber", pageNumber);
            if (!pageNumber.HasValue)PageNumber.Value = DBNull.Value;
            SqlParameter PageSize = new SqlParameter("@PageSize", pageSize);
            if (!pageSize.HasValue)PageSize.Value = DBNull.Value;

            return new EntityProc(
                $"{ProcName} @CreatedHubId,@SearchText,@StatusId,@DateFrom,@DateTo,@PageNumber,@PageSize",
                new SqlParameter[] {
                    CreatedHubId,
                    SearchText,
                    StatusId,
                    DateFrom,
                    DateTo,
                    PageNumber,
                    PageSize
                }
            );
        }
    }
}
