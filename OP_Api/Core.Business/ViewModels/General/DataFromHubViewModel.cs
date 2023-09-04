using System.Collections.Generic;
using Core.Entity.Entities;

namespace Core.Business.ViewModels.General
{
    public class DataFromHubViewModel
    {
        public DataFromHubViewModel()
        {
        }

        public int[] SelectedProvinceCiyIds { get; set; }
        public int[] SelectedDistrictIds { get; set; }
        public IEnumerable<District> Districts { get; set; }
        public IEnumerable<Ward> Wards { get; set; }
    }
}