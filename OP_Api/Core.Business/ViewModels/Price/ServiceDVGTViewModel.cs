using Core.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels
{
    public class ServiceDVGTViewModel : SimpleViewModel<ServiceDVGTViewModel, ServiceDVGT>
    {
        public ServiceDVGTViewModel() { }
        public bool IsPublish { get; set; }
    }
}
