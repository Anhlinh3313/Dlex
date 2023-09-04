using Core.Business.ViewModels;
using Core.Data;
using Core.Data.Core;
using Core.Entity.Entities;
using Core.Infrastructure.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Business.Core.Utils
{
    public static class DeadlineUtil
    {
        /// <summary>
        /// Deadline
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="type">1: Delivery, 2: Pickup, 3: Pickup & Delivery</param>
        /// <returns></returns>
        public static ResponseViewModel Deadline(DeadlineShipmentViewModel viewModel, int? type = 1)
        {
            using (var context = new ApplicationContext())
            {
                if (!viewModel.TimeStart.HasValue)
                {
                    viewModel.TimeStart = DateTime.Now;
                }
                DeadlineShipmentResultViewModel deadlineResult = new DeadlineShipmentResultViewModel();
                var unitOfWork = new UnitOfWork(context);
                // xác định trạm nhận + deadline 
                int fromHubId = 0;
                if (viewModel.FromHudId.HasValue)
                {
                    fromHubId = viewModel.FromHudId.Value;
                }
                if (fromHubId == 0)
                {
                    if (!viewModel.WardFromId.HasValue)
                    {
                        return ResponseViewModel.CreateError(ValidatorMessage.DeadlinePickupDelivery.FromWardNotEmpty);
                    }
                    else
                    {
                        var hubRoute = unitOfWork.RepositoryR<HubRoute>().GetSingle(f => f.WardId == viewModel.WardFromId);
                        if (hubRoute != null)
                        {
                            fromHubId = hubRoute.HubId;
                        }
                    }
                }
                if (fromHubId == 0)
                {
                    return ResponseViewModel.CreateError(ValidatorMessage.DeadlinePickupDelivery.FromHubNotEmpty);
                }
                var service = unitOfWork.RepositoryR<Service>().GetSingle(f => f.Id == viewModel.ServiceId);
                if (service == null)
                {
                    return ResponseViewModel.CreateError(ValidatorMessage.DeadlinePickupDelivery.ServiceNotFound);
                }
                var fromHub = unitOfWork.RepositoryR<Hub>().GetSingle(f => f.Id == fromHubId);
                if (fromHub == null)
                {
                    return ResponseViewModel.CreateError(ValidatorMessage.DeadlinePickupDelivery.FromHubIsValid);
                }
                var deadlinePickupDelivery = unitOfWork.RepositoryR<DeadlinePickupDelivery>().GetSingle(f => f.HubId == fromHub.Id);
                if (deadlinePickupDelivery == null)
                {
                    if (fromHub.PoHubId.HasValue)//trạm
                    {
                        deadlinePickupDelivery = unitOfWork.RepositoryR<DeadlinePickupDelivery>().GetSingle(f => f.HubId == fromHub.PoHubId);
                    }
                    else//TT/CN
                    {
                        deadlinePickupDelivery = unitOfWork.RepositoryR<DeadlinePickupDelivery>().GetSingle(f => f.HubId == fromHub.CenterHubId);
                    }
                }
                if (deadlinePickupDelivery == null)
                {
                    return ResponseViewModel.CreateError(ValidatorMessage.DeadlinePickupDelivery.FromHubNotDeadline);
                }
                // xác định khu vực phát
                if (!viewModel.DistrictToId.HasValue)
                {
                    return ResponseViewModel.CreateError(ValidatorMessage.DeadlinePickupDelivery.ToDistrictNotEmpty);
                }
                var areaInGroupTo = unitOfWork.RepositoryR<Area>().FindBy(g => g.AreaGroupId == deadlinePickupDelivery.AreaGroupId);
                if (areaInGroupTo == null)
                {
                    return ResponseViewModel.CreateError(ValidatorMessage.Calculator.NotFoundAreaGroup);
                }
                var listAreaIdInGroup = areaInGroupTo.Select(s => s.Id);
                var areaTo = unitOfWork.RepositoryR<AreaDistrict>().GetSingle(g => listAreaIdInGroup.Contains(g.AreaId) && g.DistrictId == viewModel.DistrictToId);
                if (areaTo == null)
                {
                    return ResponseViewModel.CreateError(ValidatorMessage.Calculator.NotFoundArea);
                }
                var deadlineDetail = unitOfWork.RepositoryR<DeadlinePickupDeliveryDetail>()
                    .GetSingle(f => f.AreaId == areaTo.AreaId && f.DeadlinePickupDeliveryId == deadlinePickupDelivery.Id
                    && f.ServiceId == service.Id);
                if (deadlineDetail == null)
                {
                    return ResponseViewModel.CreateError(ValidatorMessage.DeadlinePickupDelivery.DeadlineDetailNotFound);
                }
                //
                deadlineResult.FormatDatetime = viewModel.TimeStart.Value.AddHours(deadlineDetail.TimeDelivery);
                TimeSpan timeSpanHour = deadlineResult.FormatDatetime - Convert.ToDateTime(deadlineResult.FormatDatetime.ToString("yyyy/MM/dd"));
                int hour = timeSpanHour.Hours;
                if (hour >= 17)
                {
                    deadlineResult.FormatDatetime = deadlineResult.FormatDatetime.AddHours(24 - hour + 8);
                }
                else if (hour < 8)
                {
                    deadlineResult.FormatDatetime = deadlineResult.FormatDatetime.AddHours(8 - hour);
                }
                //
                DateTime start = viewModel.TimeStart.Value;
                DateTime stop = deadlineResult.FormatDatetime;
                int countSunday = 0;
                while (start <= stop)
                {
                    if (start.DayOfWeek == DayOfWeek.Sunday)
                    {
                        countSunday++;
                        stop.AddDays(1);
                    }
                    start = start.AddDays(1);
                }
                TimeSpan timeSpan = (deadlineResult.FormatDatetime - viewModel.TimeStart.Value);
                deadlineResult.FormatDay = timeSpan.Days;
                deadlineResult.FormatDateHH = deadlineResult.FormatDatetime.ToString("hh:mm dd/MM/yyyy");
                deadlineResult.FormatTime = timeSpan.Hours;
                deadlineResult.FormatTimeSys = deadlineDetail.TimeDelivery;
                deadlineResult.FormatDateTT = deadlineResult.FormatDatetime.ToString("tt dd/MM/yyyy");
                //
                return ResponseViewModel.CreateSuccess(deadlineResult);
            }
        }
    }
}
