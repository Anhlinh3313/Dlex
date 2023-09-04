using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entity.Entities
{
    public class ListReceiptMoneyImage : IEntityBase
    {
        public ListReceiptMoneyImage() { }

        public int Id { get; set; }
        public int ListReceiptMoneyId { get; set; }
        public DateTime? CreatedWhen { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedWhen { get; set; }
        public int? ModifiedBy { get; set; }
        public bool IsEnabled { get; set; } = true;
        public string ImagePath { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int? CompanyId { get; set; }
    }
}
