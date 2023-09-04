using System;
using Core.Entity.Abstract;

namespace Core.Entity.Entities
{
    public class ModulePage : EntitySimple
    {
        public ModulePage()
        {
        }

        public string Url { get; set; }
        public string BackColor { get; set; }
        public string Icon { get; set; }
    }
}
