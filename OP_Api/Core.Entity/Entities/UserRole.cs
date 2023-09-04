using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entity.Entities
{
    public class UserRole : IEntityBase
    {
        public UserRole() { }

        public int Id { set; get; }
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public bool IsEnabled { get; set; }
        public int? CompanyId { get; set; }
    }
}
