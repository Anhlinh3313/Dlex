using Core.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels
{
    public class PaymentCODTypeViewModel : SimpleViewModel<PaymentCODTypeViewModel, PaymentCODType>
    {
        public PaymentCODTypeViewModel() { }
        
        public string VSEOracleCode { get; set; }
    }
}
