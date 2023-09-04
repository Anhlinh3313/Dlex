using Core.Entity.Entities;

namespace Core.Business.ViewModels
{
    public class ComplainTypeViewModel : SimpleViewModel<ComplainTypeViewModel, ComplainType>
    {
        public ComplainTypeViewModel() { }
        public int? RoleId { get; set; }
        public bool IsPublish { get; set; }
    }
}
