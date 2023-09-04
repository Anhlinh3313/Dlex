using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels.NinjaVanConnections
{
    public class RequestModel
    {
        public string service_type { get; set; }
        public string service_level { get; set; }
        public string requested_tracking_number { get; set; }
        public reference reference { get; set; }
        public from From { get; set; }
        public To To { get; set; }
        public parcel_job parcel_Job { get; set; }


    }
    public class reference
    {
        public string merchant_order_number { get; set; }
    }
    public class from
    {

        public string name { get; set; }
        public string phone_number { get; set; }
        public string email { get; set; }
        public addressfrom address { get; set; }

    }
    public class addressfrom
    {
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string area { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string postcode { get; set; }
    }
    public class To
    {
        public string name { get; set; }
        public string phone_number { get; set; }
        public string email { get; set; }
        public addressto addressto { get; set; }
    }
    public class addressto
    {
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string kelurahan { get; set; }
        public string kecamatan { get; set; }
        public string city { get; set; }
        public string province { get; set; }
        public string country { get; set; }
        public string postcode { get; set; }
    }
    public class parcel_job
    {
        public bool is_pickup_required { get; set; }
        public int pickup_address_id { get; set; }
        public string pickup_service_type { get; set; }
        public string pickup_service_level { get; set; }
        public string pickup_date { get; set; }
        public pickup_timeslot pickup_timeslot { get; set; }


        public string pickup_instructions { get; set; }
        public string delivery_instructions { get; set; }
        public string delivery_start_date { get; set; }
        public delivery_timeslot delivery_timeslot { get; set; }
    }
    public class pickup_timeslot
    {
        public string start_time { get; set; }
        public string end_time { get; set; }
        public string timezone { get; set; }
    }
    public class delivery_timeslot
    {
        public string start_time { get; set; }
        public string end_time { get; set; }
        public string timezone { get; set; }
    }
}

