using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels
{
    public class PackagePriceViewModel : SimpleViewModel
    {
        public PackagePriceViewModel() { }
        
        public double ValueFrom { get; set; }
        public double ValueTo { get; set; }
        public double Price { get; set; }
        public int PackTypeId { get; set; }
        public int FormulaId { get; set; }
    }
}
