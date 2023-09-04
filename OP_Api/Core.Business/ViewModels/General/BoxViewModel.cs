using System;
using Core.Entity.Entities;

namespace Core.Business.ViewModels
{
    public class BoxViewModel : SimpleViewModel
    {
        public BoxViewModel()
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
        public int PackTypeId { get; set; }
        public double WoodWeight { get; set; }
    }
}
