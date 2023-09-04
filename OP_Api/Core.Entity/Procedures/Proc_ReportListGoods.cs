using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_ReportListGoods : IEntityProcView
    {
        public const string ProcName = "Proc_ReportListGoods";
        public Proc_ReportListGoods() { }

        public int Id { get; set; }
        public string StatusName { get; set; }
        public string Code { get; set; }
        public DateTime CreatedWhen { get; set; }
        public DateTime? ModifiedWhen { get; set; }
        public string CreatedHubName { get; set; }
        public string FromHubName { get; set; }
        public string ToHubName { get; set; }
        public double RealWeight { get; set; }
        public int? EmpId { get; set; }
        public string FullName { get; set; }
        public string TypeName { get; set; }
        public double TotalWeight { get; set; }
        public int TotalReceived { get; set; }
        public int TotalNotReceive { get; set; }
        public int TotalReceivedOther { get; set; }
        public int TotalReceivedError { get; set; }
        public int TotalShipment { get; set; }
        public int Transferring { get; set; }
        public string TransportTypeName { get; set; }
        public string TPLCode { get; set; }
        public int Delivering { get; set; }
        public int DeliveryComplete { get; set; }
        public int DeliveryFail { get; set; }
        public int TransferLostPackage { get; set; }
        public int ReturnComplete { get; set; }
        public int Returning { get; set; }
        public int AssignEmployeeDelivery { get; set; }
        public int AssignEmployeeTransfer { get; set; }
        public int AssignEmployeeReturn { get; set; }
        public int WaitingAcceptTransfer { get; set; }
        public int TotalBox { get; set; }
        public bool? IsBlock { get; set; }
        public string CreateByFullName { get; set; }
        public double TotalCalWeight { get; set; }

        public static IEntityProc GetEntityProc(int? typeId = null, int? createByHubId = null, int? fromHubId = null, 
            int? toHubId = null, int? userId = null, int? statusId = null, int? transportTypeId = null, int? tplId = null, DateTime? dateFrom = null, DateTime? dateTo = null, string listGoodsCode = null)
        {
            SqlParameter parameter1 = new SqlParameter(
           "@TypeId", typeId);
            if (!typeId.HasValue)
                parameter1.Value = DBNull.Value;
            SqlParameter parameter2 = new SqlParameter(
           "@CreateByHubId", createByHubId);
            if (!createByHubId.HasValue)
                parameter2.Value = DBNull.Value;
            SqlParameter parameter3 = new SqlParameter(
            "@FromHubId", fromHubId);
            if (!fromHubId.HasValue)
                parameter3.Value = DBNull.Value;
            SqlParameter parameter4 = new SqlParameter(
           "@ToHubId", toHubId);
            if (!toHubId.HasValue)
                parameter4.Value = DBNull.Value;
            SqlParameter parameter5 = new SqlParameter(
           "@UserId", userId);
            if (!userId.HasValue)
                parameter5.Value = DBNull.Value;
            SqlParameter parameter6 = new SqlParameter(
           "@StatusId", statusId);
            if (!statusId.HasValue)
                parameter6.Value = DBNull.Value;
            SqlParameter parameter7 = new SqlParameter(
           "@TransportTypeId", transportTypeId);
            if (!transportTypeId.HasValue)
                parameter7.Value = DBNull.Value;
            SqlParameter parameter8 = new SqlParameter(
            "@TPLId", tplId);
            if (!tplId.HasValue)
                parameter8.Value = DBNull.Value;
            SqlParameter parameter9 = new SqlParameter(
           "@DateFrom", dateFrom);
            if (!dateFrom.HasValue)
                parameter9.Value = DBNull.Value;
            SqlParameter parameter10 = new SqlParameter(
           "@DateTo", dateTo);
            if (!dateTo.HasValue)
                parameter10.Value = DBNull.Value;

            SqlParameter parameter11 = new SqlParameter(
           "@ListGoodsCode", listGoodsCode);
            if (string.IsNullOrWhiteSpace(listGoodsCode))
                parameter11.Value = DBNull.Value;

            return new EntityProc(
                $"{ProcName} @TypeId, @CreateByHubId, @FromHubId, @ToHubId, @UserId, @StatusId, @TransportTypeId, @TPLId, @DateFrom, @DateTo, @ListGoodsCode",
                new SqlParameter[] {
                parameter1,
                parameter2,
                parameter3,
                parameter4,
                parameter5,
                parameter6,
                parameter7,
                parameter8,
                parameter9,
                parameter10,
                parameter11
                }
            );
        }
    }
}
