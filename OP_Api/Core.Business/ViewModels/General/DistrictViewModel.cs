using Core.Entity.Entities;
using System;
namespace Core.Business.ViewModels.General
{
    public class DistrictViewModel : SimpleViewModel<DistrictViewModel, District>
    {
        public DistrictViewModel()
        {
        }

        public bool IsRemote { get; set; }
        public int ProvinceId { get; set; }
    }
}
