using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entity.Entities
{
    public class CustomerDiscount : EntityBasic
    {
        public CustomerDiscount() { }
        public int DiscountId { get; set; }
        public int CustomerID { get; set; }
 
    }
}
