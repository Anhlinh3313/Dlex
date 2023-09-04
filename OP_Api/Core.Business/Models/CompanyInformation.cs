using System;
namespace Core.Business.Services.Models
{
    public class CompanyInformation
    {
        public CompanyInformation()
        {
        }

        public CompanyInformation(string name, int typeShipmentCode, string prefixShipmentCode, string formatShipmentCode, int typeRequestCode, string prefixRequestCode, string fireBaseBrowserAPIKey, string apiKey)
        {
            Name = name;
            TypeShipmentCode = typeShipmentCode;
            PrefixShipmentCode = prefixShipmentCode;
            FormatShipmentCode = formatShipmentCode;
            TypeRequestCode = typeRequestCode;
            PrefixRequestCode = prefixRequestCode;
            FireBaseBrowserAPIKey = fireBaseBrowserAPIKey;
            ApiKey = apiKey;
        }

        public string Name { get; set; }
        public int TypeShipmentCode { get; set; }
        public string PrefixShipmentCode { get; set; }
        public string FormatShipmentCode { get; set; }
        public int TypeRequestCode { get; set; }
        public string PrefixRequestCode { get; set; }
        public string FireBaseBrowserAPIKey { get; set; }
        public string ApiKey { get; set; }
    }
}
