using Core.Business.ViewModels.ExportExcelModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels.TruckTransfer
{
    public class TruckTransferReportExport
    {
        public DateTime? fromDate { get; set; } = null;
        public DateTime? toDate { get; set; } = null;
        public int? fromProvinceId { get; set; } = null;
        public int? toProvinceId { get; set; } = null;
        public int? truckId { get; set; } = null;
        public int? pageNumber { get; set; } = 1;
        public int? pageSize { get; set; } = 20;
        public CustomExportFile CustomExportFile { get; set; }
    }
}
