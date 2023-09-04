using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels.PromotionCustomers
{
    public class PromotionCustomerViewModel: SimpleViewModel
    {
        public PromotionCustomerViewModel() { }
        public int CustomerId { get; set; }
        public int PromotionId { get; set; }
    }
}
