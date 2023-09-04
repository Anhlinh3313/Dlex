using Core.Business.ViewModels.ExportExcelModel;
using Core.Business.ViewModels.KPIShipmentDetails;
using Core.Entity.Entities;
using Core.Entity.Procedures;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Services.Abstract
{
    public interface IKPIShipmentDetailService
    {
        int CalculateTimeForShipment(int time, DateTime dateTimeStart);
        KPIShipmentDetailResponseModel CalculateTimeForShipment(int? cusId, int? districtId, int? wardId, int serviceId, DateTime dateTimeStart);
        dynamic InsertKPIShipmentDetail(List<KPIShipmentDetail> rquest);
        dynamic UpdateKPIShipmentDetail(List<KPIShipmentDetail> rquest);
        dynamic GetKPIShipmentDetail(int kPIShipemntId, int? PageSize, int? PageNumber);
        List<Proc_GetKPIDetail> GetKPIShipmentDetailExport(int kPIShipemntId, int? PageSize, int? PageNumber, CustomExportFile customExportFile);
    }
}
