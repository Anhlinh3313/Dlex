using System;
namespace Core.Business.ViewModels
{
    public class UploadFileListReceiptMoneyViewModel
    {
        public int Id { get; set; }
        public int ListReceiptMoneyId { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public string FileBase64String { get; set; }

        public UploadFileListReceiptMoneyViewModel()
        {
        }
    }
}
