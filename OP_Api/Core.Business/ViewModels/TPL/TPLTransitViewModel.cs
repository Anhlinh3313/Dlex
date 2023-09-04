using Core.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels
{
    public class TPLTransitViewModel : SimpleViewModel<TPLTransitViewModel,TPLTransit>
    {
        public TPLTransitViewModel() { }

        public int TPLId { get; set; }
        public string TPLNumber { get; set; }
        public int ShipmentId { get; set; }
        public int FromHubId { get; set; }
        public int ToHubId { get; set; }
        public int StatusId { get; set; }
    }
}
