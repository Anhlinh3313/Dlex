using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Core.Data.Core.Utils;

namespace Core.Business.ViewModels.General
{
    public class GetListShipmentByRequestShipmentIdViewModel : SimpleViewModel
    {
        public GetListShipmentByRequestShipmentIdViewModel()
        {
        }
        public string SearchText { get; set; }
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }

    }
}
