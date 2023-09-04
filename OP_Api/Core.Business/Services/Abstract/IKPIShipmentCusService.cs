using Core.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.Services.Abstract
{
    public interface IKPIShipmentCusService
    {
        dynamic CreateKPIShipmentCus(KPIShipmentCus request);

        dynamic UpdateKPIShipmentCus(KPIShipmentCus request);
        dynamic GetKPIShipmentCusByKPIShipment(int id);
    }
}
