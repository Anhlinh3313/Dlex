using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
namespace Core.Entity.Procedures
{
    public class Proc_ReportShipmentQuantity : IEntityProcView
    {
        public const string ProcName = "Proc_ReportShipmentQuantity";

        public Int64 RowNum { get; set; }
        public int Id { get; set; }
        public string ShipmentNumber { get; set; }
        public string PickupName { get; set; }
        public double? Weight { get; set; }
        public double? CalWeight { get; set; }
        public int TotalBox { get; set; }
        public double TotalPrice { get; set; }
        public string ShipmentNumberTo { get; set; }
        public string DeliveredName { get; set; }
        public double? WeightTo { get; set; }
        public double? CalWeightTo{ get; set; }
        public int TotalBoxTo{ get; set; }
        public double? TotalPriceTo{ get; set; }
        public string ServiceName{ get; set; }
        public double? COD { get; set; }
        public string ReceiverName { get; set; }
        public string ShipmentNumberExist { get; set; }
        public string ReasonUpdate { get; set; }
        public string CurrentHubName{ get; set; }
        public int TotalCount { get; set; }

        public Proc_ReportShipmentQuantity()
        {
        }

        public static IEntityProc GetEntityProc(int? fromHubId = null, DateTime? dateFrom = null, DateTime? dateTo = null,int? pageSize = 20, int? pageNum = 1)
        {
            SqlParameter parameter2 = new SqlParameter(
            "@DateFrom", dateFrom);
            if (!dateFrom.HasValue)
                parameter2.Value = DBNull.Value;
            SqlParameter parameter3 = new SqlParameter(
            "@DateTo", dateTo);
            if (!dateTo.HasValue)
                parameter3.Value = DBNull.Value;
            SqlParameter parameter4 = new SqlParameter(
           "@FromHubId", fromHubId);
            if (!fromHubId.HasValue)
                parameter4.Value = DBNull.Value;
            SqlParameter parameter5 = new SqlParameter(
           "@PageSize", pageSize);
            if (!pageSize.HasValue)
                parameter5.Value = 20;
            SqlParameter parameter6 = new SqlParameter(
           "@PageNum", pageNum);
            if (!pageNum.HasValue)
                parameter6.Value = 1;

            return new EntityProc(
                $"{ProcName} @DateFrom, @DateTo, @FromHubId, @PageSize, @PageNum",
                new SqlParameter[] {
                parameter2,
                parameter3,
                parameter4,
                parameter5,
                parameter6
                }
            );
        }
    }
}
