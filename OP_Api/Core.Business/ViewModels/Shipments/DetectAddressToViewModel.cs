using System;
namespace Core.Business.ViewModels.Shipments
{
    public class DetectAddressToViewModel
    {
        public DetectAddressToViewModel()
        {
        }

        public string ToProvinceName { get; set; }
        public string ToDistrictName { get; set; }
        public string ToWardName { get; set; }
        public string ToHubName { get; set; }
    }
}
