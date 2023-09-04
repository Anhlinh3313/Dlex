using System;
using Core.Data.Abstract;
using Core.Entity.Entities;
using Core.Infrastructure.Extensions;
using Core.Business.ViewModels.Abstract;

namespace Core.Business.ViewModels.Validators
{
    public class HubValidator : BaseValidator<Hub>
    {
        public HubValidator(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        protected override bool IsValid(FluentValidation.Validators.PropertyValidatorContext context)
        {
            return base.IsValid(context);
        }
    }
}
