using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Business.ViewModels.General;
using Core.Entity.Abstract;
using Core.Entity.Entities;
using Core.Entity.Procedures;

namespace Core.Business.ViewModels
{
    public class ShipmentTrackingViewModel : IEntityBase
    {
        public ShipmentTrackingViewModel()
        {
        }

        public int Id { get; set; }
        public string ShipmentNumber { get; set; }
        public string SenderName { get; set; }
        public string SenderPhone { get; set; }
        public string PickingAddress { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverPhone { get; set; }
        public string ShippingAddress { get; set; }
        public string RealRecipientName { get; set; }
        public double Weight { get; set; }
        public double? CalWeight { get; set; }
        public double Length { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public string CompanyFrom { get; set; }
        public string CompanyTo { get; set; }
        public string AddressNoteFrom { get; set; }
        public string AddressNoteTo { get; set; }
        public int PriceListId { get; set; }
        public int FromDistrictId { get; set; }
        public int FromProvinceId { get; set; }
        public int ToDistrictId { get; set; }
        public int ToProvinceId { get; set; }
        public int? ShipmentId { get; set; }
        public int TotalShipment { get; set; }
        public bool IsEnabled { get; set; }
        public int? CompanyId { get; set; }

        public virtual CustomerInfoViewModel Sender { get; set; }
        public virtual UserInfoViewModel PickUser { get; set; }
        public virtual UserInfoViewModel DeliverUser { get; set; }
        public virtual UserInfoViewModel TransferUser { get; set; }
        public virtual UserInfoViewModel TransferReturnUser { get; set; }
        public virtual UserInfoViewModel ReturnUser { get; set; }
        public virtual ShipmentInfoViewModel RequestShipment { get; set; }
        public virtual HubInfoViewModel FromHub { get; set; }
        public virtual HubInfoViewModel ToHub { get; set; }
        public virtual HubRoutingInfoViewModel FromHubRouting { get; set; }
        public virtual HubRoutingInfoViewModel ToHubRouting { get; set; }
        public virtual WardInfoViewModel FromWard { get; set; }
        public virtual WardInfoViewModel ToWard { get; set; }
        public virtual ShipmentStatus ShipmentStatus { get; set; }
        public virtual ServiceViewModel Service { get; set; }
        public virtual StructureViewModel Structure { get; set; }
        public virtual PackageViewModel Package { get; set; }
        public virtual PaymentTypeViewModel PaymentType { get; set; }
        public virtual HubInfoViewModel ReceiveHub { get; set; }
        public virtual HubInfoViewModel CurrentHub { get; set; }
        public virtual UserInfoViewModel CurrentEmp { get; set; }
        public virtual IEnumerable<Proc_LadingSchedule_Joined> LadingSchedules { get; set; } = new Collection<Proc_LadingSchedule_Joined>();
    }
}
