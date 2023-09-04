using Core.Business.ViewModels.ExportExcelModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels.Report
{
    public class ReportCODConfirmViewModel
    {
        public int? FromHubId { get; set; } = null;
        public int? ToHubId { get; set; } = null;
        public int? CurrentHubId { get; set; } = null;
        public int? ServiceId { get; set; } = null;
        public int? ShipmentStatusId { get; set; } = null;
        public int? SenderId { get; set; } = null;
        public int? CurrentUserId { get; set; } = null;
        public int? AccountingAccountId { get; set; } = null;
        public int? FromProvinceId { get; set; } = null;
        public int? ToProvinceId { get; set; } = null;
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public int? PageNumber { get; set; } = null;
        public int? PageSize { get; set; } = null;
        public string SearchText { get; set; } = null;

        public CustomExportFile CustomExportFile { get; set; }
    }
}
