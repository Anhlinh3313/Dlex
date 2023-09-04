using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entity.Entities
{
    public class CustomerSettinng : EntitySimple
    {
        public CustomerSettinng() { }

        public int CustomerId { get; set; }
        public int? FormPrintId { get; set; }
    }
}
