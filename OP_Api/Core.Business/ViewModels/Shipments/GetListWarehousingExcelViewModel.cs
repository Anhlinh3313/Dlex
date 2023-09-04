using Core.Business.ViewModels.ExportExcelModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels.Shipments
{
    public class GetListWarehousingExcelViewModel
    {
        public int? WarehousingType { get; set; } = null;
        public int? UserId { get; set; } = null;
        public int? HubId { get; set; } = null;
        public string ListGoodsList { get; set; } = null;
        public int? ToHubId { get; set; } = null;
        public int? ToUserId { get; set; } = null;
        public int? ServiceId { get; set; } = null;
        public bool? IsPrioritize { get; set; } = null;
        public bool? IsIncidents { get; set; } = null;
        public bool? IsAllShipment { get; set; } = null;
        public bool? IsHideInPackage { get; set; } = null;
        public bool? IsNullHubRouting { get; set; } = null;
        public int? SenderId { get; set; } = null;
        public DateTime? DateFrom { get; set; } = null;
        public DateTime? DateTo { get; set; } = null;
        public int? PageNumber { get; set; } = 1;
        public int? PageSize { get; set; } = 20;
        public int? UserEmpId { get; set; }

        public CustomExportFile CustomExportFile { get; set; }

    }
}
