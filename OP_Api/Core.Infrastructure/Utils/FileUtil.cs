using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Core.Infrastructure.ViewModels;
using Microsoft.DotNet.PlatformAbstractions;

namespace Core.Infrastructure.Utils
{
    public static class FileUtil
    {
        public static string SaveFile(string targetFolder, FileViewModel fileViewModel)
        {
            DateTime currentDate = DateTime.Now;
            string fileName = fileViewModel.FileName;

            string dir = $@"{ApplicationEnvironment.ApplicationBasePath}{targetFolder}/{currentDate.ToString("yyyy-MM")}";
            string fullPath = $@"{dir}/{fileName}";

            //Check Directory
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            for (int i = 0; ; i++)
            {
                FileInfo fileInfo = new FileInfo(fullPath);

                if (fileInfo.Exists)
                {
                    string[] arr = fileName.Split('.');
                    //Xóa string copy (i)
                    arr[0] = arr[0].Replace($" ({i})", "");
                    //Tạo string copy (i + 1)
                    arr[0] = $"{arr[0]} ({i + 1})";
                    //
                    fileName = string.Join(".", arr);
                    //
                    fullPath = $@"{dir}/{fileName}";
                }
                else
                {
                    break;
                }
            }
            byte[] byteArray = Convert.FromBase64String(fileViewModel.FileBase64String);
            using (var fs = new FileStream($@"{dir}/{fileName}", FileMode.CreateNew, FileAccess.Write))
            {
                fs.Write(byteArray, 0, byteArray.Length);
            }
            return $@"{targetFolder}/{currentDate.ToString("yyyy-MM")}/{fileName}";
        }

        public static string ReSizeImage(string base64String)
        {            //
            byte[] image = Convert.FromBase64String(base64String);
            using (var ms = new MemoryStream(image))
            {
                Image img = Image.FromStream(ms);
                if (img.Width > 1000 || img.Height > 1000)
                {
                    int newW = 0;
                    int newH = 0;
                    if (img.Width > img.Height)
                    {
                        newW = 1200;
                        double percent = (1200 * 100 / img.Width);
                        newH = (int)Math.Round((img.Height * percent / 100), 0);
                    }
                    else
                    {
                        newH = 1200;
                        double percent = (1200 * 100 / img.Height);
                        newW = (int)Math.Round((img.Width * percent / 100), 0);
                    }
                    //
                    Size size = new Size(newW, newH);
                    img = (Image)(new Bitmap(img, size));
                    //
                    var imageBytes = ImageToByteArray(img);
                    // Convert byte[] to Base64 String
                    string _base64String = Convert.ToBase64String(imageBytes);
                    return _base64String;
                }
                else
                {
                    return base64String;
                }
            }
        }
        public static byte[] ImageToByteArray(Image imageIn)
        {
            using (var stream = new MemoryStream())
            {
                imageIn.Save(stream, ImageFormat.Jpeg);
                var jpegByteSize = stream.Length;
                return stream.ToArray();
            }
        }

        public static ResponseViewModel IsValidationImageBase64(string imageBase64)
        {
            byte[] imageBytes;
            var result = new ResponseViewModel();
            if (imageBase64.Replace(" ", "").Length % 4 != 0)
            {
                result.Message = string.Format("File ảnh không hợp lệ!");
                return result;
            }
            try
            {
                imageBytes = Convert.FromBase64String(imageBase64);
            }
            catch(Exception EX)
            {
                result.Message = string.Format("File ảnh không hợp lệ!!!");
                return result;
            } 
            //if (imageBytes.Length > 800)
            //{
            //    result.Message = string.Format("Kích thước ảnh không hợp lệ: {0} bytes => max bytes: 800KB!", imageBytes);
            //    return result;
            //}
            using (var ms = new MemoryStream(imageBytes))
            {
                Image imageIn = Image.FromStream(ms);
                //if (imageIn.Height > 1000 || imageIn.Width > 1000)
                //{
                //    result.Message = string.Format("Kích thước ảnh không hợp lệ: {0} x {1} => max size: 1000 x 1000!", imageIn.Width, imageIn.Height);
                //    return result;
                //}
                //else
                //{
                    result.IsSuccess = true;
                    result.Message = "OK";
                    return result;
                //}
            }
        }

        public static FileViewModel GetFile(string relativePath)
        {
            FileViewModel fileViewModel = new FileViewModel();
            string fullPath = $@"{ApplicationEnvironment.ApplicationBasePath}{relativePath}";
            FileInfo file = new FileInfo(fullPath);
            if (file.Exists)
            {
                byte[] bytes = System.IO.File.ReadAllBytes(fullPath);
                fileViewModel.FileName = file.Name;
                fileViewModel.FileExtension = file.Extension;
                fileViewModel.FileBase64String = Convert.ToBase64String(bytes);
            }
            return fileViewModel;
        }

        public static bool IsBase64(string base64String)
        {
            if (base64String.Replace(" ", "").Length % 4 != 0)
            {
                return false;
            }
            try
            {
                Convert.FromBase64String(base64String);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
