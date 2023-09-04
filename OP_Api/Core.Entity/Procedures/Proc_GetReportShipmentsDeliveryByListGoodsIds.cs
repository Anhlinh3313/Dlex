using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_GetReportShipmentsDeliveryByListGoodsIds : IEntityProcView
    {
        public const string ProcName = "Proc_GetReportShipmentsDeliveryByListGoodsIds";

        [Key]
        public int Id { get; set; }
        public string FromHubName { get; set; }
        public string DeliveryUserName { get; set; }
        public string ListGoodsCode { get; set; }
        public string ShipmentNumber { get; set; }
        public DateTime? OrderDate { get; set; }
        public string SenderName { get; set; }
        public int? TotalBox { get; set; }
        public Double? Weight { get; set; }
        public string ShippingAddress { get; set; }
        public string ReceiverName { get; set; }
        public string RealRecipientName { get; set; }
        public DateTime? EndDeliveryTime { get; set; }


        public Proc_GetReportShipmentsDeliveryByListGoodsIds()
        {
        }

        public static IEntityProc GetEntityProc(string listGoodsIds)
        {
            return new EntityProc(
                $"{ProcName} @ListGoodsIds",
                new SqlParameter[] {
                    new SqlParameter("@ListGoodsIds", listGoodsIds)
                }
            );
        }
    }
}
