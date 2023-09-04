using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels
{
    public class RemotePriceDetailViewModel : IEntityBase
    {
        public RemotePriceDetailViewModel() { }
        public int Id { get; set; }
        public bool IsEnabled { get; set; }
        public double Price { get; set; }
        public int RemotePriceId { get; set; }
        public int RemoteKmId { get; set; }
        public int? CompanyId { get; set; }
    }
}
