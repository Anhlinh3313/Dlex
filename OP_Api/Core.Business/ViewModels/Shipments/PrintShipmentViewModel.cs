using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Core.Data.Core.Utils;
using Core.Entity.Abstract;

namespace Core.Business.ViewModels
{
    public class PrintShipmentViewModel
    {
        public PrintShipmentViewModel()
        {
        }

        public List<int> ShipmentIds { get; set; }
        public int PrintTypeId { get; set; }
        public bool IsHideInPackage { get; set; }
    }
}
