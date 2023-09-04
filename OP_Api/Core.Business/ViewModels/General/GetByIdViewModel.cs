using System;
namespace Core.Business.ViewModels
{
    public class GetByIdViewModel
    {
        public GetByIdViewModel()
        {
        }

        public int Id { get; set; }
        public int? ShipmentStatusId { get; set; }
        public int? PageSize { get; set; }
        public int? PageNumber { get; set; }
        public string Cols { get; set; }
        public bool IsHideInPackage { get; set; }
    }
}
