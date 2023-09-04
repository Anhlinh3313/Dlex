using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entity.Entities
{
    public class HubRoutingStreetJoin : IEntityBase
    {
        public HubRoutingStreetJoin()
        {
        }

        public int Id { get; set; }
        public int HubRoutingId { get; set; }
        public int StreetJoinId { get; set; }
        public StreetJoin StreetJoin { get; set; }
        public HubRouting HubRouting { get; set; }
        public bool IsEnabled { get; set; }
        public int? CompanyId { get; set; }

    }
}
