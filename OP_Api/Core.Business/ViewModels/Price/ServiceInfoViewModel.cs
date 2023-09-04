using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels
{
    public class ServiceInfoViewModel : SimpleViewModel
    {
        public ServiceInfoViewModel() { }

        public double Price { get; set; }
        public DateTime? ExpectedDeliveryTime { get; set; }
    }
}
