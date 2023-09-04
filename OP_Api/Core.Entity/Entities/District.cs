using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using Core.Entity.Abstract;

namespace Core.Entity.Entities
{
    public class District : EntitySimple
    {
        public District()
        {
        }

        public int ProvinceId { get; set; }
        public bool IsRemote { get; set; }
        public Province Province { get; set; }
        public IEnumerable<Ward> Wards { get; set; } = new Collection<Ward>();
        public string VSEOracleCode { get; set; }
    }
}
