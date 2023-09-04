using Core.Business.ViewModels.Validators;
using Core.Data.Abstract;
using Core.Entity.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels
{
    public class AreaViewModelValidator : GeneralAbstractValidator<AreaViewModel, Area>
    {
        public AreaViewModelValidator(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            var esvArea = new EntitySimpleValidator<Area>(unitOfWork);
            RuleFor(x => x.DistrictIds)
                .NotEmpty().WithMessage(ValidatorMessage.Area.DistrictIds);
        }
    }
}
