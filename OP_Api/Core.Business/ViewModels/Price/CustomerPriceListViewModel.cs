using Core.Entity.Abstract;
using Core.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels
{
    public class CustomerPriceListViewModel : IEntityBase
    {
        public CustomerPriceListViewModel() { }

        public int CustomerId { set; get; }
        public int PriceListId { set; get; }
        public int Id { get; set; }
        public bool IsEnabled { get; set; }
        public int? CompanyId { get; set; }
    }
}
