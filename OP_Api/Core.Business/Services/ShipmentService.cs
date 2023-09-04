using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Core.Business.Core.Utils;
using Core.Business.Services.Abstract;
using Core.Business.Services.Models;
using Core.Business.ViewModels;
using Core.Business.ViewModels.Shipments;
using Core.Data;
using Core.Data.Abstract;
using Core.Data.Core;
using Core.Entity.Entities;
using Core.Entity.Procedures;
using Core.Infrastructure.Helper;
using Core.Infrastructure.Helper.ExceptionHelper;
using Core.Infrastructure.Utils;
using Core.Infrastructure.ViewModels;
using LinqKit;
using Microsoft.Extensions.Options;

namespace Core.Business.Services
{
    public class ShipmentService : GeneralService<CreateUpdateShipmentViewModel, ShipmentInfoViewModel, Shipment>, IShipmentService
    {
        private readonly IGeneralService<CreateUpdateLadingScheduleViewModel, LadingSchedule> _iLadingScheduleService;
        private readonly IUserService _iuserService;
        private readonly IGeneralService _iGeneralServiceRaw;
        private readonly CompanyInformation _icompanyInformation;

        public ShipmentService(
            Microsoft.Extensions.Logging.ILogger<dynamic> logger,
            IOptions<AppSettings> optionsAccessor,
            IUnitOfWork unitOfWork,
            IUserService iuserService,
            IOptions<CompanyInformation> companyInformation,
            IGeneralService<CreateUpdateLadingScheduleViewModel, LadingSchedule> iLadingScheduleService,
            IGeneralService iGeneralServiceRaw) : base(logger, optionsAccessor, unitOfWork)
        {
            _iuserService = iuserService;
            _iGeneralServiceRaw = iGeneralServiceRaw;
            _iLadingScheduleService = iLadingScheduleService;
            _icompanyInformation = companyInformation.Value;
        }

        public ResponseViewModel GetByType(User user, string type, int? pageSize = null, int? pageNumber = null, string cols = null, ShipmentFilterViewModel filterViewModel = null)
        {
            try
            {
                var listHub = _iuserService.GetListHubFromUser(user);
                Expression<Func<Shipment, bool>> predicate = x => x.Id > 0;
                if (!Util.IsNull(filterViewModel))
                {
                    if (filterViewModel.IsSuccess == true)
                    {
                        if (!Util.IsNull(filterViewModel.OrderDateFrom))
                        {
                            predicate = predicate.And(x => x.EndDeliveryTime >= filterViewModel.OrderDateFrom || x.EndReturnTime >= filterViewModel.OrderDateFrom);
                        }
                        if (!Util.IsNull(filterViewModel.OrderDateTo))
                        {
                            predicate = predicate.And(x => x.EndDeliveryTime <= filterViewModel.OrderDateTo || x.EndReturnTime <= filterViewModel.OrderDateTo);
                        }
                        int[] statusComplete =
                        {
                            StatusHelper.ShipmentStatusId.DeliveryComplete,
                            StatusHelper.ShipmentStatusId.ReturnComplete
                        };
                        predicate = predicate.And(x => statusComplete.Contains(x.ShipmentStatusId));
                    }
                    else
                    {
                        if (!Util.IsNull(filterViewModel.OrderDateFrom))
                        {
                            predicate = predicate.And(x => x.OrderDate >= filterViewModel.OrderDateFrom);
                        }
                        if (!Util.IsNull(filterViewModel.OrderDateTo))
                        {
                            predicate = predicate.And(x => x.OrderDate <= filterViewModel.OrderDateTo);
                        }
                    }
                    if (!Util.IsNull(filterViewModel.HandlingEmpId))
                    {
                        predicate = predicate.And(x => x.HandlingEmpId == filterViewModel.HandlingEmpId);
                    }
                    if (!Util.IsNull(filterViewModel.ShipmentNumber))
                    {
                        predicate = predicate.And(x => x.ShipmentNumber == filterViewModel.ShipmentNumber);
                    }
                    if (!Util.IsNull(filterViewModel.ShipmentStatusIds) && filterViewModel.ShipmentStatusIds.Count() > 0)
                    {
                        predicate = predicate.And(x => filterViewModel.ShipmentStatusIds.Contains(x.ShipmentStatusId));
                    }
                    if (!Util.IsNull(filterViewModel.RequestShipmentId))
                    {
                        predicate = predicate.And(x => x.RequestShipmentId == filterViewModel.RequestShipmentId);
                    }
                    if (!Util.IsNull(filterViewModel.ShipmentNumberListSelect))
                    {
                        if (filterViewModel.ShipmentNumberListSelect.Count() > 0)
                        {
                            predicate = predicate.And(x => filterViewModel.ShipmentNumberListSelect.Contains(x.ShipmentNumber));
                        }
                    }
                    if (!Util.IsNull(filterViewModel.ShipmentNumberListUnSelect))
                    {
                        if (filterViewModel.ShipmentNumberListUnSelect.Count() > 0)
                        {
                            predicate = predicate.And(x => !filterViewModel.ShipmentNumberListUnSelect.Contains(x.ShipmentNumber));
                        }
                    }
                    if (!Util.IsNull(filterViewModel.FromProvinceId))
                    {
                        predicate = predicate.And(x => x.FromProvinceId == filterViewModel.FromProvinceId);
                    }
                    if (!Util.IsNull(filterViewModel.ToProvinceId))
                    {
                        predicate = predicate.And(x => x.ToProvinceId == filterViewModel.ToProvinceId);
                    }
                    if (!Util.IsNull(filterViewModel.SenderId))
                    {
                        predicate = predicate.And(x => x.SenderId == filterViewModel.SenderId);
                    }
                    if (!Util.IsNull(filterViewModel.WeightFrom))
                    {
                        predicate = predicate.And(x => x.Weight >= filterViewModel.WeightFrom);
                    }
                    if (!Util.IsNull(filterViewModel.WeightTo))
                    {
                        predicate = predicate.And(x => x.Weight <= filterViewModel.WeightTo);
                    }
                    if (!Util.IsNull(filterViewModel.PaymentTypeId))
                    {
                        predicate = predicate.And(x => x.PaymentTypeId == filterViewModel.PaymentTypeId);
                    }
                    if (!Util.IsNull(filterViewModel.ShipmentStatusId))
                    {
                        predicate = predicate.And(x => x.ShipmentStatusId == filterViewModel.ShipmentStatusId);
                    }
                    if (!Util.IsNull(filterViewModel.ListGoodsId))
                    {
                        predicate = predicate.And(x => x.ListGoodsId == filterViewModel.ListGoodsId);
                    }
                    if (!Util.IsNull(filterViewModel.ServiceId))
                    {
                        predicate = predicate.And(x => x.ServiceId == filterViewModel.ServiceId);
                    }
                    if (!Util.IsNull(filterViewModel.FromHubId))
                    {
                        predicate = predicate.And(x => x.FromHubId == filterViewModel.FromHubId);
                    }
                    if (!Util.IsNull(filterViewModel.ToHubId))
                    {
                        predicate = predicate.And(x => x.ToHubId == filterViewModel.ToHubId);
                    }
                    if (!Util.IsNull(filterViewModel.ReceiveHubId))
                    {
                        predicate = predicate.And(x => x.ReceiveHubId == filterViewModel.ReceiveHubId);
                    }
                    if (!Util.IsNull(filterViewModel.CurrentHubId))
                    {
                        predicate = predicate.And(x => x.CurrentHubId == filterViewModel.CurrentHubId);
                    }
                    if (filterViewModel.IsExistInfoDelivery == true)
                    {
                        predicate = predicate.And(x => Util.IsNull(x.ReceiverName) || Util.IsNull(x.ReceiverPhone) || Util.IsNull(x.ReceiverName) || !x.ServiceId.HasValue);
                    }
                    if (filterViewModel.IsExistImagePickup == true)
                    {
                        predicate = predicate.And(x => !Util.IsNull(x.PickupImagePath));
                    }
                    if (filterViewModel.DeliveryUserId.HasValue)
                    {
                        predicate = predicate.And(x => x.DeliverUserId == filterViewModel.DeliveryUserId);
                    }
                    if (filterViewModel.IsPrioritize.HasValue)
                    {
                        predicate = predicate.And(x => x.IsPrioritize == filterViewModel.IsPrioritize);
                    }
                    if (filterViewModel.IsBox.HasValue)
                    {
                        predicate = predicate.And(x => x.IsBox == filterViewModel.IsBox);
                    }
                    if (filterViewModel.CurrentEmpId.HasValue)
                    {
                        if (filterViewModel.IsGroupEmp)
                        {
                            var listUserIds = _unitOfWork.RepositoryR<UserRelation>().FindBy(f => f.UserId == filterViewModel.CurrentEmpId).Select(s => s.UserRelationId);
                            predicate = predicate.And(x => x.CurrentEmpId == filterViewModel.CurrentEmpId || listUserIds.Contains(x.CurrentEmpId.Value));
                        }
                        else
                        {
                            predicate = predicate.And(x => x.CurrentEmpId == filterViewModel.CurrentEmpId);
                        }
                    }
                    if (!Util.IsNull(filterViewModel.DeadlineType))
                    {
                        DateTime dateTime = DateTime.Now;
                        if (filterViewModel.DeadlineType == DeadlineHelper.DeadlineNormal)
                        {
                            predicate = predicate.And(x => !x.DeliveryDate.HasValue || x.DeliveryDate > dateTime.AddHours(8));
                        }
                        else if (filterViewModel.DeadlineType == DeadlineHelper.DeadlineComing)
                        {
                            predicate = predicate.And(x => x.DeliveryDate > dateTime && x.DeliveryDate < dateTime.AddHours(8));
                        }
                        else if (filterViewModel.DeadlineType == DeadlineHelper.DeadlineLate)
                        {
                            predicate = predicate.And(x => x.DeliveryDate <= dateTime);
                        }
                    }
                    if (filterViewModel.startNumDelivery.HasValue)
                    {
                        predicate = predicate.And(x => x.NumDeliver >= filterViewModel.startNumDelivery);
                        if (filterViewModel.endNumDelivery.HasValue)
                        {
                            predicate = predicate.And(x => x.NumDeliver <= filterViewModel.endNumDelivery);
                        }
                    }
                    //
                    if (filterViewModel.UploadExcelHistoryId.HasValue)
                    {
                        predicate = predicate.And(x => x.UploadExcelHistoryId == filterViewModel.UploadExcelHistoryId);
                    }
                    if (!Util.IsNull(filterViewModel.SearchText))
                    {
                        predicate = predicate
                            .And(
                            o => (
                            o.ShipmentNumber.Contains(filterViewModel.SearchText.Trim())
                            || o.SenderPhone.Contains(filterViewModel.SearchText.Trim())
                            || o.ReceiverName.Contains(filterViewModel.SearchText.Trim())
                            || o.ReceiverPhone.Contains(filterViewModel.SearchText.Trim())
                            || o.ShippingAddress.Contains(filterViewModel.SearchText.Trim())
                            || o.AddressNoteTo.Contains(filterViewModel.SearchText.Trim())
                            || o.RealRecipientName.Contains(filterViewModel.SearchText.Trim())
                            || o.Content.Contains(filterViewModel.SearchText.Trim())
                            || o.CusNote.Contains(filterViewModel.SearchText.Trim())
                            )
                        );
                    }
                }
                switch (type.ToLower())
                {
                    case ShipmentTypeHelper.WaitingToDelivery:
                        {
                            int[] statusIds = {
                                StatusHelper.ShipmentStatusId.ReadyToDelivery,
                                StatusHelper.ShipmentStatusId.StoreInWarehouseDelivery };
                            predicate = predicate.And(x => user.HubId == x.CurrentHubId.Value);
                            predicate = predicate.And(x => statusIds.Contains(x.ShipmentStatusId));
                            break;
                        }
                    case ShipmentTypeHelper.WaitingToDeliveryAndHubOther:
                        {
                            int[] statusIds = {
                                StatusHelper.ShipmentStatusId.ReadyToDelivery,
                                StatusHelper.ShipmentStatusId.StoreInWarehouseDelivery,
                                StatusHelper.ShipmentStatusId.WaitingToTransfer,
                                StatusHelper.ShipmentStatusId.StoreInWarehouseTransfer,
                                StatusHelper.ShipmentStatusId.StoreInWarehouseReturnTransfer
                            };
                            predicate = predicate.And(x => user.HubId == x.CurrentHubId.Value);
                            predicate = predicate.And(x => statusIds.Contains(x.ShipmentStatusId));
                            break;
                        }
                    case ShipmentTypeHelper.AssignDelivery:
                        {
                            int[] statusIds = { StatusHelper.ShipmentStatusId.AssignEmployeeDelivery };
                            predicate = predicate.And(x => user.HubId == x.CurrentHubId.Value);
                            predicate = predicate.And(x => statusIds.Contains(x.ShipmentStatusId));
                            break;
                        }
                    case ShipmentTypeHelper.Delivering:
                        {
                            int[] statusIds = { StatusHelper.ShipmentStatusId.Delivering };
                            predicate = predicate.And(x => user.HubId == x.CurrentHubId.Value);
                            predicate = predicate.And(x => statusIds.Contains(x.ShipmentStatusId));
                            break;
                        }
                    case ShipmentTypeHelper.DeliveryComplete:
                        {
                            int[] statusIds = { StatusHelper.ShipmentStatusId.DeliveryComplete };
                            predicate = predicate.And(x => user.HubId == x.CurrentHubId.Value);
                            predicate = predicate.And(x => statusIds.Contains(x.ShipmentStatusId));
                            break;
                        }
                    //case ShipmentTypeHelper.DeliveryCancel:
                    //    {
                    //        int[] statusIds = { StatusHelper.ShipmentStatusId.Cancel};
                    //        predicate = predicate.And(x => user.HubId == x.CurrentHubId.Value);
                    //        predicate = predicate.And(x => statusIds.Contains(x.ShipmentStatusId));
                    //        break;
                    //    }
                    case ShipmentTypeHelper.DeliveryFail:
                        {
                            int[] statusIds = { StatusHelper.ShipmentStatusId.DeliveryFail };
                            predicate = predicate.And(x => user.HubId == x.CurrentHubId.Value);
                            predicate = predicate.And(x => statusIds.Contains(x.ShipmentStatusId));
                            break;
                        }
                    case ShipmentTypeHelper.Delivery:
                        {
                            int[] statusIds = { StatusHelper.ShipmentStatusId.ReadyToDelivery, StatusHelper.ShipmentStatusId.StoreInWarehouseDelivery, StatusHelper.ShipmentStatusId.AssignEmployeeDelivery };
                            predicate = predicate.And(x => user.HubId == x.CurrentHubId.Value);
                            predicate = predicate.And(x => statusIds.Contains(x.ShipmentStatusId));
                            break;
                        }
                    case ShipmentTypeHelper.UpdateDelivery:
                        {
                            int[] statusIds = {
                                (int)StatusHelper.ShipmentStatusId.Delivering,
                                StatusHelper.ShipmentStatusId.DeliveryFail,
                                StatusHelper.ShipmentStatusId.WaitingToTransfer,
                                StatusHelper.ShipmentStatusId.ReadyToDelivery,
                                StatusHelper.ShipmentStatusId.AssignEmployeeDelivery,
                                StatusHelper.ShipmentStatusId.AssignEmployeeReturn,
                            };
                            predicate = predicate.And(x => user.HubId == x.CurrentHubId.Value);
                            predicate = predicate.And(x => statusIds.Contains(x.ShipmentStatusId));
                            break;
                        }
                    case ShipmentTypeHelper.TransferAllHub:
                        {
                            predicate = predicate.And(x => (
                                (user.HubId == x.CurrentHubId.Value && StatusHelper.GetReadyToTransitId().Contains(x.ShipmentStatusId)) ||
                                (user.HubId == x.ToHubId.Value && x.ShipmentStatusId == StatusHelper.ShipmentStatusId.ReadyToDelivery)
                            ));
                            break;
                        }
                    case ShipmentTypeHelper.Transferring:
                        {
                            int[] statusIds = { StatusHelper.ShipmentStatusId.Transferring, StatusHelper.ShipmentStatusId.AssignEmployeeTransfer, StatusHelper.ShipmentStatusId.TransferReturning, StatusHelper.ShipmentStatusId.AssignEmployeeTransferReturn };
                            predicate = predicate.And(x => user.HubId == x.CurrentHubId.Value);
                            predicate = predicate.And(x => statusIds.Contains(x.ShipmentStatusId));
                            break;
                        }
                    case ShipmentTypeHelper.UpdateTransfer:
                        {
                            int[] statusIds = {
                                StatusHelper.ShipmentStatusId.Transferring,
                                StatusHelper.ShipmentStatusId.TransferReturning,
                                StatusHelper.ShipmentStatusId.AssignEmployeeTransfer,
                                StatusHelper.ShipmentStatusId.AssignEmployeeReturn
                            };
                            // lấy ra Id của bk có trạng thái chờ trung chuyển, đang trung chuyển
                            var lgTransferIds = _unitOfWork.RepositoryR<ListGoods>().FindBy(lgTransfer =>
                                (lgTransfer.ListGoodsStatusId == ListGoodsStatusHelper.READY_TO_TRANSFER || lgTransfer.ListGoodsStatusId == ListGoodsStatusHelper.TRANSFERRING)
                                && lgTransfer.ToHubId == user.HubId
                            ).Select(lgTransfer => lgTransfer.Id).ToArray();
                            predicate = predicate.And(x => lgTransferIds.Contains(x.ListGoodsId.Value));
                            //predicate = predicate.And(x => user.HubId == x.CurrentHubId.Value);
                            predicate = predicate.And(x => statusIds.Contains(x.ShipmentStatusId));
                            // lấy ra BK chưa mở seal (nếu có)
                            var truckScheduleNotOpened = _unitOfWork.RepositoryR<TruckSchedule>().FindBy(x => x.TruckScheduleStatusId != StatusHelper.TruckScheduleStatusId.OpenedSeal).Select(x => x.Id).ToArray();
                            var lg = _unitOfWork.RepositoryR<ListGoods>().FindBy(x => x.ListGoodsTypeId == ListGoodsTypeHelper.BK_CTTT && truckScheduleNotOpened.Contains(x.TruckScheduleId.Value)).Select(x => x.Id).ToArray();
                            if (lg.Length > 0)
                            {
                                predicate = predicate.And(x => !lg.Contains(x.ListGoodsId.Value));
                            }
                            break;
                        }
                    case ShipmentTypeHelper.ReShipTransfer:
                        {
                            int[] statusIds = { (int)StatusHelper.ShipmentStatusId.ReShip, StatusHelper.ShipmentStatusId.ReturnReShip };
                            predicate = predicate.And(x => user.HubId == x.CurrentHubId.Value);
                            predicate = predicate.And(x => statusIds.Contains(x.ShipmentStatusId));
                            break;
                        }
                    case ShipmentTypeHelper.UpdateReturn:
                        {
                            int[] statusIds = { (int)StatusHelper.ShipmentStatusId.AssignEmployeeReturn, (int)StatusHelper.ShipmentStatusId.Returning, StatusHelper.ShipmentStatusId.ReturnFail };
                            predicate = predicate.And(x => user.HubId == x.CurrentHubId.Value);
                            predicate = predicate.And(x => statusIds.Contains(x.ShipmentStatusId));
                            break;
                        }
                    case ShipmentTypeHelper.PackPackage:
                        {
                            int[] statusIds =
                            {
                                StatusHelper.ShipmentStatusId.StoreInWarehouseTransfer,
                                StatusHelper.ShipmentStatusId.StoreInWarehouseReturnTransfer,
                                StatusHelper.ShipmentStatusId.WaitingToTransfer,
                            };
                            predicate = predicate.And(x => user.HubId == x.CurrentHubId.Value);
                            predicate = predicate.And(x => statusIds.Contains(x.ShipmentStatusId));
                            break;
                        }
                    case ShipmentTypeHelper.OpenPackage:
                        {
                            int[] statusIds =
                            {
                                StatusHelper.ShipmentStatusId.StoreInWarehousePickup,
                                StatusHelper.ShipmentStatusId.StoreInWarehouseReturn,
                                StatusHelper.ShipmentStatusId.StoreInWarehouseDelivery,
                                StatusHelper.ShipmentStatusId.StoreInWarehouseTransfer,
                                StatusHelper.ShipmentStatusId.StoreInWarehouseReturnTransfer,
                                StatusHelper.ShipmentStatusId.WaitingToTransfer,
                                StatusHelper.ShipmentStatusId.ReadyToDelivery,
                            };
                            predicate = predicate.And(x => user.HubId == x.CurrentHubId.Value);
                            predicate = predicate.And(x => statusIds.Contains(x.ShipmentStatusId));
                            break;
                        }
                    case ShipmentTypeHelper.AccountantConfirmMoneyFromHub:
                        {
                            int[] statusIds =
                            {
                                StatusHelper.ShipmentStatusId.HubReceivedCOD,
                            };
                            predicate = predicate.And(x => user.HubId == x.CurrentHubId.Value);
                            predicate = predicate.And(x => statusIds.Contains(x.ShipmentStatusId));
                            break;
                        }
                    case ShipmentTypeHelper.TreasurerConfirmMoneyFromAccountant:
                        {
                            int[] statusIds =
                            {
                                StatusHelper.ShipmentStatusId.AccountantReceivedCOD,
                            };
                            predicate = predicate.And(x => user.HubId == x.CurrentHubId.Value);
                            predicate = predicate.And(x => statusIds.Contains(x.ShipmentStatusId));
                            break;
                        }
                    case ShipmentTypeHelper.ParentHubConfirmMoneyFromHub:
                        {
                            int[] statusIds =
                            {
                                StatusHelper.ShipmentStatusId.HubReceivedCOD,
                                StatusHelper.ShipmentStatusId.AccountantReceivedCOD,
                                StatusHelper.ShipmentStatusId.TreasurertReceivedCOD,
                            };
                            predicate = predicate.And(x => user.HubId == x.CurrentHubId.Value);
                            predicate = predicate.And(x => statusIds.Contains(x.ShipmentStatusId));
                            break;
                        }
                    case ShipmentTypeHelper.TransferTPL:
                        {
                            int[] statusIds =
                            {
                                StatusHelper.ShipmentStatusId.ReadyToDelivery,
                                StatusHelper.ShipmentStatusId.WaitingToTransfer,
                                StatusHelper.ShipmentStatusId.StoreInWarehouseDelivery,
                                StatusHelper.ShipmentStatusId.StoreInWarehousePickup,
                                StatusHelper.ShipmentStatusId.StoreInWarehouseTransfer,
                            };
                            predicate = predicate.And(x => user.HubId == x.CurrentHubId.Value);
                            predicate = predicate.And(x => statusIds.Contains(x.ShipmentStatusId));
                            predicate = predicate.And(x => x.TPLCreatedWhen.HasValue == false && x.TPLId.HasValue == false);
                            break;
                        }
                    case ShipmentTypeHelper.Inventory:
                        {
                            int[] statusIds =
                           {
                                StatusHelper.ShipmentStatusId.ReadyToDelivery,
                                StatusHelper.ShipmentStatusId.WaitingToTransfer,
                                StatusHelper.ShipmentStatusId.StoreInWarehousePickup,
                                StatusHelper.ShipmentStatusId.StoreInWarehouseTransfer,
                                StatusHelper.ShipmentStatusId.StoreInWarehouseDelivery,
                                StatusHelper.ShipmentStatusId.StoreInWarehouseReturnTransfer,
                                StatusHelper.ShipmentStatusId.StoreInWarehouseReturn,
                            };
                            predicate = predicate.And(x => user.HubId == x.CurrentHubId.Value);
                            predicate = predicate.And(x => statusIds.Contains(x.ShipmentStatusId));
                            if (!Util.IsNull(filterViewModel))
                            {
                                if (!Util.IsNull(filterViewModel.CurrentHubDelivery))
                                {
                                    if (filterViewModel.CurrentHubDelivery == 1)
                                    {
                                        predicate = predicate.And(x => x.ToHubId == user.HubId);
                                    }
                                    else if (filterViewModel.CurrentHubDelivery == 2)
                                    {

                                        predicate = predicate.And(x => x.ToHubId != user.HubId);
                                    }
                                }
                            }
                            break;
                        }
                    case ShipmentTypeHelper.ReportSumnary:
                        {
                            int[] statusIds =
                            {
                                StatusHelper.ShipmentStatusId.NewRequest
                            };
                            predicate = predicate.And(x => !statusIds.Contains(x.ShipmentStatusId));
                            break;
                        }
                    case ShipmentTypeHelper.IsReturn:
                        {
                            predicate = predicate.And(x => x.IsReturn);
                            break;
                        }
                    case ShipmentTypeHelper.CancelReturn:
                        {
                            predicate = predicate.And(x => x.IsReturn && x.ShipmentStatusId != StatusHelper.ShipmentStatusId.DeliveryComplete && x.ShipmentStatusId != StatusHelper.ShipmentStatusId.ReturnComplete);
                            break;
                        }
                    case ShipmentTypeHelper.CODReadyRecive:
                        {
                            predicate = predicate.And(x => x.COD > 0 && x.EndPickTime.HasValue && x.IsReturn == false && x.IsCreditTransfer == false &&
                            (!x.KeepingCODEmpId.HasValue || (x.KeepingCODEmpId.HasValue && !x.ListReceiptMoneyCODId.HasValue)));
                            break;
                        }
                    case ShipmentTypeHelper.CODRecivedReadyPayment:
                        {
                            predicate = predicate.And(x => x.COD > 0 && x.KeepingCODEmpId.HasValue && x.ShipmentStatusId == StatusHelper.ShipmentStatusId.DeliveryComplete && x.IsPaidCODToCustomer == false);
                            break;
                        }
                    case ShipmentTypeHelper.WaitingHandling:
                        {
                            int[] statusIds =
                            {
                                StatusHelper.ShipmentStatusId.WaitingHandling
                            };
                            predicate = predicate.And(x => statusIds.Contains(x.ShipmentStatusId));
                            break;
                        }
                    case ShipmentTypeHelper.Incident:
                        {
                            int[] statusIds =
                            {
                                StatusHelper.ShipmentStatusId.Incident
                            };
                            predicate = predicate.And(x => statusIds.Contains(x.ShipmentStatusId));
                            break;
                        }
                    case ShipmentTypeHelper.Compensation:
                        {
                            int[] statusIds =
                            {
                                StatusHelper.ShipmentStatusId.Compensation
                            };
                            predicate = predicate.And(x => statusIds.Contains(x.ShipmentStatusId));
                            break;
                        }
                }
                var data = new ResponseViewModel();
                if (!Util.IsNull(filterViewModel))
                {
                    data = FindBy(predicate, filterViewModel.IsSortDescending.Value, pageSize, pageNumber, cols);
                }
                else
                {
                    data = FindBy(predicate, pageSize, pageNumber, cols);
                }
                return data;
            }
            catch (Exception ex)
            {
                return ResponseViewModel.CreateError(ex.Message);
            }
        }
        public ResponseViewModel GetLadingHistory(User user, string type, DateTime fromDate, DateTime toDate)
        {
            try
            {
                var listHub = _iuserService.GetListHubFromUser(user);
                var data = _unitOfWork.Repository<Proc_LadingSchedule_report>()
                                      .ExecProcedure(Proc_LadingSchedule_report.GetEntityProc(0, fromDate, toDate));

                switch (type.ToLower())
                {
                    case ShipmentTypeHelper.Delivery:
                        {
                            data = data.Where(x => StatusHelper.GetDeliveryListId().Contains(x.ShipmentStatusId) && listHub.Contains((int)x.LadingScheduleHubId));
                            break;
                        }
                    case ShipmentTypeHelper.Transfer:
                        {
                            data = data.Where(x => StatusHelper.GetTransferListId().Contains(x.ShipmentStatusId) || StatusHelper.GetReturnTransferListId().Contains(x.ShipmentStatusId) && listHub.Contains((int)x.LadingScheduleHubId));
                            break;
                        }
                    case ShipmentTypeHelper.Return:
                        {
                            data = data.Where(x => StatusHelper.GetReturnListId().Contains(x.ShipmentStatusId) && listHub.Contains((int)x.LadingScheduleHubId));
                            break;
                        }
                    case ShipmentTypeHelper.HubConfirmMoneyFromRider:
                        {
                            int[] statusIds =
                            {
                                StatusHelper.ShipmentStatusId.HubReceivedCOD,
                            };
                            data = data.Where(x => statusIds.Contains(x.ShipmentStatusId) && listHub.Contains((int)x.LadingScheduleHubId));
                            break;
                        }
                    case ShipmentTypeHelper.AccountantConfirmMoneyFromHub:
                        {
                            int[] statusIds =
                            {
                                StatusHelper.ShipmentStatusId.AccountantReceivedCOD,
                            };
                            data = data.Where(x => statusIds.Contains(x.ShipmentStatusId) && listHub.Contains((int)x.LadingScheduleHubId));
                            break;
                        }
                    case ShipmentTypeHelper.TreasurerConfirmMoneyFromAccountant:
                        {
                            int[] statusIds =
                            {
                                StatusHelper.ShipmentStatusId.TreasurertReceivedCOD,
                            };
                            data = data.Where(x => statusIds.Contains(x.ShipmentStatusId) && listHub.Contains((int)x.LadingScheduleHubId));
                            break;
                        }
                }

                return ResponseViewModel.CreateSuccess(data);
            }
            catch (Exception ex)
            {
                return ResponseViewModel.CreateError(ex.Message);
            }
        }
        public ResponseViewModel GetToPayment(int type, int customerId, int? pageSize = null, int? pageNumber = null, string cols = null)
        {
            Expression<Func<Shipment, bool>> predicate = x => x.SenderId == customerId;
            if (type == ListCustomerPaymentTypeHelper.PAYMENT_COD)
            {
                predicate = predicate.And(x => x.COD > 0 && x.IsCreditTransfer == false && !x.ListCustomerPaymentCODId.HasValue && x.ShipmentStatusId == StatusHelper.ShipmentStatusId.DeliveryComplete);
            }
            else
            {
                predicate = predicate.And(x => x.TotalPrice > 0 && !x.ListCustomerPaymentTotalPriceId.HasValue && x.PaymentTypeId == PaymentTypeHelper.THANH_TOAN_CUOI_THANG);
            }
            return this.FindBy(predicate, pageSize, pageNumber, cols);
        }
        public string GetCodeByType(int type, string prefixCode, int shipmentId, int countIdentity, int? fromProvinceId = 0)
        {
            var shipmentNumberBasic = "";
            if (type == TypeCodeHelper.Shipment.Normal)
            {
                //var randomCode = RandomUtil.GetCode(shipmentId, 7);
                var randomCode = shipmentId.ToString(_icompanyInformation.FormatShipmentCode);
                shipmentNumberBasic = $"{prefixCode}{randomCode}";
            }
            else if (type == TypeCodeHelper.Shipment.Vietstar)
            {

                if (countIdentity == 0) return null;
                var fromProvinceCode = _unitOfWork.RepositoryR<Province>().GetSingle(p => p.Id == fromProvinceId)?.Code;
                if (string.IsNullOrWhiteSpace(fromProvinceCode))
                {
                    fromProvinceCode = "00";
                }
                var randomSevenCode = countIdentity.ToString(_icompanyInformation.FormatShipmentCode);
                DateTime gDate = DateTime.Now;
                string fYearMonth = gDate.ToString("yyMM");
                var shipmentNumberTemp = $"{fromProvinceCode}{fYearMonth}{randomSevenCode}";
                // get sum 
                var sumShipmentNumber = 0;
                for (int i = 0; i < shipmentNumberTemp.Length; i++)
                {
                    int num;
                    bool isSuccess;
                    isSuccess = int.TryParse(shipmentNumberTemp[i].ToString(), out num);
                    if (isSuccess == true)
                        sumShipmentNumber += (num * (i + 1));
                }
                // check number
                var checkNumber = 10 - (sumShipmentNumber % 10);
                if (checkNumber == 10) checkNumber = 0;
                shipmentNumberBasic = $"{shipmentNumberTemp}{checkNumber}";
            }
            return shipmentNumberBasic;
        }
        public string GetBoxCode(int countBox, string shipmentNumberRef)
        {
            return $"{shipmentNumberRef}-{String.Format("{0:D2}", countBox)}";
        }
        public async Task<Shipment> CreateShipmentNoneInfo(User currentUser, string shipmentNumber)
        {
            var newShipment = new CreateUpdateShipmentViewModel();
            newShipment.ShipmentNumber = shipmentNumber;
            newShipment.FromHubId = currentUser.HubId;
            newShipment.CurrentHubId = currentUser.HubId;
            var currentHub = _unitOfWork.RepositoryR<Hub>().GetSingle(currentUser.HubId.Value, x => x.District);
            newShipment.FromProvinceId = currentHub.District.ProvinceId;
            newShipment.FromDistrictId = currentUser.Hub.DistrictId;
            newShipment.FromWardId = currentUser.Hub.WardId;
            newShipment.PickUserId = currentUser.Id;
            newShipment.EndPickTime = DateTime.Now;
            newShipment.OrderDate = DateTime.Now;
            newShipment.ShipmentStatusId = StatusHelper.ShipmentStatusId.WaitingToTransfer;
            var res = await _iGeneralServiceRaw.Create<Shipment, CreateUpdateShipmentViewModel>(newShipment);
            if (res.IsSuccess)
            {
                var shipment = res.Data as Shipment;
                // thêm hành trình loại nhập kho
                var ladingWarehouseNewShip = new LadingSchedule(
                    shipment.Id,
                    currentUser.HubId,
                    null,
                    currentUser.Id,
                    StatusHelper.ShipmentStatusId.StoreInWarehousePickup,
                    newShipment.CurrentLat,
                    newShipment.CurrentLng,
                    newShipment.Location,
                    null,
                    0
                );
                _unitOfWork.RepositoryCRUD<LadingSchedule>().Insert(ladingWarehouseNewShip);
                _unitOfWork.Commit();
                return shipment;
            }
            else
            {
                return null;
            }
        }
        public async Task<Shipment> ReCalculatePrice(int shipmentId)
        {
            var viewModel = _unitOfWork.RepositoryR<Shipment>().GetSingle(f => f.Id == shipmentId);
            if (Util.IsNull(viewModel)) return null;
            var totalShipmentBox = _unitOfWork.Repository<Proc_GetTotalShipmentBox>()
                   .ExecProcedureSingle(Proc_GetTotalShipmentBox.GetEntityProc(viewModel.Id));
            if (!Util.IsNull(totalShipmentBox) && totalShipmentBox.TotalBox > 0)
            {
                viewModel.TotalBox = totalShipmentBox.TotalBox;
                viewModel.Weight = totalShipmentBox.TotalWeight;
                viewModel.CalWeight = totalShipmentBox.TotalCalWeight;
                viewModel.CusWeight = totalShipmentBox.TotalCusWeight;
            }
            var dataCalculate = new ShipmentCalculateViewModel();
            if (viewModel.FromDistrictId.HasValue) dataCalculate.FromDistrictId = viewModel.FromDistrictId.Value;
            else return null;
            if (viewModel.ToDistrictId.HasValue) dataCalculate.ToDistrictId = viewModel.ToDistrictId.Value;
            else
            {
                if (_icompanyInformation.Name == "gsdp") dataCalculate.ToDistrictId = 723; // Lấy mặc định 1 quận huyện để tính giá
                else return null;
            }
            if (viewModel.ServiceId.HasValue) dataCalculate.ServiceId = viewModel.ServiceId.Value;
            if (!viewModel.CalWeight.HasValue) viewModel.CalWeight = 0;
            if (viewModel.Weight >= viewModel.CalWeight) dataCalculate.Weight = viewModel.Weight;
            else dataCalculate.Weight = viewModel.CalWeight.Value;
            dataCalculate.Insured = viewModel.Insured;
            dataCalculate.COD = viewModel.COD;
            dataCalculate.FromProvinceId = viewModel.FromProvinceId;
            dataCalculate.FromDistrictId = viewModel.FromDistrictId;
            dataCalculate.FromWardId = viewModel.FromWardId;
            dataCalculate.Distance = viewModel.Distance;
            dataCalculate.IsReturn = viewModel.IsReturn;
            if (viewModel.SenderId.HasValue) dataCalculate.SenderId = viewModel.SenderId.Value;
            //
            var getDVGT = _unitOfWork.RepositoryR<ShipmentServiceDVGT>().FindBy(f => f.ShipmentId == viewModel.Id);
            dataCalculate.priceDVGTs = new List<PriceDVGTViewModel>();
            if (getDVGT != null && getDVGT.Count() > 0)
            {
                foreach (var itemDVGT in getDVGT)
                {
                    if (itemDVGT.ServiceId.HasValue)
                    {
                        var shipmentDVGT = new PriceDVGTViewModel();
                        shipmentDVGT.ServiceId = itemDVGT.ServiceId;
                        shipmentDVGT.IsAgree = itemDVGT.IsAgree;
                        shipmentDVGT.TotalPrice = itemDVGT.Price;
                        dataCalculate.priceDVGTs.Add(shipmentDVGT);
                    }
                }
                dataCalculate.ServiceDVGTIds = dataCalculate.priceDVGTs.Select(s => s.ServiceId.Value).ToList();
            }
            //
            var calculate = PriceUtil.Calculate(dataCalculate, _icompanyInformation.Name, true);
            if (calculate.IsSuccess)
            {
                var price = calculate.Data as PriceViewModel;
                viewModel.DefaultPrice = price.DefaultPrice;
                viewModel.TotalDVGT = price.TotalDVGT;
                viewModel.RemoteAreasPrice = price.RemoteAreasPrice;
                viewModel.FuelPrice = price.FuelPrice;
                viewModel.OtherPrice = price.OtherPrice;
                viewModel.VATPrice = price.VATPrice;
                viewModel.TotalPrice = price.TotalPrice;
                viewModel.PriceCOD = price.PriceCOD;
                viewModel.PriceReturn = price.PriceReturn;
                viewModel.TotalPriceSYS = price.TotalPriceSYS;
            }
            //var res = await _iGeneralServiceRaw.Update<Shipment>(viewModel);

            using (var context = new ApplicationContext())
            {
                var unitOfWork = new UnitOfWork(context);
                var res = unitOfWork.Repository<Proc_UpdateShipmentAcceptReturn>()
                      .ExecProcedureSingle(Proc_UpdateShipmentAcceptReturn.GetEntityProc(viewModel.Id, viewModel.TotalBox, viewModel.Weight, viewModel.CalWeight, viewModel.CusWeight, viewModel.DefaultPrice, viewModel.TotalDVGT, viewModel.RemoteAreasPrice, viewModel.FuelPrice, viewModel.OtherPrice, viewModel.VATPrice, viewModel.TotalPrice, viewModel.PriceCOD, viewModel.PriceReturn, viewModel.TotalPriceSYS));

                return viewModel;
            }
        }
        public Proc_GetInfoHubRouting GetInfoRouting(bool? isTruckDelivery, int? districtId, int? wardId, double? weight)
        {
            using (var context = new ApplicationContext())
            {
                var unitOfWork = new UnitOfWork(context);
                var data = unitOfWork.Repository<Proc_GetInfoHubRouting>()
                      .ExecProcedureSingle(Proc_GetInfoHubRouting.GetEntityProc(isTruckDelivery, districtId, wardId, weight));
                return data;
            }
        }
    }
}
