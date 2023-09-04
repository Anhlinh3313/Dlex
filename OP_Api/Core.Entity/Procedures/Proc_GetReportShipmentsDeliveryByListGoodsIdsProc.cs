using Core.Entity.Abstract;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace Core.Entity.Procedures
{
    public class Proc_GetReportShipmentsDeliveryByListGoodsIdsProc : IEntityProcView
    {
        public const string ProcName = "Proc_GetReportShipmentsDeliveryByListGoodsIdsProc";
        [Key]
        public int Id { get; set; }
        public string ShipmentNumber { get; set; }
        public string SOENTRY { get; set; }
        public string CurrentEmpCode { get; set; }
        public string CurrentEmpFullName { get; set; }
        public string ShipmentStatusName { get; set; }
        public DateTime? EndPickTime { get; set; }
        public DateTime? InOutDate { get; set; }
        public DateTime? EndDeliveryTime { get; set; }
        public string ReceiverName { get; set; }
        public string ShippingAddress { get; set; }
        public double? Cod { get; set; }
        public string CusNote { get; set; }
        public string LastDeliveryFailNote { get; set; }
        public int? ShipmentStatusId { get; set; }
        public Proc_GetReportShipmentsDeliveryByListGoodsIdsProc() { }
        public static IEntityProc GetEntityProc(string ids, int? shipmentStatusId = null, int? pageNumber = null, int? pageSize = null)
        {
            SqlParameter Ids = new SqlParameter("@Ids", ids);

            SqlParameter ShipmentStatusId = new SqlParameter("@ShipmentStatusId", shipmentStatusId);
            if (!shipmentStatusId.HasValue) ShipmentStatusId.Value = DBNull.Value;

            SqlParameter PageNumber = new SqlParameter("@PageNumber", pageNumber);
            if (!pageNumber.HasValue) PageNumber.Value = DBNull.Value;

            SqlParameter PageSize = new SqlParameter("@PageSize", pageSize);
            if (!pageSize.HasValue) PageSize.Value = DBNull.Value;

            return new EntityProc(
                $"{ProcName} @Ids, @ShipmentStatusId, @PageNumber, @PageSize",
                new SqlParameter[] {
                    Ids,
                    ShipmentStatusId,
                    PageNumber,
                    PageSize
                }
            );
        }
    }
}
