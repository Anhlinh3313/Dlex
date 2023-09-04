using Core.Business.Services.Abstract;
using Core.Business.Services.Models;
using Core.Business.ViewModels;
using Core.Business.ViewModels.Shipments;
using Core.Business.ViewModels.TruckSchedules;
using Core.Data.Abstract;
using Core.Entity.Entities;
using Core.Infrastructure.Utils;
using Core.Infrastructure.ViewModels;
using LinqKit;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Core.Business.Services
{
    public class TruckScheduleService : GeneralService<TruckScheduleViewModel, TruckScheduleInfoViewModel, TruckSchedule>, ITruckScheduleService
    {
        public TruckScheduleService(
            Microsoft.Extensions.Logging.ILogger<dynamic> logger,
            IOptions<AppSettings> optionsAccessor,
            IUnitOfWork unitOfWork) : base(logger, optionsAccessor, unitOfWork) { }

        public ResponseViewModel Search(TruckScheduleFilterViewModel model)
        {
            try
            {
                Expression<Func<TruckSchedule, bool>> predicate = x => x.Id > 0;
                if (!Util.IsNull(model))
                {
                    if (!Util.IsNull(model.FromHubId))
                    {
                        predicate = predicate.And(x => x.FromHubId == model.FromHubId);
                    }
                    if (!Util.IsNull(model.ToHubId))
                    {
                        predicate = predicate.And(x => x.ToHubId == model.ToHubId);
                    }
                    if (!Util.IsNull(model.TruckScheduleStatusId))
                    {
                        predicate = predicate.And(x => x.TruckScheduleStatusId == model.TruckScheduleStatusId);
                    }
                    if (!Util.IsNull(model.TruckId))
                    {
                        predicate = predicate.And(x => x.TruckId == model.TruckId);
                    }
                    if (!Util.IsNull(model.FromDate))
                    {
                        predicate = predicate.And(x => x.StartDatetime >= model.FromDate);
                    }
                    if (!Util.IsNull(model.ToDate))
                    {
                        predicate = predicate.And(x => x.StartDatetime <= model.ToDate);
                    }
                    if (!Util.IsNull(model.SearchText))
                    {
                        predicate = predicate.And(x => x.Code.Contains(model.SearchText.Trim()) || x.StartKM.ToString().Contains(model.SearchText.Trim()));
                    }
                }

                var data = FindBy(predicate, model.PageSize, model.PageNumber, model.Cols);

                return data;
            }
            catch (Exception ex)
            {
                return ResponseViewModel.CreateError(ex.Message);
            }
        }
    }
}
