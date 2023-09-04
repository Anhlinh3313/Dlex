using Core.Business.ViewModels.ExportExcelModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels.Report
{
    public class ReportListGoodsConfirmTSMViewModel
    {
        public int? FromHubId { get; set; } = null;
        public int? ToHubId { get; set; } = null;
        public int? CurrentUserId { get; set; } = null;
        public int? AccountingAccountId { get; set; } = null;
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public int? PageNumber { get; set; } = null;
        public int? PageSize { get; set; } = null;
        public string SearchText { get; set; } = null;

        public CustomExportFile CustomExportFile { get; set; }
    }
}
