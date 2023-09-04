using System.Collections.Generic;
using Core.Entity.Procedures;

namespace Core.Business.ViewModels.General
{
    public class GetWardIdsByHubIdViewModel
    {
        public GetWardIdsByHubIdViewModel()
        {
        }
        
        public int[] SelectedWardIds { get; set; }
		public IEnumerable<Proc_GetWardIdsByHubId> WardIds { get; set; }
    }
}
