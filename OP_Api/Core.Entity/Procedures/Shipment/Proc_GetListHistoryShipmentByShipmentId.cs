using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_GetListHistoryShipmentByShipmentId : IEntityProcView
    {
        public const string ProcName = "Proc_GetListHistoryShipmentByShipmentId";


        [Key]
        public int Id { get; set; }
        public string ShipmentNumber { get; set; }
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
        public string ToDistrictName { get; set;}
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

        public static IEntityProc GetEntityProc(
            int? shipmentId,
            int? id

            )
        {
            SqlParameter ShipmentId = new SqlParameter("@Shipmentid", shipmentId);
            if (!shipmentId.HasValue)
                ShipmentId.Value = DBNull.Value;

            SqlParameter Id = new SqlParameter("@Id", id);
            if (!id.HasValue)
                Id.Value = DBNull.Value;
            return new EntityProc(
                $"{ProcName} @ShipmentId, @Id ",
                new SqlParameter[]
                {
                    ShipmentId,
                    Id
                }
            );
        }
    }
}
