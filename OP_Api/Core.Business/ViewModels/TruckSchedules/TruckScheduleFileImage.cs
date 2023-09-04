using Core.Infrastructure.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels
{
    public class TruckScheduleFileImage
    {
        public TruckScheduleFileImage()
        {
        }

        public int TruckScheduleId { get; set; }
        public List<FileViewModel> FileViewModels { get; set; }
    }
}
