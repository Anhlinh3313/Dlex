using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels
{
    public class PackageScanViewModel
    {
        public PackageScanViewModel() { }

        public string ShipmentNumber { get; set; }
        public int PackageId { get; set; }
    }
}
