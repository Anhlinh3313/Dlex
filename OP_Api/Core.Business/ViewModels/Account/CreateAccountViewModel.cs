using System;
using System.Collections.Generic;
using Core.Entity.Abstract;

namespace Core.Business.ViewModels
{
    public class CreateAccountViewModel : IEntityBase
    {
        public CreateAccountViewModel()
        {
        }

        public int TypeUserId { get; set; }
        public bool IsBlocked { get; set; }
        public int Id { get; set; }
		public string UserName { get; set; }
		public string PassWord { get; set; }
		public string FullName { get; set; }
		public string Code { get; set; }
		public int? HubId { get; set; }
		public int? RoleId { get; set; }
		public int? DepartmentId { get; set; }
		public string Email { get; set; }
		public string PhoneNumber { get; set; }
		public string Address { get; set; }
        public string IdentityCard { get; set; }
		public string AvatarBase64 { get; set; }
		public string ConcurrencyStamp { get; set; }
		public bool? IsGlobalAdministrator { get; set; }
        public bool IsEnabled { get; set; }
        public List<int> RoleIds { get; set; }
        public List<int> StructureIds { get; set; }
        public List<int> CustomerIds { get; set; }
        public string VSEOracleCode { get; set; }
        public DateTime? ExpiresDate { get; set; }
        public int? BlockTime { get; set; }
        public bool? IsPassWordBasic { get; set; }
        public string SeriNumber { get; set; }
        public int? ManageHubId { get; set; }
        public int? CompanyId { get; set; }
    }
}
