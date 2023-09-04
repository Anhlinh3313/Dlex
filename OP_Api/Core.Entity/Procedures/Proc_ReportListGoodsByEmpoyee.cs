using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_ReportListGoodsByEmployee : IEntityProcView
    {
        public const string ProcName = "Proc_ReportListGoodsByEmployee";

        [Key]
        public Int64? RowNum { get; set; }
        public int Id { get; set; }
        public DateTime? CreatedWhen { get; set; }
        public int? UserId { get; set; }
        public string FullName { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string PickingAddress { get; set; }
        public string ShipmentNumber { get; set; }
        public double? COD { get; set; }
        public double? CODPayment { get; set; }
        public double? CODUnPayment { get; set; }
        public bool? Paid { get; set; } 
        public int? ListGoodsId { get; set; }
        public DateTime? FirstLockDate { get; set; }
        public DateTime? ListGoodsCreatedDate { get; set; }
        public int? TotalEmp { get; set; }
        public double? TotalCOD { get; set; }
        public double? TotalCODPayment { get; set; }
        public double? TotalCODUnPayment { get; set; }
        public DateTime? DelevirySuccessDate { get; set; }
        public string Code { get; set; }
        public DateTime? AcceptDate { get; set; }
        public string PartnerCode { get; set; }
        public string PartnerName { get; set; }
        public string UserDeliveryName { get; set; }
        public string ShipmentStatusName { get; set; }
        public int? TotalCount { get; set; }

        public Proc_ReportListGoodsByEmployee()
        {
        }

        public static IEntityProc GetEntityProc(
            DateTime? dateFrom = null,
            DateTime? dateTo = null, 
            int? empId = null,
            bool? isGroupEmp = null,
            string searchtext = null,
            int? senderId = null,
            bool? isCreateListReceiptCOD = null,
            int? listReceiptCODStatusId = null,
            bool? isSubmitCOD = null,
            int? pageNumber = null, 
            int? pageSize = null
            )
        {
            SqlParameter DateFrom = new SqlParameter("@DateFrom", dateFrom);
            if (!dateFrom.HasValue)
                DateFrom.Value = DBNull.Value;

            SqlParameter DateTo = new SqlParameter("@DateTo", dateTo);
            if (!dateTo.HasValue)
                DateTo.Value = DBNull.Value;

            SqlParameter EmpId = new SqlParameter("@EmpId", empId);
            if (!empId.HasValue)
                EmpId.Value = DBNull.Value;
          
            SqlParameter PageNumber = new SqlParameter("@PageNumber", pageNumber);
            if (!pageNumber.HasValue)
                PageNumber.Value = DBNull.Value;

            SqlParameter PageSize = new SqlParameter("@PageSize", pageSize);
            if (!pageSize.HasValue)
                PageSize.Value = DBNull.Value;

            SqlParameter IsGroupEmp = new SqlParameter("@IsGroupEmp", isGroupEmp);
            if (!isGroupEmp.HasValue)
                IsGroupEmp.Value = DBNull.Value;

            SqlParameter SearchText = new SqlParameter("@SearchText", searchtext);
            if (string.IsNullOrWhiteSpace(searchtext))
                SearchText.Value = DBNull.Value;

            SqlParameter SenderId = new SqlParameter("@SenderId", senderId);
            if (!senderId.HasValue)
                SenderId.Value = DBNull.Value;

            SqlParameter IsCreateListReceiptCOD = new SqlParameter("@IsCreateListReceiptCOD", isCreateListReceiptCOD);
            if (!isCreateListReceiptCOD.HasValue)
                IsCreateListReceiptCOD.Value = DBNull.Value;

            SqlParameter ListReceiptCODStatusId = new SqlParameter("@ListReceiptCODStatusId", listReceiptCODStatusId);
            if (!listReceiptCODStatusId.HasValue)
                ListReceiptCODStatusId.Value = DBNull.Value;

            SqlParameter IsSubmitCOD = new SqlParameter("@IsSubmitCOD", isSubmitCOD);
            if (!isSubmitCOD.HasValue)
                IsSubmitCOD.Value = DBNull.Value;


            return new EntityProc(
                $"{ProcName} @DateFrom,@DateTo,@EmpId,@PageNumber,@PageSize,@IsGroupEmp,@SearchText," +
                $"@SenderId,@IsCreateListReceiptCOD,@ListReceiptCODStatusId,@IsSubmitCOD",
                new SqlParameter[] {
                    DateFrom,
                    DateTo,
                    EmpId,
                    PageNumber,
                    PageSize,
                    IsGroupEmp,
                    SearchText,
                    SenderId,
                    IsCreateListReceiptCOD,
                    ListReceiptCODStatusId,
                    IsSubmitCOD
                }
            );
        }
    }
}
