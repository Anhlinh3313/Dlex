using Core.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels
{
    public class PriceServiceDetailViewModel : SimpleViewModel
    {
        public PriceServiceDetailViewModel() { }

        public int PriceServiceId { set; get; }
        public int WeightId { get; set; }
        public int AreaId { get; set; }
        public double Price { get; set; }
    }
}
