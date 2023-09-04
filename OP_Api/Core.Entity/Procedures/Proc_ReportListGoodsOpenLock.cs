using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_ReportListGoodsOpenLock : IEntityProcView
    {
        public const string ProcName = "Proc_ReportListGoodsOpenLock";

        [Key]
        public Int64? RowNum { get; set; }
        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public string Code { get; set; }
        public string FullName { get; set; }
        public double? GrandTotal { get; set; }
        public bool? IsTransfer { get; set; }
        public int? TohubId { get; set; }
        public string HubName { get; set; }
        public string ShipmentNumber { get; set; }
        public string ReceiverCode2 { get; set; }
        public double? COD { get; set; }
        public string PaymentTypeName { get; set; }
        public double? TotalCOD { get; set; }
        public int? TotalListGoods { get; set; }

        public int? TotalCount { get; set; }

        public Proc_ReportListGoodsOpenLock()
        {
        }

        public static IEntityProc GetEntityProc(
            DateTime? dateFrom = null,
            DateTime? dateTo = null, 
            int? hubId = null,
            int? pageNumber = null, 
            int? pageSize = null
            )
        {
            SqlParameter DateFrom = new SqlParameter("@DateFrom", dateFrom);
            if (!dateFrom.HasValue)
                DateFrom.Value = DBNull.Value;

            SqlParameter DateTo = new SqlParameter("@DateTo", dateTo);
            if (!dateTo.HasValue)
                DateTo.Value = DBNull.Value;

            SqlParameter HubId = new SqlParameter("@HubId", hubId);
            if (!hubId.HasValue)
                HubId.Value = DBNull.Value;
          
            SqlParameter PageNumber = new SqlParameter("@PageNumber", pageNumber);
            if (!pageNumber.HasValue)
                PageNumber.Value = DBNull.Value;
            SqlParameter PageSize = new SqlParameter("@PageSize", pageSize);
            if (!pageSize.HasValue)
                PageSize.Value = DBNull.Value;


            return new EntityProc(
                $"{ProcName} @DateFrom,@DateTo,@HubId,@PageNumber,@PageSize",
                new SqlParameter[] {
                    DateFrom,
                    DateTo,
                    HubId,
                    PageNumber,
                    PageSize,
                }
            );
        }
    }
}
