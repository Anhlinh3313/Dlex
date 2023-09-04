using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entity.Entities
{
    public class FromProvinceArea : IEntityBase
    {
        public int Id { get; set; }
        public bool IsEnabled { get; set; }
        public int ProvinceId { get; set; }
        public int AreaId { get; set; }
        public int? CompanyId { get; set; }
    }
}
