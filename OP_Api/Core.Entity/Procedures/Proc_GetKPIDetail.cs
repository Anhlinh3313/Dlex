using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_GetKPIDetail : IEntityProcView
    {
        public const string ProcName = "Proc_GetKPIDetail";
        [Key]
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedWhen { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedWhen { get; set; }
        public int? ModifiedBy { get; set; }
        public string ConcurrencyStamp { get; set; }
        public bool IsEnabled { get; set; } = true;
        public int WardId { get; set; }
        public int? DistrictId { get; set; }
        public string Vehicle { get; set; }
        public int TargetDeliveryTime { get; set; }
        public int KPIShipmentId { get; set; }
        public int? TargetPaymentCOD { get; set; }
        //public string CustomerCode { get; set; }
        //public string CustomerName { get; set; }
        //public string CustomerPhoneNumber { get; set; }
        //public string CustomerAddress { get; set; }
        public string WardName { get; set; }
        public string DistrictName { get; set; }
        public string ProvinceName { get; set; }
        public int? TotalCount { get; set; }

        public Proc_GetKPIDetail() { }
        public static IEntityProc GetEntityProc(int kPIShipmentId, int? pageNumber, int? pageSize)
        {
            SqlParameter KPIShipmentId = new SqlParameter("@KPIShipmentId", kPIShipmentId);

            SqlParameter PageNumber = new SqlParameter("@PageNumber", pageNumber);
            if (!pageNumber.HasValue) PageNumber.Value = DBNull.Value;

            SqlParameter PageSize = new SqlParameter("@PageSize", pageSize);
            if (!pageSize.HasValue) PageSize.Value = DBNull.Value;

            return new EntityProc(
                $"{ProcName} @KPIShipmentId,@PageNumber,@PageSize",
                new SqlParameter[] {
                    KPIShipmentId,
                    PageNumber,
                    PageSize
                }
            );
        }
    }
}
