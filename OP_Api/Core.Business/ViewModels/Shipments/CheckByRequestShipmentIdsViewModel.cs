using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Core.Data.Core.Utils;

namespace Core.Business.ViewModels.General
{
    public class CheckByRequestShipmentIdsViewModel : SimpleViewModel
    {
        public CheckByRequestShipmentIdsViewModel()
        {
        }
        public string ShipmentNumber { get; set; }
        public List<int> RequestShipmentIds { get; set; }
    }
}
