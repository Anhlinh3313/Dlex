using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entity.Entities
{
    public class PackagePrice : EntitySimple
    {
        public PackagePrice() { }

        public double ValueFrom { get; set; }
        public double ValueTo { get; set; }
        public double Price { get; set; }
        public int PackTypeId { get; set; }
        public int FormulaId { get; set; }

        public PackType PackType { get; set; }
        public Formula Formula { get; set; }
    }
}
