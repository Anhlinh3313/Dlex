using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_GetListShipmentDelay : IEntityProcView
    {
        public const string ProcName = "Proc_GetListShipmentDelay";

        [Key]
        public int Id { get; set; }
        public DateTime? CreatedWhen { get; set; }
        public int? CreatedBy { get; set; }
        public int ShipmentId { get; set; }
        public string ShipmentNumber { get; set; }
        public int? SenderId { get; set; }
        public string SenderName { get; set; }
        public string SenderPhone { get; set; }
        public string PickingAddress { get; set; }
        public double? Weight { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverPhone { get; set; }
        public string ShippingAddress { get; set; }
        public string ServiceName { get; set; }
        public DateTime? OrderDate { get; set; }
        public bool IsPrioritize { get; set; }
        public string Note { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string ShipmentStatusName { get; set; }
        public string CurrentHubName { get; set; }
        public string ReasonDelayName { get; set; }
        public int TotalCount { get; set; }

        public Proc_GetListShipmentDelay()
        {
        }

        public static IEntityProc GetEntityProc(DateTime? fromDate, DateTime? toDate, int? customerId, int? serviceId, int? reasonDelayId, int? pageSize, int? pageNum)
        {
            SqlParameter sqlParameter1 = new SqlParameter("@FromDate", fromDate);
            if (!fromDate.HasValue) sqlParameter1.Value = DBNull.Value;
            SqlParameter sqlParameter2 = new SqlParameter("@ToDate", toDate);
            if (!toDate.HasValue) sqlParameter2.Value = DBNull.Value;
            SqlParameter sqlParameter3 = new SqlParameter("@Customerid", customerId);
            if (!customerId.HasValue) sqlParameter3.Value = DBNull.Value;
            SqlParameter sqlParameter4 = new SqlParameter("@ServiceId", serviceId);
            if (!serviceId.HasValue) sqlParameter4.Value = DBNull.Value;
            SqlParameter sqlParameter5 = new SqlParameter("@ReasonDelayId", reasonDelayId);
            if (!reasonDelayId.HasValue) sqlParameter5.Value = DBNull.Value;
            SqlParameter sqlParameter6 = new SqlParameter("@PageSize", pageSize);
            if (!pageSize.HasValue) sqlParameter6.Value = DBNull.Value;
            SqlParameter sqlParameter7 = new SqlParameter("@PageNum", pageNum);
            if (!pageNum.HasValue) sqlParameter7.Value = DBNull.Value;
            return new EntityProc(
                $"{ProcName} @FromDate,@ToDate,@CustomerId,@ServiceId,@ReasonDelayId,@PageSize,@PageNum",
                new SqlParameter[] {
                sqlParameter1,
                sqlParameter2,
                sqlParameter3,
                sqlParameter4,
                sqlParameter5,
                sqlParameter6,
                sqlParameter7
                }
            );
        }
    }
}
