using System;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Entity.Abstract;

namespace Core.Entity.Entities
{
    public class User : EntityBasic
    {
        public User()
        {
        }

        public int TypeUserId { get; set; }
        public int? CompanyId { get; set; }
        public bool IsBlocked { get; set; }
        public int? RoleId { get; set; }
        public int? DepartmentId { get; set; }  
        public int? HubId { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string Code { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string AvatarPath { get; set; }
        public string IdentityCard { get; set; }
        public bool IsGlobalAdministrator { get; set; }
        public bool IsHidden { get; set; }
        public string FullName { get; set; }
        public int AccessFailedCount { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool LockoutEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public string NormalizedEmail { get; set; }
        public string NormalizedUserName { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public string SecurityStamp { get; set; } = Guid.NewGuid().ToString();
        public bool TwoFactorEnabled { get; set; }
        public string FireBaseToken { get; set; }
        public bool? IsPassWordBasic { get; set; }
        //
        public double? CurrentLat { get; set; }
        public double? CurrentLng { get; set; }
        public double? OldLat { get; set; }
        public double? OldLng { get; set; }
        public DateTime? LastUpdateLocationTime { get; set; }
        public DateTime? ExpiresDate { get; set; }
        public int? BlockTime { get; set; }
        public int? ManageHubId { get; set; }

        public virtual Department Department { get; set; }
        public virtual Hub Hub { get; set; }
        public virtual Role Role { get; set; }
        public string VSEOracleCode { get; set; }
        public string CodeResetPassWord { get; set; }
        public DateTime? ResetPassWordSentat { get; set; }
        [ForeignKey("ManageHubId")]
        public Hub ManageHub { get; set; }
    }
}
