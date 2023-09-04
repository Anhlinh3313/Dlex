using System;

namespace Core.Business.ViewModels
{
    public class FilterViewModel
    {
        public FilterViewModel() { }

        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public int? PageSize { get; set; }
        public int? PageNumber { get; set; }
        public string SearchText { get; set; }
        public int? CountryId { get; set; }
        public int? ProvinceId { get; set; }
        public int? Districtid { get; set; }
        public bool? IsRemote { get; set; }
        public bool? IsAccept { get; set; }
        public int? CompanyId { get; set; }
        public int? UserId { get; set; }
        public int? CenterHubId { get; set; }
        public int? POHubId { get; set; }
        public int? CustomerId { get; set; }
        public int? PromotionTypeId { get; set; }
        public bool? IsPublic { get; set; }
        public bool? IsHidden { get; set; }
        public int? PromotionId { get; set; }
        public int? ServiceId { get; set; }
        public int? PriceListId { get; set; }
        public int? SenderId { get; set; }
        public int? ProvinceFromId { get; set; }
        public int? ProvinceToId { get; set; }
    }
}
