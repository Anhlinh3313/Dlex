using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Business.ViewModels
{
    public class ListGoodsFilterViewModel
    {
        public int type { get; set; }
        public int? pageSize { get; set; }
        public int? pageNumber { get; set; }
        public string cols { get; set; }
        //public int? Id { get; set; }
        public bool? IsEnabled { get; set; }
        public DateTime? OrderDateFrom { get; set; }
        public DateTime? OrderDateTo { get; set; }
    }
}
