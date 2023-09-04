using Core.Business.ViewModels.ExportExcelModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels.Report
{
    public class GetReportByRevenueMonthExportExcelViewModel
    {
        public CustomExportFile CustomExportFile { get; set; }
        public DateTime? dateFrom { get; set; } = null;
        public DateTime? dateTo { get; set; } = null;
    }
}
