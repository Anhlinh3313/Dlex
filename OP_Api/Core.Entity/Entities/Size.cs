using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entity.Entities
{
    [Table("Post_Size")]
    public class Size : EntitySimple
    {
        public Size()
        {
        }

        public double Length { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public bool IsPackage { get; set; }
        public bool IsBox { get; set; }
    }
}
