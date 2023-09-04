using System;
using Core.Infrastructure.ViewModels;

namespace Core.Business.ViewModels
{
    public class ShipmentFileViewModel
    {
        public ShipmentFileViewModel()
        {
        }

        public int ShipmentId { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public string FileBase64String { get; set; }
        public int? IncidentsId { get; set; }
    }
}
