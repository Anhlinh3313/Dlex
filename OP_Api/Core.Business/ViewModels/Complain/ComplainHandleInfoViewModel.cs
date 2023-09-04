using Core.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels
{
    public class ComplainHandleInfoViewModel : SimpleViewModel
    {
        public ComplainHandleInfoViewModel() { }

        public int ComplainId { get; set; }
        public int ComplainStatusId { get; set; }
        public string HandleContent { get; set; }
        public string HandleImagePath { get; set; }
        public int HandleEmpId { get; set; }
        public int HandleHubId { get; set; }
        //
        public virtual ComplainStatus ComplainStatus { get; set; }
        public virtual User HandleEmp { get; set; }
        public virtual Hub HandleHub { get; set; }
    }
}
