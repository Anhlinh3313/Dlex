using Core.Entity.Entities;

namespace Core.Business.ViewModels
{
    public class ServiceViewModel : SimpleViewModel<ServiceViewModel, Service>
    {
        public ServiceViewModel() { }
        public bool IsSub { get; set; }
        public bool IsReturn { get; set; }
        public int? NUMBER_L_W_H_MULTIP { set; get; }
        public int? NUMBER_L_W_H_DIM { set; get; }

        public bool IsPublish { set; get; }
        public string VSEOracleCode { get; set; }
    }
}
