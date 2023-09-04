using Core.Business.ViewModels.Validators;
using Core.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Core.Data.Abstract;

namespace Core.Business.ViewModels
{
    public class TPLViewModelValidation : GeneralAbstractValidator<TPLViewModel, TPL>
    {
        public TPLViewModelValidation(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            
        }
    }
}
