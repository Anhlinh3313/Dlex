using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Core.Data.Core.Utils;
using Core.Entity.Abstract;

namespace Core.Business.ViewModels
{
    public class UpdateStatusViewModel : IEntityBase, IValidatableObject
    {
        public UpdateStatusViewModel()
        {
        }

        public int Id { get; set; }
        public int? EmpId { get; set; }
        public string ShipmentNumber { get; set; }
        public string ReferencesCode { get; set; }
        public int ShipmentStatusId { get; set; }
        public DateTime? appointmentTime { get; set; }
        public DateTime? EndDeliveryTime { get; set; }
        public DateTime? EndReturnTime { get; set; }
        public int? ReasonId { get; set; }
        public string RealRecipientName { get; set; }
        public double? CurrentLat { get; set; }
        public double? CurrentLng { get; set; }
        public string Location { get; set; }
        public string Note { get; set; }
        public int[] ServiceDVGTIds { get; set; }
        public int? StructureId { get; set; }
        public bool IsEnabled { get; set; } = true;
        public double CompensationValue { get; set; }
        public int? HandlingEmpId { get; set; }
        public string TPLCode { get; set; }
        public string TPLNumber { get; set; }
        public string UserCode { get; set; }
        public bool IsPushCustomer { get; set; }
        public DateTime? EndPickTime { get; set; }
        public int? PickUserId { get; set; }
        public int? CurrentHubId { get; set; }
        public int? CurrentEmpId { get; set; }
        //
        public double Weight { get; set; }
        public double Length { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double? CalWeight { get; set; }
        public double? PriceReturn { get; set; }
        public double? PriceCOD { get; set; }
        public double? DefaultPrice { get; set; }
        public double? DefaultPriceS { get; set; }
        public double? DefaultPriceP { get; set; }
        public double? RemoteAreasPrice { get; set; }
        public double? FuelPrice { get; set; }
        public double? TotalDVGT { get; set; }
        public double? OtherPrice { get; set; }
        public double? VATPrice { get; set; }
        public double? TotalPrice { get; set; }
        public double? TotalPriceSYS { get; set; }
        public bool IsPackage { get; set; }
        //
        public int? KeepingTotalPriceEmpId { get; set; }
        public int? KeepingCODEmpId { get; set; }
        //
        public int? CompanyId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validator = new UpdateStatusViewModelValidator(EntityUtil.GetUnitOfWork(validationContext));
            var result = validator.Validate(this);
            return result.Errors.Select(item => new ValidationResult(item.ErrorMessage, new[] { item.PropertyName }));
        }
    }
}
