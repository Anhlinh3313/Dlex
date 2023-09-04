using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels
{
    public class BoxPriceViewModel
    {
        public BoxPriceViewModel() { }

        public int Count { get; set; }
        public double Length { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public int PackTypeId { get; set; }
        public double Price { get; set; }
        public bool IsCalculate { get; set; }
    }
}
