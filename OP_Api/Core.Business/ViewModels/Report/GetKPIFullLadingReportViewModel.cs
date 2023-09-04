using Core.Business.ViewModels.ExportExcelModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels.Report
{
	public class GetKPIFullLadingReportViewModel
	{
		public CustomExportFile CustomExportFile { get; set; }
		public int? centerHubId { get; set; }
		public int? poHubId { get; set; }
		public int? stationId { get; set; }
		public int? customerId { get; set; } 
		public DateTime? fromDate { get; set; } 
		public DateTime? toDate { get; set; }
		public string searchText { get; set; }
		public int? pageNumber { get; set; }
		public int? pageSize { get; set; }
		public bool? isSortDescending { get; set; }
	}
}

