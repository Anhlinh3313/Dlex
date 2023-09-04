using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels
{
    public class ComplainFilterViewModel
    {
        public ComplainFilterViewModel() { }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public List<int> CenterHubIds { get; set; }
        public List<int> CustomerIds { get; set; }
        public List<int> ComplainTypeIds { get; set; }
        public List<int> ComplainStatusIds { get; set; }
        public int Type { get; set; }
        public string cols { get; set; }
        public int PageSize { get; set; }
        public int PageNum { get; set; }

    }
}
