using System;
using AutoMapper;
using Core.Business.Services.Abstract;
using Core.Business.Services.Models;
using Core.Data.Abstract;
using Core.Entity.Entities;
using Core.Infrastructure.Utils;
using Microsoft.Extensions.Options;

namespace Core.Business.Services
{
    public class HubService : GeneralService, IHubService
    {
        private IUnitOfWork _unitOfWork;

        public HubService(Microsoft.Extensions.Logging.ILogger<dynamic> logger, IOptions<AppSettings> optionsAccessor, IUnitOfWork unitOfWork) : base(logger, optionsAccessor, unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public User GetByHubWardId(int wardId, int? streetId)
        {
            var streetJoin = _unitOfWork.RepositoryR<StreetJoin>().GetSingle(f=>f.WardId==wardId && f.StreetId== (streetId>>0));
            if (Util.IsNull(streetJoin))
            {
                var hrw = _unitOfWork.RepositoryR<HubRoutingWard>().GetSingle(x => x.WardId == wardId);
                if (hrw != null)
                {
                    var hr = _unitOfWork.RepositoryR<HubRouting>().GetSingle(x => x.Id == hrw.HubRoutingId);
                    if (hr != null && hr.UserId.HasValue)
                    {
                        return _unitOfWork.RepositoryR<User>().GetSingle(hr.UserId.Value);
                    }
                }
            }
            else
            {
                var hrw = _unitOfWork.RepositoryR<HubRoutingStreetJoin>().GetSingle(x => x.StreetJoinId == streetJoin.Id);
                if (hrw != null)
                {
                    var hr = _unitOfWork.RepositoryR<HubRouting>().GetSingle(x => x.Id == hrw.HubRoutingId);
                    if (hr != null && hr.UserId.HasValue)
                    {
                        return _unitOfWork.RepositoryR<User>().GetSingle(hr.UserId.Value);
                    }
                }
            }
            return null;
        }
    }
}
