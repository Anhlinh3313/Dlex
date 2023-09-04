using System;
using Core.Business.ViewModels;
using Core.Data.Abstract;
using Core.Entity.Entities;
using Core.Infrastructure.Extensions;
using Core.Infrastructure.Security;
using Core.Infrastructure.Utils;
using FluentValidation.Validators;

namespace Core.Business.ViewModels.Validators.Properties
{
    public class AccountValidator : BaseValidator<User>
    {
        public AccountValidator(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public bool IdentityCard(string value)
        {
            return value.Length <= 12 ? true : false;
        }

        public bool UniqueUserName(string userName)
        {
            return !_unitOfWork.RepositoryR<User>().Any(x => x.UserName == userName);
        }

        public bool UniqueCode(string code)
        {
            return !_unitOfWork.RepositoryR<User>().Any(x => x.Code == code);
        }

        public bool UniqueCode(User user)
        {
            if (user.Id == 0)
                return !_unitOfWork.RepositoryR<User>().Any(x => x.Code == user.Code);
            else
                return !_unitOfWork.RepositoryR<User>().Any(x => x.Code == user.Code && x.Id != user.Id);
        }

        public bool UserEnabled(int id)
        {
            return _unitOfWork.RepositoryR<User>().Any(x => x.Id.Equals(id));
        }

        public bool UserEnabled(string userName)
        {
            return _unitOfWork.RepositoryR<User>().Any(x => x.UserName.Equals(userName));
        }

        public bool SecurityStampValid(int userId, string securityStampHash)
        {
            return _unitOfWork.RepositoryR<User>().Any(x => new Encryption().HashSHA256(x.SecurityStamp).Equals(securityStampHash));
        }
    }
}
