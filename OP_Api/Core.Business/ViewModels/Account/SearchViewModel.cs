using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels
{
    public class SearchViewModel
    {
        public string SearchText { get; set; }
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
        public string Cols { get; set; }
        public int? PromotionId { get; set; }
    }
}
