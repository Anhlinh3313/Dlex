using Core.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels
{
    public class ComplainHandleViewModel : EntitySimple
    {
        public ComplainHandleViewModel() { }
        public int ComplainId { get; set; }
        public int ComplainStatusId { get; set; }
        public string HandleContent { get; set; }
        public string HandleImagePath { get; set; }
        public int? HandleEmpId { get; set; }
        public int? HandleHubId { get; set; }
        public int? ForwardToEmpId { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
