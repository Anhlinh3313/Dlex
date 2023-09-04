using System;

namespace Core.Infrastructure.ViewModels
{
    public class ResponseOfRequestShipmentViewModel
    {
        public ResponseOfRequestShipmentViewModel()
        {
        }

        public ResponseOfRequestShipmentViewModel(bool isSuccess, string message, int? dataCount, dynamic data, dynamic sumOfRequestShipment, Exception exception)
        {
            IsSuccess = isSuccess;
            Message = message;
            DataCount = dataCount;
            Data = data;
            SumOfRequestShipment = sumOfRequestShipment;
            Exception = exception;
        }

        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public int? DataCount { get; set; }
        public dynamic Data { get; set; }
        public dynamic SumOfRequestShipment { get; set; }
        public Exception Exception { get; set; }

        public static ResponseOfRequestShipmentViewModel CreateSuccess(object data = null, string message = null, int? dataCount = null, object sumOfRequestShipment = null)
        {
            return new ResponseOfRequestShipmentViewModel(true, message, dataCount, data, sumOfRequestShipment, null);
        }

        public static ResponseOfRequestShipmentViewModel CreateError(string message = null)
        {
            return new ResponseOfRequestShipmentViewModel(false, message, null, null, null, null);
        }

        public static ResponseOfRequestShipmentViewModel CreateErrorObject(object data)
        {
            return new ResponseOfRequestShipmentViewModel(false, null, null, data, null, null);
        }

        public static ResponseOfRequestShipmentViewModel CreateErrorObject(Exception ex)
        {
            return new ResponseOfRequestShipmentViewModel(false, null, null, null, null, ex);
        }
    }
}
