using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entity.Entities
{
    public class ListGoods : EntitySimple
    {
        public ListGoods()
        {
        }

        public ListGoods(int listGoodsTypeId, int createdByHub, int? listGoodsStatusId = null, int? tplId = null, int? fromHubId = null, int? toHubId = null, int? empId = null)
        {
            ListGoodsTypeId = listGoodsTypeId;
            CreatedByHub = createdByHub;
            ListGoodsStatusId = listGoodsStatusId;
            TPLId = tplId;
            FromHubId = fromHubId;
            ToHubId = toHubId;
            EmpId = empId;
            TotalBox = 0;
        }

        public int TotalBox { get; set; }
        public double FeeRent { get; set; }
        public int ListGoodsTypeId { get; set; }
        public int NumPrint { get; set; }
        public int CreatedByHub { get; set; }
        public string SealNumber { get; set; }
        public int? ListGoodsStatusId { get; set; }
        public int? TPLId { get; set; }
        public int? FromHubId { get; set; }
        public int? ToHubId { get; set; }
        public DateTime? CancelTime { get; set; }
        public string CancelNote { get; set; }
        public string Note { get; set; }
        public bool IsBlock { get; set; }
        //Transit
        public int? EmpId { get; set; }
        public int? TransportTypeId { get; set; }
        public DateTime? StartExpectedTime { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndExpectedTime { get; set; }
        public DateTime? EndTime { get; set; }
        public double RealWeight { get; set; }
        public string TruckNumber { get; set; }
        public string MAWB { get; set; }
        //
        public int TotalReceived { get; set; }
        public int TotalNotReceive { get; set; }
        public int TotalReceivedOther { get; set; }
        public int TotalReceivedError { get; set; }
        public int? TruckScheduleId { get; set; }
        public int? TruckId { get; set; }
        //Custom field
        [NotMapped]
        public int TotalShipment { get; set; }
        public ListGoodsType ListGoodsType { get; set; }
        public ListGoodsStatus ListGoodsStatus { get; set; }
        public virtual TransportType TransportType { get; set; }
        public TPL TPL { get; set; }
        [ForeignKey("FromHubId")]
        public Hub FromHub { get; set; }
        [ForeignKey("CreatedBy")]
        public User CreatedByUser { get; set; }
        [ForeignKey("ToHubId")]
        public Hub ToHub { get; set; }
        [ForeignKey("EmpId")]
        public User Emp { get; set; }
        public Truck Truck { get; set; }
        //public Hub CreatedHub { get; set; }
    }
}
