using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels
{
    public class DeadlineUploadExcelViewModel
    {
        public string AreaCode { get; set; }
        public string ServiceCode { get; set; }
        public double? TimePickup { get; set; }
        public double? TimeDelivery { get; set; }
    }
}
