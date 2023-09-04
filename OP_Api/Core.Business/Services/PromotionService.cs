using System;
using System.Linq;
using Core.Business.Services.Abstract;
using Core.Business.Services.Models;
using Core.Business.ViewModels.PromotionDetails;
using Core.Business.ViewModels.Promotions;
using Core.Data.Abstract;
using Core.Entity.Entities;
using Core.Infrastructure.Helper;
using Core.Infrastructure.ViewModels;
using Microsoft.Extensions.Options;

namespace Core.Business.Services
{
    public class PromotionService : GeneralService<PromotionViewModel, Promotion>, IPromotionService
    {
        public PromotionService(
            Microsoft.Extensions.Logging.ILogger<dynamic> logger,
            IOptions<AppSettings> optionsAccessor,
            IUnitOfWork unitOfWork) : base(logger, optionsAccessor, unitOfWork)
        {
        }

        public ResponseViewModel Create(PromotionViewModel viewModel)
        {
            if (string.IsNullOrEmpty(viewModel.Code) || viewModel.Code.Trim() == "")
            {
                return ResponseViewModel.CreateError("Vui lòng nhập mã khuyến mãi");
            }

            if (viewModel.FromDate.HasValue && viewModel.ToDate.HasValue && DateTime.Compare(viewModel.FromDate.Value, viewModel.ToDate.Value) > 0)
            {
                return ResponseViewModel.CreateError("Vui lòng nhập từ ngày không được lớn hơn đến ngày");
            }

            if (viewModel.PromotionDetails.Count == 0)
            {
                return ResponseViewModel.CreateError("Vui lòng nhập ít nhất 1 điều kiện khuyến mãi");
            }

            for (int i = 0; i < viewModel.PromotionDetails.Count; i++)
            {
                PromotionDetailInfoViewModel detail = viewModel.PromotionDetails[i];
                if ((!detail.PromotionFormulaId.HasValue || !detail.ValueFrom.HasValue || !detail.ValueTo.HasValue || !detail.Value.HasValue) && detail.PromotionDetailServiceDVGTs.Count == 0)
                {
                    return ResponseViewModel.CreateError("Vui lòng nhập đầy đủ thông tin điều kiện khuyến mãi");
                }

                if (detail.ValueFrom > detail.ValueTo)
                {
                    return ResponseViewModel.CreateError("Vui lòng nhập thông tin điều kiện hợp lệ");
                }
            }

            viewModel.Code = viewModel.Code.Trim();
            bool isExist = _unitOfWork.RepositoryR<Promotion>().Any(x => x.Code.ToLower() == viewModel.Code.ToLower());

            if (isExist)
            {
                return ResponseViewModel.CreateError("Mã khuyến mãi đã tồn tại");
            }

            Promotion promotion = new Promotion
            {
                Code = viewModel.Code,
                PromotionTypeId = viewModel.PromotionTypeId,
                PromotionNot = viewModel.PromotionNot,
                TotalPromotion = viewModel.TotalPromotion,
                TotalCode = viewModel.TotalCode,
                FromDate = viewModel.FromDate,
                ToDate = viewModel.ToDate,
                IsPublic = viewModel.IsPublic,
                IsHidden = viewModel.IsHidden,
            };

            _unitOfWork.RepositoryCRUD<Promotion>().Insert(promotion);
            _unitOfWork.Commit();

            foreach (var detail in viewModel.PromotionDetails)
            {
                PromotionDetail promotionDetail = new PromotionDetail
                {
                    PromotionId = promotion.Id,
                    PromotionFormulaId = detail.PromotionFormulaId,
                    ValueFrom = detail.ValueFrom,
                    ValueTo = detail.ValueTo,
                    Value = detail.Value
                };

                _unitOfWork.RepositoryCRUD<PromotionDetail>().Insert(promotionDetail);
                _unitOfWork.Commit();

                if (detail.PromotionDetailServiceDVGTs != null)
                {
                    foreach (var service in detail.PromotionDetailServiceDVGTs)
                    {
                        service.PromotionDetailId = promotionDetail.Id;
                        _unitOfWork.RepositoryCRUD<PromotionDetailServiceDVGT>().Insert(service);
                    }
                    _unitOfWork.Commit();
                }
            }

            return ResponseViewModel.CreateSuccess(viewModel);
        }

        public ResponseViewModel Update(PromotionViewModel viewModel)
        {
            Promotion promotion = _unitOfWork.RepositoryR<Promotion>().GetSingle(x => x.Id == viewModel.Id);
            if (promotion == null)
            {
                return ResponseViewModel.CreateError("Không tìm thấy mã khuyến mãi");
            }

            if (viewModel.FromDate.HasValue && viewModel.ToDate.HasValue && DateTime.Compare(viewModel.FromDate.Value, viewModel.ToDate.Value) > 0)
            {
                return ResponseViewModel.CreateError("Vui lòng nhập từ ngày không được lớn hơn đến ngày");
            }

            if (viewModel.PromotionDetails.Count == 0)
            {
                return ResponseViewModel.CreateError("Vui lòng nhập ít nhất 1 điều kiện khuyến mãi");
            }

            for (int i = 0; i < viewModel.PromotionDetails.Count; i++)
            {
                PromotionDetailInfoViewModel detail = viewModel.PromotionDetails[i];
                if ((!detail.PromotionFormulaId.HasValue|| !detail.ValueFrom.HasValue || !detail.ValueTo.HasValue || !detail.Value.HasValue) && detail.PromotionDetailServiceDVGTs.Count == 0)
                {
                    return ResponseViewModel.CreateError("Vui lòng nhập đầy đủ thông tin điều kiện khuyến mãi");
                }

                if (detail.ValueFrom > detail.ValueTo)
                {
                    return ResponseViewModel.CreateError("Vui lòng nhập thông tin điều kiện hợp lệ");
                }
            }

            promotion.Code = viewModel.Code;
            promotion.PromotionNot = viewModel.PromotionNot;
            promotion.TotalPromotion = viewModel.TotalPromotion;
            promotion.TotalCode = viewModel.TotalCode;
            promotion.FromDate = viewModel.FromDate;
            promotion.ToDate = viewModel.ToDate;
            promotion.PromotionTypeId = viewModel.PromotionTypeId;
            promotion.IsPublic = viewModel.IsPublic;
            promotion.IsHidden = viewModel.IsHidden;
            _unitOfWork.RepositoryCRUD<Promotion>().Update(promotion);
            _unitOfWork.Commit();

            var promotionDetailIds = _unitOfWork.RepositoryR<PromotionDetail>().FindBy(x => x.PromotionId == promotion.Id).Select(x => x.Id).ToList();

            _unitOfWork.RepositoryCRUD<PromotionDetail>().DeleteWhere(x => x.PromotionId == promotion.Id);
            _unitOfWork.Commit();

            _unitOfWork.RepositoryCRUD<PromotionDetailServiceDVGT>().DeleteWhere(x => promotionDetailIds.Contains(x.PromotionDetailId));
            _unitOfWork.Commit();

            foreach (var detail in viewModel.PromotionDetails)
            {
                PromotionDetail promotionDetail = new PromotionDetail
                {
                    PromotionId = promotion.Id,
                    ConcurrencyStamp = detail.ConcurrencyStamp,
                    PromotionFormulaId = detail.PromotionFormulaId,
                    ValueFrom = detail.ValueFrom,
                    ValueTo = detail.ValueTo,
                    Value = detail.Value,
                };

                _unitOfWork.RepositoryCRUD<PromotionDetail>().Insert(promotionDetail);
                _unitOfWork.Commit();

                if (detail.PromotionDetailServiceDVGTs != null)
                {
                    foreach (var service in detail.PromotionDetailServiceDVGTs)
                    {
                        service.PromotionDetailId = promotionDetail.Id;
                        _unitOfWork.RepositoryCRUD<PromotionDetailServiceDVGT>().Insert(service);
                    }
                    _unitOfWork.Commit();
                }
            }

            return ResponseViewModel.CreateSuccess(viewModel);
        }

        public ResponseViewModel Delete(int promotionId)
        {
            Promotion promotion = _unitOfWork.RepositoryR<Promotion>().GetSingle(promotionId);
            if (promotion == null)
            {
                return ResponseViewModel.CreateError("Không tìm thấy mã khuyến mãi");
            }

            //var isUsed = _unitOfWork.RepositoryR<Shipment>().Any(x => x.PromotionId == promotion.Id);
            //if (isUsed)
            //{
            //    return ResponseViewModel.CreateError("Mã đang được sử dụng");
            //}

            var promotionDetailIds = _unitOfWork.RepositoryR<PromotionDetail>().FindBy(x => x.PromotionId == promotion.Id).Select(x => x.Id);
            if (promotionDetailIds.Count() > 0)
            {
                _unitOfWork.RepositoryCRUD<PromotionDetailServiceDVGT>().DeleteWhere(x => promotionDetailIds.Contains(x.PromotionDetailId));
                _unitOfWork.Commit();

                _unitOfWork.RepositoryCRUD<PromotionDetail>().DeleteWhere(x => x.PromotionId == promotion.Id);
                _unitOfWork.Commit();

            }
            _unitOfWork.RepositoryCRUD<Promotion>().Delete(promotion.Id);
            _unitOfWork.Commit();
            return ResponseViewModel.CreateSuccess("Xoá thành công");
        }

        public ResponseViewModel GetByPromotionCode(string value, bool? isPublic)
        {
            if (value == "%") value = "";
            else value = value.Replace("%", "");
            return base.FindBy(x => x.Code.ToUpper().Contains(value.ToUpper()) && (x.IsPublic == isPublic || isPublic == null) && x.IsEnabled == true, 20, 1);

        }
    }
}
