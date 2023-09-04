using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Core.Data.Core.Utils;

namespace Core.Business.ViewModels
{
    public class ListShipmentUpdateStatusViewModel : IValidatableObject
    {
        public ListShipmentUpdateStatusViewModel()
        {
        }
        public int ListGoodsId { get; set; }
        public bool? DeliveryOther { set; get; }
        public int EmpId { get; set; }
        public int? ReceiveHubId { get; set; }
        public int ShipmentStatusId { get; set; }
        public double CurrentLat { get; set; }
        public double CurrentLng { get; set; }
        public string Location { get; set; }
        public string Note { get; set; }
        public string CusNote { get; set; }
        public int? TPLId { get; set; }
        public string TPLNumber { get; set; }
        public List<int> ShipmentIds { get; set; }
        public List<int> UnShipmentIds { get; set; }
        public List<int> AddShipmentIds { get; set; }
        public List<int> NotInShipmentIds { get; set; }
        public List<string> ScheduleErrorShipmentIds { get; set; }
        public string SealNumber { get; set; }
        public int? FromHubId { get; set; }
        public int? ToHubId { get; set; }
        //
        public string RealRecipientName { get; set; }
        public DateTime? EndExpectedTime { get; set; }

        //Transit
        public int? TransportTypeId { get; set; }
        public DateTime? StartExpectedTime { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndDeliveryTime { get; set; }
        public DateTime? EndTime { get; set; }
        public double RealWeight { get; set; }
        public string TruckNumber { get; set; }
        public string MAWB { get; set; }
        public int[] ServiceDVGTIds { get; set; }
        public int? StructureId { get; set; }
        public int? ReasonId { get; set; }
        public bool? IsTransferAllHub { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validator = new ListShipmentUpdateStatusViewModelValidator(EntityUtil.GetUnitOfWork(validationContext));
            var result = validator.Validate(this);
            return result.Errors.Select(item => new ValidationResult(item.ErrorMessage, new[] { item.PropertyName }));
        }
    }
}
