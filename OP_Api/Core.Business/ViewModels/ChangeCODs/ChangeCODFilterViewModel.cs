using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels.ChangeCODS
{
    public class ChangeCODFilterViewModel
    {
        public int? UserId { get; set; }
        public bool? IsAccept { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; } 
    }
}
