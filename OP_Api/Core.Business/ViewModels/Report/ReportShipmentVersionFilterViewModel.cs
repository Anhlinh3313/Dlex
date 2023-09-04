using Core.Business.ViewModels.ExportExcelModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels.Report
{
    public class ReportShipmentVersionFilterViewModel
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int? HubId { get; set; } = null;
        public int? SenderId { get; set; } = null;
        public int? EmpId { get; set; } = null;
        public int? ShipmentId { get; set; } = null;
        public string ShipmentNumber { get; set; } = null;
        public int? PageNumber { get; set; } = null;
        public int? PageSize { get; set; } = null;
        public bool? IsSortByCOD { get; set; } = false;

        public CustomExportFile CustomExportFile { get; set; }
    }
}
