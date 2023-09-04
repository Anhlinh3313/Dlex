using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Core.Data.Core.Utils;

namespace Core.Business.ViewModels.General
{
    public class HubRoutingCreateUpdateViewModel : SimpleViewModel, IValidatableObject
    {
        public HubRoutingCreateUpdateViewModel()
        {
        }

        public int HubId { get; set; }
        public int? UserId { get; set; }
        public int[] WardIds { get; set; }
        public string CodeConnect { get; set; }
        public bool? IsTruckDelivery { get; set; }
        public int[] StreetJoinIds { get; set; }
        public double? RadiusServe { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validator = new HubRoutingCreateUpdateViewModelValidator(EntityUtil.GetUnitOfWork(validationContext));
            var result = validator.Validate(this);
            return result.Errors.Select(item => new ValidationResult(item.ErrorMessage, new[] { item.PropertyName }));
        }
    }
}
