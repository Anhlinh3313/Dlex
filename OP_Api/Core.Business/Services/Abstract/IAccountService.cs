using System;
using System.Threading.Tasks;
using Core.Business.ViewModels;

namespace Core.Business.Services.Abstract
{
    public interface IAccountService
    {
        Task<dynamic> CreateAccount(CreateAccountViewModel model);
        Task<dynamic> UpdateAccount(UpdateAccountViewModel model);
        Task<dynamic> ChangePassWord(ChangePassWordViewModel model);
        Task<dynamic> DeleteAccount(BasicViewModel model);
        dynamic GetAccountInfo(int id);
        dynamic GetAccountList();
        Task<dynamic> SignIn(SignInViewModel model);
    }
}
