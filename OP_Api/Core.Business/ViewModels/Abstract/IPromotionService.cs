using Core.Business.ViewModels.Promotions;
using Core.Infrastructure.ViewModels;

namespace Core.Business.Services.Abstract
{
    public interface IPromotionService
    {
        ResponseViewModel Create(PromotionViewModel viewModel);
        ResponseViewModel Update(PromotionViewModel viewModel);
        ResponseViewModel Delete(int promotionId);
        ResponseViewModel GetByPromotionCode(string value, bool? isPublic);
    }
}