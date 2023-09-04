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
    public class RequestShipmentValidator : BaseValidator<RequestShipment>
    {
        public RequestShipmentValidator(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            bool result = true;

            return result;
        }

        public bool UniqueShipmentNumber(CreateUpdateRequestShipmentViewModel model)
        {
            if (model.Id == 0)
                return !_unitOfWork.RepositoryR<RequestShipment>().Any(x28 => x28.ShipmentNumber==model.ShipmentNumber.Trim());
            else
                return !_unitOfWork.RepositoryR<RequestShipment>().Any(x30 => x30.ShipmentNumber==model.ShipmentNumber.Trim() && x30.Id != model.Id);
        }
    }
}
