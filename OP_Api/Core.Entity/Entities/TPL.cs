using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entity.Entities
{
    public class TPL : EntitySimple
    {
        public TPL() { }

        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public bool IsInternational { get; set; }
        public int? PriceListId { get; set; }
        public PriceList PriceList { get; set; }
    }
}
