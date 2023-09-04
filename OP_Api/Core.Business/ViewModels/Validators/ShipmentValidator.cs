using System;
using Core.Business.ViewModels.Shipments;
using Core.Data.Abstract;
using Core.Entity.Entities;
using Core.Infrastructure.Extensions;
using Core.Infrastructure.Security;
using Core.Infrastructure.Utils;
using FluentValidation.Validators;

namespace Core.Business.ViewModels.Validators.Properties
{
    public class ShipmentValidator : BaseValidator<Shipment>
    {
        public ShipmentValidator(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            bool result = true;

            return result;
        }
        
    }
}
