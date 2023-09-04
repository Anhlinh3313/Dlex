using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entity.Entities
{
    public class RemoteKm : IEntityBase
    {
        public int Id { get; set; }
        public bool IsEnabled { get; set; }
        public double FromKm { get; set; }
        public double ToKm { get; set; }
        public int? CompanyId { get; set; }
    }
}
