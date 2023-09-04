using System;
using System.Collections.Generic;
using Core.Business.ViewModels;

namespace Core.Business.Core.Utils
{
    public static class MapUtil
    {
        public static List<UpdateStatusViewModel> Map(ListRequestShipmentUpdateStatusViewModel viewModel)
        {
            var list = new List<UpdateStatusViewModel>();

            foreach (var id in viewModel.ShipmentIds)
            {
                var obj = new UpdateStatusViewModel();
                obj.Id = id;
                obj.EmpId = viewModel.EmpId;
                obj.ShipmentStatusId = viewModel.ShipmentStatusId;
                obj.CurrentLat = viewModel.CurrentLat;
                obj.CurrentLng = viewModel.CurrentLng;
                obj.Location = viewModel.Location;
                obj.Note = viewModel.Note;
            }

            return list;
        }

        public static List<UpdateStatusViewModel> Map(ListShipmentUpdateStatusViewModel viewModel)
        {
            var list = new List<UpdateStatusViewModel>();

            foreach (var id in viewModel.ShipmentIds)
            {
                var obj = new UpdateStatusViewModel();
                obj.Id = id;
                obj.EmpId = viewModel.EmpId;
                obj.ShipmentStatusId = viewModel.ShipmentStatusId;
                obj.CurrentLat = viewModel.CurrentLat;
                obj.CurrentLng = viewModel.CurrentLng;
                obj.Location = viewModel.Location;
                obj.Note = viewModel.Note;
            }

            return list;
        }
    }
}
