using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels
{
    public class UpdateLadingScheduleViewModel
    {
        public UpdateLadingScheduleViewModel() { }

        public string TPLNumber { get; set; }
        public string StatusCode { get; set; }
        public string Location { get; set; }
        public string Note { get; set; }
        public DateTime? UpdatedWhen { get; set; }
        public string SignalBy { get; set; }
    }
}
