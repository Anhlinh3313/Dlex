using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Core.Infrastructure.ViewModels
{
    public class ResponseOfReportViewModel
    {
        public ResponseOfReportViewModel()
        {
        }

        public ResponseOfReportViewModel(bool isSuccess, string message, int? dataCount, dynamic data, dynamic sumOfReport, Exception exception)
        {
            IsSuccess = isSuccess;
            Message = message;
            DataCount = dataCount;
            Data = data;
            SumOfReport = sumOfReport;
            Exception = exception;
        }

        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public int? DataCount { get; set; }
        public dynamic Data { get; set; }
        public dynamic SumOfReport { get; set; }
        public Exception Exception { get; set; }

        public static ResponseOfReportViewModel CreateSuccess(object data = null, string message = null, int? dataCount = null, object sumOfReport = null)
        {
            return new ResponseOfReportViewModel(true, message, dataCount, data, sumOfReport, null);
        }

        public static ResponseOfReportViewModel CreateError(string message = null)
        {
            return new ResponseOfReportViewModel(false, message, null, null, null, null);
        }

        public static ResponseOfReportViewModel CreateErrorObject(object data)
        {
            return new ResponseOfReportViewModel(false, null, null, data, null, null);
        }

        public static ResponseOfReportViewModel CreateErrorObject(Exception ex)
        {
            return new ResponseOfReportViewModel(false, null, null, null, null, ex);
        }
    }
}
