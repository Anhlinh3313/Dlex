using Core.Infrastructure.Helper;
using System;
namespace Core.Infrastructure.Utils
{
    public static class WarehousingUtil
    {
        public static int GetStatusWarehousing(int typeWarehousing)
        {
            var statusWarehousing = 0;
            switch (typeWarehousing)
            {
                case 2:
                    statusWarehousing = StatusHelper.ShipmentStatusId.StoreInWarehousePickup;
                    break;
                case 7:
                    statusWarehousing = StatusHelper.ShipmentStatusId.StoreInWarehouseDelivery;
                    break;
                default:
                    statusWarehousing = StatusHelper.ShipmentStatusId.StoreInWarehouseTransfer;
                    break;
            }
            return statusWarehousing;
        }

    }
}
