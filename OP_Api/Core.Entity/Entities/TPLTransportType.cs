using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Entity.Abstract;

namespace Core.Entity.Entities
{
    public class TPLTransportType : IEntityBase
    {
        public TPLTransportType()
        {
        }

        public int Id { get; set; }
        public int TPLId { get; set; }
        public int TransportTypeId { get; set; }
        public bool IsEnabled { get; set; }
        public int? CompanyId { get; set; }
        public TPL TPL { get; set; }
        public TransportType TransportType { get; set; }
    }
}
