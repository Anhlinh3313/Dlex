using System;
using Core.Business.ViewModels.Validators;
using Core.Business.ViewModels.Validators.Properties;
using Core.Data.Abstract;
using Core.Entity.Entities;
using FluentValidation;

namespace Core.Business.ViewModels
{
    public class CreateUpdateRequestShipmentViewModelValidator : BaseAbstractValidator<CreateUpdateRequestShipmentViewModel, RequestShipment>
    {
        public CreateUpdateRequestShipmentViewModelValidator(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            var shipmentValidator = new RequestShipmentValidator(unitOfWork);
            var esvWard = new EntitySimpleValidator<Ward>(unitOfWork);
            var esvCustomer = new EntitySimpleValidator<Customer>(unitOfWork);
            //var esvPayment = new EntitySimpleValidator<Payment>(unitOfWork);
            //var esvService = new EntitySimpleValidator<Service>(unitOfWork);


            RuleFor(x => x).Must(shipmentValidator.UniqueShipmentNumber)
                           .WithMessage("Mã đơn hàng đã tồn tại")
                           .Unless(x => string.IsNullOrWhiteSpace(x.ShipmentNumber));
            //RuleFor(x => x.FromWardId).Must(esvWard.Exist).WithMessage("Phường xã lấy hàng không tồn tại");
            //When(x => x.ToWardId.HasValue, () =>
            //{
            //    RuleFor(x => x.ToWardId.Value).Must(esvWard.Exist).WithMessage("Phường xã giao hàng không tồn tại");
            //});
            //RuleFor(x => x.SenderId).Must(esvCustomer.Exist).WithMessage("Người gửi không tồn tại");
            //RuleFor(x => x.ServiceId).Must(esvService.Exist).WithMessage("Dịch vụ không tồn tại");
            //RuleFor(x => x.PaymentTypeId).Must(esvPayment.Exist).WithMessage("Hình thức thanh toán không tồn tại");
        }
    }
}
