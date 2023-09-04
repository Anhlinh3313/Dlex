using Core.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels
{
    public class PaymentTypeViewModel : SimpleViewModel<PaymentTypeViewModel, PaymentType>
    {
        public PaymentTypeViewModel() { }
        public Boolean? PaymentNow { get; set; }
        public int? SortOrder { get; set; }
        public string VSEOracleCode { get; set; }
        public string VSEOracleTRA_NGAY { get; set; }
    }
}
