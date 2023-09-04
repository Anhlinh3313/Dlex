using System;
using Core.Business.ViewModels.Validators;
using Core.Data.Abstract;
using Core.Entity.Entities;
using FluentValidation;
using FluentValidation.Results;

namespace Core.Business.ViewModels
{
    public class WeightViewModelValidator : BaseCRUDAbstractValidator<WeightViewModel, Weight>
    {
        public WeightViewModelValidator(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            var esvWeight = new EntitySimpleValidator<Formula>(unitOfWork);

            RuleFor(x => x.Code).NotEmpty().WithMessage(ValidatorMessage.Weight.CodeNotEmpty)
                .Must(esvWeight.UniqueCode).WithMessage(ValidatorMessage.Weight.UniqueCode);

            RuleFor(x => x.FormulaId)
                .NotEmpty().WithMessage(ValidatorMessage.Weight.FormulaId)
                .Must(esvWeight.Exist).WithMessage(ValidatorMessage.Formula.NoExist);

            RuleFor(x => x.WeightFrom).NotEmpty().WithMessage(ValidatorMessage.Weight.WeightFromNotEmpty);

            RuleFor(x => x.WeightTo)
                .NotEmpty().WithMessage(ValidatorMessage.Weight.WeightToNotEmpty);

            RuleFor(x => x.WeightPlus)
                .NotEmpty().WithMessage(ValidatorMessage.Weight.WeightPlusNotEmpty);
        }
    }
}
