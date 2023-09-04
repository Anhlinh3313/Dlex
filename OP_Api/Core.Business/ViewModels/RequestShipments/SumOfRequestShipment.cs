using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Business.ViewModels.General;
using Core.Entity.Abstract;
using Core.Entity.Entities;
using Core.Entity.Procedures;

namespace Core.Business.ViewModels
{
    public class SumOfRequestShipment
    {
        public int? SumCountShipmentAccept { get; set; } // tổng vận đơn đã xác nhận
        public int? SumTotalShipmentFilledUp { get; set; } // tổng vận đơn đã nhập
        public int? SumTotalShipmentNotFill { get; set; } // tổng vận đơn chưa nhập
    }
}
