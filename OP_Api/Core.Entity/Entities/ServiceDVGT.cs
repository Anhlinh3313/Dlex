using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entity.Entities
{
    public class ServiceDVGT : EntitySimple
    {
        public ServiceDVGT() { }

        public bool IsPublish { get; set; }

        public string VSEOracleCode { get; set; }
    }
}
