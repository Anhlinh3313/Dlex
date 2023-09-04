using Core.Business.ViewModels.ExportExcelModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels.Report
{
	public class ReportListGoodsByCustomerViewModel
    {
		public CustomExportFile CustomExportFile { get; set; }
		public int? CustomerId { get; set; } = null;
        public bool? IsCreatedPayment { get; set; }
        public DateTime? FromDate { get; set; } = null;
		public DateTime? ToDate { get; set; } = null;
		public int? PageNumber { get; set; } = null;
		public int? PageSize { get; set; } = null;
		public bool? IsSortDescending { get; set; } = null;
        public int? HubId { get; set; }
        public int? AccountingAcountId { get; set; }
    }

}
