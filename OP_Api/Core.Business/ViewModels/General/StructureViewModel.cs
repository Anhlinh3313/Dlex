using Core.Entity.Entities;
using System;
namespace Core.Business.ViewModels
{
    public class StructureViewModel : SimpleViewModel<StructureViewModel, Structure>
    {
        public StructureViewModel()
        {
        }
        public string VSEOracleCode { get; set; }
    }
}
