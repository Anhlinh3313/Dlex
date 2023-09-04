using Core.Entity.Entities;

namespace Core.Business.ViewModels
{
    public class GetListFormPrintViewModel
    {
        public GetListFormPrintViewModel() { }

        public int? FormPrintId { get; set; }
        public int? FormPrintTypeId { get; set; }
        public string SearchText { get; set; }
        public int? PageNum { get; set; }
        public int? PageSize { get; set; }
    }
}
