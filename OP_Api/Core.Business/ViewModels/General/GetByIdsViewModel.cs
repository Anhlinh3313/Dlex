using System;
namespace Core.Business.ViewModels
{
    public class GetByIdsViewModel
    {
        public GetByIdsViewModel()
        {
        }

        public int[] Ids { get; set; }
        public int? ShipmentStatusId { get; set; }
        public int? PageSize { get; set; }
        public int? PageNumber { get; set; }
        public string Cols { get; set; }
        public bool IsHideInPackage { get; set; }
    }
}
