using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entity.Entities
{
    public class PaymentType : EntitySimple
    {
        public PaymentType() { }
        public Boolean? PaymentNow { get; set; }
        public int? SortOrder { get; set; }
        public string VSEOracleCode { get; set; }
        public string VSEOracleTRA_NGAY { get; set; }
    }
}
