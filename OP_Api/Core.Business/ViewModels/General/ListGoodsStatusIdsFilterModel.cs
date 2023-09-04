using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels
{
    public class ListGoodsStatusIdsFilterModel
    {
        public ListGoodsStatusIdsFilterModel()
        {
        }
        public int?[] statusIds { get; set; }
        public int fromHubId { get; set; }
    }
}
