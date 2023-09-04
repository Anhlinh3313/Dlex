using Core.Entity.Entities;

namespace Core.Business.ViewModels.Roles
{
    public class RoleViewModel : SimpleViewModel<RoleViewModel, Role>
    {
        public RoleViewModel()
        {
        }
        public int? ParrentRoleId { get; set; }
    }
}
