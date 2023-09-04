using System;
using AutoMapper;
using Core.Business.ViewModels.General;
using Core.Business.ViewModels.Shipments;
using Core.Data;
using Core.Data.Core;
using Core.Entity.Entities;
using Core.Infrastructure.Security;
using System.Collections.Generic;
using Core.Infrastructure.Helper;
using Core.Infrastructure.Utils;
using System.Linq;
using Core.Business.ViewModels.TruckSchedules;
using Core.Business.ViewModels.Discounts;
using Core.Business.ViewModels.CutOffTimes;
using Core.Business.ViewModels.TransferTimes;
using Core.Business.ViewModels.Companies;
using Core.Business.ViewModels.PromotionFormulas;
using Core.Business.ViewModels.Promotions;
using Core.Business.ViewModels.PromotionCustomers;
using Core.Business.ViewModels.PriceListSettings;
using Core.Business.ViewModels.PromotionDetails;

namespace Core.Business.ViewModels.Mappings
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
             CreateMap<User, UserInfoViewModel>().AfterMap(
                (src, dest) =>
                {
                    using (var context = new ApplicationContext())
                    {
                        UnitOfWork unitOfWork = new UnitOfWork(context);
                        dest.RoleIds = unitOfWork.RepositoryR<UserRole>().FindBy(f => f.UserId == src.Id).Select(s => s.RoleId).ToList();
                        dest.Roles = unitOfWork.RepositoryR<Role>().FindBy(f => dest.RoleIds.Contains(f.Id)).ToList();
                    }
                }
               ).ReverseMap();
            CreateMap<Branch, BranchInfoViewModel>().ReverseMap();
            CreateMap<BankAccount, BankAccountInfoViewModel>().ReverseMap();
            CreateMap<CustomerPriceListDVGT, CustomerPriceListDVGTInfoViewModel>().ReverseMap();
            CreateMap<CustomerPriceService, CustomerPriceServiceInfoViewModel>().ReverseMap();
            CreateMap<CusDepartment, CusDepartmentViewModel>().ReverseMap();
            CreateMap<Customer, CustomerViewModel>().ReverseMap();
            CreateMap<Customer, CustomerInfoViewModel>().ReverseMap();
            CreateMap<Shipment, ShipmentInfoViewModel>().AfterMap((src, dest) =>
            {
                using (var context = new ApplicationContext())
                {
                    UnitOfWork unitOfWork = new UnitOfWork(context);
                    dest.TotalShipment = unitOfWork.RepositoryR<Shipment>().Count(domainToView => domainToView.ShipmentId == src.Id && domainToView.IsBox == false);
                }
            }).ReverseMap();
            CreateMap<Shipment, ShipmentTrackingViewModel>().AfterMap((src, dest) =>
            {
                using (var context = new ApplicationContext())
                {
                    UnitOfWork unitOfWork = new UnitOfWork(context);
                    dest.TotalShipment = unitOfWork.RepositoryR<Shipment>().Count(domainToView => domainToView.ShipmentId == src.Id);
                }
            }).ReverseMap();
            CreateMap<RequestShipment, RequestShipmentInfoViewModel>()
            .AfterMap((src, dest) =>
            {
                using (var context = new ApplicationContext())
                {
                    UnitOfWork unitOfWork = new UnitOfWork(context);
                    dest.TotalShipment = unitOfWork.RepositoryR<Shipment>().Count(domainToView => domainToView.RequestShipmentId == src.Id);
                }
            }).ReverseMap();
            CreateMap<RequestShipment, RequestShipmentTrackingViewModel>().AfterMap((src, dest) =>
            {
                using (var context = new ApplicationContext())
                {
                    UnitOfWork unitOfWork = new UnitOfWork(context);
                    dest.TotalShipment = unitOfWork.RepositoryR<Shipment>().Count(domainToView => domainToView.RequestShipmentId == src.Id);
                }
            }).ReverseMap();

            CreateMap<Country, CountryViewModel>().ReverseMap();
            CreateMap<Department, DepartmentViewModel>().ReverseMap();
            CreateMap<Hub, HubInfoViewModel>().ReverseMap();
            CreateMap<Hub, HubViewModel>().ReverseMap();
            CreateMap<HubRouting, HubRoutingInfoViewModel>().ReverseMap();
            CreateMap<Province, ProvinceInfoViewModel>().ReverseMap();
            CreateMap<Province, ProvinceViewModel>().ReverseMap();
            CreateMap<District, DistrictViewModel>().ReverseMap();
            CreateMap<District, DistrictInfoViewModel>().ReverseMap();
            CreateMap<Ward, WardViewModel>().ReverseMap();
            CreateMap<Ward, WardInfoViewModel>().ReverseMap();
            CreateMap<Reason, ReasonViewModel>().ReverseMap();
            CreateMap<Area, AreaViewModel>().ReverseMap();
            CreateMap<Area, AreaInfoViewModel>().ReverseMap();
            CreateMap<AreaGroup, AreaGroupViewModel>().ReverseMap();
            CreateMap<AreaGroup, AreaGroupInfoViewModel>().ReverseMap();
            CreateMap<Weight, WeightViewModel>().ReverseMap();
            CreateMap<Weight, WeightInfoViewModel>().ReverseMap();
            CreateMap<PriceList, PriceListViewModel>().ReverseMap();
            CreateMap<PriceList, PriceListInfoViewModel>().ReverseMap();
            CreateMap<PriceService, PriceServiceInfoViewModel>().ReverseMap();
            CreateMap<ServiceDVGTPrice, ServiceDVGTPriceInfoViewModel>().ReverseMap();
            CreateMap<CustomerPriceList, CustomerPriceListInfoViewModel>().ReverseMap();
            CreateMap<Package, PackageViewModel>().ReverseMap();
            CreateMap<Size, SizeViewModel>().ReverseMap();
            CreateMap<PriceServiceDetail, PriceServiceDetailInfoViewModel>().ReverseMap();
            CreateMap<Package, PackageInfoViewModel>().ReverseMap();
            CreateMap<Box, BoxInfoViewModel>().ReverseMap();
            CreateMap<RequestShipment, ShipmentInfoViewModel>().ReverseMap();
            CreateMap<ListReceiptMoney, ListReceiptMoneyInfoViewModel>().ReverseMap();
            CreateMap<ListReceiptMoney, ListReceiptMoneyViewModel>().ReverseMap();
            CreateMap<ListReceiptMoneyViewModel, ListReceiptMoneyInfoViewModel>().ReverseMap();
            CreateMap<ListGoods, ListGoodsInfoViewModel>().ReverseMap();
            CreateMap<ShipmentVersion, ShipmentVersionInfoViewModel>().ReverseMap();
            CreateMap<ListReceiptMoneyStatus, ListReceiptMoneyStatusInfoViewModel>().ReverseMap();
            CreateMap<ListReceiptMoneyType, ListReceiptMoneyTypeInfoViewModel>().ReverseMap();
            CreateMap<ListReceiptMoneyShipment, ListReceiptMoneyShipmentInfoViewModel>().ReverseMap();
            CreateMap<ListCustomerPayment, ListCustomerPaymentInfoViewModel>().ReverseMap();
            CreateMap<ListCustomerPayment, ListCustomerPaymentViewModel>().ReverseMap();
            CreateMap<ListCustomerPaymentViewModel, ListCustomerPaymentInfoViewModel>().ReverseMap();
            CreateMap<ListCustomerPaymentType, ListCustomerPaymentTypeInfoViewModel>().ReverseMap();
            CreateMap<TPL, TPLViewModel>().ReverseMap();
            CreateMap<TPLTransit, TPLTransitInfoViewModel>().ReverseMap();
            CreateMap<TPLTransit, TPLTransitViewModel>().ReverseMap();
            CreateMap<ProvideCode, ProvideCodeViewModel>().ReverseMap();
            CreateMap<DeadlinePickupDelivery, DeadlinePickupDeliveryViewModel>().ReverseMap();
            CreateMap<DeadlinePickupDelivery, DeadlinePickupDeliveryInfoViewModel>().ReverseMap();
            CreateMap<PackagePrice, PackagePriceInfoViewModel>().ReverseMap();
            CreateMap<Complain, ComplainInfoViewModel>().ReverseMap();
            CreateMap<ComplainType, ComplainTypeInfoViewModel>().ReverseMap();
            CreateMap<ComplainHandle, ComplainHandleInfoViewModel>().ReverseMap();
            CreateMap<Incidents, IncidentsInfoViewModel>().ReverseMap();
            CreateMap<ChangeCODType, ChangeCODTypeViewModel>().ReverseMap();
            CreateMap<Compensation, CompensationInfoViewModel>().ReverseMap();
            CreateMap<AccountingAccount, AccountingAccountInfoViewModel>().ReverseMap();
            CreateMap<AccountingAccountViewModel, AccountingAccountInfoViewModel>().ReverseMap();
            CreateMap<UploadExcelHistory, UploadExcelHistoryInfoViewModel>()
                .AfterMap((src, dest) =>
                {
                    using (var context = new ApplicationContext())
                    {
                        UnitOfWork unitOfWork = new UnitOfWork(context);
                        var listShipmentId = unitOfWork.RepositoryR<Shipment>().FindBy(x131 => x131.UploadExcelHistoryId == src.Id).AsParallel().Select(s => s.ShipmentId);
                        dest.CountShipment = listShipmentId.Count();
                    }
                });
            CreateMap<Discount, DiscountViewModel>().ReverseMap();
            CreateMap<CustomerDiscount, CustomerDiscountViewModel>().ReverseMap();
            CreateMap<ListGoods, ListGoodsViewModel>()
                .AfterMap((src, dest) =>
            {
                using (var context = new ApplicationContext())
                {
                    UnitOfWork unitOfWork = new UnitOfWork(context);
                    var listShipmentId = unitOfWork.RepositoryR<ShipmentListGoods>().FindBy(x141 => x141.ListGoodsId == src.Id).AsParallel().Select(s => s.ShipmentId);
                    dest.TotalShipment = listShipmentId.Count();
                }
            });
            CreateMap<TruckSchedule, TruckScheduleInfoViewModel>().AfterMap((src, dest) =>
            {
                using (var context = new ApplicationContext())
                {
                    UnitOfWork unitOfWork = new UnitOfWork(context);
                    var listGoodIds = unitOfWork.RepositoryR<ListGoods>().FindBy(x => x.TruckScheduleId == src.Id).Select(s => s.Id).ToList();
                    if (listGoodIds.Count() > 0)
                    {
                        var listShipmentIds = unitOfWork.RepositoryR<ShipmentListGoods>().FindBy(x => listGoodIds.Contains(x.ListGoodsId)).Select(s => s.ShipmentId).ToList();
                        if (listShipmentIds.Count() > 0)
                        {
                            var listShipments = unitOfWork.RepositoryR<Shipment>().FindBy(f156 => listShipmentIds.Contains(f156.Id));
                            if (listShipments.Count() > 0)
                            {
                                dest.TotalWeight = listShipments.AsParallel().Sum(s => s.Weight);
                                dest.TotalBox = listShipments.AsParallel().Sum(s => s.TotalBox);
                            }
                        }
                    }
                    var userIds = unitOfWork.RepositoryR<TruckScheduleRider>().FindBy(f => f.TruckScheduleId == src.Id).Select(s => s.RiderId).ToList();
                    if (userIds.Count() > 0)
                    {
                        dest.Riders = unitOfWork.RepositoryR<User>().FindBy(f => userIds.Contains(f.Id)).ToArray();
                    }
                }
            }).ReverseMap();
            CreateMap<CutOffTime, CutOffTimeInfoViewModel>().ReverseMap();
            CreateMap<TransferTime, TransferTimeInfoViewModel>().ReverseMap();
            CreateMap<Company, CompaniesViewModel>().ReverseMap();
            CreateMap<HubRouting, HubRoutingInfoViewModel>();
            CreateMap<UserRelation, UserRelationViewModel>();
            CreateMap<FormPrint, FormPrintViewModel>();
            CreateMap<PromotionFormula, PromotionFormulaViewModel>();
            CreateMap<Promotion, PromotionViewModel>();
            CreateMap<PromotionCustomer, PromotionCustomerViewModel>();
            CreateMap<PriceListSetting, PriceListSettingViewModel>();
            CreateMap<CustomerInfoLog, CustomerInfoLogViewModel>();
            //CreateMap<PromotionDetail, PromotionDetailInfoViewModel>().ReverseMap();
            CreateMap<PromotionDetail, PromotionDetailInfo>()
                 .AfterMap((src, dest) =>
                 {
                     using (var context = new ApplicationContext())
                     {
                         UnitOfWork unitOfWork = new UnitOfWork(context);
                         List<PromotionDetailServiceDVGT> data = unitOfWork.RepositoryR<PromotionDetailServiceDVGT>().FindBy(x => x.PromotionDetailId == src.Id && x.IsEnabled == true).ToList();
                         dest.PromotionDetailServiceDVGTs = data;
                     }
                 }
                );
            CreateMap<PricingType, PricingTypeViewModel>();

        }
    }
}
