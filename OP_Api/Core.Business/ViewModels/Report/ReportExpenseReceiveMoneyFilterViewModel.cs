using Core.Business.ViewModels.ExportExcelModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels.Report
{
    public class ReportExpenseReceiveMoneyFilterViewModel
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int? HubId { get; set; }
        public int? AccountingAccountId { get; set; }
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
        public int? UserId { get; set; }

        public CustomExportFile CustomExportFile { get; set; }
    }
}
