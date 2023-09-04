using System;
using Core.Entity.Entities;
using Core.Infrastructure.ViewModels;

namespace Core.Business.Services.Abstract
{
    public interface IRequestShipmentService
    {
        ResponseViewModel GetByType(User user, string type, int? pageSize = null, int? pageNumber = null, string cols = null);
        int GetCountByType(User user, string type);
        ResponseViewModel GetLadingHistory(User user, string type, DateTime fromDate, DateTime toDate);
        String GetCodeByType(int type, string prefixCode, RequestShipment requestShipment);
    }
}
