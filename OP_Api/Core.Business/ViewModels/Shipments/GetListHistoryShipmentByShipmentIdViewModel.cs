using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Core.Data.Core.Utils;

namespace Core.Business.ViewModels.General
{
    public class GetListHistoryShipmentByShipmentIdViewModel : SimpleViewModel
    {
        public GetListHistoryShipmentByShipmentIdViewModel()
        {
        }
        public int? ShipmentId { get; set; }
        public int? Id { get; set; }
    }
}
