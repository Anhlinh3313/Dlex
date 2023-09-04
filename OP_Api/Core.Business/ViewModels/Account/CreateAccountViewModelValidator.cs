using Core.Business.ViewModels.Validators;
using Core.Business.ViewModels.Validators.Properties;
using Core.Data.Abstract;
using Core.Entity.Entities;
using FluentValidation;

namespace Core.Business.ViewModels.Accounts
{
    public class CreateAccountViewModelValidator : BaseAbstractValidator<CreateAccountViewModel, User>
    {
        public CreateAccountViewModelValidator(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            AccountValidator accountValidator = new AccountValidator(unitOfWork);
            var roleValidator = new EntitySimpleValidator<Role>(unitOfWork);
            var departmentValidator = new EntitySimpleValidator<Department>(unitOfWork);
            var hubValidator = new EntitySimpleValidator<Hub>(unitOfWork);

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage(ValidatorMessage.Account.UserNameNotEmpty)
                .Must(accountValidator.UniqueUserName).WithMessage(ValidatorMessage.Account.UniqueUserName);
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage(ValidatorMessage.Account.CodeNotEmpty)
                .Must(accountValidator.UniqueCode).WithMessage(ValidatorMessage.Account.UniqueCode);
            RuleFor(x => x.FullName).NotEmpty().WithMessage(ValidatorMessage.Account.FullNameNotEmpty);
            RuleFor(x => x.PassWord).NotEmpty().WithMessage(ValidatorMessage.Account.PassWordNotEmpty);
            RuleFor(x => x.Email)
                .EmailAddress().WithMessage(ValidatorMessage.Account.EmailInvalid)
                .Unless(x => string.IsNullOrEmpty(x.Email));
            RuleFor(x => x.IdentityCard)
                .Must(accountValidator.IdentityCard).WithMessage(ValidatorMessage.Account.IdentityCardInvalid)
                .Unless(x => string.IsNullOrEmpty(x.IdentityCard));
        }
    }
}
