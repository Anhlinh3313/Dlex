using System;
namespace Core.Business.ViewModels
{
    public class GetByListGoodsCodeViewModel
    {
        public GetByListGoodsCodeViewModel()
        {
        }

        public string Code { get; set; }
        public int[] StatusIds { get; set; }
        public string Cols { get; set; }
    }
}
