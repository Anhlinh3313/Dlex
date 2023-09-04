using Core.Business.ViewModels.Shipments;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels
{
    public class UploadExcelShipment
    {
        public UploadExcelShipment() { }

        public List<CreateUpdateShipmentViewModel> ListData { get; set; }
    }
}
