using Core.Entity.Abstract;

namespace Core.Entity.Entities
{
    public class RolePage : IEntityBase
    {
        public RolePage()
        {
        }
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int PageId { get; set; }
        public bool IsAccess { get; set; }
        public bool IsAdd { get; set; }
        public bool IsEdit { get; set; }
        public bool IsDelete { get; set; }
        public bool IsEnabled { get; set; }
        public int? CompanyId { get; set; }
    }
}
