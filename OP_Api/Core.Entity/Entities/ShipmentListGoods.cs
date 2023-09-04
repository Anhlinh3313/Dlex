using System;
using Core.Entity.Abstract;

namespace Core.Entity.Entities
{
    public class ShipmentListGoods : IEntityBase
    {
        public ShipmentListGoods()
        {
            InOutDate = DateTime.Now;
        }

        public ShipmentListGoods(int shipmentId, int listGoodsId)
        {
            ShipmentId = shipmentId;
            ListGoodsId = listGoodsId;
            InOutDate = DateTime.Now;
        }

        public int Id { get; set; }
        public int ShipmentId { get; set; }
        public int ListGoodsId { get; set; }
        public bool IsEnabled { get; set; }
        public int? CompanyId { get; set; }
        public DateTime? InOutDate { get; set; }

        public Shipment Shipment { get; set; }
    }
}
