using Core.Business.Services.Abstract;
using Core.Business.Services.Models;
using Core.Data.Abstract;
using Core.Entity.Entities;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.Services
{
    public class KPIShipmentCusService : BaseService, IKPIShipmentCusService
    {
        public KPIShipmentCusService(Microsoft.Extensions.Logging.ILogger<dynamic> logger,
            IOptions<AppSettings> optionsAccessor,
            IUnitOfWork unitOfWork
            )
            : base(logger, optionsAccessor, unitOfWork)
        {

        }

        public dynamic CreateKPIShipmentCus(KPIShipmentCus request)
        {
            _unitOfWork.RepositoryCRUD<KPIShipmentCus>().Insert(request);
            _unitOfWork.Commit();
            return true;
        }

        public dynamic UpdateKPIShipmentCus(KPIShipmentCus request)
        {
            _unitOfWork.RepositoryCRUD<KPIShipmentCus>().Update(request);
            _unitOfWork.Commit();
            return true;
        }

        public dynamic GetKPIShipmentCusByKPIShipment(int id)
        {
           return  _unitOfWork.RepositoryR<KPIShipmentCus>().FindBy(x=>x.KPIShipmentId == id);
        }
    }
}
