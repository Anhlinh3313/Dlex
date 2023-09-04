using System;
namespace Core.Entity.Entities
{
    public class Box : EntitySimple
    {
        public Box()
        {
        }

        public string Content { get; set; }
        public double Weight { get; set; }
        public double CalWeight { get; set; }
        public double ExcWeight { get; set; }
        public double Length { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public int ShipmentId { get; set; }
        public int? PackTypeId { get; set; }
        public double WoodWeight { get; set; }

        public Shipment Shipment { get; set; }
        public PackType PackType { get; set; }
    }
}
