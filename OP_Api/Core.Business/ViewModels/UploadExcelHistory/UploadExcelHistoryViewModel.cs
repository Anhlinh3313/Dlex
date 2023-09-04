using Core.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels
{
    public class UploadExcelHistoryViewModel : EntitySimple
    {
        public UploadExcelHistoryViewModel() { }

        public int? UserId { get; set; }
        public int? TotalCreated { get; set; }
        public int? TotalUpdated { get; set; }
    }
}
