using System;
namespace Core.Business.ViewModels
{
    public class RemoveShipmentViewModel
    {
        public RemoveShipmentViewModel()
        {
        }

        public int Id { get; set; }
        public int[] ShipmentIds { get; set; }
    }
}
