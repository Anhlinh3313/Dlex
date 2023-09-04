using Core.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels
{
    public class ProvideCodeInfoViewModel: SimpleViewModel
    {
        public int ProvideCodeStatusId { get; set; }
        public int? ProvideHubId { get; set; }
        public int? ProvideUserId { get; set; }
        public int? ProvideCustomerId { get; set; }

        public ProvideCodeStatus ProvideCodeStatus { get; set; }
        public Hub Hub { get; set; }
        public User User { get; set; }
        public Customer Customer { get; set; }
    }
}
