using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entity.Entities
{
    public class CustomerPriceList : IEntityBase
    {
        public CustomerPriceList() { }

        public int CustomerId { set; get; }
        public int PriceListId { set; get; }
        public int Id { get; set; }
        public virtual PriceList PriceList { set; get; }
        public bool IsEnabled { get; set; }
        public int? CompanyId { get; set; }
    }
}
