using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Core.Data.Core.Utils;

namespace Core.Business.ViewModels.General
{
	public class HubRouteSaveChangeViewModel : IValidatableObject
	{
		public HubRouteSaveChangeViewModel()
		{
		}

		public int HubId { get; set; }
		public int[] WardIds { get; set; }

		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			var validator = new HubRouteSaveChangeViewModelValidator(EntityUtil.GetUnitOfWork(validationContext));
			var result = validator.Validate(this);
			return result.Errors.Select(item => new ValidationResult(item.ErrorMessage, new[] { item.PropertyName }));
		}
	}
}