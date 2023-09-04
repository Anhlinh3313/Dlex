using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels
{
    public class DeadlinePickupDeliveryDetailUploadExcelViewModel
    {
        public DeadlinePickupDeliveryDetailUploadExcelViewModel() { }

        public string[] AreaCodes { get; set; }
        public string[] ServiceCodes { get; set; }
        public DeadlinePickupDeliveryViewModel DeadlinePickupDeliveryViewModel { get; set; }
        public DeadlineUploadExcelViewModel[] DeadlineUploadExcelViewModels { get; set; }
    }
}
