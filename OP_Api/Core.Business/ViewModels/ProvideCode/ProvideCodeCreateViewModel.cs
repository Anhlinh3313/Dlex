using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels
{
    public class ProvideCodeCreateViewModel
    {
        public ProvideCodeCreateViewModel() { }

        public string ShipmentNumber { get; set; }
        public int Count { get; set; }
        public string Prefix { get; set; }
        public ushort Length { get; set; }
        public int NumberStart { get; set; }
        public int NumberEnd { get; set; }

        public int ProvideCodeStatusId { get; set; }
        public int? ProvideHubId { get; set; }
        public int? ProvideUserId { get; set; }
        public int? ProvideCustomerId { get; set; }
    }
}
