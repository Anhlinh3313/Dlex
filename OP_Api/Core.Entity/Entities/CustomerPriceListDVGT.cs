using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entity.Entities
{
    public class CustomerPriceListDVGT : EntitySimple
    {
        public CustomerPriceListDVGT() { }

        public int? CustomerId { get; set; }
        public int? PriceListDVGTId { get; set; }
        public Customer Customer { get; set; }
    }
}
