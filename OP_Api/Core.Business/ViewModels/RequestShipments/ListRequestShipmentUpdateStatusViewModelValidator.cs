using System;
using Core.Business.ViewModels.Validators;
using Core.Business.ViewModels.Validators.Properties;
using Core.Data.Abstract;
using Core.Entity.Entities;
using FluentValidation;

namespace Core.Business.ViewModels
{
    public class ListRequestShipmentUpdateStatusViewModelValidator : BaseAbstractValidator<ListRequestShipmentUpdateStatusViewModel, RequestShipment>
    {
        public ListRequestShipmentUpdateStatusViewModelValidator(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            var accountValidator = new AccountValidator(unitOfWork);
            var esvShipmentStatus = new EntitySimpleValidator<ShipmentStatus>(unitOfWork);

            RuleFor(x => x.EmpId)
                .Must(accountValidator.Exist).WithMessage(ValidatorMessage.Account.NotExist);
            RuleFor(x => x.ShipmentStatusId)
                .Must(esvShipmentStatus.Exist).WithMessage("Trạng thái không tồn tại");
            RuleFor(x => x.ShipmentIds)
                .NotNull().WithMessage("Không có vận đơn để phân nhân viên lấy hàng");
            RuleFor(x => x.ShipmentIds)
                .NotNull().WithMessage("Không có vận đơn để phân nhân viên lấy hàng");
        }
    }
}
