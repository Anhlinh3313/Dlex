using Core.Data.Abstract;
using FluentValidation;

namespace Core.Business.ViewModels.General
{
    public class HubRouteSaveChangeViewModelValidator : AbstractValidator<HubRouteSaveChangeViewModel>
    {
        public HubRouteSaveChangeViewModelValidator(IUnitOfWork unitOfWork)
        {
            RuleFor(x => x.HubId).NotEmpty().WithMessage(ValidatorMessage.StationHub.NotEmpty);
            RuleFor(x => x.WardIds).NotEmpty().WithMessage(ValidatorMessage.Ward.WardListNotEmpty);
            RuleFor(x => x.WardIds.Length).GreaterThan(0).WithMessage(ValidatorMessage.Ward.WardListNotEmpty);
        }
    }
}
