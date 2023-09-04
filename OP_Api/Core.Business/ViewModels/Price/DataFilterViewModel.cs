using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels
{
    public class DataFilterViewModel
    {
        public DataFilterViewModel()
        {
            
        }

        public int typeInt1 { get; set; }
        public int typeInt2 { get; set; }
        public int typeInt3 { get; set; }

        public int[] arrayInt1 { get; set; }
        public int[] arrayInt2 { get; set; }

        public string typeString1 { get; set; }
    }
}
