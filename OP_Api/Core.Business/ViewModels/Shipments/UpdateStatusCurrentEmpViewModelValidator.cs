using System;
using Core.Business.ViewModels.Validators;
using Core.Data.Abstract;
using Core.Entity.Entities;

namespace Core.Business.ViewModels
{
    public class UpdateStatusCurrentEmpViewModelValidator : BaseAbstractValidator<UpdateStatusCurrentEmpViewModel, RequestShipment>
    {
        public UpdateStatusCurrentEmpViewModelValidator(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
