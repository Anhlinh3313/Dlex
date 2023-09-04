using System;
namespace Core.Business.ViewModels.Shipments
{
    public class ShipmentReportEmpViewModel
    {
        public int AssignEmployeePickup { get; set; }
        public int Picking { get; set; }
        public int AssignEmployeeTransfer { get; set; }
        public int Transferring { get; set; }
        public int AssignEmployeeTransferReturn { get; set; }
        public int TransferReturning { get; set; }
        public int AssignEmployeeDelivery { get; set; }
        public int Delivering { get; set; }
        public int AssignEmployeeReturn { get; set; }
        public int Returning { get; set; }
        public int PickupComplete { get; set; }
        public int DeliveryComplete { get; set; }
        public int ReturnComplete { get; set; }
        public int WaitingToTransfer { get; set; }

        public ShipmentReportEmpViewModel()
        {
        }
    }
}
