using System;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Entity.Abstract;

namespace Core.Entity.Entities
{
    public class HubRoutingWard : IEntityBase
    {
        public HubRoutingWard()
        {
        }

        public int Id { get; set; }
        public int HubRoutingId { get; set; }
        public int WardId { get; set; }
        public bool IsEnabled { get; set; }
        public int? CompanyId { get; set; }

        public virtual HubRouting HubRouting { get; set; }
        public virtual Ward Ward { get; set; }
    }
}
