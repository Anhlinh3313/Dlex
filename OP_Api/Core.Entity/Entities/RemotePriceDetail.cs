using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entity.Entities
{
    public class RemotePriceDetail : IEntityBase
    {
        public int Id { get; set; }
        public bool IsEnabled { get; set; }
        public int RemotePriceId { get; set; }
        public int RemoteKmId { get; set; }
        public double Price { get; set; }
        public int? CompanyId { get; set; }
    }
}
