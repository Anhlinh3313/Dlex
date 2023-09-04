using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entity.Entities
{
    public class TransportType : EntitySimple
    {
        public TransportType()
        {
        }

        public bool IsRequiredTPL { get; set; }
        public virtual List<TPLTransportType> TPLTransportTypes { get; set; }
    }
}
