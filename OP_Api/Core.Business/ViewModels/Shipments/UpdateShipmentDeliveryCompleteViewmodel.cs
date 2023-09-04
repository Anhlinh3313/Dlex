using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Core.Data.Core.Utils;
using Core.Entity.Abstract;

namespace Core.Business.ViewModels
{
    public class UpdateShipmentDeliveryCompleteViewmodel
    {
        public UpdateShipmentDeliveryCompleteViewmodel()
        {
        }

        public int Id { get; set; }
        public DateTime EndDeliveryTime { get; set; }
        public string RealRecipientName { get; set; }
    }
}
