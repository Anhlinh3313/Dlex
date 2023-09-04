using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entity.Entities
{
    public class Service : EntitySimple
    {
        public Service() { }

		public bool IsPublish { set; get; }
        public bool IsSub { get; set; }
        public bool IsReturn { get; set; }
        public int? NUMBER_L_W_H_MULTIP { set; get; }
		public int? NUMBER_L_W_H_DIM { set; get; }

        public string VSEOracleCode { get; set; }
    }
}
