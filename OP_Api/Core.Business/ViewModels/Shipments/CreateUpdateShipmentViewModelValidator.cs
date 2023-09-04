using System;
using Core.Business.ViewModels.Validators;
using Core.Business.ViewModels.Validators.Properties;
using Core.Data.Abstract;
using Core.Entity.Entities;
using FluentValidation;

namespace Core.Business.ViewModels.Shipments
{
    public class CreateUpdateShipmentViewModelValidator : BaseAbstractValidator<CreateUpdateShipmentViewModel, Shipment>
    {
        public CreateUpdateShipmentViewModelValidator(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            var shipmentValidator = new ShipmentValidator(unitOfWork);

            //RuleFor(x => x).Must(shipmentValidator.UniqueShipmentNumber).WithMessage("Mã đơn hàng đã tồn tại").Unless(x => string.IsNullOrWhiteSpace(x.ShipmentNumber));
        }
    }
}
