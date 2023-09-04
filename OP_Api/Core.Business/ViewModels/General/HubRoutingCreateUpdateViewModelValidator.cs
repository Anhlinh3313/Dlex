using Core.Business.ViewModels.Validators;
using Core.Data.Abstract;
using Core.Entity.Entities;
using FluentValidation;

namespace Core.Business.ViewModels.General
{
	public class HubRoutingCreateUpdateViewModelValidator : GeneralAbstractValidator<HubRoutingCreateUpdateViewModel, HubRouting>
	{
		public HubRoutingCreateUpdateViewModelValidator(IUnitOfWork unitOfWork) : base(unitOfWork)
		{
			RuleFor(x => x.WardIds.Length)
				.GreaterThan(0).WithMessage(ValidatorMessage.Ward.WardListNotEmpty);
		}
	}
}

