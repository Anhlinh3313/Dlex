namespace Core.Business.ViewModels
{
    public class GetWardByDistrictIdsViewModel
    {
        public GetWardByDistrictIdsViewModel()
        {
        }

        public int[] Ids { get; set; }
        public string Cols { get; set; }
        public bool IsHideExistWard { get; set; }
    }
}
