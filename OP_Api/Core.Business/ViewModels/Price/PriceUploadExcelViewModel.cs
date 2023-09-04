using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels
{
    public class PriceUploadExcelViewModel
    {
        public PriceUploadExcelViewModel() { }

        public string AreaCode { get; set; }
        public string WeightCode { get; set; }
        public int Price { get; set; }
    }
}
