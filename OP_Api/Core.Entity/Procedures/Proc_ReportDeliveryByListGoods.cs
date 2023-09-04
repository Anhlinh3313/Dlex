using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_ReportDeliveryByListGoods : IEntityProcView
    {
        public const string ProcName = "Proc_ReportDeliveryByListGoods";
        public Int64 Id { get; set; }
        public int HubId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }
        public string FullName { get; set; }
        public int TotalListGoods { get; set; }
        public int TotalListGoodsRecived { get; set; }
        public int TotalShipmentWaiting { get; set; }
        public int TotalShipmentDelivering { get; set; }
        public int TotalShipmentDeliveryFaile { get; set; }
        public int TotalShipmentDelivered { get; set; }
        public Proc_ReportDeliveryByListGoods() { }
        public static IEntityProc GetEntityProc(bool? isAllChild = false, int? hubId = null, int? userId = null, DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            SqlParameter parameter1 = new SqlParameter(
           "@IsAllChild", isAllChild);
            if (!isAllChild.HasValue)
                parameter1.Value = false;
            SqlParameter parameter2 = new SqlParameter(
            "@HubId", hubId);
            if (!hubId.HasValue)
                parameter2.Value = DBNull.Value;
            SqlParameter parameter3 = new SqlParameter(
            "@UserId", userId);
            if (!userId.HasValue)
                parameter3.Value = DBNull.Value;
            SqlParameter parameter4 = new SqlParameter(
            "@DateFrom", dateFrom);
            if (!dateFrom.HasValue)
                parameter4.Value = DBNull.Value;
            SqlParameter parameter5 = new SqlParameter(
            "@DateTo", dateTo);
            if (!dateTo.HasValue)
                parameter5.Value = DBNull.Value;

            return new EntityProc(
                $"{ProcName} @IsAllChild, @HubId, @UserId, @DateFrom, @DateTo",
                new SqlParameter[] {
                parameter1,
                parameter2,
                parameter3,
                parameter4,
                parameter5
                }
            );
        }
    }
}
