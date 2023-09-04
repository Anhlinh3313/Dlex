using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entity.Entities
{
    public class TelephoneAreaCodes : IEntityBase
    {
        public TelephoneAreaCodes() { }

        public int Id { set; get; }
        public int ProvinceId { set; get; }
        public int Code { set; get; }
        public string Name { set; get; }
        public bool IsEnabled { set; get; }
        public int? CompanyId { get; set; }
    }
}
