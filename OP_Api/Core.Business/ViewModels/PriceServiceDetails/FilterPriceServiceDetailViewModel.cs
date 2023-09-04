using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels
{
    public class FilterPriceServiceDetailViewModel
    {
        public int PriceServiceId { get; set; }
        public List<int> WeightIds { get; set; }
    }
}
