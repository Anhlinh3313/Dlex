using Core.Entity.Entities;

namespace Core.Business.ViewModels
{
    public class TransportTypeViewModel : SimpleViewModel<TransportTypeViewModel, TransportType>
    {
        public TransportTypeViewModel()
        {
        }

        public bool IsRequiredTPL { get; set; }
        public int[] TPLIds { get; set; }
    }
}
