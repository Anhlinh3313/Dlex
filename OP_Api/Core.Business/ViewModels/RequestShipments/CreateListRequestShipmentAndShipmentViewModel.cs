using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Core.Business.ViewModels.Shipments;
using Core.Data.Core.Utils;
using Core.Entity.Abstract;

namespace Core.Business.ViewModels
{
    public class CreateListRequestShipmentAndShipmentViewModel
    {
        public CreateListRequestShipmentAndShipmentViewModel()
        {
        }

        public int RequestShipmentId { get; set; }
        public List<ListRequestShipmentAndShipment> ListRequestShipmentAndShipment { get; set; }
    }
}
