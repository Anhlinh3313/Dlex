using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entity.Entities
{
    public class FormPrint : EntitySimple
    {
        public FormPrint() { }

        public int NumOrder { get; set; }
        public bool IsPublic { get; set; }
        public string FormPrintBody { get; set; }
        public int FormPrintTypeId { get; set; }
        public double? Width { get; set; }
        public double? Height { get; set; }

    }
}
