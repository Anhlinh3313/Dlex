using System;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using Core.Entity.Abstract;

namespace Core.Entity.Procedures
{
    public class Proc_GetByRequestShipmentIds : IEntityProcView
    {
        public const string ProcName = "Proc_GetByRequestShipmentIds";

        public int Id { get; set; }
        public string ShipmentNumber { get; set; }
        public string SenderName { get; set; }
        public string Note { get; set; }
        public string ShippingAddress { get; set; }
        public string ReceiverName { get; set; }
        public int? TotalBox { get; set; }
        public double? Weight { get; set; }
        public double? COD { get; set; }
        public int? ShipmentStatusId { get; set; }
        public int? BoxUnPicking { get; set; }
        public int? SumTotalBox { get; set; }
        public Int64 RowNum { get; set; }
        public int? TotalBoxUnPicking { get; set; }
        public int? TotalCount { get; set; }
        public int? TotalShipmentUnPicking { get; set; }

        public Proc_GetByRequestShipmentIds()
        {
        }

        public static IEntityProc GetEntityProc(string ids, string searchText, int? pageNumber = null,
            int? pageSize = null)
        {
            SqlParameter Ids = new SqlParameter("@Ids", ids);
            if (string.IsNullOrEmpty(ids))
                Ids.Value = DBNull.Value;

            SqlParameter SearchText = new SqlParameter("@SearchText", searchText);
            if (string.IsNullOrEmpty(searchText))
                SearchText.Value = DBNull.Value;
            SqlParameter PageNumber = new SqlParameter("@PageNumber", pageNumber);
            if (!pageNumber.HasValue)
                PageNumber.Value = DBNull.Value;
            SqlParameter PageSize = new SqlParameter("@PageSize", pageSize);
            if (!pageSize.HasValue)
                PageSize.Value = DBNull.Value;
            return new EntityProc(
                $"{ProcName} @Ids, @SearchText, @PageNumber, @PageSize",
                new SqlParameter[] {
                    Ids,
                    SearchText,
                    PageNumber,
                    PageSize
                }
            );
        }
    }
}
