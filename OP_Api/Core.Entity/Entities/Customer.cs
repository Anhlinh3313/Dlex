using System;
namespace Core.Entity.Entities
{
    public class Customer : EntitySimple
    {
        public Customer()
        {
        }

        public string NameEn { get; set; }
        public string Address { get; set; }
        public string AddressNote { get; set; }
        public string BusinessLicenseNumber { get; set; }
        public string Email { get; set; }
        public string Fax { get; set; }
        public string LegalRepresentative { get; set; }
        public string Notes { get; set; }
        public int? ParentCustomerId { get; set; }
        public string PhoneNumber { get; set; }
        public int? SalesOrganizationId { get; set; }
        public int? PickupUserId { get; set; }
        public int? SalesUserId { get; set; }
        public int? CustomerStatusId { get; set; }
        public int? StopServiceAlertDuration { get; set; }
        public int? SupportOrganizationId { get; set; }
        public int? SupportUserId { get; set; }
        public int? AccountingUserId { get; set; }
        public string TaxCode { get; set; }
        public string TradingName { get; set; }
        public int? CustomerTypeId { get; set; }
        public int? ProvinceId { get; set; }
        public int? DistrictId { get; set; }
        public int? WardId { get; set; }
        public int? HubId { get; set; }
        public int? PaymentTypeId { get; set; }
        public string Website { get; set; }
        public int? WorkTimeId { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        public double VAT { get; set; }
        public bool IsShowPrice { get; set; }
        public string VSEOracleCode { get; set; }
        public string AddressCompany { get; set; }
        public string CompanyPhone { get; set; }
        public string CompanyEmail { get; set; }
        public string CompanyName { get; set; }
        public string Professions { get; set; }
        public string SignRole { get; set; }
        public string SignName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public double? CommissionCus { get; set; }
        public string UserName { get; set; }
        public DateTime? TimeStopUsing { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
    }
}
