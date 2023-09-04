using System;
using Core.Entity.Abstract;

namespace Core.Entity.Entities
{
    public class Province : EntitySimple
    {
        public Province()
        {
        }

        public int CountryId { get; set; }

        public Country Country { get; set; }
        public string VSEOracleCode { get; set; }
    }
}
