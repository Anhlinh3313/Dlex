using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Core.Business.ViewModels.General;
using Core.Data.Core.Utils;
using Core.Entity.Abstract;
using Core.Entity.Entities;

namespace Core.Business.ViewModels.Shipments
{
    public class ShipmentInfoMapping : IEntityBase
    {
        public ShipmentInfoMapping()
        {
        }

        public int Id { get; set; }
        public bool IsEnabled { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string ShipmentNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public int? SellerId { get; set; }
        public string SellerName { get; set; }
        public string SellerPhone { get; set; }
        public int SenderId { get; set; }
        public string SenderName { get; set; }
        public string SenderPhone { get; set; }
        public string PickingAddress { get; set; }
        public int FromWardId { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverPhone { get; set; }
        public string ShippingAddress { get; set; }
        public int? ToWardId { get; set; }
        public int? ServiceId { get; set; }
        public int? PaymentTypeId { get; set; }
        public string CusNote { get; set; }
        public string Note { get; set; }
        public string Content { get; set; }
        public double Weight { get; set; }
        public double Length { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }
        public double CalWeight { get; set; }
        public double LatFrom { get; set; }
        public double LngFrom { get; set; }
        public double LatTo { get; set; }
        public double LngTo { get; set; }
        public int? CurrentEmpId { get; set; }
        public int? CurrentHubId { get; set; }
        public int? RequestShipmentId { get; set; }
        public double COD { get; set; }
        public double Insured { get; set; }
        public bool IsAgreementPrice { get; set; }
        public double DefaultPrice { get; set; }
        public double RemoteAreasPrice { get; set; }
        public double FuelPrice { get; set; }
        public double TotalDVGT { get; set; }
        public double OtherPrice { get; set; }
        public double VATPrice { get; set; }
        public double TotalPrice { get; set; }
        public int? PriceListId { get; set; }
        public int? FromDistrictId { get; set; }
        public int? FromProvinceId { get; set; }
        public int? ToDistrictId { get; set; }
        public int? ToProvinceId { get; set; }
        public string CompanyFrom { get; set; }
        public string CompanyTo { get; set; }
        public string AddressNoteFrom { get; set; }
        public string AddressNoteTo { get; set; }
        public int TotalBox { get; set; }
        public int? StructureId { get; set; }
        public DateTime? FirstPickupTime { get; set; }
        public DateTime? FirstDeliveredTime { get; set; }
        public DateTime? FirstReturnTime { get; set; }
        public DateTime? FirstTransferTime { get; set; }
        public DateTime? FirstReturnTransferTime { get; set; }
        public bool IsPaidPrice { get; set; }
        public int? FromHubId { get; set; }
        public int? ToHubId { get; set; }
        public int? ShipmentStatusId { get; set; }
        public int? PickUserId { get; set; }
        public int? DeliverUserId { get; set; }
        public DateTime? ExpectedDeliveryTime { get; set; }
        public DateTime? ExpectedDeliveryTimeSystem { get; set; }
        public DateTime? EndPickTime { get; set; }
        public string STRServiceDVGTIds { get; set; }
        public List<Box> Boxes { get; set; }
        //Lading Schedule
        public double CurrentLat { get; set; }
        public double CurrentLng { get; set; }
        public string Location { get; set; }
        public int? KeepingTotalPriceEmpId { get; set; }
        public int? CusDepartmentId { get; set; }
        public int? ShipmentId { get; set; }
        public bool IsCreditTransfer { get; set; }
        public bool IsRecoveryDeliveryComplete { get; set; }
        public bool IsReturn { get; set; }
        public int? ToStreetId { get; set; }
        //TPL
        public int? TPLCurrentId { get; set; }
        public int? TPLId { get; set; }
        public string TPLNumber { get; set; }
        public DateTime? TPLCreatedWhen { get; set; }
        public int? ReasonId { get; set; }
        //Change ShippingAddress
        public string DefaultShippingAddress { get; set; }
        public Boolean? IsChangeShippingAddress { get; set; }
        public int? CompanyId { get; set; }

        //
        public int? NumPick { get; set; }
        public int? NumDeliver { get; set; }
        public int? NumReturn { get; set; }
        public int? NumTransfer { get; set; }
        public int? NumTransferReturn { get; set; }
        public virtual CustomerInfoViewModel Sender { get; set; }
        public virtual WardInfoViewModel FromWard { get; set; }
        public virtual WardInfoViewModel ToWard { get; set; }
        public virtual ServiceViewModel Service { get; set; }
        public virtual StructureViewModel Structure { get; set; }

    }
}
