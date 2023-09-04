namespace Core.Business.ViewModels
{
    public class GetUsersViewModel
    {
        public int? CompanyId { get; set; }
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
        public string SearchText { get; set; }
    }
}
