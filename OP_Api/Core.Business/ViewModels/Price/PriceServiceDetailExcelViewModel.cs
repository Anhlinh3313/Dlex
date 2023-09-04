using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels
{
    public class PriceServiceDetailExcelViewModel
    {
        public PriceServiceDetailExcelViewModel() {  }

        public string[] AreaCodes { get; set; }
        public string[] WeightCodes { get; set; }
        public PriceServiceViewModel PriceServiceViewModel { get; set; }
        public PriceUploadExcelViewModel[] PriceUploadExcelViewModel { get; set; }
    }
}
