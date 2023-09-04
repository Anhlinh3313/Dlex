using System;
using Core.Business.ViewModels.Validators;
using Core.Business.ViewModels.Validators.Properties;
using Core.Data.Abstract;
using Core.Entity.Entities;
using FluentValidation;

namespace Core.Business.ViewModels
{
    public class UpdateStatusViewModelValidator : BaseAbstractValidator<UpdateStatusViewModel, Shipment>
    {
        public UpdateStatusViewModelValidator(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
}
