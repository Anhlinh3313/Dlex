using Core.Business.ViewModels.ExportExcelModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels.Report
{
    public class GetReportByRevenueYearExportExcelViewModel
    {
         public int year { get; set; }
          public CustomExportFile CustomExportFile { get; set; }
    }
}
