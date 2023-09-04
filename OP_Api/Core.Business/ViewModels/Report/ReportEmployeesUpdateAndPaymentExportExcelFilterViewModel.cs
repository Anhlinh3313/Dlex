using Core.Business.ViewModels.ExportExcelModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels.Report
{
    public class ReportEmployeesUpdateAndPaymentExportExcelFilterViewModel
    {
        public DateTime? dateFrom { get; set; } = null;
        public DateTime? dateTo { get; set; } = null;
        public int? hubId { get; set; } = null;
        public int? empId { get; set; } = null;
        public int? pageNumber { get; set; } = 1;
        public int? pageSize { get; set; } = 20;
        public CustomExportFile CustomExportFile { get; set; }
    }
}
