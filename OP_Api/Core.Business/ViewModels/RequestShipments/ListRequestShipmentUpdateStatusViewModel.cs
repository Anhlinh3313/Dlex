using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Core.Data.Core.Utils;
using Core.Entity.Abstract;

namespace Core.Business.ViewModels
{
    public class ListRequestShipmentUpdateStatusViewModel : IValidatableObject
    {
        public ListRequestShipmentUpdateStatusViewModel()
        {
        }

        public int EmpId { get; set; }
        public int ShipmentStatusId { get; set; }
        public double CurrentLat { get; set; }
        public double CurrentLng { get; set; }
        public string Location { get; set; }
        public string Note { get; set; }
        public List<int> ShipmentIds { get; set; }
        public int[] ServiceDVGTIds { get; set; }
        public int? StructureId { get; set; }
        public int? ReasonId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validator = new ListRequestShipmentUpdateStatusViewModelValidator(EntityUtil.GetUnitOfWork(validationContext));
            var result = validator.Validate(this);
            return result.Errors.Select(item => new ValidationResult(item.ErrorMessage, new[] { item.PropertyName }));
        }
    }
}
