using Core.Entity.Abstract;
using Core.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels.Discounts
{
    public class CustomerDiscountViewModel : EntityBasic
    {
        public int DiscountId { get; set; }
        public int CustomerID { get; set; }
    }
}
