using Core.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels
{
    public class TPLTransitInfoViewModel : SimpleViewModel
    {
        public TPLTransitInfoViewModel() { }

        public string TPLNumber { get; set; }
        public int ShipmentId { get; set; }
        public int TPLId { get; set; }
        public TPLViewModel TPL { get; set; }
        public int FromHubId { get; set; }
        public Hub FromHub { get; set; }
        public int ToHubId { get; set; }
        public Hub ToHub { get; set; }
        public int StatusId { get; set; }
        public ShipmentStatusViewModel ShipmentStatus { get; set; }
    }
}
