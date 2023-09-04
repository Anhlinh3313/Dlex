using Core.Api.Infrastruture;
using Core.Business.Core.Helpers;
using Core.Business.ViewModels;
using Core.Business.ViewModels.Shipments;
using Core.Data;
using Core.Data.Abstract;
using Core.Data.Core;
using Core.Entity.Entities;
using Core.Entity.Procedures;
using Core.Infrastructure.Api;
using Core.Infrastructure.Helper;
using Core.Infrastructure.Utils;
using Core.Infrastructure.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using static Core.Api.Infrastruture.VSEApi;

namespace Core.Business.Core.Utils
{
    public static class PushUtil
    {
        public static void PushBillVSE()
        {
            //IOptions<CompanyInformation> companyInformation;
            //if (companyInformation.Value.Name == "vietstar")
            //{
            int setTime = 1;
            while (true)
            {
                using (var context = new ApplicationContext())
                {
                    var _unitOfWork = new UnitOfWork(context);
                    try
                    {
                        List<int> listSuccess = new List<int>();
                        List<int> listFaile = new List<int>();
                        var listShipment = _unitOfWork.RepositoryR<Shipment>()
                            .FindBy(f => (f.IsPushVSE == false || !f.IsPushVSE.HasValue) &&
                            (f.PickUserId.HasValue) && f.ShipmentStatusId != StatusHelper.ShipmentStatusId.NewRequest &&
                        f.SenderId.HasValue && f.ToProvinceId.HasValue &&
                        f.PaymentTypeId.HasValue && f.ServiceId.HasValue && f.FromProvinceId.HasValue && f.FromDistrictId.HasValue
                        && (f.CountPushVSE == 0 || !f.CountPushVSE.HasValue) && f.SenderId != 4).OrderBy(o => o.CountPushVSE).Take(50);
                        int loop = 0;
                        while (listShipment.Count() > 0 && listShipment.Count() > loop)
                        {
                            var shipment = listShipment.ToList()[loop];
                            //PUSH BILL VSE
                            string ToDistrictVSE = "";
                            string ToWardVSE = "";
                            string ToStructureVSE = "";
                            string UserVSE = "";
                            if (shipment.PickUserId.HasValue)
                            {
                                var user = _unitOfWork.RepositoryR<User>().GetSingle(shipment.PickUserId.Value);
                                if (Util.IsNull(user.VSEOracleCode))
                                {
                                    UserVSE = user.Code;
                                }
                                else
                                {
                                    UserVSE = user.VSEOracleCode;
                                }
                            }
                            else if (shipment.CreatedBy.HasValue)
                            {
                                var user = _unitOfWork.RepositoryR<User>().GetSingle(shipment.CreatedBy.Value);
                                if (Util.IsNull(user.VSEOracleCode))
                                {
                                    UserVSE = user.Code;
                                }
                                else
                                {
                                    UserVSE = user.VSEOracleCode;
                                }
                            }

                            //
                            if (shipment.ToDistrictId.HasValue) ToDistrictVSE = _unitOfWork.RepositoryR<District>().GetSingle(shipment.ToDistrictId.Value)?.VSEOracleCode;
                            if (shipment.ToWardId.HasValue) ToWardVSE = _unitOfWork.RepositoryR<Ward>().GetSingle(shipment.ToWardId.Value)?.VSEOracleCode;
                            if (shipment.StructureId.HasValue) ToStructureVSE = _unitOfWork.RepositoryR<Structure>().GetSingle(shipment.StructureId > 0 ? shipment.StructureId.Value : 0)?.VSEOracleCode;
                            //
                            List<PHI_ITEM> LIST_PHI = new List<PHI_ITEM>();
                            if (shipment.RemoteAreasPrice > 0)
                            {
                                PHI_ITEM PHI = new PHI_ITEM();
                                PHI.SO_VAN_DON = shipment.ShipmentNumber;
                                PHI.MA_PHI = FMIS_PHI.PHI_HANG_XA.ToString();
                                PHI.PHI = shipment.RemoteAreasPrice + "";
                                PHI.PHI_KS = shipment.RemoteAreasPrice;
                                LIST_PHI.Add(PHI);
                            }
                            var listPriceDVGTs = _unitOfWork.RepositoryR<ShipmentServiceDVGT>().FindBy(f => f.ShipmentId == shipment.Id);
                            foreach (var item in listPriceDVGTs)
                            {
                                var serviceDVGT = _unitOfWork.RepositoryR<Service>().GetSingle(f => f.Id == item.ServiceId);
                                if (!Util.IsNull(serviceDVGT))
                                {
                                    PHI_ITEM PHI = new PHI_ITEM();
                                    PHI.SO_VAN_DON = shipment.ShipmentNumber;
                                    PHI.MA_PHI = serviceDVGT.VSEOracleCode;
                                    PHI.PHI = item.Price + "";
                                    PHI.PHI_KS = item.Price;
                                    LIST_PHI.Add(PHI);
                                }
                            }
                            //
                            var HINH_THUC_TT = new PaymentType();
                            if (shipment.PaymentTypeId > 0)
                            {
                                HINH_THUC_TT = _unitOfWork.RepositoryR<PaymentType>().GetSingle(shipment.PaymentTypeId.Value);
                            }
                            //
                            string tu_tinh = _unitOfWork.RepositoryR<Province>().GetSingle(shipment.FromProvinceId.Value)?.VSEOracleCode;
                            string tu_huyen = "";
                            if (shipment.FromDistrictId.HasValue) tu_huyen = _unitOfWork.RepositoryR<District>().GetSingle(shipment.FromDistrictId.Value)?.VSEOracleCode;
                            string kh_gui = _unitOfWork.RepositoryR<Customer>().GetSingle(shipment.SenderId.Value)?.VSEOracleCode;
                            string tinh_den = _unitOfWork.RepositoryR<Province>().GetSingle(shipment.ToProvinceId.Value)?.VSEOracleCode;
                            string dich_vu = _unitOfWork.RepositoryR<Service>().GetSingle(shipment.ServiceId.Value)?.VSEOracleCode;
                            VSEApi vSEApi = new VSEApi();
                            DateTime createdWhen = DateTime.Now;
                            if (!Util.IsNull(shipment.CreatedWhen)) createdWhen = shipment.CreatedWhen.Value;
                            var taskVSE = vSEApi.CreateEBill(
                                    SO_VAN_DON: shipment.ShipmentNumber,
                                    NGAY_VD: createdWhen,
                                    CREATEDBY: UserVSE,
                                    TU_TINH: tu_tinh,
                                    TU_HUYEN: tu_huyen,
                                    KH_GUI: kh_gui,
                                    NG_SDT: shipment.SenderPhone,
                                    DIA_CHI_NGUOI_GUI: shipment.PickingAddress,
                                    DEN_TINH: tinh_den,
                                    DEN_HUYEN: ToDistrictVSE,
                                    DEN_XA: ToWardVSE,
                                    NGUOI_GUI: shipment.SenderName,
                                    NGUOI_NHAN: shipment.ReceiverName,
                                    NN_SDT: shipment.ReceiverPhone,
                                    DIA_CHI_NGUOI_NHAN: shipment.AddressNoteTo + " " + shipment.ShippingAddress,
                                    KL_VAN_DON: shipment.Weight,
                                    KL_VAN_DON_QD: shipment.CalWeight == null ? 0 : shipment.CalWeight.Value,
                                    TIEN_CUOC: shipment.DefaultPrice + shipment.FuelPrice + shipment.RemoteAreasPrice,
                                    TIEN_CUOC_KS: shipment.TotalPrice,
                                    DICH_VU: dich_vu,
                                    HINH_THUC_TT: HINH_THUC_TT?.VSEOracleCode,
                                    GHI_CHU_NHAN_HANG: shipment.CusNote,
                                    TRA_NGAY: HINH_THUC_TT?.VSEOracleTRA_NGAY,
                                    TEN_HANG_HOA: shipment.Content,
                                    TIEN_THU_HO: shipment.COD,
                                    LOAI_HH: ToStructureVSE,
                                    SO_KIEN: shipment.TotalBox == 0 ? 1 : shipment.TotalBox,
                                    NV_NHAN: UserVSE,
                                    TIME_NHAN: createdWhen,
                                    REF_NUMBER: shipment.ShopCode,
                                    PhiItem: LIST_PHI
                            );
                            var resultVSE = taskVSE.Result;
                            bool checkResult = false;
                            if (resultVSE.ToLower().Contains("success") && resultVSE.ToLower().Contains("false"))
                            {
                                listFaile.Add(shipment.Id);
                            }
                            else
                            {
                                listSuccess.Add(shipment.Id);
                                checkResult = true;
                                //
                            }
                            resultVSE += " : {" + shipment.ShipmentNumber + "} - OAB";
                            using (var context2 = new ApplicationContext())
                            {
                                var _unitOfWork2 = new UnitOfWork(context2);
                                _unitOfWork2.Repository<Proc_UpdateCountPushVSE>()
                                .ExecProcedureSingle(Proc_UpdateCountPushVSE.GetEntityProc(shipment.Id, checkResult, "", resultVSE));
                            }
                            loop++;
                            var count = listShipment.Count();
                        }
                        if (listShipment == null)
                        {
                            if (setTime < 2)
                            {
                                setTime = setTime * 2;
                            }
                        }
                        else
                        {
                            if (listShipment.Count() <= 0)
                            {
                                if (setTime < 2)
                                {
                                    setTime = setTime * 2;
                                }
                            }
                            else if (listShipment.Count() > 0)
                            {
                                setTime = 1;
                            }
                        }
                        _unitOfWork.Repository<Proc_TaskScheduler>()
                        .ExecProcedureSingle(Proc_TaskScheduler.GetEntityProc("PUSHBILL-PICKUP Run Task Push VSE Oracle auto - OAB", setTime, ""));
                        _unitOfWork.Commit();
                        Thread.Sleep(setTime * setTime * setTime * setTime * 10 * 1000);//
                    }
                    catch (Exception ex)
                    {
                        _unitOfWork.Repository<Proc_TaskScheduler>()
                         .ExecProcedureSingle(Proc_TaskScheduler.GetEntityProc("PUSHBILL-PICKUP Run Task Push VSE Oracle auto - OAB", setTime, "Error: " + ex.Message));
                        _unitOfWork.Commit();
                        Thread.Sleep(1 * 30 * 1000);//
                    }
                }
            }
        }

        public static bool PushUpdateBillVSE(CreateUpdateShipmentViewModel shipment)
        {
            using (var context = new ApplicationContext())
            {
                var _unitOfWork = new UnitOfWork(context);
                try
                {
                    //PUSH BILL VSE
                    string ToDistrictVSE = "";
                    string ToWardVSE = "";
                    string ToStructureVSE = "";
                    string UserVSE = "";
                    if (shipment.PickUserId.HasValue)
                    {
                        var user = _unitOfWork.RepositoryR<User>().GetSingle(shipment.PickUserId.Value);
                        if (Util.IsNull(user.VSEOracleCode))
                        {
                            UserVSE = user.Code;
                        }
                        else
                        {
                            UserVSE = user.VSEOracleCode;
                        }
                    }
                    //else if (shipment.CreatedBy.HasValue)
                    //{
                    //    var user = _unitOfWork.RepositoryR<User>().GetSingle(shipment.CreatedBy.Value);
                    //    if (Util.IsNull(user.VSEOracleCode))
                    //    {
                    //        UserVSE = user.Code;
                    //    }
                    //    else
                    //    {
                    //        UserVSE = user.VSEOracleCode;
                    //    }
                    //}

                    //
                    if (shipment.ToDistrictId.HasValue) ToDistrictVSE = _unitOfWork.RepositoryR<District>().GetSingle(shipment.ToDistrictId.Value)?.VSEOracleCode;
                    if (shipment.ToWardId.HasValue) ToWardVSE = _unitOfWork.RepositoryR<Ward>().GetSingle(shipment.ToWardId.Value)?.VSEOracleCode;
                    if (shipment.StructureId.HasValue) ToStructureVSE = _unitOfWork.RepositoryR<Structure>().GetSingle(shipment.StructureId > 0 ? shipment.StructureId.Value : 0)?.VSEOracleCode;
                    //
                    List<PHI_ITEM> LIST_PHI = new List<PHI_ITEM>();
                    if (shipment.RemoteAreasPrice > 0)
                    {
                        PHI_ITEM PHI = new PHI_ITEM();
                        PHI.SO_VAN_DON = shipment.ShipmentNumber;
                        PHI.MA_PHI = FMIS_PHI.PHI_HANG_XA.ToString();
                        PHI.PHI = shipment.RemoteAreasPrice + "";
                        PHI.PHI_KS = shipment.RemoteAreasPrice;
                        LIST_PHI.Add(PHI);
                    }
                    var listPriceDVGTs = _unitOfWork.RepositoryR<ShipmentServiceDVGT>().FindBy(f => f.ShipmentId == shipment.Id);
                    foreach (var item in listPriceDVGTs)
                    {
                        var serviceDVGT = _unitOfWork.RepositoryR<Service>().GetSingle(f => f.Id == item.ServiceId);
                        if (!Util.IsNull(serviceDVGT))
                        {
                            PHI_ITEM PHI = new PHI_ITEM();
                            PHI.SO_VAN_DON = shipment.ShipmentNumber;
                            PHI.MA_PHI = serviceDVGT.VSEOracleCode;
                            PHI.PHI = item.Price + "";
                            PHI.PHI_KS = item.Price;
                            LIST_PHI.Add(PHI);
                        }
                    }
                    //
                    var HINH_THUC_TT = new PaymentType();
                    if (shipment.PaymentTypeId > 0)
                    {
                        HINH_THUC_TT = _unitOfWork.RepositoryR<PaymentType>().GetSingle(shipment.PaymentTypeId.Value);
                    }
                    //
                    string tu_tinh = _unitOfWork.RepositoryR<Province>().GetSingle(shipment.FromProvinceId.Value)?.VSEOracleCode;
                    string tu_huyen = _unitOfWork.RepositoryR<District>().GetSingle(shipment.FromDistrictId.Value)?.VSEOracleCode;
                    string kh_gui = _unitOfWork.RepositoryR<Customer>().GetSingle(shipment.SenderId.Value)?.VSEOracleCode;
                    string tinh_den = _unitOfWork.RepositoryR<Province>().GetSingle(shipment.ToProvinceId.Value)?.VSEOracleCode;
                    string dich_vu = _unitOfWork.RepositoryR<Service>().GetSingle(shipment.ServiceId.Value)?.VSEOracleCode;
                    VSEApi vSEApi = new VSEApi();
                    DateTime createdWhen = DateTime.Now;
                    //BB if (!Util.IsNull(shipment.CreatedWhen)) createdWhen = shipment.CreatedWhen.Value;
                    var taskVSE = vSEApi.CreateEBill(
                            SO_VAN_DON: shipment.ShipmentNumber,
                            NGAY_VD: createdWhen,
                            CREATEDBY: UserVSE,
                            TU_TINH: tu_tinh,
                            TU_HUYEN: tu_huyen,
                            KH_GUI: kh_gui,
                            NG_SDT: shipment.SenderPhone,
                            DIA_CHI_NGUOI_GUI: shipment.PickingAddress,
                            DEN_TINH: tinh_den,
                            DEN_HUYEN: ToDistrictVSE,
                            DEN_XA: ToWardVSE,
                            NGUOI_GUI: shipment.SenderName,
                            NGUOI_NHAN: shipment.ReceiverName,
                            NN_SDT: shipment.ReceiverPhone,
                            DIA_CHI_NGUOI_NHAN: shipment.AddressNoteTo + " " + shipment.ShippingAddress,
                            KL_VAN_DON: shipment.Weight,
                            KL_VAN_DON_QD: shipment.CalWeight, //BNB == null ? 0 : shipment.CalWeight.Value,
                            TIEN_CUOC: shipment.DefaultPrice + shipment.FuelPrice + shipment.RemoteAreasPrice,
                            TIEN_CUOC_KS: shipment.TotalPrice,
                            DICH_VU: dich_vu,
                            HINH_THUC_TT: HINH_THUC_TT?.VSEOracleCode,
                            GHI_CHU_NHAN_HANG: shipment.CusNote,
                            TRA_NGAY: HINH_THUC_TT?.VSEOracleTRA_NGAY,
                            TEN_HANG_HOA: shipment.Content,
                            TIEN_THU_HO: shipment.COD,
                            LOAI_HH: ToStructureVSE,
                            SO_KIEN: shipment.TotalBox == 0 ? 1 : shipment.TotalBox,
                            NV_NHAN: UserVSE,
                            TIME_NHAN: createdWhen,
                            REF_NUMBER: shipment.ShopCode,
                            PhiItem: LIST_PHI
                    );
                    var resultVSE = taskVSE.Result;
                    bool checkResult = false;
                    resultVSE += " : {" + shipment.ShipmentNumber + "} - OAB => UPDATE INFOMATION";
                    if (resultVSE.ToLower().Contains("success") && resultVSE.ToLower().Contains("false"))
                    {
                        return false;
                    }
                    else
                    {
                        checkResult = true;
                        using (var context2 = new ApplicationContext())
                        {
                            var _unitOfWork2 = new UnitOfWork(context2);
                            _unitOfWork2.Repository<Proc_UpdateCountPushVSE>()
                            .ExecProcedureSingle(Proc_UpdateCountPushVSE.GetEntityProc(shipment.Id, checkResult, "", resultVSE));
                        }
                        //                    
                        _unitOfWork.Commit();
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    var a = ex.Message;
                    return false;
                }
            }
        }

        public static void GetInfoBillVSE()
        {
            int setTime = 1;
            while (true)
            {
                using (var context = new ApplicationContext())
                {
                    var _unitOfWork = new UnitOfWork(context);
                    try
                    {
                        var listShipment = _unitOfWork.Repository<Proc_GetBillUpdateInfo>().ExecProcedure(Proc_GetBillUpdateInfo.GetEntityProc());
                        int loop = 0;
                        while (listShipment.Count() > 0 && listShipment.Count() > loop)
                        {
                            var ship = listShipment.ToList()[loop];
                            VSEApi vSEApi = new VSEApi();
                            var taskVSE = vSEApi.GetBillInfo(ship.ShipmentNumber);
                            var infoBill = taskVSE.Result;
                            var shipment = _unitOfWork.RepositoryCRUD<Shipment>().GetSingle(ship.Id);
                            if (infoBill != null)
                            {
                                //
                                shipment.OrderDate = infoBill.NGAY_VD;
                                if (infoBill.HINH_THUC_TT.ToUpper() == "NG")
                                {
                                    if (infoBill.TRA_NGAY == "0") shipment.PaymentTypeId = PaymentTypeHelper.THANH_TOAN_CUOI_THANG;
                                    else if (infoBill.TRA_NGAY == "1") shipment.PaymentTypeId = PaymentTypeHelper.NGUOI_GUI_THANH_TOAN;
                                }
                                else if (infoBill.HINH_THUC_TT.ToUpper() == "NN")
                                {
                                    shipment.PaymentTypeId = PaymentTypeHelper.NGUOI_NHAN_THANH_TOAN;
                                    if (shipment.DeliverUserId.HasValue && shipment.ShipmentStatusId == StatusHelper.ShipmentStatusId.DeliveryComplete && !shipment.KeepingTotalPriceEmpId.HasValue)
                                    {
                                        shipment.KeepingTotalPriceEmpId = shipment.DeliverUserId;
                                    }
                                }
                                if (infoBill.IS_CHUYEN_HOAN == "1") shipment.IsReturn = true;
                                else if (infoBill.IS_CHUYEN_HOAN == "0")
                                {
                                    if (shipment.ShipmentStatusId == StatusHelper.ShipmentStatusId.DeliveryComplete && shipment.COD > 0 && shipment.DeliverUserId.HasValue && !shipment.KeepingCODEmpId.HasValue)
                                    {
                                        shipment.KeepingCODEmpId = shipment.DeliverUserId;
                                    }
                                    shipment.IsReturn = false;
                                }
                                var fromProvince = _unitOfWork.RepositoryR<Province>().GetSingle(f => f.VSEOracleCode == infoBill.TU_TINH);
                                if (!Util.IsNull(fromProvince)) shipment.FromProvinceId = fromProvince.Id;
                                var fromDistrict = _unitOfWork.RepositoryR<District>().GetSingle(f => f.VSEOracleCode == infoBill.TU_HUYEN_MA);
                                if (!Util.IsNull(fromDistrict)) shipment.FromDistrictId = fromDistrict.Id;
                                var sender = _unitOfWork.RepositoryR<Customer>().GetSingle(f => f.VSEOracleCode == infoBill.KH_GUI_VALUE);
                                if (!Util.IsNull(sender)) shipment.SenderId = sender.Id;
                                shipment.PickingAddress = infoBill.DIA_CHI_NGUOI_GUI;
                                var receiver = _unitOfWork.RepositoryR<Customer>().GetSingle(f => f.VSEOracleCode == infoBill.KH_TRA_VALUE);
                                if (!Util.IsNull(receiver)) shipment.ReceiverId = receiver.Id;
                                var toProvince = _unitOfWork.RepositoryR<Province>().GetSingle(f => f.VSEOracleCode == infoBill.DEN_TINH);
                                if (!Util.IsNull(toProvince)) shipment.ToProvinceId = toProvince.Id;
                                var toDistrict = _unitOfWork.RepositoryR<District>().GetSingle(f => f.VSEOracleCode == infoBill.DEN_HUYEN_MA);
                                if (!Util.IsNull(toDistrict)) shipment.ToDistrictId = toDistrict.Id;
                                var toWard = _unitOfWork.RepositoryR<Ward>().GetSingle(f => f.VSEOracleCode == infoBill.DEN_XA_MA);
                                if (!Util.IsNull(toWard)) shipment.ToWardId = toWard.Id;
                                shipment.SenderName = infoBill.NGUOI_GUI;
                                shipment.ReceiverName = infoBill.NGUOI_NHAN;
                                shipment.ShippingAddress = infoBill.DIA_CHI_NGUOI_NHAN;
                                if (Util.IsNull(infoBill.KL_VAN_DON_QD_KS)) infoBill.KL_VAN_DON_QD_KS = 0;
                                shipment.CalWeight = infoBill.KL_VAN_DON_QD_KS;
                                if (Util.IsNull(infoBill.KL_VAN_DON_KS)) infoBill.KL_VAN_DON_KS = 0;
                                shipment.Weight = infoBill.KL_VAN_DON_KS.Value;
                                if (Util.IsNull(infoBill.TIEN_CUOC)) infoBill.TIEN_CUOC = 0;
                                shipment.DefaultPriceS = infoBill.TIEN_CUOC.Value;
                                if (Util.IsNull(infoBill.TIEN_CUOC_KS)) infoBill.TIEN_CUOC_KS = 0;
                                shipment.DefaultPrice = infoBill.TIEN_CUOC_KS.Value;
                                var service = _unitOfWork.RepositoryR<Service>().GetSingle(f => f.VSEOracleCode == infoBill.PRODUCT_VALUE);
                                if (!Util.IsNull(service)) shipment.ServiceId = service.Id;
                                shipment.Content = infoBill.GHI_CHU_NHAN_HANG;
                                shipment.CusNote = infoBill.GHI_CHU_TRA_HANG;
                                if (Util.IsNull(infoBill.PHI_KS)) infoBill.PHI_KS = 0;
                                shipment.TotalDVGT = infoBill.PHI_KS.Value;
                                var pickUser = _unitOfWork.RepositoryR<User>().GetSingle(f => f.Code == infoBill.NV_NHAN_VALUE);
                                if (!Util.IsNull(pickUser)) shipment.PickUserId = pickUser.Id;
                                shipment.EndPickTime = infoBill.TIME_NHAN;
                                var deliveryUser = _unitOfWork.RepositoryR<User>().GetSingle(f => f.Code == infoBill.NV_TRA_VALUE);
                                if (!Util.IsNull(deliveryUser)) shipment.DeliverUserId = deliveryUser.Id;
                                //shipment.EndDeliveryTime = infoBill.TIME_TRA;
                                //shipment.RealRecipientName = infoBill.TEN_NGUOI_NHAN;
                                var inputUser = _unitOfWork.RepositoryR<User>().GetSingle(f => f.Code == infoBill.NGUOI_UPDATE_LIEN_TRANG);
                                if (!Util.IsNull(inputUser)) shipment.InputUserId = inputUser.Id;
                                if (Util.IsNull(infoBill.DOANH_THU)) infoBill.DOANH_THU = 0;
                                shipment.TotalPrice = infoBill.DOANH_THU.Value;
                                if (Util.IsNull(infoBill.TIEN_THU_HO)) infoBill.TIEN_THU_HO = 0;
                                shipment.COD = infoBill.TIEN_THU_HO.Value;
                                var structure = _unitOfWork.RepositoryR<Structure>().GetSingle(f => f.VSEOracleCode == infoBill.LOAI_HH);
                                if (!Util.IsNull(structure)) shipment.StructureId = structure.Id;
                                if (infoBill.SO_KIEN.HasValue) shipment.TotalBox = infoBill.SO_KIEN.Value;
                                if (Util.IsNull(infoBill.GIA_TRI_HH)) infoBill.GIA_TRI_HH = 0;
                                shipment.Insured = infoBill.GIA_TRI_HH.Value;
                                if (Util.IsNull(infoBill.GIA_PUBLIC)) infoBill.GIA_PUBLIC = 0;
                                shipment.DefaultPriceP = infoBill.GIA_PUBLIC.Value;
                                var tohub = _unitOfWork.RepositoryR<Hub>().GetSingle(f => f.Code == infoBill.MA_BUU_CUC_TRA);
                                if (!Util.IsNull(tohub)) shipment.ToHubId = tohub.Id;
                                shipment.SenderPhone = infoBill.NG_SDT;
                                shipment.ReceiverPhone = infoBill.NN_SDT;
                                if (!shipment.Note.Contains("(API-A)")) shipment.Note = "(API-A)" + shipment.Note;
                                //
                                _unitOfWork.RepositoryCRUD<ShipmentServiceDVGT>().DeleteEmptyWhere(x => x.ShipmentId == shipment.Id);
                                if (!Util.IsNull(infoBill.DT_PHI))
                                {
                                    if (infoBill.DT_PHI.Count() > 0)
                                    {
                                        foreach (var item in infoBill.DT_PHI)
                                        {
                                            var dvgt = _unitOfWork.RepositoryR<ServiceDVGT>().GetSingle(f => f.VSEOracleCode == item.MA_PHI);
                                            if (!Util.IsNull(dvgt))
                                            {
                                                var ssDVGT = new ShipmentServiceDVGT();
                                                ssDVGT.ShipmentId = shipment.Id;
                                                ssDVGT.ServiceId = dvgt.Id;
                                                ssDVGT.IsAgree = true;
                                                if (!Util.IsNull(item.PHI_KS)) item.PHI_KS = 0;
                                                ssDVGT.Price = item.PHI_KS.Value;
                                                _unitOfWork.RepositoryCRUD<ShipmentServiceDVGT>().InsertNoneUser(ssDVGT);
                                            }
                                        }
                                        _unitOfWork.RepositoryCRUD<ShipmentServiceDVGT>().CommitAsync();
                                    }
                                }
                            }
                            shipment.ModifiedWhen = DateTime.Now;
                            shipment.IsPushVSE = true;
                            _unitOfWork.Commit();
                            //
                            loop++;
                        }
                        if (listShipment == null)
                        {
                            if (setTime < 2)
                            {
                                setTime = setTime * 2;
                            }
                        }
                        else
                        {
                            if (listShipment.Count() <= 0)
                            {
                                if (setTime < 2)
                                {
                                    setTime = setTime * 2;
                                }
                            }
                            else if (listShipment.Count() > 0)
                            {
                                setTime = 1;
                            }
                        }
                        Thread.Sleep(setTime * setTime * setTime * setTime * 10 * 1000);//
                    }
                    catch (Exception ex)
                    {
                        var a = ex.Message;
                        _unitOfWork.Commit();
                        Thread.Sleep(1 * 30 * 1000);//
                    }
                }
            }
        }

        public static void PushDeliveryInfo()
        {
            int setTime = 1;
            while (true)
            {
                using (var context = new ApplicationContext())
                {
                    var _unitOfWork = new UnitOfWork(context);
                    try
                    {
                        var listShipment = _unitOfWork.Repository<Proc_GetBillDeliveryInfo>().ExecProcedure(Proc_GetBillDeliveryInfo.GetEntityProc());
                        int loop = 0;
                        while (listShipment.Count() > 0 && listShipment.Count() > loop)
                        {
                            var ship = listShipment.ToList()[loop];
                            if (ship.CreatedWhen == null) ship.CreatedWhen = DateTime.Now;
                            if (ship.EndDeliveryTime == null) ship.EndDeliveryTime = DateTime.Now;
                            var file = FileUtil.GetFile(ship.ImagePath);
                            string fileName = "NOT IMAGE";
                            string image = "NOT FOUNT";
                            string result = "NOT INFOMATION";
                            if (file != null)
                                if (file.FileBase64String != null)
                                {
                                    image = file.FileBase64String;
                                    fileName = string.Format("{0}_{1}{2}", ship.DeliveryUserCode, ship.CreatedWhen.Value.ToString("yyyyMMddHHmmssfff"), file.FileExtension);
                                    VSEApi vSEApi = new VSEApi();
                                    //
                                    var objContent = JsonConvert.SerializeObject(new
                                    {
                                        VD_ID = "0",
                                        SVD = ship.ShipmentNumber.ToUpper(),
                                        SMS_ID = "0",
                                        FILENAME = fileName,
                                        MANV = ship.DeliveryUserCode,
                                        LY_DO = ship.DeliveryNote,
                                        KY_NHAN = ship.RealRecipientName,
                                        IMAGE = "Base 64",
                                        NGAY = ship.EndDeliveryTime.Value.ToString("yyyy-MM-dd HH:mm:ss")
                                    });
                                    //
                                    var taskVSE = vSEApi.PushImageDelivery("0", ship.ShipmentNumber.ToUpper(), "0", ship.DeliveryUserCode,
                                        ship.RealRecipientName, ship.DeliveryNote, fileName, image, ship.EndDeliveryTime.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                                    try
                                    {
                                        result = taskVSE.Result;
                                    }
                                    catch (Exception ex)
                                    {
                                        result = "Error: " + ex.Message;
                                    }
                                    //
                                    var isPush = false;
                                    if (result == "success")
                                    {
                                        isPush = true;
                                    }
                                    using (var context2 = new ApplicationContext())
                                    {
                                        var _unitOfWork2 = new UnitOfWork(context2);
                                        _unitOfWork2.Repository<Proc_UpdateCountImageVSE>()
                                        .ExecProcedureSingle(Proc_UpdateCountImageVSE.GetEntityProc(ship.Id, isPush, string.Format("PUSH IMAGE DELIVERY - DATA: {0}", objContent.ToString()), string.Format("({0}) => {1}", ship.ShipmentNumber, result)));
                                    }
                                }
                            //
                            loop++;
                        }
                        if (listShipment == null)
                        {
                            if (setTime < 2)
                            {
                                setTime = setTime * 2;
                            }
                        }
                        else
                        {
                            if (listShipment.Count() <= 0)
                            {
                                if (setTime < 2)
                                {
                                    setTime = setTime * 2;
                                }
                            }
                            else if (listShipment.Count() > 0)
                            {
                                setTime = 1;
                            }
                        }
                        Thread.Sleep(setTime * setTime * setTime * setTime * 10 * 1000);//
                    }
                    catch (Exception ex)
                    {
                        Thread.Sleep(1 * 30 * 1000);//
                        using (var context2 = new ApplicationContext())
                        {
                            var _unitOfWork2 = new UnitOfWork(context2);
                            _unitOfWork2.Repository<Proc_UpdateCountImageVSE>()
                            .ExecProcedureSingle(Proc_UpdateCountImageVSE.GetEntityProc(0, false, "PUSH IMAGE DELIVERY ERROR", ex.Message));
                        }
                    }
                }
            }
        }


        public static void PushPickupInfo()
        {
            int setTime = 1;
            while (true)
            {
                using (var context = new ApplicationContext())
                {
                    var _unitOfWork = new UnitOfWork(context);
                    try
                    {
                        var listShipment = _unitOfWork.Repository<Proc_GetBillPickupInfo>().ExecProcedure(Proc_GetBillPickupInfo.GetEntityProc());
                        int loop = 0;
                        while (listShipment.Count() > 0 && listShipment.Count() > loop)
                        {
                            var ship = listShipment.ToList()[loop];
                            if (ship.CreatedWhen == null) ship.CreatedWhen = DateTime.Now;
                            if (ship.EndPickTime == null) ship.EndPickTime = ship.CreatedWhen;
                            var file = FileUtil.GetFile(ship.ImagePath);
                            string fileName = "NOT IMAGE";
                            string image = "NOT FOUNT";
                            string result = "NOT INFORMATION";
                            if (file != null)
                                if (file.FileBase64String != null)
                                {
                                    image = file.FileBase64String;
                                    fileName = string.Format("{0}_{1}{2}", ship.PickupUserCode, ship.CreatedWhen.Value.ToString("yyyyMMddHHmmssfff"), file.FileExtension);
                                    VSEApi vSEApi = new VSEApi();
                                    var taskVSE = vSEApi.PushImagePickup("0", ship.ShipmentNumber.ToUpper(), "0",
                                        fileName, ship.PickupUserCode, "", "", image);
                                    try
                                    {
                                        result = taskVSE.Result;
                                    }
                                    catch (Exception ex)
                                    {
                                        result = "Error: " + ex.Message;
                                    }
                                    //
                                    var isPush = false;
                                    if (result == "success")
                                    {
                                        isPush = true;
                                    }
                                    using (var context2 = new ApplicationContext())
                                    {
                                        var _unitOfWork2 = new UnitOfWork(context2);
                                        _unitOfWork2.Repository<Proc_UpdateCountImageVSE>()
                                        .ExecProcedureSingle(Proc_UpdateCountImageVSE.GetEntityProc(ship.Id, isPush, "PUSH IMAGE PICKUP", string.Format("({0}) => {1}", ship.ShipmentNumber, result)));
                                    }
                                }
                            //
                            loop++;
                        }
                        if (listShipment == null)
                        {
                            if (setTime < 2)
                            {
                                setTime = setTime * 2;
                            }
                        }
                        else
                        {
                            if (listShipment.Count() <= 0)
                            {
                                if (setTime < 2)
                                {
                                    setTime = setTime * 2;
                                }
                            }
                            else if (listShipment.Count() > 0)
                            {
                                setTime = 1;
                            }
                        }
                        Thread.Sleep(setTime * setTime * setTime * setTime * 10 * 1000);//
                    }
                    catch (Exception ex)
                    {
                        Thread.Sleep(1 * 30 * 1000);//
                        using (var context2 = new ApplicationContext())
                        {
                            var _unitOfWork2 = new UnitOfWork(context2);
                            _unitOfWork2.Repository<Proc_UpdateCountImageVSE>()
                            .ExecProcedureSingle(Proc_UpdateCountImageVSE.GetEntityProc(0, false, "PUSH IMAGE DELIVERY ERROR", ex.Message));
                        }
                    }
                }
            }
        }

        //
        public static void GetInfoBillGSDP()
        {
            int setTime = 1;
            while (true)
            {
                try
                {
                    GSDPApi gSDPApi = new GSDPApi();
                    string date = DateTime.Now.ToString("ddMMyyy");
                    var taskVSE = gSDPApi.GetBillInfo(date);
                    var infoBills = taskVSE.Result;
                    if (infoBills != null)
                    {
                        if (infoBills.Count > 0)
                        {
                            using (var context = new ApplicationContext())
                            {
                                var _unitOfWork = new UnitOfWork(context);

                                int countLoop = 0;
                                infoBills = infoBills.ToList();

                                List<GetKPIModel> listShipmentSAP = new List<GetKPIModel>();
                                foreach (var infoBill in infoBills)
                                {
                                    Console.WriteLine(infoBill);
                                    int createTime = int.Parse(infoBill.CreateTime);
                                    int currentTime = int.Parse(DateTime.Now.ToString("HHmm"));
                                    if (currentTime > (createTime + 1))
                                    {

                                        string shipmentNumber = "HDN" + infoBill.DocumentNo;
                                        var checkNumber = _unitOfWork.Repository<Proc_GetShipmentByShipmentNumber>()
                                                            .ExecProcedure(Proc_GetShipmentByShipmentNumber.GetEntityProc(shipmentNumber)).ToList();
                                        if (checkNumber.Count() == 0)
                                        {
                                            var shipment = new Shipment();
                                            if (infoBill != null)
                                            {
                                                bool isAllowInsert = false;
                                                //Save Log 
                                                string s = JsonConvert.SerializeObject(infoBill);
                                                var resultLog = _unitOfWork.Repository<Proc_SaveLogReceiveData>()
                                                                    .ExecProcedureSingle(Proc_SaveLogReceiveData.GetEntityProc(s, infoBill.DocumentNo));
                                                isAllowInsert = resultLog.IsSuccess;
                                                if (isAllowInsert == true)
                                                {
                                                    shipment.OrderDate = DateTime.Now;
                                                    shipment.ShipmentNumber = shipmentNumber;
                                                    shipment.PaymentTypeId = PaymentTypeHelper.THANH_TOAN_CUOI_THANG;
                                                    var sender = _unitOfWork.RepositoryR<Customer>().GetSingle(f => f.VSEOracleCode == infoBill.CustomerCode);
                                                    if (!Util.IsNull(sender))
                                                    {
                                                        shipment.FromProvinceId = sender.ProvinceId;
                                                        shipment.FromDistrictId = sender.DistrictId;
                                                        shipment.FromWardId = sender.WardId;
                                                        shipment.SenderId = sender.Id;
                                                        shipment.SenderName = sender.Name;
                                                        shipment.SenderPhone = sender.PhoneNumber;
                                                        shipment.PickingAddress = sender.Address;
                                                        if (shipment.FromWardId.HasValue)
                                                        {
                                                            var hubRoute = _unitOfWork.RepositoryR<HubRoute>().GetSingle(f => f.WardId == shipment.FromWardId);
                                                            if (hubRoute != null)
                                                            {
                                                                shipment.FromHubId = hubRoute.HubId;
                                                            }
                                                        }
                                                    };
                                                    //var receiver = _unitOfWork.RepositoryR<Customer>().GetSingle(f => f.VSEOracleCode == infoBill.ReceiverCode);
                                                    //if (!Util.IsNull(receiver)) shipment.ReceiverId = receiver.Id;
                                                    shipment.ReceiverName = infoBill.ReceiverName;
                                                    shipment.ReceiverPhone = infoBill.RecTel;
                                                    shipment.ShippingAddress = infoBill.RecAddress;
                                                    shipment.SOENTRY = infoBill.SOENTRY;
                                                    shipment.SODOCDATE = infoBill.SODOCDATE;
                                                    shipment.SODOCTIME = infoBill.SODOCTIME;
                                                    shipment.ProducerName = infoBill.FirmName;
                                                    var TimeConvert = infoBill.CreateTime.Insert(2, ":");
                                                    var convertDate = infoBill.ShipDate + " " + TimeConvert;
                                                    //DateTime myDate = DateTime.Parse(convertDate);
                                                    var myDate = DateTime.ParseExact(convertDate, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture);
                                                    shipment.ARDate = myDate;
                                                    shipment.GroupCode = infoBill.GroupCode;
                                                    shipment.GroupName = infoBill.GroupName;
                                                    //
                                                    if (!Util.IsNull(infoBill.Province))
                                                    {
                                                        var fromProvineC = infoBill.Province.Split('-')[0].Trim();
                                                        var fromProvine = _unitOfWork.RepositoryR<Province>().FindBy(f => f.VSEOracleCode == fromProvineC).FirstOrDefault();
                                                        if (!Util.IsNull(fromProvine))
                                                        {
                                                            shipment.FromProvinceId = fromProvine.Id;
                                                        }
                                                    }
                                                    //
                                                    string toProvinceName = ""; /*string toDistrictName = ""; string toWardName = "";*/


                                                    //
                                                    if (!Util.IsNull(infoBill.RecProvince))
                                                    {
                                                        var toProvineC = infoBill.RecProvince.Split('-')[0].Trim();
                                                        string toProName = infoBill.RecProvince.Split('-')[1].Trim();
                                                        var toProvine = _unitOfWork.RepositoryR<Province>().FindBy(f => f.VSEOracleCode == toProvineC).FirstOrDefault();
                                                        if (!Util.IsNull(toProvine))
                                                        {
                                                            shipment.ToProvinceId = toProvine.Id;
                                                            toProvinceName = toProvine.Name;
                                                        }
                                                        else
                                                        {
                                                            toProvinceName = toProName;
                                                            if (!Util.IsNull(toProvinceName))
                                                            {
                                                                var provinces = _unitOfWork.RepositoryCRUD<Province>().FindBy(o => o.CountryId == 1);
                                                                var provinceId = (int)StringHelper.GetBestMatches(provinces, "Id", "Name", toProvinceName, null,
                                                                                                                    StringHelper._REPLACES_LOCATION_NAME);
                                                                if (!Util.IsNull(provinceId))
                                                                {
                                                                    shipment.ToProvinceId = provinceId;
                                                                }
                                                            }
                                                        }

                                                    }
                                                    if (!Util.IsNull(infoBill.RecDistrict))
                                                    {
                                                        var toDistrictC = infoBill.RecDistrict.Split('-')[0].Trim();
                                                        var toDistrict = _unitOfWork.RepositoryR<District>().FindBy(f => f.VSEOracleCode == toDistrictC).FirstOrDefault();
                                                        if (!Util.IsNull(toDistrict))
                                                        {
                                                            shipment.ToProvinceId = toDistrict.Id;
                                                            toProvinceName = toDistrict.Name;
                                                        }
                                                    }
                                                    if (!Util.IsNull(infoBill.RecWard))
                                                    {
                                                        var toWardC = infoBill.RecWard.Split('-')[0].Trim();
                                                        var toWard = _unitOfWork.RepositoryR<Ward>().FindBy(f => f.VSEOracleCode == toWardC).FirstOrDefault();
                                                        if (!Util.IsNull(toWard))
                                                        {
                                                            shipment.ToProvinceId = toWard.Id;
                                                            toProvinceName = toWard.Name;
                                                        }
                                                    }

                                                    if (!Util.IsNull(infoBill.Route))
                                                    {
                                                        var route = _unitOfWork.RepositoryR<HubRouting>().GetSingle(f => f.CodeConnect == infoBill.Route);
                                                        if (!Util.IsNull(route))
                                                        {
                                                            shipment.ToHubRoutingId = route.Id;
                                                            shipment.ToHubId = route.HubId;
                                                        }
                                                        else
                                                        {
                                                            // break;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        // break;
                                                    }
                                                    //
                                                    shipment.CalWeight = 0;
                                                    shipment.Weight = infoBill.Weight;
                                                    shipment.TotalBox = infoBill.SoKien;
                                                    var service = _unitOfWork.RepositoryR<Service>().GetSingle(f => f.Code == "CPN" || f.Code == "cpn");
                                                    if (!Util.IsNull(service)) shipment.ServiceId = service.Id;
                                                    shipment.COD = infoBill.CODAmount;
                                                    var structure = _unitOfWork.RepositoryR<Structure>().GetSingle(f => f.Code == "HH" || f.Code == "hh");
                                                    if (!Util.IsNull(structure)) shipment.StructureId = structure.Id;
                                                    shipment.Insured = infoBill.DocTotal;
                                                    shipment.Content = infoBill.Contents;
                                                    shipment.CusNote = infoBill.DieuKienBaoQuan;
                                                    shipment.PickupNote = infoBill.MaKho;
                                                    shipment.Note = infoBill.Remark;
                                                    shipment.DeliveryNote = infoBill.Route;
                                                    shipment.ReceiverCode2 = infoBill.ReceiverCode;
                                                    //if (Util.IsNull(shipment.Note) || !shipment.Note.Contains("(API-A)")) shipment.Note = "(API-A)" + shipment.Note;
                                                    //
                                                    ShipmentCalculateViewModel sh = new ShipmentCalculateViewModel();
                                                    sh.COD = shipment.COD;
                                                    sh.DefaultPrice = shipment.DefaultPrice;
                                                    sh.FromDistrictId = shipment.FromDistrictId.Value;
                                                    sh.FromWardId = shipment.FromWardId.Value;
                                                    sh.Insured = shipment.Insured;
                                                    sh.IsAgreementPrice = shipment.IsAgreementPrice;
                                                    sh.OtherPrice = shipment.OtherPrice;
                                                    sh.SenderId = shipment.SenderId.Value;
                                                    if (!shipment.TotalItem.HasValue) shipment.TotalItem = 0;
                                                    if (Util.IsNull(sh.ServiceDVGTIds)) sh.ServiceDVGTIds = new List<int>();
                                                    if (sh.COD > 0)
                                                    {
                                                        var cod = _unitOfWork.RepositoryR<Service>().GetSingle(f => f.IsSub == true && (f.Code == "COD" || f.Code == "COD"));
                                                        if (!Util.IsNull(cod) && !sh.ServiceDVGTIds.Contains(cod.Id)) sh.ServiceDVGTIds.Add(cod.Id);
                                                    }
                                                    sh.Weight = shipment.Weight;
                                                    if (!Util.IsNull(shipment.CalWeight))
                                                    {
                                                        if (sh.Weight < shipment.CalWeight)
                                                            sh.Weight = shipment.CalWeight.Value;
                                                    }
                                                    sh.ServiceId = shipment.ServiceId.Value;
                                                    sh.StructureId = shipment.StructureId;
                                                    if (!Util.IsNull(shipment.ToDistrictId)) sh.ToDistrictId = shipment.ToDistrictId.Value;
                                                    else sh.ToDistrictId = 723; // Lấy mặc định 1 quận huyện để tính giá
                                                    if (shipment.ToWardId.HasValue) sh.ToWardId = shipment.ToWardId.Value;
                                                    sh.TotalItem = shipment.TotalItem.Value;
                                                    ResponseViewModel result = PriceUtil.Calculate(sh, "gsdp", true);
                                                    List<PriceDVGTViewModel> priceDVGTs = new List<PriceDVGTViewModel>();
                                                    if (result.IsSuccess == true)
                                                    {
                                                        PriceViewModel price = result.Data as PriceViewModel;
                                                        shipment.DefaultPrice = price.DefaultPrice;
                                                        shipment.TotalDVGT = price.TotalDVGT;
                                                        shipment.RemoteAreasPrice = price.RemoteAreasPrice;
                                                        shipment.FuelPrice = price.FuelPrice;
                                                        shipment.OtherPrice = price.OtherPrice;
                                                        shipment.VATPrice = price.VATPrice;
                                                        shipment.TotalPrice = price.TotalPrice;
                                                        shipment.PriceCOD = price.PriceCOD;
                                                        shipment.PriceReturn = price.PriceReturn;
                                                        shipment.TotalPriceSYS = price.TotalPriceSYS;
                                                        priceDVGTs = price.PriceDVGTs;
                                                    }
                                                    shipment.ShipmentStatusId = StatusHelper.ShipmentStatusId.NewRequest;

                                                    _unitOfWork.RepositoryCRUD<Shipment>().InsertNoneUser(shipment);
                                                    _unitOfWork.Commit();


                                                    countLoop++;
                                                    if ((priceDVGTs != null && priceDVGTs.Count() > 0))
                                                    {
                                                        foreach (var priceDVGT in priceDVGTs)
                                                        {
                                                            var ssDVGT = new ShipmentServiceDVGT();
                                                            ssDVGT.ShipmentId = shipment.Id;
                                                            ssDVGT.ServiceId = priceDVGT.ServiceId;
                                                            ssDVGT.IsAgree = priceDVGT.IsAgree;
                                                            ssDVGT.Price = priceDVGT.TotalPrice;
                                                            _unitOfWork.RepositoryCRUD<ShipmentServiceDVGT>().Insert(ssDVGT);
                                                        }
                                                        _unitOfWork.Commit();
                                                    }
                                                    if (shipment.Id == 0)
                                                    {
                                                        var getShipment = _unitOfWork.Repository<Proc_GetShipmentByShipmentNumber>()
                                    .ExecProcedureSingle(Proc_GetShipmentByShipmentNumber.GetEntityProc(shipmentNumber));
                                                        if (getShipment != null)
                                                        {
                                                            shipment.Id = getShipment.Id;
                                                        }
                                                    }
                                                    var lading = new LadingSchedule();
                                                    lading.ShipmentId = shipment.Id;
                                                    lading.ShipmentStatusId = shipment.ShipmentStatusId;
                                                    _unitOfWork.RepositoryCRUD<LadingSchedule>().InsertNoneUser(lading);
                                                    _unitOfWork.Commit();

                                                    // dua vao thoi gian hien tai => COT,TG XH, TG BD TT, TTTT, ....
                                                    // chay qua proc : paramst shipmentId, ARDate
                                                    // insert KPIShpmentSAP 
                                                    GetKPIModel obj = new GetKPIModel();
                                                    obj.ShipmentId = shipment.Id;
                                                    obj.ARDate = shipment.ARDate;
                                                    listShipmentSAP.Add(obj);
                                                    if (countLoop > 30) break;
                                                }
                                                //end add bill
                                            }
                                        }


                                    }
                                    else
                                    {
                                        //var a = 0; //delay 5 phút
                                    }
                                }

                                if (listShipmentSAP.Count > 0)
                                {
                                    InsertKPIByProc sqlUserList = new InsertKPIByProc();
                                    sqlUserList.AddRange(listShipmentSAP);
                                    try
                                    {
                                        var data = _unitOfWork.Repository<Proc_InserKPIShipmentSAP>()
                                                        .ExecProcedureSingle(Proc_InserKPIShipmentSAP.GetEntityProc(sqlUserList));
                                        if (data.Result == true)
                                        {
                                            foreach (var item in listShipmentSAP)
                                            {
                                                var res = new Proc_CalculateKPISAPExpectedTime();
                                                res = _unitOfWork.Repository<Proc_CalculateKPISAPExpectedTime>().
                                                    ExecProcedureSingle(Proc_CalculateKPISAPExpectedTime.GetEntityProc(item.ARDate, 1, item.ShipmentId));
                                                if (res != null)
                                                {
                                                    item.COT = res.COT;
                                                    item.KPIFullLading = res.KPIFullLading;
                                                    item.KPIFullLadingDay = res.KPIFullLadingDay;
                                                    item.KPIExportSAP = res.KPIExportSAP;
                                                    item.StartTransferTime = res.StartTransferTime;
                                                    item.KPITransfer = res.KPITransfer;
                                                    item.StartDeliveryTime = res.StartDeliveryTime;
                                                    item.KPIDelivery = res.KPIDelivery;
                                                    item.KPIPaymentMoney = res.KPIPaymentMoney;
                                                    item.KPIConfirmPaymentMoney = res.KPIConfirmPaymentMoney;

                                                }
                                            }

                                            InsertKPIByProc sqlUserList1 = new InsertKPIByProc();
                                            sqlUserList1.AddRange(listShipmentSAP);
                                            var dataUpdate = _unitOfWork.Repository<Proc_UpdateKPIDetail>()
                                            .ExecProcedureSingle(Proc_UpdateKPIDetail.GetEntityProc(sqlUserList1));
                                            Console.WriteLine(dataUpdate);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.Write(ex.Message);
                                    }
                                    ///////////////////////////////////////////////
                                }

                            }
                        }
                        /////////////////////////////////////////////
                        if (infoBills == null)
                        {
                            if (setTime < 2)
                            {
                                setTime = setTime * 2;
                            }
                        }
                        else
                        {
                            if (infoBills.Count() <= 0)
                            {
                                if (setTime < 2)
                                {
                                    setTime = setTime * 2;
                                }
                            }
                            else if (infoBills.Count() > 0)
                            {
                                setTime = 1;
                            }
                        }
                        Thread.Sleep(1 * 60 * 1000);
                        //Thread.Sleep(setTime * setTime * setTime * setTime * 10 * 1000);//
                    }
                    else
                    {
                        Thread.Sleep(1 * 60 * 1000);
                    }
                }
                catch (Exception ex)
                {
                    var a = ex.Message;
                    Thread.Sleep(1 * 60 * 1000);
                    //Thread.Sleep(1 * 30 * 1000);//
                }
            }
        }
        //GSDP STAGING
        public static void GetInfoBillGSDP_STAGING()
        {
            int setTime = 1;
            while (true)
            {
                try
                {
                    GSDPApi gSDPApi = new GSDPApi();
                    string date = DateTime.Now.ToString("ddMMyyyy");
                    var taskVSE = gSDPApi.GetBillInfoStaging(date);
                    var infoBills = taskVSE.Result;
                    if (infoBills != null)
                    {
                        if (infoBills.Count > 0)
                        {
                            int countLoop = 0;
                            infoBills = infoBills.ToList();
                            foreach (var infoBill in infoBills)
                            {
                                int createTime = int.Parse(infoBill.CreateTime);
                                int currentTime = int.Parse(DateTime.Now.ToString("HHmm"));
                                if (currentTime > (createTime + 1))
                                {
                                    using (var context = new ApplicationContext())
                                    {
                                        string shipmentNumber = "HDN" + infoBill.DocumentNo;
                                        var _unitOfWork = new UnitOfWork(context);
                                        var checkNumber = _unitOfWork.Repository<Proc_GetShipmentByShipmentNumber>()
                    .ExecProcedure(Proc_GetShipmentByShipmentNumber.GetEntityProc(shipmentNumber)).ToList();
                                        if (checkNumber.Count() == 0)
                                        {
                                            var shipment = new Shipment();
                                            if (infoBill != null)
                                            {
                                                bool isAllowInsert = false;
                                                //Save Log
                                                using (var context3 = new ApplicationContext())
                                                {
                                                    string s = JsonConvert.SerializeObject(infoBill);
                                                    var _unitOfWork3 = new UnitOfWork(context3);
                                                    var resultLog = _unitOfWork3.Repository<Proc_SaveLogReceiveData>()
                                                    .ExecProcedureSingle(Proc_SaveLogReceiveData.GetEntityProc(s, infoBill.DocumentNo));
                                                    isAllowInsert = resultLog.IsSuccess;
                                                }
                                                if (isAllowInsert == true)
                                                {
                                                    //
                                                    //if(infoBill.ShipDate.HasValue) shipment.OrderDate = infoBill.ShipDate.Value;
                                                    //else shipment.OrderDate = DateTime.Now;
                                                    shipment.OrderDate = DateTime.Now;
                                                    shipment.ShipmentNumber = shipmentNumber;
                                                    shipment.PaymentTypeId = PaymentTypeHelper.THANH_TOAN_CUOI_THANG;
                                                    var sender = _unitOfWork.RepositoryR<Customer>().GetSingle(f => f.VSEOracleCode == infoBill.CustomerCode);
                                                    if (!Util.IsNull(sender))
                                                    {
                                                        shipment.FromProvinceId = sender.ProvinceId;
                                                        shipment.FromDistrictId = sender.DistrictId;
                                                        shipment.FromWardId = sender.WardId;
                                                        shipment.SenderId = sender.Id;
                                                        shipment.SenderName = sender.Name;
                                                        shipment.SenderPhone = sender.PhoneNumber;
                                                        shipment.PickingAddress = sender.Address;
                                                        if (shipment.FromWardId.HasValue)
                                                        {
                                                            var hubRoute = _unitOfWork.RepositoryR<HubRoute>().GetSingle(f => f.WardId == shipment.FromWardId);
                                                            if (hubRoute != null)
                                                            {
                                                                shipment.FromHubId = hubRoute.HubId;
                                                            }
                                                        }
                                                    };
                                                    //var receiver = _unitOfWork.RepositoryR<Customer>().GetSingle(f => f.VSEOracleCode == infoBill.ReceiverCode);
                                                    //if (!Util.IsNull(receiver)) shipment.ReceiverId = receiver.Id;
                                                    shipment.ReceiverName = infoBill.ReceiverName;
                                                    shipment.ReceiverPhone = infoBill.RecTel;
                                                    shipment.ShippingAddress = infoBill.RecAddress;
                                                    shipment.SOENTRY = infoBill.SOENTRY;
                                                    shipment.SODOCDATE = infoBill.SODOCDATE;
                                                    shipment.SODOCTIME = infoBill.SODOCTIME;
                                                    shipment.ProducerName = infoBill.FirmName;
                                                    shipment.ARDate = infoBill.CreateDate;
                                                    shipment.GroupCode = infoBill.GroupCode;
                                                    shipment.GroupName = infoBill.GroupName;
                                                    //
                                                    if (!Util.IsNull(infoBill.Province))
                                                    {
                                                        var fromProvineC = infoBill.Province.Split('-')[0].Trim();
                                                        var fromProvine = _unitOfWork.RepositoryR<Province>().FindBy(f => f.VSEOracleCode == fromProvineC).FirstOrDefault();
                                                        if (!Util.IsNull(fromProvine))
                                                        {
                                                            shipment.FromProvinceId = fromProvine.Id;
                                                        }
                                                    }
                                                    //
                                                    string toProvinceName = ""; /*string toDistrictName = ""; string toWardName = "";*/
                                                    //
                                                    if (!Util.IsNull(infoBill.RecProvince))
                                                    {
                                                        var toProvineC = infoBill.RecProvince.Split('-')[0].Trim();
                                                        var toProvine = _unitOfWork.RepositoryR<Province>().FindBy(f => f.VSEOracleCode == toProvineC).FirstOrDefault();
                                                        if (!Util.IsNull(toProvine))
                                                        {
                                                            shipment.ToProvinceId = toProvine.Id;
                                                            toProvinceName = toProvine.Name;
                                                        }
                                                    }
                                                    //if (!Util.IsNull(infoBill.RecAddress))
                                                    //{
                                                    //    string[] arrAddress = infoBill.RecAddress.Split(',');
                                                    //    for (int i = (arrAddress.Count() - 1); i >= 0; i--)
                                                    //    {
                                                    //        if (i == arrAddress.Count() - 1) toProvinceName = arrAddress[i];
                                                    //        if (i == arrAddress.Count() - 2) toDistrictName = arrAddress[i];
                                                    //        if (i == arrAddress.Count() - 3) toWardName = arrAddress[i];
                                                    //    }
                                                    //    if (!Util.IsNull(toProvinceName))
                                                    //    {
                                                    //        var provinces = _unitOfWork.RepositoryCRUD<Province>().FindBy(o => o.CountryId == 1);
                                                    //        var provinceId = (int)StringHelper.GetBestMatches(provinces, "Id", "Name", toProvinceName, null,
                                                    //                                                            StringHelper._REPLACES_LOCATION_NAME);
                                                    //        if (!Util.IsNull(provinceId))
                                                    //        {
                                                    //            shipment.ToProvinceId = provinceId;
                                                    //        }
                                                    //    }
                                                    //    //
                                                    //    if (shipment.ToProvinceId.HasValue && !!Util.IsNull(toDistrictName))
                                                    //    {
                                                    //        var districts = _unitOfWork.RepositoryCRUD<District>().FindBy(o => o.ProvinceId == shipment.ToProvinceId);
                                                    //        var districtId = (int)StringHelper.GetBestMatches(districts, "Id", "Name", toDistrictName, null,
                                                    //                                                            StringHelper._REPLACES_LOCATION_NAME);
                                                    //        if (!Util.IsNull(districtId))
                                                    //        {
                                                    //            shipment.ToDistrictId = districtId;
                                                    //        }
                                                    //    }
                                                    //    //
                                                    //    if (shipment.ToDistrictId.HasValue && !Util.IsNull(toWardName))
                                                    //    {
                                                    //        var wards = _unitOfWork.RepositoryCRUD<Ward>().FindBy(o => o.DistrictId == shipment.ToDistrictId);
                                                    //        if (!Util.IsNull(wards))
                                                    //        {
                                                    //            if (wards.Count() > 0)
                                                    //            {
                                                    //                var wardId = (int)StringHelper.GetBestMatches(wards, "Id", "Name", toWardName, null,
                                                    //                                                                    StringHelper._REPLACES_LOCATION_NAME);
                                                    //                if (!Util.IsNull(wardId))
                                                    //                {
                                                    //                    shipment.ToWardId = wardId;
                                                    //                }
                                                    //            }
                                                    //        }
                                                    //    }
                                                    //}
                                                    //
                                                    if (!Util.IsNull(infoBill.Route))
                                                    {
                                                        var route = _unitOfWork.RepositoryR<HubRouting>().GetSingle(f => f.CodeConnect == infoBill.Route);
                                                        if (!Util.IsNull(route))
                                                        {
                                                            shipment.ToHubRoutingId = route.Id;
                                                            shipment.ToHubId = route.HubId;
                                                        }
                                                        else
                                                        {
                                                            // break;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        // break;
                                                    }
                                                    //
                                                    shipment.CalWeight = 0;
                                                    shipment.Weight = infoBill.Weight;
                                                    var service = _unitOfWork.RepositoryR<Service>().GetSingle(f => f.Code == "CPN" || f.Code == "cpn");
                                                    if (!Util.IsNull(service)) shipment.ServiceId = service.Id;
                                                    shipment.COD = infoBill.CODAmount;
                                                    var structure = _unitOfWork.RepositoryR<Structure>().GetSingle(f => f.Code == "HH" || f.Code == "hh");
                                                    if (!Util.IsNull(structure)) shipment.StructureId = structure.Id;
                                                    shipment.TotalBox = 1;
                                                    shipment.Insured = infoBill.DocTotal;
                                                    shipment.Content = infoBill.Contents;
                                                    shipment.CusNote = infoBill.DieuKienBaoQuan;
                                                    shipment.PickupNote = infoBill.MaKho;
                                                    shipment.Note = infoBill.Remark;
                                                    shipment.DeliveryNote = infoBill.Route;
                                                    shipment.ReceiverCode2 = infoBill.ReceiverCode;
                                                    //if (Util.IsNull(shipment.Note) || !shipment.Note.Contains("(API-A)")) shipment.Note = "(API-A)" + shipment.Note;
                                                    //
                                                    ShipmentCalculateViewModel sh = new ShipmentCalculateViewModel();
                                                    sh.COD = shipment.COD;
                                                    sh.DefaultPrice = shipment.DefaultPrice;
                                                    sh.FromDistrictId = shipment.FromDistrictId;
                                                    sh.FromWardId = shipment.FromWardId;
                                                    sh.Insured = shipment.Insured;
                                                    sh.IsAgreementPrice = shipment.IsAgreementPrice;
                                                    sh.OtherPrice = shipment.OtherPrice;
                                                    sh.SenderId = shipment.SenderId.Value;
                                                    if (!shipment.TotalItem.HasValue) shipment.TotalItem = 0;
                                                    if (Util.IsNull(sh.ServiceDVGTIds)) sh.ServiceDVGTIds = new List<int>();
                                                    if (sh.COD > 0)
                                                    {
                                                        var cod = _unitOfWork.RepositoryR<Service>().GetSingle(f => f.IsSub == true && (f.Code == "COD" || f.Code == "COD"));
                                                        if (!Util.IsNull(cod) && !sh.ServiceDVGTIds.Contains(cod.Id)) sh.ServiceDVGTIds.Add(cod.Id);
                                                    }
                                                    sh.Weight = shipment.Weight;
                                                    if (!Util.IsNull(shipment.CalWeight))
                                                    {
                                                        if (sh.Weight < shipment.CalWeight)
                                                            sh.Weight = shipment.CalWeight.Value;
                                                    }
                                                    sh.ServiceId = shipment.ServiceId.Value;
                                                    sh.StructureId = shipment.StructureId;
                                                    if (!Util.IsNull(shipment.ToDistrictId)) sh.ToDistrictId = shipment.ToDistrictId.Value;
                                                    else sh.ToDistrictId = 723; // Lấy mặc định 1 quận huyện để tính giá
                                                    if (shipment.ToWardId.HasValue) sh.ToWardId = shipment.ToWardId.Value;
                                                    sh.TotalItem = shipment.TotalItem.Value;
                                                    ResponseViewModel result = PriceUtil.Calculate(sh, "gsdp", true);
                                                    List<PriceDVGTViewModel> priceDVGTs = new List<PriceDVGTViewModel>();
                                                    if (result.IsSuccess == true)
                                                    {
                                                        PriceViewModel price = result.Data as PriceViewModel;
                                                        shipment.DefaultPrice = price.DefaultPrice;
                                                        shipment.TotalDVGT = price.TotalDVGT;
                                                        shipment.RemoteAreasPrice = price.RemoteAreasPrice;
                                                        shipment.FuelPrice = price.FuelPrice;
                                                        shipment.OtherPrice = price.OtherPrice;
                                                        shipment.VATPrice = price.VATPrice;
                                                        shipment.TotalPrice = price.TotalPrice;
                                                        shipment.PriceCOD = price.PriceCOD;
                                                        shipment.PriceReturn = price.PriceReturn;
                                                        shipment.TotalPriceSYS = price.TotalPriceSYS;
                                                        priceDVGTs = price.PriceDVGTs;
                                                    }
                                                    shipment.ShipmentStatusId = StatusHelper.ShipmentStatusId.NewRequest;
                                                    _unitOfWork.RepositoryCRUD<Shipment>().InsertNoneUser(shipment);
                                                    _unitOfWork.Commit();
                                                    countLoop++;
                                                    if ((priceDVGTs != null && priceDVGTs.Count() > 0))
                                                    {
                                                        foreach (var priceDVGT in priceDVGTs)
                                                        {
                                                            var ssDVGT = new ShipmentServiceDVGT();
                                                            ssDVGT.ShipmentId = shipment.Id;
                                                            ssDVGT.ServiceId = priceDVGT.ServiceId;
                                                            ssDVGT.IsAgree = priceDVGT.IsAgree;
                                                            ssDVGT.Price = priceDVGT.TotalPrice;
                                                            _unitOfWork.RepositoryCRUD<ShipmentServiceDVGT>().Insert(ssDVGT);
                                                        }
                                                        _unitOfWork.Commit();
                                                    }
                                                    if (shipment.Id == 0)
                                                    {
                                                        var getShipment = _unitOfWork.Repository<Proc_GetShipmentByShipmentNumber>()
                                    .ExecProcedureSingle(Proc_GetShipmentByShipmentNumber.GetEntityProc(shipmentNumber));
                                                        if (getShipment != null)
                                                        {
                                                            shipment.Id = getShipment.Id;
                                                        }
                                                    }
                                                    var lading = new LadingSchedule();
                                                    lading.ShipmentId = shipment.Id;
                                                    lading.ShipmentStatusId = shipment.ShipmentStatusId;
                                                    _unitOfWork.RepositoryCRUD<LadingSchedule>().InsertNoneUser(lading);
                                                    _unitOfWork.Commit();
                                                    if (countLoop > 30) break;
                                                }
                                                //end add bill
                                            }
                                        }

                                    }
                                }
                                else
                                {
                                    //var a = 0; //delay 5 phút
                                }
                            }
                        }
                        if (infoBills == null)
                        {
                            if (setTime < 2)
                            {
                                setTime = setTime * 2;
                            }
                        }
                        else
                        {
                            if (infoBills.Count() <= 0)
                            {
                                if (setTime < 2)
                                {
                                    setTime = setTime * 2;
                                }
                            }
                            else if (infoBills.Count() > 0)
                            {
                                setTime = 1;
                            }
                        }
                        Thread.Sleep(2 * 60 * 1000);
                        //Thread.Sleep(setTime * setTime * setTime * setTime * 10 * 1000);//
                    }
                    else
                    {
                        Thread.Sleep(2 * 60 * 1000);
                    }
                }
                catch (Exception ex)
                {
                    var a = ex.Message;
                    Thread.Sleep(2 * 60 * 1000);
                    //Thread.Sleep(1 * 30 * 1000);//
                }
            }
        }
        //
        public static void GetInfoBillReturnGSDP()
        {
            while (true)
            {
                try
                {
                    GSDPApi gSDPApi = new GSDPApi();
                    string date = DateTime.Now.ToString("ddMMyyy");
                    var taskVSE = gSDPApi.GetBillReturn(date);
                    var infoBills = taskVSE.Result;
                    if (infoBills != null)
                    {
                        if (infoBills.Count > 0)
                        {
                            int countLoop = 0;
                            infoBills = infoBills.ToList();
                            foreach (var infoBill in infoBills)
                            {
                                using (var context = new ApplicationContext())
                                {
                                    string shipmentNumber = "HDN" + infoBill.ARNo;
                                    var _unitOfWork = new UnitOfWork(context);
                                    var checkNumbers = _unitOfWork.Repository<Proc_GetShipmentByShipmentNumber>()
                .ExecProcedure(Proc_GetShipmentByShipmentNumber.GetEntityProc(shipmentNumber)).ToList();
                                    if (checkNumbers.Count() > 0)
                                    {
                                        var checkNumber = checkNumbers[0];
                                        var shipment = _unitOfWork.RepositoryCRUD<Shipment>().GetSingle(checkNumber.Id);
                                        if (!shipment.ARReturnDate.HasValue)
                                        {
                                            //Save Log
                                            using (var context3 = new ApplicationContext())
                                            {
                                                string s = JsonConvert.SerializeObject(infoBill);
                                                var _unitOfWork3 = new UnitOfWork(context3);
                                                var resultLog = _unitOfWork3.Repository<Proc_SaveLogReceiveData>()
                                                .ExecProcedureSingle(Proc_SaveLogReceiveData.GetEntityProc(s, infoBill.ARNo + "_BILL_RETURN"));
                                            }
                                            shipment.ARReturnDate = infoBill.ARCreditMemoDocDate;
                                            _unitOfWork.Commit();
                                            if (countLoop > 50) break;
                                        }
                                    }
                                    //end add bill
                                }
                            }
                        }
                        Thread.Sleep(30 * 60 * 1000);
                    }
                    else
                    {
                        Thread.Sleep(30 * 60 * 1000);
                    }
                }
                catch (Exception ex)
                {
                    var a = ex.Message;
                    Thread.Sleep(30 * 60 * 1000);
                }
            }
        }

        public static void GetInfoBillReturnGSDPStaging()
        {
            while (true)
            {
                try
                {
                    GSDPApi gSDPApi = new GSDPApi();
                    string date = DateTime.Now.ToString("ddMMyyy");
                    var taskVSE = gSDPApi.GetBillReturn(date);
                    var infoBills = taskVSE.Result;
                    if (infoBills != null)
                    {
                        if (infoBills.Count > 0)
                        {
                            int countLoop = 0;
                            infoBills = infoBills.ToList();
                            foreach (var infoBill in infoBills)
                            {
                                using (var context = new ApplicationContext())
                                {
                                    string shipmentNumber = "HDN" + infoBill.ARNo;
                                    var _unitOfWork = new UnitOfWork(context);
                                    var checkNumbers = _unitOfWork.Repository<Proc_GetShipmentByShipmentNumber>()
                .ExecProcedure(Proc_GetShipmentByShipmentNumber.GetEntityProc(shipmentNumber)).ToList();
                                    if (checkNumbers.Count() > 0)
                                    {
                                        var checkNumber = checkNumbers[0];
                                        var shipment = _unitOfWork.RepositoryCRUD<Shipment>().GetSingle(checkNumber.Id);
                                        if (!shipment.ARReturnDate.HasValue)
                                        {
                                            //Save Log
                                            using (var context3 = new ApplicationContext())
                                            {
                                                string s = JsonConvert.SerializeObject(infoBill);
                                                var _unitOfWork3 = new UnitOfWork(context3);
                                                var resultLog = _unitOfWork3.Repository<Proc_SaveLogReceiveData>()
                                                .ExecProcedureSingle(Proc_SaveLogReceiveData.GetEntityProc(s, infoBill.ARNo + "_BILL_RETURN"));
                                            }
                                            shipment.ARReturnDate = infoBill.ARCreditMemoDocDate;
                                            _unitOfWork.Commit();
                                            if (countLoop > 50) break;
                                        }
                                    }
                                    //end add bill
                                }
                            }
                        }
                        Thread.Sleep(30 * 60 * 1000);
                    }
                    else
                    {
                        Thread.Sleep(30 * 60 * 1000);
                    }
                }
                catch (Exception ex)
                {
                    var a = ex.Message;
                    Thread.Sleep(30 * 60 * 1000);
                }
            }
        }
        //
        public static void PushStatusLazada()
        {

            int setTime = 1;
            while (true)
            {
                using (var context = new ApplicationContext())
                {
                    var _unitOfWork = new UnitOfWork(context);
                    try
                    {
                        // Id = 11 Lazada
                        var listStatus = _unitOfWork.Repository<Proc_GetShipmentStatusPush>()
                            .ExecProcedure(Proc_GetShipmentStatusPush.GetEntityProc(11));
                        foreach (var status in listStatus)
                        {
                            try
                            {
                                //PUSH 
                                if (!status.Lat.HasValue) status.Lat = 0;
                                if (!status.Lng.HasValue) status.Lng = 0;
                                //log
                                if (Util.IsNull(status.ReasonCode)) status.ReasonCode = "";
                                if (Util.IsNull(status.Location)) status.Location = "";
                                if (Util.IsNull(status.RealRecipientName)) status.RealRecipientName = "";
                                if (Util.IsNull(status.UserFullName)) status.UserFullName = "";
                                if (Util.IsNull(status.UserNumberPhone)) status.UserNumberPhone = "";
                                string _signature = "";
                                string _linkTracking = "";
                                if (status.StatusId == StatusHelper.ShipmentStatusId.DeliveryComplete || status.StatusId == StatusHelper.ShipmentStatusId.ReturnComplete)
                                {
                                    _signature = string.Format("https://be-express-tracker.be.xyz/order-image/{0}", status.ShipmentNumber);
                                }
                                if (status.StatusId == StatusHelper.ShipmentStatusId.Delivering)
                                {
                                    _linkTracking = string.Format("https://be-express-tracker.be.xyz/order-tracker/{0}", status.ShipmentNumber);
                                }
                                //
                                var objContent = JsonConvert.SerializeObject(new
                                {
                                    tracking_number = status.ShipmentNumber,
                                    status = status.StatusCode,
                                    reason_code = status.ReasonCode,
                                    datetime = status.Timestamp.ToString(),
                                    location = status.Location,
                                    latitude = status.Lat.ToString(),
                                    longitude = status.Lng.ToString(),
                                    driver_name = status.UserFullName,
                                    driver_contact = status.UserNumberPhone,
                                    receiver_name = status.RealRecipientName,
                                    receiver_signature = _signature,
                                    tracking_url = _linkTracking
                                });
                                var logData = objContent.ToString();
                                //
                                ApiLazada apiLazada = new ApiLazada();
                                var taskRes = apiLazada.PushStatusToLazada(status.ShipmentNumber, status.StatusCode, status.ReasonCode,
                                    status.Timestamp.ToString(), status.Location, status.Lat.ToString(), status.Lng.ToString(),
                                    status.RealRecipientName, _signature,
                                    status.UserFullName, status.UserNumberPhone, _linkTracking
                                );
                                var res = taskRes.Result;
                                bool checkResult = false;
                                int res_status_code = 0;
                                string res_message = "-";
                                if (!Util.IsNull(res))
                                {
                                    if (res.status_code == 202)
                                    {
                                        checkResult = true;
                                    }
                                    res_status_code = res.status_code;
                                    res_message = res.message;
                                }
                                string message = string.Format("Lazada - LadingScheduleId:{0}, ShipmentNumber: {1}, StatusCode: {2}, Message: {3}, shipmentStatusCode: {4}, timestamp: {5}", status.Id, status.ShipmentNumber, res_status_code, res_message, status.StatusCode, status.Timestamp);
                                using (var context2 = new ApplicationContext())
                                {
                                    var _unitOfWork2 = new UnitOfWork(context2);
                                    _unitOfWork2.Repository<Proc_UpdateCountPushLazada>()
                                    .ExecProcedureSingle(Proc_UpdateCountPushLazada.GetEntityProc(status.Id, checkResult, logData, message));
                                }
                            }
                            catch (Exception ex)
                            {
                                var a = ex.Message;
                                continue;
                            }
                        }
                        if (listStatus == null)
                        {
                            if (setTime < 2)
                            {
                                setTime = setTime * 2;
                            }
                        }
                        else
                        {
                            if (listStatus.Count() <= 0)
                            {
                                if (setTime < 2)
                                {
                                    setTime = setTime * 2;
                                }
                            }
                            else if (listStatus.Count() > 0)
                            {
                                setTime = 1;
                            }
                        }
                        _unitOfWork.Repository<Proc_TaskScheduler>()
                        .ExecProcedureSingle(Proc_TaskScheduler.GetEntityProc("Push Statusn Shipment Run Task Push Lazada auto - OAB", setTime, ""));
                        _unitOfWork.Commit();
                        Thread.Sleep(setTime * setTime * setTime * setTime * 10 * 1000);//
                    }
                    catch (Exception ex)
                    {
                        _unitOfWork.Repository<Proc_TaskScheduler>()
                         .ExecProcedureSingle(Proc_TaskScheduler.GetEntityProc("Push Statusn Shipment Run Task Push Lazada auto - OAB", setTime, "Error: " + ex.Message));
                        _unitOfWork.Commit();
                        Thread.Sleep(1 * 30 * 1000);//
                    }
                }
            }
        }

        public static void PushStatusADayRoi()
        {

            int setTime = 1;
            while (true)
            {
                using (var context = new ApplicationContext())
                {
                    var _unitOfWork = new UnitOfWork(context);
                    try
                    {
                        // Id = 13 A Day Roi
                        var listStatus = _unitOfWork.Repository<Proc_GetShipmentStatusPush>()
                            .ExecProcedure(Proc_GetShipmentStatusPush.GetEntityProc(13)); // A Day Roi
                        foreach (var status in listStatus)
                        {
                            try
                            {
                                //PUSH 
                                string eventDatetime = "";
                                if (status.ModifiedWhen.HasValue)
                                {
                                    eventDatetime = status.ModifiedWhen.Value.ToString("yyyy-MM-dd HH:mm:ss");
                                }
                                else
                                {
                                    eventDatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                }
                                if (!status.Lat.HasValue) status.Lat = 0;
                                if (!status.Lng.HasValue) status.Lng = 0;
                                //
                                var objContent = JsonConvert.SerializeObject(new
                                {
                                    shipmentNumber = status.ShipmentNumber,
                                    shipmentStatusId = status.StatusId,
                                    shipmentStatusName = status.StatusName,
                                    receiverName = status.RealRecipientName,
                                    eventDate = eventDatetime,
                                    ladingSchedules = new List<object>()
                                });
                                //
                                ApiADayRoi apiADayRoi = new ApiADayRoi();
                                var taskRes = apiADayRoi.PushStatusToADayRoi(status.ShipmentNumber, status.StatusId, status.StatusName, status.RealRecipientName, eventDatetime);
                                var res = taskRes.Result;
                                bool checkResult = false;
                                int res_status_code = 0;
                                string res_message = "-";
                                if (!Util.IsNull(res))
                                {
                                    if (res.status == 200)
                                    {
                                        checkResult = true;
                                    }
                                    res_status_code = res.status;
                                    res_message = res.message;
                                }
                                string message = string.Format("ADayRoi - LadingScheduleId:{0}, ShipmentNumber: {1}, StatusCode: {2}, Message: {3}", status.Id, status.ShipmentNumber, res_status_code, res_message);
                                using (var context2 = new ApplicationContext())
                                {
                                    var _unitOfWork2 = new UnitOfWork(context2);
                                    _unitOfWork2.Repository<Proc_UpdateCountPushLazada>()
                                    .ExecProcedureSingle(Proc_UpdateCountPushLazada.GetEntityProc(status.Id, checkResult, objContent.ToString(), message));
                                }
                            }
                            catch (Exception ex)
                            {
                                var a = ex.Message;
                                continue;
                            }
                        }
                        if (listStatus == null)
                        {
                            if (setTime < 2)
                            {
                                setTime = setTime * 2;
                            }
                        }
                        else
                        {
                            if (listStatus.Count() <= 0)
                            {
                                if (setTime < 2)
                                {
                                    setTime = setTime * 2;
                                }
                            }
                            else if (listStatus.Count() > 0)
                            {
                                setTime = 1;
                            }
                        }
                        _unitOfWork.Repository<Proc_TaskScheduler>()
                        .ExecProcedureSingle(Proc_TaskScheduler.GetEntityProc("Push Statusn Shipment Run Task Push ADayRoi auto - OAB", setTime, ""));
                        _unitOfWork.Commit();
                        Thread.Sleep(setTime * setTime * setTime * setTime * 10 * 1000);//
                    }
                    catch (Exception ex)
                    {
                        _unitOfWork.Repository<Proc_TaskScheduler>()
                         .ExecProcedureSingle(Proc_TaskScheduler.GetEntityProc("Push Statusn Shipment Run Task Push ADayRoi auto - OAB", setTime, "Error: " + ex.Message));
                        _unitOfWork.Commit();
                        Thread.Sleep(1 * 30 * 1000);//
                    }
                }
            }
        }

        public static void PushStatusShipNhanh()
        {

            int setTime = 1;
            while (true)
            {
                using (var context = new ApplicationContext())
                {
                    var _unitOfWork = new UnitOfWork(context);
                    try
                    {
                        // Id = 13 A Day Roi
                        var listStatus = _unitOfWork.Repository<Proc_GetShipmentStatusPush>()
                            .ExecProcedure(Proc_GetShipmentStatusPush.GetEntityProc(77)); // SHIPNHANH
                        foreach (var status in listStatus)
                        {
                            try
                            {
                                //PUSH 
                                string eventDatetime = "";
                                if (status.ModifiedWhen.HasValue)
                                {
                                    eventDatetime = status.ModifiedWhen.Value.ToString("yyyy-MM-dd HH:mm:ss");
                                }
                                else
                                {
                                    eventDatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                }
                                if (!status.Lat.HasValue) status.Lat = 0;
                                if (!status.Lng.HasValue) status.Lng = 0;
                                //
                                var objContent = JsonConvert.SerializeObject(new
                                {
                                    shipmentNumber = status.ShipmentNumber,
                                    shipmentStatusId = status.StatusId,
                                    shipmentStatusName = status.StatusName,
                                    receiverName = status.RealRecipientName,
                                    eventDate = eventDatetime,
                                    ladingSchedules = new List<object>()
                                });
                                //
                                ApiShipNhanh shipShipNhanh = new ApiShipNhanh();
                                var taskRes = shipShipNhanh.PushStatusToShipNhanh(status.ShipmentNumber, status.StatusId, status.StatusName, status.RealRecipientName, eventDatetime, status.Location);
                                var res = taskRes.Result;
                                bool checkResult = false;
                                string res_message = "-";
                                if (!Util.IsNull(res))
                                {
                                    if (res.isSuccess == true)
                                    {
                                        checkResult = true;
                                    }
                                    res_message = res.message;
                                }
                                string message = string.Format("ShipNhanh - LadingScheduleId:{0}, ShipmentNumber: {1}, IsSuccess: {2}, Message: {3}", status.Id, status.ShipmentNumber, checkResult, res_message);
                                using (var context2 = new ApplicationContext())
                                {
                                    var _unitOfWork2 = new UnitOfWork(context2);
                                    _unitOfWork2.Repository<Proc_UpdateCountPushLazada>()
                                    .ExecProcedureSingle(Proc_UpdateCountPushLazada.GetEntityProc(status.Id, checkResult, objContent.ToString(), message));
                                }
                            }
                            catch (Exception ex)
                            {
                                var a = ex.Message;
                                continue;
                            }
                        }
                        if (listStatus == null)
                        {
                            if (setTime < 2)
                            {
                                setTime = setTime * 2;
                            }
                        }
                        else
                        {
                            if (listStatus.Count() <= 0)
                            {
                                if (setTime < 2)
                                {
                                    setTime = setTime * 2;
                                }
                            }
                            else if (listStatus.Count() > 0)
                            {
                                setTime = 1;
                            }
                        }
                        _unitOfWork.Repository<Proc_TaskScheduler>()
                        .ExecProcedureSingle(Proc_TaskScheduler.GetEntityProc("Push Status Shipment Run Task Push ShipNhanh auto - OAB", setTime, ""));
                        _unitOfWork.Commit();
                        Thread.Sleep(setTime * setTime * setTime * setTime * 10 * 1000);//
                    }
                    catch (Exception ex)
                    {
                        _unitOfWork.Repository<Proc_TaskScheduler>()
                         .ExecProcedureSingle(Proc_TaskScheduler.GetEntityProc("Push Statusn Shipment Run Task Push ADayRoi auto - OAB", setTime, "Error: " + ex.Message));
                        _unitOfWork.Commit();
                        Thread.Sleep(1 * 30 * 1000);//
                    }
                }
            }
        }
    }
}
