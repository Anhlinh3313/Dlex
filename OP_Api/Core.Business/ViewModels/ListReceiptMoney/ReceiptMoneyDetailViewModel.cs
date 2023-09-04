using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Core.Data.Core.Utils;

namespace Core.Business.ViewModels.General
{
    public class ReceiptMoneyDetailViewModel : SimpleViewModel
    {
        public ReceiptMoneyDetailViewModel()
        {
        }
        public int? ListReceiptMoneyId { get; set; }
        public int? Id { get; set; }
    }
}
