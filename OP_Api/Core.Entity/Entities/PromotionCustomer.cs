using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entity.Entities
{
    public class PromotionCustomer: EntitySimple
    {
        public PromotionCustomer() {}
        public int CustomerId { get; set; }
        public int PromotionId { get; set; }

    }
}
