using Core.Data.Core.Utils;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Core.Business.ViewModels.Pages
{
    public class PageViewModel : SimpleViewModel
    {
        public PageViewModel()
        {
        }

		public int? ParentPageId { get; set; }
		public string AliasPath { get; set; }
		public int PageOrder { get; set; }
		public bool IsAccess { get; set; }
		public bool IsAdd { get; set; }
		public bool IsEdit { get; set; }
		public bool IsDelete { get; set; }
		public int? ModulePageId { get; set; }
		public string Icon { get; set; }

		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			var validator = new PageViewModelValidator(EntityUtil.GetUnitOfWork(validationContext));
			var result = validator.Validate(this);
			return result.Errors.Select(item => new ValidationResult(item.ErrorMessage, new[] { item.PropertyName }));
		}
	}
}
