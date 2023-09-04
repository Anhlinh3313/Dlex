using Core.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels
{
    public class ServiceDVGTPriceViewModel : SimpleViewModel
    {
        public ServiceDVGTPriceViewModel() { }

        public int? ServiceDVGTId { get; set; }
        public int? FormulaId { get; set; }
        public double ValueFrom { get; set; }
        public double ValueTo { set; get; }
        public double Price { get; set; }
        public double? ValuePlus { get; set; }
        public int? PriceListDVGTId { get; set; }
        public int? CalculateById { get; set; }
    }
}
