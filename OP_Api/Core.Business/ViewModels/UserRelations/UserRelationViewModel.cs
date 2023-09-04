namespace Core.Business.ViewModels.UserRelations
{
    public class UserRelationViewModel: SimpleViewModel
    {
        public UserRelationViewModel()
        {
        }
        public int UserId { get; set; }
        public int UserRelationId { get; set; }
    }
}
