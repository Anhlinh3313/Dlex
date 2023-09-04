using System;
using Core.Entity.Entities;

namespace Core.Business.Services.Abstract
{
    public interface IHubService
    {
        User GetByHubWardId(int wardId, int? streetId);
    }
}
