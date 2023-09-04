using Core.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels
{
    public class ReportDeliveryAndReturnComplateViewModel
    {

        public ReportDeliveryAndReturnComplateViewModel()
        {

        }
        public ReportDeliveryAndReturnComplateViewModel(int totalDeliveyCompleteOfCurrentDay, int totalDeliveyCompleteOfCurrentMonth, int totalReturnCompleteOfCurrentDay, int totalReturnCompleteOfCurrentMonth, int totalPickupCompleteInDay, int totalPickupCompleteInMonth)
        {
            TotalDeliveyCompleteOfCurrentDay = totalDeliveyCompleteOfCurrentDay;
            TotalDeliveyCompleteOfCurrentMonth = totalDeliveyCompleteOfCurrentMonth;
            TotalReturnCompleteOfCurrentDay = totalReturnCompleteOfCurrentDay;
            TotalReturnCompleteOfCurrentMonth = totalReturnCompleteOfCurrentMonth;
            TotalPickupCompleteOfCurrentDay = totalPickupCompleteInDay;
            TotalPickupCompleteOfCurrentMonth = totalPickupCompleteInMonth;
        }

        public int TotalDeliveyCompleteOfCurrentDay { get; set; }
        public int TotalDeliveyCompleteOfCurrentMonth { get; set; }
        public int TotalReturnCompleteOfCurrentDay { get; set; }
        public int TotalReturnCompleteOfCurrentMonth { get; set; }
        public int TotalPickupCompleteOfCurrentDay { get; set; }
        public int TotalPickupCompleteOfCurrentMonth { get; set; }
    }
}