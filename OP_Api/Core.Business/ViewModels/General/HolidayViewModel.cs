using Core.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels
{
    public class HolidayViewModel : SimpleViewModel<HolidayViewModel, Holiday> /*SimpleViewModel<HolidayViewModel,Holiday>*/
    {
        public HolidayViewModel() { }
        public string NotHoliday { set; get; }
        public DateTime? Date { get; set; }
        public bool IsSa { get; set; }
        public bool IsSu { get; set; }
        public bool IsFull { get; set; }
    }
}
