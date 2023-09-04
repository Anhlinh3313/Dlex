using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using AutoMapper;
using Core.Business.Core.Extensions;
using Core.Business.ViewModels;
using Core.Business.ViewModels.ChangeCODS;
using Core.Business.ViewModels.Discounts;
using Core.Business.ViewModels.Shipments;
using Core.Business.ViewModels.TruckSchedules;
using Core.Data;
using Core.Data.Core;
using Core.Entity.Abstract;
using Core.Entity.Entities;
using Core.Infrastructure.Extensions;
using Core.Infrastructure.Helper;
using Core.Infrastructure.Http;
using Core.Infrastructure.Security;
using Core.Infrastructure.Utils;
using Microsoft.EntityFrameworkCore;
using Core.Business.ViewModels.CutOffTimes;
using Core.Business.ViewModels.TransferTimes;
using Core.Business.ViewModels.Roles;
using Core.Business.ViewModels.Pages;
using Core.Business.ViewModels.Companies;
using Core.Business.ViewModels.General;
using Core.Business.ViewModels.PromotionFormulas;
using Core.Business.ViewModels.Promotions;
using Core.Business.ViewModels.PromotionCustomers;
using Core.Business.ViewModels.PriceListSettings;

namespace Core.Business.ViewModels.Mappings
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            CreateMap<CreateAccountViewModel, User>()
                  .ForMember(dest => dest.NormalizedUserName, opts => opts.MapFrom(src => src.UserName.ToUpper()))
                  .ForMember(dest => dest.NormalizedEmail, opts => opts.MapFrom(src => (!string.IsNullOrWhiteSpace(src.Email)) ? src.Email.ToUpper() : null))
                  .AfterMap((src, dest) =>
                  {
                      dest.PasswordHash = new Encryption().EncryptPassword(src.PassWord + src.TypeUserId + src.CompanyId, dest.SecurityStamp);
                  }).ReverseMap();
            CreateMap<UpdateAccountViewModel, User>()
                .ForMember(dest => dest.NormalizedEmail, opts => opts.MapFrom(src => (!string.IsNullOrWhiteSpace(src.Email)) ? src.Email.ToUpper() : null))
                .AfterMap((src, dest) =>
                {
                    if (!Util.IsNull(src.PassWord))
                    {
                        dest.SecurityStamp = Guid.NewGuid().ToString();
                        dest.PasswordHash = new Encryption().EncryptPassword(src.PassWord + dest.TypeUserId + dest.CompanyId, dest.SecurityStamp);
                    }
                }).ReverseMap();
            CreateMap<ChangePassWordViewModel, User>()
                .AfterMap((src, dest) =>
                {
                    if (!Util.IsNull(src.NewPassWord))
                    {
                        dest.SecurityStamp = Guid.NewGuid().ToString();
                        dest.PasswordHash = new Encryption().EncryptPassword(src.NewPassWord + dest.TypeUserId + dest.CompanyId, dest.SecurityStamp);
                    }
                }).ReverseMap();
            CreateMap<CustomerViewModel, Customer>()
                .ForMember(dest => dest.UserName, opts => opts.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Email, opts => opts.MapFrom(src => src.Email))
                .AfterMap((src, dest) =>
                {
                    if (src.Id == 0)
                    {
                        dest.SecurityStamp = Guid.NewGuid().ToString();
                        dest.CompanyId = HttpContext.CurrentCompanyId;
                        dest.PasswordHash = new Encryption().EncryptPassword(src.PassWord + src.CustomerTypeId + src.CompanyId, dest.SecurityStamp);
                    } else
                    {
                        if (!Util.IsNull(src.PassWord))
                        {
                            dest.SecurityStamp = Guid.NewGuid().ToString();
                            dest.CompanyId = HttpContext.CurrentCompanyId;
                            dest.PasswordHash = new Encryption().EncryptPassword(src.PassWord + dest.CustomerTypeId + dest.CompanyId, dest.SecurityStamp);
                        }
                    }
                }).ReverseMap();
            CreateMap<HubRoutingCreateUpdateViewModel, HubRouting>()
                .AfterMap((src, dest) => 
                { 
                    if (src.Id > 0) 
                        SetGeneralColsUpdate(dest);
                    else SetGeneralColsCreate(dest); 
                }).ReverseMap();
            CreateMap<CreateUpdateShipmentViewModel, Shipment>()
                .ForMember(dest => dest.OrderDate, opts => opts.MapFrom(source => !Util.IsNull(source.OrderDate) ? source.OrderDate : DateTime.Now))
                .AfterMap((src, dest) =>
                {
                    if (src.Id == 0)
                    {
                        var user = GetCurrentUser();
                        //if (src.ShipmentStatusId == StatusHelper.ShipmentStatusId.AssignEmployeePickup && !dest.FirstPickupTime.HasValue) dest.FirstPickupTime = DateTime.Now;
                        dest.CurrentHubId = src.FromHubId;
                        dest.CreatedHubId = user.HubId;
                    }
                }).ReverseMap();
            CreateMap<CreateUpdateRequestShipmentViewModel, RequestShipment>()
                .ForMember(dest => dest.STRServiceDVGTIds, opts => opts.MapFrom(source => source.ServiceDVGTIds.Count() > 0 ? string.Join(",", source.ServiceDVGTIds) : ""))
                .AfterMap((src, dest) =>
                {
                    if (src.Id == 0) 
                    {
                        var user = GetCurrentUser();
                        //if (src.ShipmentStatusId == StatusHelper.ShipmentStatusId.AssignEmployeePickup && !dest.FirstPickupTime.HasValue) dest.FirstPickupTime = DateTime.Now;
                        dest.CurrentHubId = src.FromHubId;
                        dest.CreatedHubId = user.HubId;
                    }

                    if (!dest.OrderDate.HasValue)
                    {
                        dest.OrderDate = DateTime.Now;
                    }

                    using (var context = new ApplicationContext())
                    {
                        UnitOfWork unitOfWork = new UnitOfWork(context);
                        var hubRoutingWards = GetHubFromWard(unitOfWork, src.FromWardId, src.ToWardId);
                        if (!Util.IsNull(hubRoutingWards))
                        {
                            foreach (var item in hubRoutingWards)
                            {
                                if (item.WardId == src.FromWardId)
                                {
                                    //dest.FromHubId = item.HubRouting.HubId;
                                    dest.FromHubRoutingId = item.HubRouting.Id;
                                }

                                if (item.WardId == src.ToWardId)
                                {
                                    //dest.ToHubId = item.HubRouting.HubId;
                                    dest.ToHubRoutingId = item.HubRouting.Id;
                                }
                            }
                        }
                    }
                }).ReverseMap();

            CreateMap<RoleViewModel, Role>().AfterMap((src, dest) => { if (src.Id > 0) SetGeneralColsUpdate(dest); else SetGeneralColsCreate(dest); }).ReverseMap();
            CreateMap<PageViewModel, Page>().AfterMap((src, dest) => { if (src.Id > 0) SetGeneralColsUpdate(dest); else SetGeneralColsCreate(dest); }).ReverseMap();
            CreateMap<CompaniesViewModel, Company>().AfterMap((src, dest) => { if (src.Id > 0) SetGeneralColsUpdate(dest); else SetGeneralColsCreate(dest); }).ReverseMap();

            CreateMap<RequestShipment, Shipment>()
                .ForMember(dest => dest.Note, opts => opts.Ignore())
                .ReverseMap();

            CreateMap<UpdateStatusViewModel, Shipment>()
                .ForMember(dest => dest.Note, opts => opts.Ignore())
                .AfterMap((src, dest) =>
                {
                    DateTime date = DateTime.Now;
                    var currentUser = GetCurrentUser();

                    if (StatusHelper.GetPickupListId().Contains(dest.ShipmentStatusId))
                    {
                        if (dest.ShipmentStatusId == (int)StatusHelper.ShipmentStatusId.PickupComplete ||
                           dest.ShipmentStatusId == (int)StatusHelper.ShipmentStatusId.PickupCancel ||
                           dest.ShipmentStatusId == (int)StatusHelper.ShipmentStatusId.PickupLostPackage)
                        {
                            dest.EndPickTime = date;
                        }

                        if (dest.ShipmentStatusId == (int)StatusHelper.ShipmentStatusId.PickupComplete &&
                            dest.PaymentTypeId == PaymentTypeHelper.NGUOI_GUI_THANH_TOAN)
                        {
                            dest.KeepingTotalPriceEmpId = currentUser.Id;
                            //dest.KeepingTotalPriceHubId = currentUser.HubId;
                        }
                    }
                    else if (StatusHelper.GetDeliveryListId().Contains(dest.ShipmentStatusId))
                    {
                        if (dest.ShipmentStatusId == (int)StatusHelper.ShipmentStatusId.DeliveryComplete ||
                            dest.ShipmentStatusId == (int)StatusHelper.ShipmentStatusId.DeliveryLostPackage)
                        {
                            dest.EndDeliveryTime = date;
                            if (dest.PaymentTargetCOD.HasValue && dest.COD > 0) dest.PaymentTargetCODConversion = 24 - date.Hour + dest.PaymentTargetCOD;
                        }

                        if (dest.ShipmentStatusId == (int)StatusHelper.ShipmentStatusId.DeliveryComplete)
                        {
                            dest.DeliverUserId = currentUser.Id;
                            if (dest.PaymentTypeId == PaymentTypeHelper.NGUOI_NHAN_THANH_TOAN)
                            {
                                dest.KeepingTotalPriceEmpId = currentUser.Id;
                                //dest.KeepingTotalPriceHubId = currentUser.HubId;
                            }

                            if (dest.COD > 0)
                                dest.KeepingCODEmpId = currentUser.Id;
                        }

                        if (dest.ShipmentStatusId == (int)StatusHelper.ShipmentStatusId.DeliveryFail)
                        {
                            dest.NumDeliver += 1;
                            if (dest.NumDeliver == 1) dest.FirstDeliveryTime = DateTime.Now;
                        }

                        dest.DeliveryNote = !string.IsNullOrEmpty(dest.DeliveryNote) ? $"{dest.DeliveryNote}, {src.Note}" : src.Note;
                    }
                    else if (StatusHelper.GetReturnListId().Contains(dest.ShipmentStatusId))
                    {
                        if (dest.ShipmentStatusId == (int)StatusHelper.ShipmentStatusId.ReturnComplete ||
                            dest.ShipmentStatusId == (int)StatusHelper.ShipmentStatusId.ReturnLostPackage)
                        {
                            dest.EndReturnTime = date;
                        }
                        dest.ReturnNote = !string.IsNullOrEmpty(dest.ReturnNote) ? $"{dest.ReturnNote}, {src.Note}" : src.Note;
                    }
                    //SetGeneralColsUpdate(dest);
                }).ReverseMap();
            CreateMap<UpdateStatusViewModel, RequestShipment>()
                .ForMember(dest => dest.Note, opts => opts.Ignore())
                .ForMember(dest => dest.STRServiceDVGTIds, opts => opts.MapFrom(source => source.ServiceDVGTIds.Count() > 0 ? string.Join(",", source.ServiceDVGTIds) : ""))
                .AfterMap((src, dest) =>
                {
                    if (StatusHelper.GetPickupListId().Contains(dest.ShipmentStatusId))
                    {
                        if (!dest.StartPickTime.HasValue && dest.ShipmentStatusId == (int)StatusHelper.ShipmentStatusId.Picking)
                        {
                            dest.StartPickTime = DateTime.Now;
                        }
                        //if (dest.ShipmentStatusId == (int)StatusHelper.ShipmentStatusId.PickupFail)
                        //{
                        //    dest.NumPick += 1;
                        //    dest.PickupAppointmentTime = src.appointmentTime.HasValue ? src.appointmentTime : dest.PickupAppointmentTime;
                        //}
                        //dest.PickupNote = !string.IsNullOrEmpty(dest.PickupNote) ? $"{dest.PickupNote}, {src.Note}" : src.Note;
                    }

                    dest.Note = !string.IsNullOrEmpty(dest.Note) ? $"{dest.Note}, {src.Note}" : src.Note;
                    //SetGeneralColsUpdate(dest);
                }).ReverseMap();

            CreateMap<CreateUpdateLadingScheduleViewModel, LadingSchedule>().ReverseMap();
            CreateMap<CreateUpdateLadingScheduleViewModel, RequestLadingSchedule>().ReverseMap();
            CreateMap<ReasonViewModel, Reason>().ReverseMap();
            CreateMap<ShipmentStatusViewModel, ShipmentStatus>().ReverseMap();
            CreateMap<AreaGroupViewModel, AreaGroup>().ReverseMap();
            CreateMap<AreaViewModel, Area>().ReverseMap();
            CreateMap<FormulaViewModel, Formula>().ReverseMap();
            CreateMap<WeightViewModel, Weight>().ReverseMap();
            CreateMap<PriceListViewModel, PriceList>().ReverseMap();
            CreateMap<PriceServiceViewModel, PriceService>().ReverseMap();
            CreateMap<CustomerPriceListViewModel, CustomerPriceList>().ReverseMap();
            CreateMap<WeightGroupViewModel, WeightGroup>().ReverseMap();
            CreateMap<PriceServiceDetailViewModel, PriceServiceDetail>().ReverseMap();
            CreateMap<ServiceViewModel, Service>().ReverseMap();
            CreateMap<ServiceDVGTViewModel, ServiceDVGT>().ReverseMap();
            CreateMap<PackTypeViewModel, PackType>().ReverseMap();
            CreateMap<PaymentTypeViewModel, PaymentType>().ReverseMap();
            CreateMap<StructureViewModel, Structure>().ReverseMap();
            CreateMap<ServiceDVGTPriceViewModel, ServiceDVGTPrice>().ReverseMap();
            CreateMap<BoxViewModel, Box>().ReverseMap();
            CreateMap<LadingSchedule, LadingScheduleViewModel>().ReverseMap();
            CreateMap<RequestLadingSchedule, LadingScheduleViewModel>().ReverseMap();
            CreateMap<RequestLadingSchedule, LadingSchedule>().ReverseMap();
            CreateMap<PackageViewModel, Package>().AfterMap((src, dest) => { dest.CreatedHubId = GetCurrentUser().HubId; }).ReverseMap();
            CreateMap<ChargedCODViewModel, ChargedCOD>().ReverseMap();
            CreateMap<ChargedRemoteViewModel, ChargedRemote>().ReverseMap();
            CreateMap<ListGoodsCreateUpdateViewModel, ListGoods>().ReverseMap();
            CreateMap<TPLViewModel, TPL>().ReverseMap();
            CreateMap<TPLTransitViewModel, TPLTransit>().ReverseMap();
            CreateMap<ProvideCodeStatusViewModel, ProvideCodeStatus>().ReverseMap();
            CreateMap<ProvideCodeViewModel, ProvideCode>().ReverseMap();
            CreateMap<DeadlinePickupDeliveryViewModel, DeadlinePickupDelivery>().ReverseMap();
            CreateMap<DeadlinePickupDeliveryDetailViewModel, DeadlinePickupDeliveryDetail>().ReverseMap();
            CreateMap<ListGoodsStatusViewModel, ListGoodsStatus>().ReverseMap();
            CreateMap<ListGoodsTypeViewModel, ListGoodsType>().ReverseMap();
            CreateMap<PriceListDVGTViewModel, PriceListDVGT>().ReverseMap();
            CreateMap<PackagePriceViewModel, PackagePrice>().ReverseMap();
            CreateMap<CalculateByViewModel, CalculateBy>().ReverseMap();
            CreateMap<CreateUpdateShipmentViewModel, ShipmentVersion>()
                .ForMember(dest => dest.STRServiceDVGTIds, opts => opts.MapFrom(source => source.ServiceDVGTIds.Count() > 0 ? string.Join(",", source.ServiceDVGTIds) : ""))
                .AfterMap(
                    (src, dest) =>
            {
                using (var context = new ApplicationContext())
                {
                    UnitOfWork unitOfWork = new UnitOfWork(context);
                    dest.Id = 0;
                    dest.ShipmentId = src.Id;
                    dest.Version = $"VS{dest.ShipmentNumber}{RandomUtil.GetCode(unitOfWork.RepositoryR<ShipmentVersion>().Count(x => x.ShipmentId == dest.ShipmentId) + 1, 2)}";
                }
            });
            CreateMap<ShipmentInfoViewModel, ShipmentInfoMapping>()
                .AfterMap((src, dest) =>
                { }).ReverseMap();
            CreateMap<TruckScheduleViewModel, TruckSchedule>().ReverseMap();
            CreateMap<TruckScheduleDetailViewModel, TruckScheduleDetail>().ReverseMap();
            CreateMap<RemotePriceViewModel, RemotePrice>().ReverseMap();
            CreateMap<CustomerPriceServiceViewModel, CustomerPriceService>().ReverseMap();
            CreateMap<TransportTypeViewModel, TransportType>().ReverseMap();
            CreateMap<CustomerPriceListDVGTViewModel, CustomerPriceListDVGT>().ReverseMap();
            CreateMap<PaymentCODTypeViewModel, PaymentCODType>().ReverseMap();
            CreateMap<FormPrintViewModel, FormPrint>().ReverseMap();
            CreateMap<PromotionFormulaViewModel, PromotionFormula>().ReverseMap();
            CreateMap<HolidayViewModel, Holiday>().ReverseMap();
            CreateMap<ComplainTypeViewModel, ComplainType>().ReverseMap();
            CreateMap<ComplainViewModel, Complain>().ReverseMap();
            CreateMap<ComplainHandleViewModel, ComplainHandle>().ReverseMap();
            CreateMap<AddIncidentsViewModel, Incidents>().ReverseMap();
            CreateMap<CompensationViewModel, Compensation>().ReverseMap();
            CreateMap<BankAccountViewModel, BankAccount>().ReverseMap();
            CreateMap<ChangeCODTypeViewModel, ChangeCODType>().ReverseMap();
            CreateMap<ChangeCODViewModel, ChangeCOD>().ReverseMap();
            CreateMap<CustomerDiscountViewModel, CustomerDiscount>().ReverseMap();
            CreateMap<DiscountViewModel, Discount>().ReverseMap();
            CreateMap<CutOffTimeViewModel, CutOffTime>().ReverseMap();
            CreateMap<TransferTimeViewModel, TransferTime>().ReverseMap();
            CreateMap<TransferTimeViewModel, TransferTime>().ReverseMap();
            CreateMap<TransferTimeViewModel, TransferTime>().ReverseMap();
            CreateMap<PromotionViewModel, Promotion>().ReverseMap();
            CreateMap<PromotionCustomerViewModel, PromotionCustomer>().ReverseMap();
            CreateMap<PriceListSettingViewModel, PriceListSetting>().ReverseMap();
            CreateMap<CustomerInfoLogViewModel, CustomerInfoLog>().ReverseMap();
            CreateMap<PriceServiceDetailInfoViewModel, PriceServiceDetail>().ReverseMap();
            CreateMap<PricingTypeViewModel, PricingType>().ReverseMap();
        }

        public void SetGeneralColsCreate(IEntityBasic data)
        {
            var currentDate = DateTime.Now;
            var currentUserId = HttpContext.CurrentUserId;

            data.Id = 0;
            data.ConcurrencyStamp = Guid.NewGuid().ToString();
            data.CreatedWhen = currentDate;
            data.CreatedBy = currentUserId;
            data.ModifiedWhen = currentDate;
            data.ModifiedBy = currentUserId;
        }

        public void SetGeneralColsUpdate(IEntityBasic data)
        {
            data.ConcurrencyStamp = Guid.NewGuid().ToString();
            data.ModifiedWhen = DateTime.Now;
            data.ModifiedBy = HttpContext.CurrentUserId;
        }

        public IEnumerable<HubRoutingWard> GetHubFromWard(UnitOfWork unitOfWork, int? fromWardId = null, int? toWardId = null)
        {
            if (Util.IsNull(fromWardId) || Util.IsNull(toWardId))
            {
                return null;
            }
            return unitOfWork.RepositoryR<HubRoutingWard>()
                             .FindBy(x => x.WardId == fromWardId.Value || x.WardId == toWardId.Value)
                             .Include("HubRouting");
        }

        public int GetCurrentUserId()
        {
            System.Security.Claims.ClaimsPrincipal currentUser = HttpContext.Current.User;
            var nameIdentifier = currentUser.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            return nameIdentifier.Value.ToSafeInt();
        }

        public User GetCurrentUser()
        {
            using (var context = new ApplicationContext())
            {
                UnitOfWork unitOfWork = new UnitOfWork(context);
                var currentUserId = GetCurrentUserId();
                return unitOfWork.RepositoryR<User>().GetSingle(currentUserId, x => x.Hub);
            }
        }

        //public string GetRequestHeaders(string key)
        //{
        //    var header = HttpContext.Current.Request.Headers[key];
        //    var headerName = (!String.IsNullOrEmpty(header)) ? header.ToString() : "";
        //    return headerName;
        //}
    }
}
