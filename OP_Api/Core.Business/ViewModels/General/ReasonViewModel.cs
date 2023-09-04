using System;
using Core.Entity.Entities;

namespace Core.Business.ViewModels
{
    public class ReasonViewModel : SimpleViewModel<ReasonViewModel, Reason>
    {
        public ReasonViewModel()
        {
        }

        public bool PickFail { get; set; }
        public bool PickCancel { get; set; }
        public bool PickReject { get; set; }
        public bool DeliverFail { get; set; }
        public bool DeliverCancel { get; set; }
        public bool ReturnFail { get; set; }
        public bool ReturnCancel { get; set; }
        public bool IsDelay { get; set; }
        public bool IsIncidents { get; set; }
        public bool IsPublic { get; set; }
        public bool IsAcceptReturn { get; set; }
        public bool IsMustInput { get; set; }
        public int ItemOrder { get; set; }
        public int? RoleId { get; set; }
        public bool IsUnlockListGood { get; set; }
    }
}
