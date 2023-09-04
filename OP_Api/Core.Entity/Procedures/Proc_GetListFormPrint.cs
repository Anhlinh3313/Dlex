using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using Core.Entity.Abstract;

namespace Core.Entity.Procedures
{
    public class Proc_GetListFormPrint : IEntityProcView
    {
        public const string ProcName = "Proc_GetListFormPrint";

        [Key]
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string FormPrintBody { get; set; }
        public int? NumOrder { get; set; }
        public double? Width { get; set; }
        public double? Height { get; set; }
        public bool IsPublic { get; set; }
        public int FormPrintTypeId { get; set; }
        public string FormPrintTypeCode { get; set; }
        public string FormPrintTypeName { get; set; }
        public Int64? RowNum { get; set; }
        public int? TotalCount { get; set; }

        public Proc_GetListFormPrint()
        {
        }

        public static IEntityProc GetEntityProc(int? formPrintId, int? formPrintTypeId, string searchText, int? pageNumber, int? pageSize)
        {
            SqlParameter FormPrintId = new SqlParameter("@FormPrintId", formPrintId);
            if (!formPrintId.HasValue)
                FormPrintId.Value = DBNull.Value;

            SqlParameter FormPrintTypeId = new SqlParameter("@FormPrintTypeId", formPrintTypeId);
            if (!formPrintTypeId.HasValue)
                FormPrintTypeId.Value = DBNull.Value;

            SqlParameter SearchText = new SqlParameter("@SearchText", searchText);
            if (string.IsNullOrEmpty(searchText))
                SearchText.Value = DBNull.Value;

            SqlParameter PageSize = new SqlParameter("@PageSize", pageSize);
            if (!pageSize.HasValue)
                PageSize.Value = DBNull.Value;

            SqlParameter PageNumber = new SqlParameter("@PageNumber", pageNumber);
            if (!pageNumber.HasValue)
                PageNumber.Value = DBNull.Value;

            return new EntityProc(
                $"{ProcName} @FormPrintId, @FormPrintTypeId, @SearchText,@PageNumber, @PageSize",
                new SqlParameter[] {
                    FormPrintId,
                    FormPrintTypeId,
                    SearchText,
                    PageNumber,
                    PageSize,
                }
            );
        }
    }
}
