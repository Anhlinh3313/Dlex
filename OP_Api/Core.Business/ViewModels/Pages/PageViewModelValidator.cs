using Core.Business.ViewModels.Validators;
using Core.Data.Abstract;
using Core.Entity.Entities;

namespace Core.Business.ViewModels.Pages
{
    public class PageViewModelValidator : GeneralAbstractValidator<PageViewModel, Page>
    {
        public PageViewModelValidator(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
