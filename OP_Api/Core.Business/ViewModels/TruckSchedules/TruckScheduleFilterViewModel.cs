using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels.TruckSchedules
{
    public class TruckScheduleFilterViewModel
    {
       public int? FromHubId { get; set; }
       public int? ToHubId { get; set; }
       public int? TruckScheduleStatusId { get; set; }
       public int? TruckId { get; set; }
       public string SearchText { get; set; }
       public string TruckNumber { get; set; }
       public DateTime? FromDate { get; set; }
       public DateTime? ToDate { get; set; }
       public string Cols { get; set; }
       public int? PageSize { get; set; }
       public int? PageNumber { get; set; }
    }
}
