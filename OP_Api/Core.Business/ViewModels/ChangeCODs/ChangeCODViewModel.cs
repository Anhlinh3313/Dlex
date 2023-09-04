using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Business.ViewModels.ChangeCODS
{
    public class ChangeCODViewModel : IEntityBase
    {
        public int Id { get; set; }
        public bool IsEnabled { get; set; }
        public string ConcurrencyStamp { get; set; }
        public int? CompanyId { get; set; }
        // 
        public int ShipmentId { get; set; }
        public int ChangeCODTypeId { get; set; }
        public double OldCOD { get; set; }
        public double NewCOD { get; set; }
        public int? ShipmentSupportId { get; set; }
        public bool IsAccept { get; set; }
        public string Note { get; set; }
    }
}
