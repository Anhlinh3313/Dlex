using System;
using Core.Entity.Abstract;

namespace Core.Entity.Entities
{
    public class HubRoute : IEntityBase
    {
        public HubRoute()
        {
        }

        public int Id { get; set; }
        public int HubId { get; set; }
        public int WardId { get; set; }
        public bool IsEnabled { get; set; }
        public int? CompanyId { get; set; }
    }
}
