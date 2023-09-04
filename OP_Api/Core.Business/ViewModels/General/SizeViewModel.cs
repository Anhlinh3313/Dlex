using System;
using Core.Entity.Entities;

namespace Core.Business.ViewModels
{
    public class SizeViewModel : SimpleViewModel<SizeViewModel, Size>
    {
        public SizeViewModel()
        {
        }

        public double Length { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public bool IsPackage { get; set; }
        public bool IsBox { get; set; }
    }
}
