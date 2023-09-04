using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels
{
    public class WebhooksNinjaVanViewModel
    {
        public int? shipper_id { get; set; }
        public string status { get; set; }
        public string shipper_ref_no { get; set; }
        public string tracking_ref_no { get; set; }
        public string shipper_order_ref_no { get; set; }
        public string timestamp { get; set; }
        public string id { get; set; }
        public string previous_status { get; set; }
        public string tracking_id { get; set; }
        public string comments { get; set; }
    }
}
