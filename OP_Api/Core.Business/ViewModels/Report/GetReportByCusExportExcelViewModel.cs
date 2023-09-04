using Core.Business.ViewModels.ExportExcelModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels.Report
{
    public class GetReportByCusExportExcelViewModel
    {
        public CustomExportFile CustomExportFile { get; set; }
        public int? senderId { get; set; } = null;
        public DateTime? dateFrom { get; set; } = null;
        public DateTime? dateTo { get; set; } = null;
        public string listProvinceIds { get; set; } = null;
        public string listDeliveryIds { get; set; } = null;
        public int? pageNumber { get; set; } = 1;
        public int? pageSize { get; set; } = 20;
    }
}
