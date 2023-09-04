using System;
using System.Linq;
using System.Linq.Expressions;
using Core.Business.Services.Abstract;
using Core.Business.Services.Models;
using Core.Business.ViewModels;
using Core.Business.ViewModels.Shipments;
using Core.Data.Abstract;
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
    public class RequestShipmentService : GeneralService<CreateUpdateRequestShipmentViewModel, RequestShipmentInfoViewModel, RequestShipment>, IRequestShipmentService
    {
        private readonly IUserService _iuserService;

        public RequestShipmentService(
            Microsoft.Extensions.Logging.ILogger<dynamic> logger, 
            IOptions<AppSettings> optionsAccessor, 
            IUnitOfWork unitOfWork,
            IUserService iuserService) : base(logger, optionsAccessor, unitOfWork)
        {
            _iuserService = iuserService;
        }

        public ResponseViewModel GetByType(User user, string type, int? pageSize = null, int? pageNumber = null, string cols = null)
        {
            try
            {
                var listHub = _iuserService.GetListHubFromUser(user);

                Expression<Func<RequestShipment, bool>> predicate = pbtx => pbtx.Id > 0;

                switch (type.ToLower())
                {
                    case ShipmentTypeHelper.WaitingToPickup:
                        {
                            int[] statusIds = { StatusHelper.ShipmentStatusId.NewRequest, StatusHelper.ShipmentStatusId.ReadyToPick, StatusHelper.ShipmentStatusId.RejectPickup, StatusHelper.ShipmentStatusId.PickupFail };
                            predicate = predicate.And(pbtx => pbtx.FromHubId.HasValue && listHub.Contains(pbtx.FromHubId.Value));
                            predicate = predicate.And(pbtx => statusIds.Contains(pbtx.ShipmentStatusId));
                            break;
                        }
                    case ShipmentTypeHelper.AssignPickup:
                        {
                            int[] statusIds = { StatusHelper.ShipmentStatusId.AssignEmployeePickup };
                            predicate = predicate.And(pbtx => pbtx.FromHubId.HasValue && listHub.Contains(pbtx.FromHubId.Value));
                            predicate = predicate.And(pbtx => statusIds.Contains(pbtx.ShipmentStatusId));
                            break;
                        }
                    case ShipmentTypeHelper.Picking:
                        {
                            int[] statusIds = { StatusHelper.ShipmentStatusId.Picking };
                            predicate = predicate.And(pbtx => pbtx.FromHubId.HasValue && listHub.Contains(pbtx.FromHubId.Value));
                            predicate = predicate.And(pbtx => statusIds.Contains(pbtx.ShipmentStatusId));
                            break;
                    }
                    case ShipmentTypeHelper.PickupComplete:
                        {
                            int[] statusIds = { StatusHelper.ShipmentStatusId.PickupComplete };
                            predicate = predicate.And(pbtx => pbtx.FromHubId.HasValue && listHub.Contains(pbtx.FromHubId.Value));
                            predicate = predicate.And(pbtx => statusIds.Contains(pbtx.ShipmentStatusId));
                            break;
                    }
                    case ShipmentTypeHelper.PickupCancel:
                        {
                            int[] statusIds = { StatusHelper.ShipmentStatusId.PickupCancel };
                            predicate = predicate.And(pbtx => pbtx.FromHubId.HasValue && listHub.Contains(pbtx.FromHubId.Value));
                            predicate = predicate.And(pbtx => statusIds.Contains(pbtx.ShipmentStatusId));
                            break;
                        }
                    case ShipmentTypeHelper.Pickup:
                        {
                            int[] statusIds = { StatusHelper.ShipmentStatusId.NewRequest, StatusHelper.ShipmentStatusId.ReadyToPick, StatusHelper.ShipmentStatusId.RejectPickup, StatusHelper.ShipmentStatusId.PickupFail, StatusHelper.ShipmentStatusId.AssignEmployeePickup };
                            predicate = predicate.And(pbtx => pbtx.FromHubId.HasValue && listHub.Contains(pbtx.FromHubId.Value));
                            predicate = predicate.And(pbtx => statusIds.Contains(pbtx.ShipmentStatusId));
                            break;
                        }
                    case ShipmentTypeHelper.UpdatePickup:
                        {
                            int[] statusIds = { StatusHelper.ShipmentStatusId.Picking, StatusHelper.ShipmentStatusId.PickupComplete };
                            predicate = predicate.And(pbtx => pbtx.FromHubId.HasValue && listHub.Contains(pbtx.FromHubId.Value));
                            predicate = predicate.And(pbtx => statusIds.Contains(pbtx.ShipmentStatusId));
                            break;
                        }
                }
                return FindBy(predicate, pageSize, pageNumber, cols);
            }
            catch (Exception ex)
            {
                return ResponseViewModel.CreateError(ex.Message);
            }
        }

        public int GetCountByType(User user, string type)
        {
            try
            {
                var listHub = _iuserService.GetListHubFromUser(user);
                Expression<Func<RequestShipment, bool>> predicate = gcbtx => gcbtx.Id > 0;

                switch (type.ToLower())
                {
                    case ShipmentTypeHelper.WaitingToPickup:
                        {
                            int[] statusIds = { StatusHelper.ShipmentStatusId.NewRequest, StatusHelper.ShipmentStatusId.ReadyToPick, StatusHelper.ShipmentStatusId.RejectPickup, StatusHelper.ShipmentStatusId.PickupFail };
                            predicate = predicate.And(gcbtx => gcbtx.FromHubId.HasValue && listHub.Contains(gcbtx.FromHubId.Value));
                            predicate = predicate.And(gcbtx => statusIds.Contains(gcbtx.ShipmentStatusId));
                            break;
                        }
                    case ShipmentTypeHelper.AssignPickup:
                        {
                            int[] statusIds = { StatusHelper.ShipmentStatusId.AssignEmployeePickup };
                            predicate = predicate.And(gcbtx => gcbtx.FromHubId.HasValue && listHub.Contains(gcbtx.FromHubId.Value));
                            predicate = predicate.And(gcbtx => statusIds.Contains(gcbtx.ShipmentStatusId));
                            break;
                        }
                    case ShipmentTypeHelper.Picking:
                        {
                            int[] statusIds = { StatusHelper.ShipmentStatusId.Picking };
                            predicate = predicate.And(x => x.FromHubId.HasValue && listHub.Contains(x.FromHubId.Value));
                            predicate = predicate.And(x => statusIds.Contains(x.ShipmentStatusId));
                            break;
                        }
                    case ShipmentTypeHelper.PickupComplete:
                        {
                            int[] statusIds = { StatusHelper.ShipmentStatusId.PickupComplete };
                            predicate = predicate.And(gcbtx => gcbtx.FromHubId.HasValue && listHub.Contains(gcbtx.FromHubId.Value));
                            predicate = predicate.And(gcbtx => statusIds.Contains(gcbtx.ShipmentStatusId));
                            break;
                        }
                    case ShipmentTypeHelper.PickupCancel:
                        {
                            int[] statusIds = { StatusHelper.ShipmentStatusId.PickupCancel };
                            predicate = predicate.And(gcbtx => gcbtx.FromHubId.HasValue && listHub.Contains(gcbtx.FromHubId.Value));
                            predicate = predicate.And(gcbtx => statusIds.Contains(gcbtx.ShipmentStatusId));
                            break;
                        }
                    case ShipmentTypeHelper.Pickup:
                        {
                            int[] statusIds = { StatusHelper.ShipmentStatusId.NewRequest, StatusHelper.ShipmentStatusId.ReadyToPick, StatusHelper.ShipmentStatusId.RejectPickup, StatusHelper.ShipmentStatusId.PickupFail, StatusHelper.ShipmentStatusId.AssignEmployeePickup };
                            predicate = predicate.And(gcbtx => gcbtx.FromHubId.HasValue && listHub.Contains(gcbtx.FromHubId.Value));
                            predicate = predicate.And(gcbtx => statusIds.Contains(gcbtx.ShipmentStatusId));
                            break;
                        }
                    case ShipmentTypeHelper.UpdatePickup:
                        {
                            int[] statusIds = { StatusHelper.ShipmentStatusId.Picking, StatusHelper.ShipmentStatusId.PickupComplete };
                            predicate = predicate.And(gcbtx => gcbtx.FromHubId.HasValue && listHub.Contains(gcbtx.FromHubId.Value));
                            predicate = predicate.And(gcbtx => statusIds.Contains(gcbtx.ShipmentStatusId));
                            break;
                        }
                }
                return _unitOfWork.RepositoryR<RequestShipment>().FindBy(predicate).Count();
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public ResponseViewModel GetLadingHistory(User user, string type, DateTime fromDate, DateTime toDate)
        {
            try
            {
                var listHub = _iuserService.GetListHubFromUser(user);
                var data = _unitOfWork.Repository<Proc_LadingSchedule_report>()
                                      .ExecProcedure(Proc_LadingSchedule_report.GetEntityProc(1, fromDate, toDate));

                switch (type.ToLower())
                {
                    case ShipmentTypeHelper.Pickup:
                        {
                            data = data.Where(x => StatusHelper.GetPickupListId().Contains(x.ShipmentStatusId) && listHub.Contains((int)x.LadingScheduleHubId));
                            break;
                        }
                }
                var a = data.ToList();
                return ResponseViewModel.CreateSuccess(data);
            }
            catch (Exception ex)
            {
                return ResponseViewModel.CreateError(ex.Message);
            }
        }

        public string GetCodeByType(int type, string prefixCode, RequestShipment requestShipment)
        {
            var shipmentNumberBasic = "";
            var requestShipmentId = requestShipment.Id;
            if (type == TypeCodeHelper.Request.Normal)
            {
                var randomCode = RandomUtil.GetCode(requestShipmentId, 6);
                shipmentNumberBasic = $"{prefixCode}{randomCode}";
            }

            if (type == TypeCodeHelper.Request.Tasetco)
            {
                var telephoneAreaCode = _unitOfWork.RepositoryR<TelephoneAreaCodes>().GetSingle(tel => tel.ProvinceId == requestShipment.FromProvinceId).Code.ToString();
                if (string.IsNullOrWhiteSpace(telephoneAreaCode))
                {
                    telephoneAreaCode = "00";
                }
                int beginYear = 2017;
                int year = DateTime.Now.Year;
                var yearToLetter = RandomUtil.IntToLetters(year - beginYear);

                //get amount of request from fromProvinId
                var amountRequest = _unitOfWork.RepositoryR<RequestShipment>().FindBy(rq => rq.FromProvinceId == requestShipment.FromProvinceId).Count();
                //set id for request has provinceId = requestShipment.FromProvinceId
                var idRequestFromProvince = amountRequest + 1;
                var lengthOfId = idRequestFromProvince.ToString().Length;
                var randomSevenCode = "";
                if (lengthOfId <= 7)
                {
                    randomSevenCode = idRequestFromProvince.ToString().PadLeft(7, '0');
                }
                else
                {
                    randomSevenCode = RandomUtil.GetCode(idRequestFromProvince, 7);
                }

                shipmentNumberBasic = $"{telephoneAreaCode}{yearToLetter}{randomSevenCode}";
            }

            return shipmentNumberBasic;
        }
    }
}
