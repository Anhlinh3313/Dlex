using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_ReportIncidents : IEntityProcView
    {
        public const string ProcName = "Proc_ReportIncidents";

        public int Id { get; set; }
        public string Code { get; set; }
        public string ReasonName { get; set; }
        public string IncidentsContent { get; set; }
        public bool? IsCompleted { get; set; }
        public bool? IsCompensation { get; set; }
        public double? CompensationValue { get; set; }
        public string IncidentsByEmpName { get; set; }
        public string HandleEmpName { get; set; }
        public string CreatedByEmpName { get; set; }
        public int TotalCount { get; set; }
        public DateTime? CreatedWhen { get; set; }

        public static IEntityProc GetEntityProc(DateTime? dateFrom = null, DateTime? dateTo = null, int? customerId = null, int? incidentEmpId = null, int? handleEmpId = null, bool? isCompensation = null, int? pageNumber = 1, int? pageSize = 20)
        {
            SqlParameter DateFrom = new SqlParameter("@DateFrom", dateFrom);
            if (!dateFrom.HasValue)
                DateFrom.Value = DBNull.Value;

            SqlParameter DateTo = new SqlParameter("@DateTo", dateTo);
            if (!dateTo.HasValue)
                DateTo.Value = DBNull.Value;

            SqlParameter CustomerId = new SqlParameter("@CustomerId", customerId);
            if (!customerId.HasValue)
                CustomerId.Value = DBNull.Value;

            SqlParameter IncidentEmpId = new SqlParameter("@IncidentEmpId", incidentEmpId);
            if (!incidentEmpId.HasValue)
                IncidentEmpId.Value = DBNull.Value;

            SqlParameter HandleEmpId = new SqlParameter("@HandleEmpId", handleEmpId);
            if (!handleEmpId.HasValue)
                HandleEmpId.Value = DBNull.Value;

            SqlParameter IsCompensation = new SqlParameter("@IsCompensation", isCompensation);
            if (!isCompensation.HasValue)
                IsCompensation.Value = DBNull.Value;

            SqlParameter PageNumber = new SqlParameter("@PageNumber", pageNumber);
            if (!pageNumber.HasValue)
                PageNumber.Value = DBNull.Value;

            SqlParameter PageSize = new SqlParameter("@PageSize", pageSize);
            if (!pageSize.HasValue)
                PageSize.Value = DBNull.Value;

            return new EntityProc($"{ProcName} @DateFrom, @DateTo, @CustomerId, @IncidentEmpId, @HandleEmpId, @IsCompensation, @PageNumber, @PageSize",
                new SqlParameter[] { DateFrom, DateTo, CustomerId, IncidentEmpId, HandleEmpId, IsCompensation, PageNumber, PageSize });
        }
    }
}


