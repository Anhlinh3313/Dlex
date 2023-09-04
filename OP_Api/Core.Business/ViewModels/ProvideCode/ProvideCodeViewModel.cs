using Core.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels
{
    public class ProvideCodeViewModel : SimpleViewModel<ProvideCodeViewModel,ProvideCode>
    {
        public ProvideCodeViewModel() {  }

        public int ProvideCodeStatusId { get; set; }
        public int? ProvideHubId { get; set; }
        public int? ProvideUserId { get; set; }
        public int? ProvideCustomerId { get; set; }
    }
}
