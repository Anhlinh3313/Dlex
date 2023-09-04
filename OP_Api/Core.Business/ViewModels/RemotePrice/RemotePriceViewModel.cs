using Core.Data.Core.Utils;
using Core.Entity.Abstract;
using Core.Entity.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Core.Business.ViewModels
{
    public class RemotePriceViewModel : SimpleViewModel<RemotePriceViewModel,RemotePrice>
    {
        public RemotePriceViewModel() { }

        public double FromValue { get; set; }
        public double ToValue { get; set; }
    }
}
