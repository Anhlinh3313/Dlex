using System;
using System.Collections.Generic;
using Core.Entity.Entities;
using Core.Infrastructure.ViewModels;
using Core.Business.ViewModels.Shipments;
using Core.Business.ViewModels;
using System.Threading.Tasks;
using Core.Entity.Procedures;

namespace Core.Business.Services.Abstract
{
    public interface IShipmentService
    {
        ResponseViewModel GetByType(User user, string type, int? pageSize = null, int? pageNumber = null
            , string cols = null, ShipmentFilterViewModel filterViewModel=null);
        ResponseViewModel GetLadingHistory(User user, string type, DateTime fromDate, DateTime toDate);
        ResponseViewModel GetToPayment(int type, int customerId, int? pageSize = null, int? pageNumber = null, string cols = null);
        String GetCodeByType(int type, string prefixCode, int shipmentId, int countIdentity, int? fromProvinceId);
        string GetBoxCode(int shipmentId, string shipmentIdRef);
        Proc_GetInfoHubRouting GetInfoRouting(bool? isTruckDelivery, int? districtId, int? wardId, double? weight);
        Task<Shipment> CreateShipmentNoneInfo(User currentUser, string shipmentNumber);
        Task<Shipment> ReCalculatePrice(int shipmentId);
    }
}
