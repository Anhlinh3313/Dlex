using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels
{
    public class EmailCenterViewModel
    {
        public EmailCenterViewModel() { }

        public string Title { get; set; }
        public string Body { get; set; }
        public string NameFrom { get; set; }
        public List<EmailCenterDetailViewModel> InfoShipments { get; set; }
    }
}
