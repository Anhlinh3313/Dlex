using System;
using System.Collections.Generic;
using Core.Infrastructure.ViewModels;

namespace Core.Business.ViewModels
{
    public class ShipmentFileImages
    {
        public ShipmentFileImages()
        {
        }

        public int ShipmentId { get; set; }
        public int? ImageType { get; set; }
        public List<FileViewModel> FileViewModels { get; set; }
    }
}
