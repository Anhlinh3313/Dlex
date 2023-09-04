using System;
using Core.Business.ViewModels.General;
using Core.Entity.Entities;

namespace Core.Business.ViewModels
{
    public class ListGoodsInfoViewModel
    {
        public ListGoodsInfoViewModel()
        {
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int ListGoodsTypeId { get; set; }
        public int NumPrint { get; set; }
        public int CreatedByHub { get; set; }

        public ListGoodsType ListGoodsType { get; set; }
        public Hub CreatedHub { get; set; }

        public double TotalWeight { get; set; }
        public int TotalShipment { get; set; }
        public int TotalReceived { get; set; }
        public int TotalNotReceive { get; set; }
        public int TotalReceivedOther { get; set; }
        public int TotalReceivedError { get; set; }
    }
}
