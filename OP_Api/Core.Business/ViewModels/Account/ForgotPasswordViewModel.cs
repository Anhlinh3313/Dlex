using System.ComponentModel.DataAnnotations;

namespace Core.Business.ViewModels.Account
{
    public class ForgotPasswordViewModel
    {
        public ForgotPasswordViewModel()
        {
        }

        [EmailAddress]
        public string CompanyCode { get; set; }
        public string Email { get; set; }
    }
}
