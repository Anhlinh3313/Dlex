using Core.Business.ViewModels.ExportExcelModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels.KPIShipmentDetails
{
    public class GetKPIDetailExportViewModel
    {
        public CustomExportFile CustomExportFile { get; set; }
        public int KPIShipmentId { get; set; }
        public int? PageSize { get; set; }
        public int? PageNumber { get; set; }
    }
}
