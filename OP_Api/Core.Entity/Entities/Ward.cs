using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Core.Entity.Abstract;

namespace Core.Entity.Entities
{
    public class Ward : EntitySimple
    {
        public Ward()
        {
        }
        public bool IsRemote { get; set; }
        public int KmNumber { get; set; }
        public int DistrictId { get; set; }
        public District District { get; set; }
        public string VSEOracleCode { get; set; }
    }
}
