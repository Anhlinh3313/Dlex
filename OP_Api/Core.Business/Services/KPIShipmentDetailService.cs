using Core.Business.Services.Abstract;
using Core.Business.Services.Models;
using Core.Business.ViewModels.ExportExcelModel;
using Core.Data.Abstract;
using Core.Entity.Entities;
using Core.Entity.Procedures;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MoreLinq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Api;
using Core.Infrastructure.Utils;
using Core.Business.ViewModels.KPIShipmentDetails;

namespace Core.Business.Services
{
    public class KPIShipmentDetailService : BaseService, IKPIShipmentDetailService
    {
        public KPIShipmentDetailService(Microsoft.Extensions.Logging.ILogger<dynamic> logger,
            IOptions<AppSettings> optionsAccessor,
            IUnitOfWork unitOfWork
            )
            : base(logger, optionsAccessor, unitOfWork)
        {

        }

        public KPIShipmentDetailResponseModel CalculateTimeForShipment(int? cusId, int? districtId, int? wardId, int serviceId, DateTime dateTimeStart)
        {
            if (dateTimeStart == DateTime.MinValue) dateTimeStart = DateTime.Now;
            KPIShipmentDetailResponseModel kPIShipment = new KPIShipmentDetailResponseModel();
            var kpiShipmentDetail = _unitOfWork.Repository<Proc_GetTimeKPI>()
                 .ExecProcedureSingle(Proc_GetTimeKPI.GetEntityProc(districtId, wardId, cusId, serviceId));
            if (!Util.IsNull(kpiShipmentDetail))
            {
                var year = DateTime.Now.Year;
                DateTime fromDate = DateTime.Parse(string.Format("{0}/01/01 01:00:00 AM", year));
                DateTime toDate = fromDate.AddMonths(13);
                var getHoliday = _unitOfWork.RepositoryR<Holiday>().FindBy(f => f.Date >= fromDate && f.Date <= toDate).ToList();
                //
                kPIShipment.Time = kpiShipmentDetail.TargetDeliveryTime;
                kPIShipment.TimeCOD = kpiShipmentDetail.TargetPaymentCOD;
                kPIShipment.RealTime = CountHours(dateTimeStart, getHoliday, kpiShipmentDetail.TargetDeliveryTime);
                kPIShipment.DeadlineDelivery = dateTimeStart.AddHours(kPIShipment.RealTime);
            }
            return kPIShipment;
        }

        public int CalculateTimeForShipment(int timezone, DateTime dateTimeStart)
        {
            DateTime fromDate = DateTime.Parse(DateTime.Now.ToString("1/1/yyyy 00:00:000"));
            DateTime toDate = fromDate.AddMonths(13);
            var getHoliday = _unitOfWork.RepositoryR<Holiday>().FindBy(f => f.Date >= fromDate && f.Date <= toDate).ToList();
            var toHours = CountHours(dateTimeStart, getHoliday, timezone);
            return toHours;

        }

        public dynamic InsertKPIShipmentDetail(List<KPIShipmentDetail> request)
        {
            return _unitOfWork.Repository<Proc_InsertKPIShipment>().ExecProcedure(Proc_InsertKPIShipment.GetEntityProc(request));
        }
        public dynamic UpdateKPIShipmentDetail(List<KPIShipmentDetail> request)
        {
            return _unitOfWork.Repository<Proc_UpdateKPIShipment>().ExecProcedure(Proc_UpdateKPIShipment.GetEntityProc(request));
        }
        public dynamic GetKPIShipmentDetail(int kPIShipemntId, int? PageSize, int? PageNumber)
        {
            var result = _unitOfWork.Repository<Proc_GetKPIDetail>().ExecProcedure(Proc_GetKPIDetail.GetEntityProc(kPIShipemntId, PageNumber, PageSize)).ToList();
            return result;
        }

        //export
        public List<Proc_GetKPIDetail> GetKPIShipmentDetailExport(int kPIShipemntId, int? PageSize, int? PageNumber, CustomExportFile customExportFile)
        {
            var result = _unitOfWork.Repository<Proc_GetKPIDetail>().ExecProcedure(Proc_GetKPIDetail.GetEntityProc(kPIShipemntId, PageNumber, PageSize)).ToList();
            return result;
        }

        static int CountDays(DayOfWeek day, DateTime start, DateTime end)
        {
            TimeSpan ts = end - start;                       // Total duration
            int count = (int)Math.Floor(ts.TotalDays / 7);   // Number of whole weeks
            int remainder = (int)(ts.TotalDays % 7);         // Number of remaining days
            int sinceLastDay = (int)(end.DayOfWeek - day);   // Number of days since last [day]
            if (sinceLastDay < 0) sinceLastDay += 7;         // Adjust for negative days since last [day]

            // If the days in excess of an even week are greater than or equal to the number days since the last [day], then count this one, too.
            if (remainder >= sinceLastDay) count++;

            return count;
        }

        static int CountHours(DateTime dateTimeStart, List<Holiday> getHoliday, int timeZone)
        {
            //var getHoliday = _unitOfWork.RepositoryR<Holiday>().GetAll();
            var timeSa = getHoliday.Where(x => x.IsSa == true).FirstOrDefault();
            var timeSu = getHoliday.Where(x => x.IsSu == true).FirstOrDefault();
            int offTimeSa = 0;
            int offTimeSu = 0;
            if (timeSa != null)
            {
                if (timeSa.IsFull == true)
                {
                    offTimeSa = 24;
                }
                else
                {
                    offTimeSa = 12;
                }
            }
            if (timeSu != null)
            {
                if (timeSu.IsFull == true)
                {
                    offTimeSu = 24;
                }
                else
                {
                    offTimeSu = 12;
                }
            }
            int _hours = timeZone;
            DateTime _datetimeStart = dateTimeStart;
            //
            var loop = 0;
            while (loop <= _hours)
            {
                //
                var holiday = getHoliday.Where(f => f.Date.Month == _datetimeStart.Month && f.Date.Day == _datetimeStart.Day && f.IsSa != true && f.IsSu != true).FirstOrDefault();
                if (holiday != null)
                {
                    _hours += 24;
                }
                else if (_datetimeStart.DayOfWeek == DayOfWeek.Sunday)
                {
                    if ((_datetimeStart.Hour + (_hours - loop) >= 12 && offTimeSu == 12) || offTimeSu == 24)
                    {
                        _hours += offTimeSu;
                    }
                }
                else if (_datetimeStart.DayOfWeek == DayOfWeek.Saturday)
                {
                    if ((_datetimeStart.Hour + (_hours - loop) >= 12 && offTimeSa == 12) || offTimeSa == 24)
                    {
                        _hours += offTimeSa;
                    }
                }
                //
                if (_hours - loop > 24)
                {
                    _datetimeStart = _datetimeStart.AddDays(1);
                }
                else
                {
                    _datetimeStart = _datetimeStart.AddHours(_hours - loop);
                }
                //
                loop += 24;
            }
            var time = _datetimeStart.Hour;
            var hours = 0;
            //if (time < 9)
            //{
            //    hours = 9 - time;
            //}
            //else if (time == 12)
            //{
            //    hours = 1;
            //}
            //else if (time > 17)
            //{
            //    hours = 24 - time + 9;
            //}
            _hours += hours;
            _datetimeStart = _datetimeStart.AddHours(hours);
            //
            var checkHoliday = true;
            while (checkHoliday == true)
            {
                var hoursLast = 0;
                //
                var holiday = getHoliday.Where(f => f.Date.Month == _datetimeStart.Month && f.Date.Day == _datetimeStart.Day && f.IsSa != true && f.IsSu != true).FirstOrDefault();
                if (holiday != null)
                {
                    hoursLast += 24;
                }
                else if (_datetimeStart.DayOfWeek == DayOfWeek.Sunday)
                {
                    if ((_datetimeStart.Hour >= 12 && offTimeSu == 12) || offTimeSu == 24)
                    {
                        hoursLast += offTimeSu;
                    }
                    else
                    {
                        checkHoliday = false;
                    }
                }
                else if (_datetimeStart.DayOfWeek == DayOfWeek.Saturday)
                {
                    if ((_datetimeStart.Hour >= 12 && offTimeSa == 12) || offTimeSa == 24)
                    {
                        hoursLast += offTimeSa;
                    }
                    else
                    {
                        checkHoliday = false;
                    }
                }
                else
                {
                    checkHoliday = false;
                }
                //     
                _hours += hoursLast;
                _datetimeStart = _datetimeStart.AddHours(hoursLast);
            }
            //
            return _hours;
        }
    }
}
