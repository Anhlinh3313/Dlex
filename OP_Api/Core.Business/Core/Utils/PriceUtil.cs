using Core.Business.ViewModels;
using Core.Data;
using Core.Data.Core;
using Core.Entity.Entities;
using Core.Entity.Procedures;
using Core.Infrastructure.Helper;
using Core.Infrastructure.Utils;
using Core.Infrastructure.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Business.Core.Utils
{
    public static class PriceUtil
    {
        public static ResponseViewModel Calculate(ShipmentCalculateViewModel viewModel, string companyName, bool isVAT = false)
        {
            if (Util.IsNull(viewModel))
            {
                return ResponseViewModel.CreateError("Thông tin vận đơn trống!");
            }
            if (Util.IsNull(viewModel.ToDistrictId) && Util.IsNull(viewModel.ToWardId))
            {
                return ResponseViewModel.CreateError("Chưa có Quận/Huyện, Phường/Xã nhận. Không thể tính giá!");
            }
            if (viewModel.Length < 0) viewModel.Length = 0;
            if (viewModel.Width < 0) viewModel.Width = 0;
            if (viewModel.Height < 0) viewModel.Height = 0;
            if (viewModel.Weight < viewModel.CalWeight) viewModel.Weight = viewModel.CalWeight;
            if (!Util.IsNull(viewModel.priceDVGTs))
            {
                if (viewModel.priceDVGTs.Count() > 0)
                {
                    viewModel.ServiceDVGTIds = viewModel.priceDVGTs.Where(w => w.ServiceId.HasValue).Select(s => s.ServiceId.Value).ToList();
                }
            }
            if (!viewModel.PriceReturn.HasValue) viewModel.PriceReturn = 0;
            if (viewModel.PricingTypeId == 0) viewModel.PricingTypeId = 1;
            if (Util.IsNull(viewModel.FromWardId)) viewModel.FromWardId = 0;
            using (var context = new ApplicationContext())
            {
                var unitOfWork = new UnitOfWork(context);
                if (viewModel.FromDistrictId == 0 || viewModel.FromDistrictId == null)
                {
                    if (viewModel.FromWardId > 0)
                    {
                        var ward = unitOfWork.RepositoryR<Ward>().GetSingle(viewModel.FromWardId.Value);
                        if (!Util.IsNull(ward))
                            viewModel.FromDistrictId = ward.DistrictId;
                    }
                }
                PriceViewModel price = new PriceViewModel();
                price.Weight = viewModel.Weight;
                price.LastWeight = viewModel.Weight;
                price.CalWeight = viewModel.CalWeight;
                if (price.CalWeight > viewModel.Weight)
                {
                    price.LastWeight = viewModel.CalWeight;
                    viewModel.Weight = viewModel.CalWeight;
                }
                //DEADLINE
                if (Util.IsNull(viewModel.OrderDate)) viewModel.OrderDate = DateTime.Now;
                if (Util.IsNull(viewModel.FromHubId)) viewModel.FromHubId = 0;
                //
                double VAT = 0;
                double Fuel = 0;
                double Remote = 0;
                double DIM = 0;
                price.OtherPrice = viewModel.OtherPrice;
                //
                var listPriceServiceDetails = unitOfWork.Repository<Proc_PriceService>()
                  .ExecProcedure(Proc_PriceService.GetEntityProc(0, viewModel.SenderId, viewModel.FromProvinceId, viewModel.FromDistrictId, viewModel.ServiceId, viewModel.ToDistrictId, viewModel.Weight, viewModel.StructureId, viewModel.PricingTypeId, viewModel.TotalItem, viewModel.Insured, viewModel.PriceServiceId, viewModel.COD, viewModel.Distance)).ToList();
                //
                if (Util.IsNull(listPriceServiceDetails))
                {
                    return ResponseViewModel.CreateError(ValidatorMessage.Calculator.NotFoundServicePrice);
                }
                if (listPriceServiceDetails.Count() == 0)
                {
                    return ResponseViewModel.CreateError(ValidatorMessage.Calculator.NotFoundServicePrice);
                }
                #region DefaultPricePublic 
                var priceServiceDetailPublic = listPriceServiceDetails.Where(f => f.IsPublic == true).FirstOrDefault();
                if (!Util.IsNull(priceServiceDetailPublic))
                {
                    if (priceServiceDetailPublic.Price == 0)
                    {
                        price.DefaultPriceP = priceServiceDetailPublic.Price;
                    }
                    else
                    {
                        //
                        var formula = unitOfWork.RepositoryR<Formula>().GetSingle(g => g.Id == priceServiceDetailPublic.FormulaId);
                        if (formula == null)
                        {
                            price.DefaultPriceP = 0;
                        }
                        else
                        {
                            //
                            if (formula.Code.ToUpper() == FormulaPrice.StandardPrice.ToString().ToUpper())
                            {
                                price.DefaultPriceP = priceServiceDetailPublic.Price;
                            }
                            else if (formula.Code.ToUpper() == FormulaPrice.MultiplyWeight.ToString().ToUpper())
                            {
                                price.DefaultPriceP = priceServiceDetailPublic.Price * viewModel.Weight;
                            }
                            else if (formula.Code.ToUpper() == FormulaPrice.MultiplyProduct.ToString().ToUpper())
                            {
                                price.DefaultPriceP = priceServiceDetailPublic.Price * viewModel.TotalItem;
                            }
                            else if (formula.Code.ToUpper() == FormulaPrice.MultiplyProduct.ToString().ToUpper())
                            {
                                price.DefaultPriceP = priceServiceDetailPublic.Price * viewModel.TotalItem;
                            }
                            else if (formula.Code.ToUpper() == FormulaPrice.MultiplyInsured.ToString().ToUpper())
                            {
                                price.DefaultPriceP = priceServiceDetailPublic.Price * viewModel.Insured / 100;
                            }
                            else if (formula.Code.ToUpper() == FormulaPrice.MultiplyCOD.ToString().ToUpper())
                            {
                                price.DefaultPriceP = priceServiceDetailPublic.Price * viewModel.COD / 100;
                            }
                            else if (formula.Code.ToUpper() == FormulaPrice.PlusPrice.ToString().ToUpper())
                            {
                                using (var context2 = new ApplicationContext())
                                {
                                    var unitOfWork2 = new UnitOfWork(context2);
                                    var listPricePlusServiceDetails = unitOfWork2.Repository<Proc_PriceService>()
                                    .ExecProcedure(Proc_PriceService.GetEntityProc(1, viewModel.SenderId, viewModel.FromProvinceId, viewModel.FromDistrictId, viewModel.ServiceId, viewModel.ToDistrictId, priceServiceDetailPublic.WeightFrom, viewModel.StructureId, viewModel.PricingTypeId, priceServiceDetailPublic.WeightFrom, priceServiceDetailPublic.WeightFrom, viewModel.PriceServiceId, priceServiceDetailPublic.WeightFrom, priceServiceDetailPublic.WeightFrom)).ToList();
                                    if (listPricePlusServiceDetails != null)
                                    {
                                        if (listPricePlusServiceDetails.Count() > 0)
                                        {
                                            var priceServiceDetailFirst = listPricePlusServiceDetails.FirstOrDefault();
                                            var formulaFirst = unitOfWork.RepositoryR<Formula>().GetSingle(g => g.Id == priceServiceDetailFirst.FormulaId);
                                            //
                                            if (formulaFirst.Code.ToUpper() == FormulaPrice.StandardPrice.ToString().ToUpper())
                                            {
                                                price.DefaultPriceP = priceServiceDetailFirst.Price;
                                            }
                                            else if (formulaFirst.Code.ToUpper() == FormulaPrice.MultiplyWeight.ToString().ToUpper())
                                            {
                                                price.DefaultPriceP = priceServiceDetailFirst.Price * priceServiceDetailFirst.WeightTo;
                                            }
                                            else if (formulaFirst.Code.ToUpper() == FormulaPrice.MultiplyProduct.ToString().ToUpper())
                                            {
                                                price.DefaultPriceP = priceServiceDetailFirst.Price * priceServiceDetailFirst.WeightTo;
                                            }
                                            //
                                            if (priceServiceDetailPublic.WeightPlus > 0)
                                            {
                                                double valuePlus = 0;
                                                if (priceServiceDetailPublic.PricingTypeId == CalculateByHelper.Weight) valuePlus = viewModel.Weight - priceServiceDetailFirst.WeightTo;
                                                else if (priceServiceDetailPublic.PricingTypeId == CalculateByHelper.DeclareValue) valuePlus = viewModel.Insured - priceServiceDetailFirst.WeightTo;
                                                else if (priceServiceDetailPublic.PricingTypeId == CalculateByHelper.COD) valuePlus = viewModel.COD - priceServiceDetailFirst.WeightTo;
                                                else if (priceServiceDetailPublic.PricingTypeId == CalculateByHelper.Items) valuePlus = viewModel.TotalItem - priceServiceDetailFirst.WeightTo;

                                                if (priceServiceDetailPublic.IsKeepWeight)
                                                {
                                                    price.DefaultPriceP += (valuePlus / priceServiceDetailPublic.WeightPlus * priceServiceDetailPublic.Price);
                                                }
                                                else
                                                {
                                                    double plus = 0;
                                                    plus = Math.Round((valuePlus / priceServiceDetailPublic.WeightPlus), 0);
                                                    if (priceServiceDetailPublic.ValueTo > priceServiceDetailPublic.ValueFrom && priceServiceDetailPublic.ValueTo > 0)
                                                    {
                                                        valuePlus = viewModel.TotalItem - priceServiceDetailFirst.WeightTo;
                                                        plus = Math.Round((valuePlus / priceServiceDetailPublic.WeightPlus), 0);
                                                    }
                                                    if (valuePlus > (plus * priceServiceDetailPublic.WeightPlus))
                                                    {
                                                        plus++;
                                                    }
                                                    price.DefaultPriceP += priceServiceDetailPublic.Price * plus;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        // VAT PUBLIC
                        var VATP = priceServiceDetailPublic.VATPercent;
                        // DefaultPrice
                        price.DefaultPriceP = Math.Round(price.DefaultPriceP * (1 + VATP / 100), 1);
                    }
                }
                #endregion
                //DefaultPrice
                double KmRemoteDefault = 0;
                //double defaultPrice = 0;
                if (listPriceServiceDetails != null)
                {
                    var priceServiceDetail = listPriceServiceDetails.FirstOrDefault();
                    if (!Util.IsNull(priceServiceDetail))
                    {
                        #region deadline delivery
                        if (priceServiceDetail.ExpectedTime > 0)
                        {
                            var pickupTime = 0;
                            var checkPickupTime = viewModel.OrderDate.Value.Hour;
                            if (checkPickupTime < 8) pickupTime = 8 - checkPickupTime;
                            else if (checkPickupTime == 12) pickupTime = 1;
                            else if (checkPickupTime >= 17) pickupTime = 24 - checkPickupTime + 8;
                            var datetime = viewModel.OrderDate.Value.AddHours(pickupTime);
                            //check holiday pickup
                            var startDate = datetime.AddDays(-1);
                            var endDate = datetime.AddDays(30);
                            var dataHolidays = unitOfWork.RepositoryR<Holiday>().FindBy(f => f.Date >= startDate && f.Date <= endDate).ToList();
                            var isPickupHoliday = true;
                            while (isPickupHoliday == true && dataHolidays.Count() > 0)
                            {
                                var checkHoliday = dataHolidays.Select(f => f.Date.Month == datetime.Month && f.Date.Day == datetime.Day).FirstOrDefault();
                                if (datetime.DayOfWeek == DayOfWeek.Sunday || checkHoliday == true)
                                {
                                    datetime = datetime.AddDays(1);
                                    datetime = datetime.AddHours(8 - datetime.Hour);
                                }
                                else
                                {
                                    isPickupHoliday = false;
                                }
                            }
                            var pickupDate = datetime;
                            DateTime deadlineDelivery = datetime.AddHours(priceServiceDetail.ExpectedTime);
                            //
                            var time = deadlineDelivery.Hour;
                            if (time < 9)
                            {
                                deadlineDelivery = deadlineDelivery.AddHours(9 - time);
                            }
                            else if (time == 12)
                            {
                                deadlineDelivery = deadlineDelivery.AddHours(1);
                            }
                            else if (time > 16)
                            {
                                deadlineDelivery = deadlineDelivery.AddHours(24 - time + 9);
                            }
                            //check delivery holiday
                            while (pickupDate.Month <= deadlineDelivery.Month && pickupDate.Day <= deadlineDelivery.Day)
                            {
                                var checkHoliday = dataHolidays.Select(f => f.Date.Month == pickupDate.Month && f.Date.Day == pickupDate.Day).FirstOrDefault();
                                if (pickupDate.DayOfWeek == DayOfWeek.Sunday || checkHoliday == true)
                                {
                                    deadlineDelivery = deadlineDelivery.AddDays(1);
                                }
                                pickupDate = pickupDate.AddDays(1);
                            }
                            price.DeadlineDelivery = deadlineDelivery;
                            //
                        }
                        #endregion
                        price.PriceService = priceServiceDetail;
                        if (priceServiceDetail.Price == 0)
                        {
                            price.DefaultPrice = priceServiceDetail.Price;
                        }
                        else
                        {
                            //
                            var formula = unitOfWork.RepositoryR<Formula>().GetSingle(g => g.Id == priceServiceDetail.FormulaId);
                            if (formula == null)
                            {
                                return ResponseViewModel.CreateError(ValidatorMessage.Calculator.NotFoundPriceList);
                            }
                            //
                            if (formula.Code.ToUpper() == FormulaPrice.StandardPrice.ToString().ToUpper())
                            {
                                price.DefaultPrice = priceServiceDetail.Price;
                            }
                            else if (formula.Code.ToUpper() == FormulaPrice.MultiplyWeight.ToString().ToUpper())
                            {
                                price.DefaultPrice = priceServiceDetail.Price * viewModel.Weight;
                            }
                            else if (formula.Code.ToUpper() == FormulaPrice.MultiplyProduct.ToString().ToUpper())
                            {
                                price.DefaultPrice = priceServiceDetail.Price * viewModel.TotalItem;
                            }
                            else if (formula.Code.ToUpper() == FormulaPrice.MultiplyInsured.ToString().ToUpper())
                            {
                                price.DefaultPrice = priceServiceDetail.Price * viewModel.Insured / 100;
                            }
                            else if (formula.Code.ToUpper() == FormulaPrice.MultiplyCOD.ToString().ToUpper())
                            {
                                price.DefaultPrice = priceServiceDetail.Price * viewModel.COD / 100;
                            }
                            else if (formula.Code.ToUpper() == FormulaPrice.MultiplyBox.ToString().ToUpper())
                            {
                                price.DefaultPrice = priceServiceDetail.Price * viewModel.TotalBox;
                            }
                            else if (formula.Code.ToUpper() == FormulaPrice.MultiplyKm.ToString().ToUpper())
                            {
                                price.TotalPrice = priceServiceDetail.Price * viewModel.Distance;
                            }
                            else if (formula.Code.ToUpper() == FormulaPrice.PlusPrice.ToString().ToUpper())
                            {
                                using (var context2 = new ApplicationContext())
                                {
                                    var unitOfWork2 = new UnitOfWork(context2);
                                    var listPricePlusServiceDetails = unitOfWork2.Repository<Proc_PriceService>()
                                    .ExecProcedure(Proc_PriceService.GetEntityProc(1, viewModel.SenderId, viewModel.FromProvinceId, viewModel.FromDistrictId, viewModel.ServiceId, viewModel.ToDistrictId, priceServiceDetail.WeightFrom, viewModel.StructureId, viewModel.PricingTypeId, priceServiceDetail.WeightFrom, priceServiceDetail.WeightFrom, viewModel.PriceServiceId, priceServiceDetail.WeightFrom, priceServiceDetail.WeightFrom)).ToList();
                                    if (listPricePlusServiceDetails == null)
                                    {
                                        return ResponseViewModel.CreateError(ValidatorMessage.Calculator.SetupWeightPlusError);
                                    }
                                    if (listPricePlusServiceDetails.Count() == 0)
                                    {
                                        return ResponseViewModel.CreateError(ValidatorMessage.Calculator.SetupWeightPlusError);
                                    }
                                    var priceServiceDetailFirst = listPricePlusServiceDetails.FirstOrDefault();
                                    var formulaFirst = unitOfWork.RepositoryR<Formula>().GetSingle(g => g.Id == priceServiceDetailFirst.FormulaId);
                                    //
                                    if (formulaFirst.Code.ToUpper() == FormulaPrice.StandardPrice.ToString().ToUpper())
                                    {
                                        price.DefaultPrice = priceServiceDetailFirst.Price;
                                    }
                                    else if (formulaFirst.Code.ToUpper() == FormulaPrice.MultiplyWeight.ToString().ToUpper())
                                    {
                                        price.DefaultPrice = priceServiceDetailFirst.Price * priceServiceDetailFirst.WeightTo;
                                    }
                                    else if (formulaFirst.Code.ToUpper() == FormulaPrice.MultiplyProduct.ToString().ToUpper())
                                    {
                                        price.DefaultPrice = priceServiceDetailFirst.Price * priceServiceDetailFirst.WeightTo;
                                    }
                                    else if (formulaFirst.Code.ToUpper() == FormulaPrice.MultiplyBox.ToString().ToUpper())
                                    {
                                        price.DefaultPrice = priceServiceDetailFirst.Price * priceServiceDetailFirst.WeightTo;
                                    }
                                    else if (formulaFirst.Code.ToUpper() == FormulaPrice.MultiplyKm.ToString().ToUpper())
                                    {
                                        price.DefaultPrice = priceServiceDetailFirst.Price * priceServiceDetailFirst.WeightTo;
                                    }
                                    //
                                    if (priceServiceDetail.WeightPlus > 0)
                                    {
                                        double plus = 0;
                                        double valuePlus = 0;
                                        if (priceServiceDetail.PricingTypeId == CalculateByHelper.Weight) valuePlus = viewModel.Weight - priceServiceDetailFirst.WeightTo;
                                        else if (priceServiceDetail.PricingTypeId == CalculateByHelper.DeclareValue) valuePlus = viewModel.Insured - priceServiceDetailFirst.WeightTo;
                                        else if (priceServiceDetail.PricingTypeId == CalculateByHelper.COD) valuePlus = viewModel.COD - priceServiceDetailFirst.WeightTo;
                                        else if (priceServiceDetail.PricingTypeId == CalculateByHelper.Items) valuePlus = viewModel.TotalItem - priceServiceDetailFirst.WeightTo;
                                        else if (priceServiceDetail.PricingTypeId == CalculateByHelper.KM) valuePlus = viewModel.Distance - priceServiceDetailFirst.WeightTo;

                                        if (priceServiceDetail.IsKeepWeight)
                                        {
                                            price.DefaultPrice += (valuePlus / priceServiceDetail.WeightPlus * priceServiceDetail.Price);
                                        }
                                        else
                                        {
                                            plus = Math.Round((valuePlus / priceServiceDetail.WeightPlus), 0);
                                            if (priceServiceDetail.ValueTo > priceServiceDetail.ValueFrom && priceServiceDetail.ValueTo > 0)
                                            {
                                                valuePlus = viewModel.TotalItem - priceServiceDetailFirst.WeightTo;
                                                plus = Math.Round((valuePlus / priceServiceDetail.WeightPlus), 0);
                                            }
                                            if (valuePlus > (plus * priceServiceDetail.WeightPlus))
                                            {
                                                plus++;
                                            }
                                            price.DefaultPrice += priceServiceDetail.Price * plus;
                                        }
                                    }
                                }
                            }
                            price.DefaultPrice = Math.Round(price.DefaultPrice, 0);
                            // VAT
                            VAT = priceServiceDetail.VATPercent;
                            Fuel = priceServiceDetail.FuelPercent;
                            Remote = priceServiceDetail.RemoteAreasPricePercent;
                            DIM = priceServiceDetail.DIM;
                            //
                            var checkMap = unitOfWork.RepositoryR<CustomerPriceService>().GetSingle(f => f.CustomerId == viewModel.SenderId && f.PriceServiceId == priceServiceDetail.Id);
                            if (!Util.IsNull(checkMap))
                            {
                                VAT = checkMap.VATPercent.Value;
                                Fuel = checkMap.FuelPercent.Value;
                                Remote = checkMap.RemoteAreasPricePercent.Value;
                                DIM = checkMap.DIM.Value;
                            }
                            //
                            price.VATPercent = VAT;
                            price.FuelPercent = Fuel;
                            price.RemotePercent = Remote;
                            price.DIM = DIM;
                            // DefaultPrice
                            price.DefaultPrice = price.DefaultPrice + Math.Round(price.DefaultPrice * (VAT / 100), 1);
                            //Fuel price
                            price.FuelPrice = Math.Round(Fuel * price.DefaultPrice / 100, 1);
                            //Remote price
                            var getInfoRemote = unitOfWork.Repository<Proc_GetInfoRemote>().ExecProcedureSingle(Proc_GetInfoRemote.GetEntityProc(viewModel.ToDistrictId, viewModel.ToWardId));
                            if (!Util.IsNull(getInfoRemote))
                            {
                                KmRemoteDefault = getInfoRemote.KmNumber;
                                if (getInfoRemote.RemoteServiceId.HasValue)
                                {
                                    if (Util.IsNull(viewModel.ServiceDVGTIds)) viewModel.ServiceDVGTIds = new List<int>();
                                    if (!viewModel.ServiceDVGTIds.Contains(getInfoRemote.RemoteServiceId.Value) && Remote <= 0)
                                    {
                                        viewModel.ServiceDVGTIds.Add(getInfoRemote.RemoteServiceId.Value);
                                    }
                                }
                            }
                            //
                        }
                    }
                }
                //CUOC DVGT
                price.PriceDVGTs = new List<PriceDVGTViewModel>();
                //DVGT
                if (viewModel.COD > 0)
                {
                    var serviceCOD = unitOfWork.RepositoryR<Service>().FindBy(f => f.IsSub == true && (f.Code == "COD" || f.VSEOracleCode == "COD" || f.VSEOracleCode == "02")).FirstOrDefault();
                    if (!Util.IsNull(serviceCOD))
                    {
                        if (Util.IsNull(viewModel.ServiceDVGTIds))
                        {
                            viewModel.ServiceDVGTIds = new List<int>();
                            viewModel.ServiceDVGTIds.Add(serviceCOD.Id);
                        }
                        else
                        {
                            if (!viewModel.ServiceDVGTIds.Contains(serviceCOD.Id))
                            {
                                viewModel.ServiceDVGTIds.Add(serviceCOD.Id);
                            }

                        }
                    }
                }
                if (viewModel.ServiceDVGTIds != null)
                {
                    if (viewModel.ServiceDVGTIds.Count() > 0)
                    {
                        var dataDVGT = unitOfWork.RepositoryR<Service>().FindBy(g => viewModel.ServiceDVGTIds.Contains(g.Id));
                        foreach (var item in dataDVGT)
                        {
                            if (item.Code.ToUpper() == "PTT" || item.Code.ToUpper() == "VSVX") price.RemoteAreasPrice = 0;
                            PriceDVGTViewModel dvgt = new PriceDVGTViewModel();
                            dvgt.ServiceId = item.Id;
                            dvgt.Code = item.Code;
                            dvgt.Name = item.Name;
                            dvgt.VSEOracleCode = item.VSEOracleCode;
                            bool isAgree = false;
                            if (!Util.IsNull(viewModel.priceDVGTs))
                            {
                                var priceDVGT = viewModel.priceDVGTs.FirstOrDefault(f => f.ServiceId == dvgt.ServiceId);
                                if (!Util.IsNull(priceDVGT))
                                {
                                    if (priceDVGT.IsAgree == true)
                                    {
                                        isAgree = true;
                                        dvgt.TotalPrice = priceDVGT.TotalPrice;
                                        dvgt.IsAgree = priceDVGT.IsAgree;
                                    }
                                }
                            }
                            if (isAgree == false)
                            {
                                using (var contextPlus = new ApplicationContext())
                                {
                                    var unitOfWorkPlus = new UnitOfWork(contextPlus);
                                    var dataDVGTPrice = unitOfWorkPlus.Repository<Proc_PriceService>()
                                        .ExecProcedureSingle(Proc_PriceService.GetEntityProc(0, viewModel.SenderId, viewModel.FromProvinceId, viewModel.FromDistrictId,
                                        item.Id, viewModel.ToDistrictId, viewModel.CalWeight, viewModel.StructureId, viewModel.PaymentTypeId, viewModel.TotalItem, viewModel.Insured, viewModel.PriceServiceId, viewModel.COD, KmRemoteDefault));
                                    if (dataDVGTPrice != null)
                                    {
                                        dvgt.PriceServiceCode = dataDVGTPrice.Code;
                                        var formulaPrice = unitOfWork.RepositoryR<Formula>().GetSingle(g => g.Id == dataDVGTPrice.FormulaId);
                                        if (formulaPrice != null)
                                        {
                                            //
                                            if (formulaPrice.Code.ToUpper() == FormulaPrice.StandardPrice.ToString().ToUpper())
                                            {
                                                dvgt.TotalPrice = dataDVGTPrice.Price;
                                            }
                                            else if (formulaPrice.Code.ToUpper() == FormulaPrice.MultiplyWeight.ToString().ToUpper())
                                            {
                                                dvgt.TotalPrice = dataDVGTPrice.Price * viewModel.Weight;
                                            }
                                            else if (formulaPrice.Code.ToUpper() == FormulaPrice.MultiplyProduct.ToString().ToUpper())
                                            {
                                                dvgt.TotalPrice = dataDVGTPrice.Price * viewModel.TotalItem;
                                            }
                                            else if (formulaPrice.Code.ToUpper() == FormulaPrice.MultiplyInsured.ToString().ToUpper())
                                            {
                                                dvgt.TotalPrice = dataDVGTPrice.Price * viewModel.Insured / 100;
                                            }
                                            else if (formulaPrice.Code.ToUpper() == FormulaPrice.MultiplyCOD.ToString().ToUpper())
                                            {
                                                dvgt.TotalPrice = dataDVGTPrice.Price * viewModel.COD / 100;
                                            }
                                            else if (formulaPrice.Code.ToUpper() == FormulaPrice.PercentDefault.ToString().ToUpper())
                                            {
                                                dvgt.TotalPrice = dataDVGTPrice.Price * price.DefaultPrice / 100;
                                            }
                                            else if (formulaPrice.Code.ToUpper() == FormulaPrice.MultiplyBox.ToString().ToUpper())
                                            {
                                                dvgt.TotalPrice = dataDVGTPrice.Price * viewModel.TotalBox;
                                            }
                                            else if (formulaPrice.Code.ToUpper() == FormulaPrice.MultiplyKm.ToString().ToUpper())
                                            {
                                                dvgt.TotalPrice = dataDVGTPrice.Price * KmRemoteDefault;
                                            }
                                            else if (formulaPrice.Code.ToUpper() == FormulaPrice.PlusPrice.ToString().ToUpper())
                                            {
                                                using (var context2 = new ApplicationContext())
                                                {
                                                    var unitOfWork2 = new UnitOfWork(context2);
                                                    var priceServiceDetailFirst = unitOfWork2.Repository<Proc_PriceService>()
                                                    .ExecProcedureSingle(Proc_PriceService.GetEntityProc(1, viewModel.SenderId, viewModel.FromProvinceId, viewModel.FromDistrictId, item.Id, viewModel.ToDistrictId, dataDVGTPrice.WeightFrom, viewModel.StructureId, viewModel.PricingTypeId, dataDVGTPrice.WeightFrom, dataDVGTPrice.WeightFrom, viewModel.PriceServiceId, dataDVGTPrice.WeightFrom, dataDVGTPrice.WeightFrom));
                                                    if (priceServiceDetailFirst != null)
                                                    {
                                                        var formulaFirst = unitOfWork.RepositoryR<Formula>().GetSingle(g => g.Id == priceServiceDetailFirst.FormulaId);
                                                        //
                                                        if (formulaFirst.Code.ToUpper() == FormulaPrice.StandardPrice.ToString().ToUpper())
                                                        {
                                                            dvgt.TotalPrice = priceServiceDetailFirst.Price;
                                                        }
                                                        else if (formulaFirst.Code.ToUpper() == FormulaPrice.MultiplyWeight.ToString().ToUpper())
                                                        {
                                                            dvgt.TotalPrice = priceServiceDetailFirst.Price * priceServiceDetailFirst.WeightTo;
                                                        }
                                                        else if (formulaFirst.Code.ToUpper() == FormulaPrice.MultiplyProduct.ToString().ToUpper())
                                                        {
                                                            dvgt.TotalPrice = priceServiceDetailFirst.Price * priceServiceDetailFirst.WeightTo;
                                                        }
                                                        else if (formulaPrice.Code.ToUpper() == FormulaPrice.MultiplyBox.ToString().ToUpper())
                                                        {
                                                            dvgt.TotalPrice = priceServiceDetailFirst.Price * priceServiceDetailFirst.WeightTo;
                                                        }
                                                        else if (formulaFirst.Code.ToUpper() == FormulaPrice.MultiplyKm.ToString().ToUpper())
                                                        {
                                                            dvgt.TotalPrice = priceServiceDetailFirst.Price * priceServiceDetailFirst.WeightTo;
                                                        }
                                                        //
                                                        if (dataDVGTPrice.WeightPlus > 0)
                                                        {
                                                            double plus = 0;
                                                            double valuePlus = 0;
                                                            if (priceServiceDetailFirst.PricingTypeId == CalculateByHelper.Weight) valuePlus = viewModel.Weight - priceServiceDetailFirst.WeightTo;
                                                            else if (priceServiceDetailFirst.PricingTypeId == CalculateByHelper.DeclareValue) valuePlus = viewModel.Insured - priceServiceDetailFirst.WeightTo;
                                                            else if (priceServiceDetailFirst.PricingTypeId == CalculateByHelper.COD) valuePlus = viewModel.COD - priceServiceDetailFirst.WeightTo;
                                                            else if (priceServiceDetailFirst.PricingTypeId == CalculateByHelper.Items) valuePlus = viewModel.TotalItem - priceServiceDetailFirst.WeightTo;
                                                            else if (priceServiceDetailFirst.PricingTypeId == CalculateByHelper.KM) valuePlus = KmRemoteDefault - priceServiceDetailFirst.WeightTo;

                                                            if (dataDVGTPrice.IsKeepWeight)
                                                            {
                                                                dvgt.TotalPrice += (valuePlus / dataDVGTPrice.WeightPlus * dataDVGTPrice.Price);
                                                            }
                                                            else
                                                            {
                                                                plus = Math.Round((valuePlus / dataDVGTPrice.WeightPlus), 0);
                                                                if (dataDVGTPrice.ValueTo > dataDVGTPrice.ValueFrom && dataDVGTPrice.ValueTo > 0)
                                                                {
                                                                    valuePlus = viewModel.TotalItem - priceServiceDetailFirst.WeightTo;
                                                                    plus = Math.Round((valuePlus / dataDVGTPrice.WeightPlus), 0);
                                                                }
                                                                if (valuePlus > (plus * dataDVGTPrice.WeightPlus))
                                                                {
                                                                    plus++;
                                                                }
                                                                dvgt.TotalPrice += dataDVGTPrice.Price * plus;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        dvgt.TotalPrice = dvgt.TotalPrice + (dvgt.TotalPrice * dataDVGTPrice.VATPercent / 100);
                                    }
                                }
                            }
                            dvgt.TotalPrice = Math.Round(dvgt.TotalPrice, 0);
                            if (dvgt.Code.ToUpper() != "COD" && dvgt.Code.ToUpper() != "02" && dvgt.Code.ToUpper() != "VSVX" && dvgt.Code.ToUpper() != "PTT") price.PriceDVGTs.Add(dvgt);
                            else if (dvgt.Code.ToUpper() == "COD" || dvgt.Code.ToUpper() == "02")
                            {
                                price.PriceCOD = dvgt.TotalPrice;
                                dvgt.TotalPrice = 0;
                                price.PriceDVGTs.Add(dvgt);
                            }
                            else if (dvgt.Code.ToUpper() == "VSVX" || dvgt.Code.ToUpper() == "PTT")
                            {
                                price.RemoteAreasPrice = dvgt.TotalPrice;
                                dvgt.TotalPrice = 0;
                                //price.PriceDVGTs.Add(dvgt);
                            }
                        }
                    }
                }
                if (price.PriceDVGTs != null)
                {
                    if (price.PriceDVGTs.Count > 0)
                    {
                        price.TotalDVGT = price.PriceDVGTs.Sum(s => s.TotalPrice);
                    }
                }
                //
                if (viewModel.IsAgreementPrice == true)
                {
                    price.FuelPrice = 0;
                    price.RemoteAreasPrice = 0;
                    price.DefaultPrice = viewModel.DefaultPrice;
                }
                var total = price.DefaultPrice + price.FuelPrice + price.VATPrice + price.OtherPrice + price.TotalDVGT + price.RemoteAreasPrice;
                if (viewModel.DisCount > 0) total = total - (total * viewModel.DisCount / 100);
                price.PriceReturn = viewModel.PriceReturn.Value;
                if (viewModel.IsReturn == true)
                {
                    if (companyName == "gsdp" || companyName == "gsdp-staging")
                    {
                        price.PriceReturn = Math.Round(price.DefaultPrice * 0.5, 0);
                    }
                    else
                    {
                        var serviceReturn = unitOfWork.RepositoryR<Service>().FindBy(f => f.IsReturn == true).FirstOrDefault();
                        if (!Util.IsNull(serviceReturn))
                        {
                            using (var context3 = new ApplicationContext())
                            {
                                var unitOfWork3 = new UnitOfWork(context3);
                                var priceServiceReturn = unitOfWork3.Repository<Proc_PriceService>()
                                .ExecProcedureSingle(Proc_PriceService.GetEntityProc(0, viewModel.SenderId, viewModel.FromProvinceId, viewModel.FromDistrictId, serviceReturn.Id, viewModel.ToDistrictId, viewModel.Weight, viewModel.StructureId, viewModel.PricingTypeId, viewModel.TotalItem, viewModel.Insured, viewModel.PriceServiceId, viewModel.COD, viewModel.Distance));
                                if (priceServiceReturn != null)
                                {
                                    //
                                    var formulaFirstReturn = unitOfWork.RepositoryR<Formula>().GetSingle(g => g.Id == priceServiceReturn.FormulaId);
                                    //
                                    if (formulaFirstReturn.Code.ToUpper() == FormulaPrice.StandardPrice.ToString().ToUpper())
                                    {
                                        price.PriceReturn = priceServiceReturn.Price;
                                    }
                                    else if (formulaFirstReturn.Code.ToUpper() == FormulaPrice.MultiplyWeight.ToString().ToUpper())
                                    {
                                        price.PriceReturn = priceServiceReturn.Price * viewModel.Weight;
                                    }
                                    else if (formulaFirstReturn.Code.ToUpper() == FormulaPrice.MultiplyProduct.ToString().ToUpper())
                                    {
                                        price.PriceReturn = priceServiceReturn.Price * viewModel.TotalItem;
                                    }
                                    else if (formulaFirstReturn.Code.ToUpper() == FormulaPrice.MultiplyBox.ToString().ToUpper())
                                    {
                                        price.PriceReturn = priceServiceReturn.Price * viewModel.TotalBox;
                                    }
                                    else if (formulaFirstReturn.Code.ToUpper() == FormulaPrice.MultiplyKm.ToString().ToUpper())
                                    {
                                        price.PriceReturn = priceServiceReturn.Price * viewModel.Distance;
                                    }
                                    else if (formulaFirstReturn.Code.ToUpper() == FormulaPrice.PercentDefault.ToString().ToUpper())
                                    {
                                        price.PriceReturn = priceServiceReturn.Price * (price.DefaultPrice + price.VATPrice) / 100;
                                    }
                                }
                            }
                        }
                    }
                    viewModel.PriceReturn = price.PriceReturn;
                }
                if (viewModel.PriceReturn > 0) total += viewModel.PriceReturn.Value;
                price.TotalPrice = Math.Round(total, 0);
                return ResponseViewModel.CreateSuccess(price);
            }
        }

        private static double getPriceByFormula(double value, Formula formula, int? calculateById = 0,
            double? weight = 0, double? m3 = 0, double? insured = 0,
            double? cod = 0, double? items = 0, int? shipments = 1, double? weightWood = 0)
        {
            double price = 0;
            double CalValue = 0;
            if (calculateById == CalculateByHelper.Weight) CalValue = weight.Value;
            else if (calculateById == CalculateByHelper.M3) CalValue = m3.Value;
            else if (calculateById == CalculateByHelper.DeclareValue) CalValue = insured.Value;
            else if (calculateById == CalculateByHelper.COD) CalValue = cod.Value;
            else if (calculateById == CalculateByHelper.Items) CalValue = items.Value;
            else if (calculateById == CalculateByHelper.Shipments) CalValue = shipments.Value;
            //else if (calculateById == CalculateByHelper.WeightWood) CalValue = weightWood.Value;
            if (formula.Code.ToUpper() == FormulaPrice.StandardPrice.ToString().ToUpper())
            {
                price = value;
            }
            else if (formula.Code.ToUpper() == FormulaPrice.MultiplyWeight.ToString().ToUpper())
            {
                price = value * CalValue;
            }
            else if (formula.Code.ToUpper() == FormulaPrice.MultiplyInsured.ToString().ToUpper())
            {
                price = value * CalValue / 100;
            }
            else if (formula.Code.ToUpper() == FormulaPrice.MultiplyCOD.ToString().ToUpper())
            {
                price = value * CalValue / 100;
            }
            else if (formula.Code.ToUpper() == FormulaPrice.MultiplyPackage.ToString().ToUpper())
            {
                price = value * CalValue;
            }
            else if (formula.Code.ToUpper() == FormulaPrice.MultiplyProduct.ToString().ToUpper())
            {
                price = value * CalValue;
            }
            else if (formula.Code.ToUpper() == FormulaPrice.MultiplyM3.ToString().ToUpper())
            {
                price = value * CalValue;
            }
            else if (formula.Code.ToUpper() == FormulaPrice.PercentDefault.ToString().ToUpper())
            {
                price = value * CalValue / 100;
            }
            return price;
        }
    }
}
