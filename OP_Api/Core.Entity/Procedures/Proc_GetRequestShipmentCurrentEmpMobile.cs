using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_GetRequestShipmentCurrentEmpMobile : IEntityProcView
    {
        public const string ProcName = "Proc_GetRequestShipmentCurrentEmpMobile";

        [Key]
        public int Id { get; set; }
        public string AddressNoteTo { get; set; }
        public string ShippingAddress { get; set; }
        public string PickingAddress { get; set; }
        public string shipmentNumber { get; set; }
        public string ReceiverName { get; set; }
        public int? TotalShipment { get; set; }
        public int? TotalBoxShipment { get; set; }
        public double? TotalWeaightShipment { get; set; }
        public double? TotalCODShipment { get; set; }
        public string SenderName { get; set; }
        public string SenderPhone { get; set; }
        public string ShipmentStatusName { get; set; }
        public int? TotalShipmentSuccess { get; set; }
        public int? ShipmentStatusId { get; set; }
        public int? TotalShipmentFail { get; set; }

        public int TotalCount { get; set; }

        public Proc_GetRequestShipmentCurrentEmpMobile() { }

        public static IEntityProc GetEntityProc(int? userId = null, string statusIds = null, string searchText = null, int? pageNumber = 1, int? pageSize = 20)
        {
            SqlParameter UserId = new SqlParameter("@UserId", userId);
            if (!userId.HasValue) UserId.Value = DBNull.Value;
            SqlParameter StatusIds = new SqlParameter("@StatusIds", statusIds);
            if (string.IsNullOrWhiteSpace(statusIds)) StatusIds.Value = DBNull.Value;
            SqlParameter SearchText = new SqlParameter("@SearchText", searchText);
            if (string.IsNullOrWhiteSpace(searchText)) SearchText.Value = DBNull.Value;
            SqlParameter PageNumber = new SqlParameter("@PageNUmber", pageNumber);
            SqlParameter PageSize = new SqlParameter("@PageSize", pageSize);

            return new EntityProc(
                $"{ProcName} @UserId,@StatusIds,@SearchText,@PageNUmber,@PageSize",
                new SqlParameter[] {
                    UserId,
                    StatusIds,
                    SearchText,
                    PageNumber,
                    PageSize
                }
            );
        }
    }
}
