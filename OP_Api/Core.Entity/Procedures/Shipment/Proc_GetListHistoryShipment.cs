using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_GetListHistoryShipment : IEntityProcView
    {
        public const string ProcName = "Proc_GetListHistoryShipment";


        [Key]
        public int Id { get; set; }
        public string ShipmentNumber { get; set; }
        public DateTime? CreatedWhen { get; set; }
        public string DataChanged { get; set; }
        public int? ShipmentId { get; set; }
        public double? Insured { get; set; }
        public string ServiceName { get; set; }
        public string CusNote { get; set; }
        public int? FromWardId { get; set; }
        public string FromWardName { get; set; }
        public int? ToWardId { get; set; }
        public string ToWardName { get; set; }
        public int? FromDistrictId { get; set; }
        public string FromDistrictName { get; set; }
        public int? ToDistrictId { get; set; }
        public string ToDistrictName { get; set; }
        public int? FromProvinceId { get; set; }
        public string FromProvinceName { get; set; }
        public int? ToProvinceId { get; set; }
        public string ToProvinceName { get; set; }
        public string ShippingAddress { get; set; }
        public string PickingAddress { get; set; }
        public string SenderName { get; set; }
        public string SenderPhone { get; set; }
        public string SenderCode { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverPhone { get; set; }
        public string ModifiedByName { get; set; }
        public int? FromHubId { get; set; }
        public string FromHubName { get; set; }
        public int? ToHubId { get; set; }
        public string ToHubName { get; set; }
        public int? TotalBox { get; set; }
        public double? Weight { get; set; }
        public Int64 RowNum { get; set; }
        public int TotalCount { get; set; }

        public static IEntityProc GetEntityProc(
            DateTime? fromDate,
            DateTime? toDate,
            string searchText,
            int? pageNumber,
            int? pageSize

            )
        {
            SqlParameter FromDate = new SqlParameter("FromDate", fromDate);
            if (!fromDate.HasValue)
                FromDate.Value = DBNull.Value;
            SqlParameter ToDate = new SqlParameter("ToDate", toDate);
            if (!toDate.HasValue)
                ToDate.Value = DBNull.Value;
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
                $"{ProcName} @FromDate, @ToDate, @SearchText, @PageNumber, @PageSize ",
                new SqlParameter[]
                {
                    FromDate,
                    ToDate,
                    SearchText,
                    PageNumber,
                    PageSize
                }
            );
        }
    }
}
