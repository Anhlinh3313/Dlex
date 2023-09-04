using Core.Business.ViewModels.ExportExcelModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels.Report
{
	public class ReturnLicenseReportViewModel
	{
		public CustomExportFile CustomExportFile { get; set; }
		public int? CenterHubId { get; set; } = null;
		public int? POHubId { get; set; } = null;
		public int? StationHubId { get; set; } = null;
		public int? CustomerId { get; set; } = null;
		public int? DeliveryUserId { get; set; } = null;
		public DateTime? FromDate { get; set; } = null;
		public DateTime? ToDate { get; set; } = null;
		public string SearchText { get; set; }
		public int? PageNumber { get; set; } = null;
		public int? PageSize { get; set; } = null;
		public bool? IsSortDescending { get; set; } = null;
	}

}
