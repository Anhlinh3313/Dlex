using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels
{
    public class GoogleDistnaceModel
    {
        public GoogleDistnaceModel() { }

        public string AddressFrom { get; set; }
        public double? LatFrom { get; set; }
        public double? LngFrom { get; set; }
        public string AddressTo { get; set; }
        public double? LatTo { get; set; }
        public double? LngTo { get; set; }
    }
}
