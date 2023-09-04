using System;
using System.Collections.Generic;
using Core.Entity.Procedures;

namespace Core.Business.ViewModels.General
{
    public class GetDatasFromHubViewModel
    {
        public GetDatasFromHubViewModel()
        {
        }

        public int[] SelectedWardIds { get; set; }
		public IEnumerable<Proc_Hub_RoutingWard_Availability> Wards { get; set; }
    }
}
