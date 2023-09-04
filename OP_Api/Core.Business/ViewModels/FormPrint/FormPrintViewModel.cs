using Core.Entity.Entities;

namespace Core.Business.ViewModels
{
    public class FormPrintViewModel : SimpleViewModel<FormPrintViewModel, FormPrint>
    {
        public FormPrintViewModel() { }

        public int NumOrder { get; set; }
        public bool IsPublic { get; set; }
        public string FormPrintBody { get; set; }
        public int FormPrintTypeId { get; set; }
        public double? Width { get; set; }
        public double? Height { get; set; }
    }
}
