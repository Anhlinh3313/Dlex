using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels
{
    public class UpdateRemotePriceViewModel
    {
        public List<RemotePriceViewModel> RemotePrices { get; set; }
        public List<RemoteKmViewModel> RemoteKms { get; set; }
        public List<RemotePriceDetailViewModel> RemotePriceDetails { get; set; }
    }
}
