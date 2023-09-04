using Core.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels
{
    public class UploadExcelHistoryInfoViewModel : EntitySimple
    {
        public UploadExcelHistoryInfoViewModel() { }

        public int? UserId { get; set; }
        public int? TotalCreated { get; set; }
        public int? TotalUpdated { get; set; }
        public int CountShipment { get; set; }
        public User User { get; set; }
    }
}
