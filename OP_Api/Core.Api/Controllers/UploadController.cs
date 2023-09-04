using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Core.Business.Services.Abstract;
using Core.Business.Services.Models;
using Core.Business.ViewModels;
using Core.Data.Abstract;
using Core.Entity.Entities;
using Core.Entity.Procedures;
using Core.Infrastructure.Helper;
using Core.Infrastructure.Utils;
using Core.Infrastructure.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Core.Api.Controllers
{
    [Route("api/[controller]")]
    public class UploadController : BaseController
    {
        public UploadController(Microsoft.Extensions.Logging.ILogger<dynamic> logger, IOptions<AppSettings> optionsAccessor, IOptions<JwtIssuerOptions> jwtOptions, IUnitOfWork unitOfWork, IGeneralService iGeneralService) : base(logger, optionsAccessor, jwtOptions, unitOfWork, iGeneralService)
        {
        }

        [HttpPost("UploadDeliveryComplete")]
        public async Task<JsonResult> UploadDeliveryComplete([FromBody]ShipmentFileViewModel viewModel)
        {
            try
            {
                var userId = GetCurrentUserId();
                var shipment = _unitOfWork.RepositoryR<Shipment>().GetSingle(viewModel.ShipmentId);
                if (shipment == null) return JsonUtil.Error("Vận đơn không tồn tại");
                var fileInfo = new FileViewModel();
                var shipmentImage = new ShipmentImage();
                shipmentImage.ShipmentId = shipment.Id;
                fileInfo.FileName = viewModel.FileName;
                fileInfo.FileExtension = viewModel.FileExtension;
                fileInfo.FileBase64String = viewModel.FileBase64String;
                var result = FileUtil.IsValidationImageBase64(fileInfo.FileBase64String);
                if (!result.IsSuccess) return JsonUtil.Error(result.Message);
                shipmentImage.ImagePath = FileUtil.SaveFile("Uploads/ShipmentImage", fileInfo);
                shipmentImage.CreatedBy = userId;
                shipmentImage.CreatedWhen = DateTime.Now;
                shipmentImage.ImageType = 2;//Giao hang
                _unitOfWork.RepositoryCRUD<ShipmentImage>().Insert(shipmentImage);
                shipment.DeliveryImagePath = shipmentImage.ImagePath;
                _unitOfWork.RepositoryCRUD<Shipment>().Update(shipment);
                await _unitOfWork.CommitAsync();
                return JsonUtil.Success();
            }
            catch (Exception ex)
            {
                return JsonUtil.Error(ex.Message);
            }
        }

        [HttpPost("UploadImageIncidents")]
        public async Task<JsonResult> UploadImageIncidents([FromBody]ShipmentFileViewModel viewModel)
        {
            try
            {
                var userId = GetCurrentUserId();
                var fileInfo = new FileViewModel();
                var shipmentImage = new ShipmentImage();
                shipmentImage.ShipmentId = viewModel.ShipmentId;
                fileInfo.FileName = viewModel.FileName;
                fileInfo.FileExtension = viewModel.FileExtension;
                fileInfo.FileBase64String = viewModel.FileBase64String;

                var result = FileUtil.IsValidationImageBase64(fileInfo.FileBase64String);
                if (!result.IsSuccess) return JsonUtil.Error(result.Message);
                shipmentImage.ImagePath = FileUtil.SaveFile("Uploads/ShipmentImage", fileInfo);
                shipmentImage.CreatedBy = userId;
                shipmentImage.CreatedWhen = DateTime.Now;
                shipmentImage.ImageType = 3;//ảnh báo sự cố
                shipmentImage.IncidentsId = viewModel.IncidentsId;
                _unitOfWork.RepositoryCRUD<ShipmentImage>().Insert(shipmentImage);
                await _unitOfWork.CommitAsync();
                return JsonUtil.Success();
            }
            catch (Exception ex)
            {
                return JsonUtil.Error(ex.Message);
            }
        }

        [HttpPost("UploadDocCompensation")]
        public async Task<JsonResult> UploadDocCompensation([FromBody]UploadFileViewModel viewModel)
        {
            try
            {
                var compensation = _unitOfWork.RepositoryR<Compensation>().GetSingle(viewModel.Id);
                var fileInfo = new FileViewModel();
                fileInfo.FileName = viewModel.FileName;
                fileInfo.FileExtension = viewModel.FileExtension;
                fileInfo.FileBase64String = viewModel.FileBase64String;

                if (compensation == null) return JsonUtil.Error("Không tìm thấy dữ liệu!");
                if (!FileUtil.IsBase64(fileInfo.FileBase64String)) return JsonUtil.Error("File upload không hợp lệ");
                compensation.DocAliasPath = FileUtil.SaveFile($"Uploads/Compensation/Doc/{viewModel.Id}", fileInfo);
                _unitOfWork.RepositoryCRUD<Compensation>().Update(compensation);
                await _unitOfWork.CommitAsync();
                return JsonUtil.Success();
            }
            catch (Exception ex)
            {
                return JsonUtil.Error(ex.Message);
            }
        }

        [HttpPost("UploadImagePickupComplete")]
        public async Task<JsonResult> UploadImagePickupComplete([FromBody]ShipmentFileViewModel viewModel)
        {
            try
            {
                var userId = GetCurrentUserId();
                var shipment = _unitOfWork.RepositoryR<Shipment>().GetSingle(viewModel.ShipmentId);
                if (shipment == null) return JsonUtil.Error("Vận đơn không tồn tại");
                var fileInfo = new FileViewModel();
                var shipmentImage = new ShipmentImage();
                shipmentImage.ShipmentId = shipment.Id;
                fileInfo.FileName = viewModel.FileName;
                fileInfo.FileExtension = viewModel.FileExtension;
                fileInfo.FileBase64String = viewModel.FileBase64String;

                var result = FileUtil.IsValidationImageBase64(fileInfo.FileBase64String);
                if (!result.IsSuccess) return JsonUtil.Error(result.Message);
                shipmentImage.ImagePath = FileUtil.SaveFile("Uploads/ShipmentImage", fileInfo);
                shipmentImage.CreatedBy = userId;
                shipmentImage.CreatedWhen = DateTime.Now;
                shipmentImage.ImageType = 1;//lay hang
                _unitOfWork.RepositoryCRUD<ShipmentImage>().Insert(shipmentImage);
                shipment.PickupImagePath = shipmentImage.ImagePath;
                _unitOfWork.RepositoryCRUD<Shipment>().Update(shipment);
                await _unitOfWork.CommitAsync();
                return JsonUtil.Success();
            }
            catch (Exception ex)
            {
                return JsonUtil.Error(ex.Message);
            }
        }

        [HttpPost("UploadAvatarAccount")]
        public async Task<JsonResult> UploadAvatarAccount([FromBody]UploadFileViewModel viewModel)
        {
            try
            {
                var user = _unitOfWork.RepositoryR<User>().GetSingle(viewModel.Id);
                var fileInfo = new FileViewModel();
                fileInfo.FileName = viewModel.FileName;
                fileInfo.FileExtension = viewModel.FileExtension;
                fileInfo.FileBase64String = viewModel.FileBase64String;

                if (user == null) return JsonUtil.Error("Tài khoản không tồn tại");
                var result = FileUtil.IsValidationImageBase64(fileInfo.FileBase64String);
                if (!result.IsSuccess) return JsonUtil.Error(result.Message);
                user.AvatarPath = FileUtil.SaveFile($"Uploads/Accounts/Avatar/{viewModel.Id}", fileInfo);
                _unitOfWork.RepositoryCRUD<User>().Update(user);
                await _unitOfWork.CommitAsync();
                return JsonUtil.Success();
            }
            catch (Exception ex)
            {
                return JsonUtil.Error(ex.Message);
            }
        }

        [HttpPost("UploadIamgeSubmitToTreasurer")]
        public async Task<JsonResult> UploadIamgeSubmitToTreasurer([FromBody]UploadFileViewModel viewModel)
        {
            try
            {
                var submitMoney = _unitOfWork.RepositoryR<ListReceiptMoney>().GetSingle(viewModel.Id);
                var fileInfo = new FileViewModel();
                fileInfo.FileName = viewModel.FileName;
                fileInfo.FileExtension = viewModel.FileExtension;
                fileInfo.FileBase64String = viewModel.FileBase64String;
                if (submitMoney == null) return JsonUtil.Error("Phiên nộp tiền không tồn tại");
                var result = FileUtil.IsValidationImageBase64(fileInfo.FileBase64String);
                if (!result.IsSuccess) return JsonUtil.Error(result.Message);
                submitMoney.ImagePathDOC = FileUtil.SaveFile($"Uploads/SubmitToTreasurer/{viewModel.Id}", fileInfo);
                _unitOfWork.RepositoryCRUD<ListReceiptMoney>().Update(submitMoney);
                await _unitOfWork.CommitAsync();
                return JsonUtil.Success();
            }
            catch (Exception ex)
            {
                return JsonUtil.Error(ex.Message);
            }
        }

        [HttpPost("UploadIamgeListReceiptMoneyImage")]
        public async Task<JsonResult> UploadIamgeListReceiptMoneyImage([FromBody]UploadFileListReceiptMoneyViewModel viewModel)
        {
            try
            {
                var submitMoney = _unitOfWork.RepositoryR<ListReceiptMoney>().GetSingle(viewModel.ListReceiptMoneyId);
                if (submitMoney != null)
                {
                    var fileInfo = new FileViewModel();
                    fileInfo.FileName = viewModel.FileName;
                    fileInfo.FileExtension = viewModel.FileExtension;
                    fileInfo.FileBase64String = viewModel.FileBase64String;
                    var result = FileUtil.IsValidationImageBase64(fileInfo.FileBase64String);
                    if (!result.IsSuccess) return JsonUtil.Error(result.Message);
                    submitMoney.ImagePathDOC = FileUtil.SaveFile($"Uploads/SubmitToTreasurer/{viewModel.Id}", fileInfo);
                    var fListReceiptMoney = _unitOfWork.RepositoryR<ListReceiptMoneyImage>().FindBy(f => f.ListReceiptMoneyId == viewModel.ListReceiptMoneyId && f.Id == viewModel.Id).FirstOrDefault();
                    if (fListReceiptMoney != null)
                    {
                        fListReceiptMoney.ImagePath = submitMoney.ImagePathDOC;
                        _unitOfWork.RepositoryCRUD<ListReceiptMoneyImage>().Update(fListReceiptMoney);
                        await _unitOfWork.CommitAsync();
                    }
                    else
                    {
                        var listReceiptMoneyImage = new ListReceiptMoneyImage();
                        listReceiptMoneyImage.ListReceiptMoneyId = submitMoney.Id;
                        listReceiptMoneyImage.ImagePath = submitMoney.ImagePathDOC;

                        _unitOfWork.RepositoryCRUD<ListReceiptMoneyImage>().Insert(listReceiptMoneyImage);
                        await _unitOfWork.CommitAsync();
                    }
                    return JsonUtil.Success();

                } else
                {
                    return JsonUtil.Error("Phiên nộp tiền không tồn tại");
                }
            }
            catch (Exception ex)
            {
                return JsonUtil.Error(ex.Message);
            }
        }
        [HttpPost("GetImageListReceiptMoney")]
        public JsonResult GetImageListReceiptMoney([FromBody]GetImageByListReceiptMoneyId viewModel)
        {
            try
            {
                var fListReceiptMoney = _unitOfWork.RepositoryR<ListReceiptMoneyImage>().FindBy(f => f.ListReceiptMoneyId == viewModel.ListReceiptMoneyId);
                return JsonUtil.Success(fListReceiptMoney);
            }
            catch (Exception ex)
            {
                return JsonUtil.Error(ex.Message);
            }
        }

        [HttpPost("UploadImageComplainHandle")]
        public async Task<JsonResult> UploadImageComplainHandle([FromBody]UploadFileViewModel viewModel)
        {
            try
            {
                var complainHandle = _unitOfWork.RepositoryR<ComplainHandle>().GetSingle(viewModel.Id);
                var fileInfo = new FileViewModel();
                fileInfo.FileName = viewModel.FileName;
                fileInfo.FileExtension = viewModel.FileExtension;
                fileInfo.FileBase64String = viewModel.FileBase64String;

                if (complainHandle == null) return JsonUtil.Error("Không tìm thấy dữ liệu!");
                var result = FileUtil.IsValidationImageBase64(fileInfo.FileBase64String);
                if (!result.IsSuccess) return JsonUtil.Error(result.Message);
                complainHandle.HandleImagePath = FileUtil.SaveFile($"Uploads/Complain/ComplainHandle/{viewModel.Id}", fileInfo);
                _unitOfWork.RepositoryCRUD<ComplainHandle>().Update(complainHandle);
                await _unitOfWork.CommitAsync();
                return JsonUtil.Success();
            }
            catch (Exception ex)
            {
                return JsonUtil.Error(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("ComplainImage")]
        public async Task<JsonResult> ComplainImage([FromBody]UploadFileViewModel viewModel)
        {
            try
            {
                var complain = _unitOfWork.RepositoryR<Complain>().GetSingle(viewModel.Id);
                var fileInfo = new FileViewModel();
                fileInfo.FileName = viewModel.FileName;
                fileInfo.FileExtension = viewModel.FileExtension;
                fileInfo.FileBase64String = viewModel.FileBase64String;

                if (complain == null) return JsonUtil.Error("Không tìm thấy hỗ trợ/khiếu nại");
                var result = FileUtil.IsValidationImageBase64(fileInfo.FileBase64String);
                if (!result.IsSuccess) return JsonUtil.Error(result.Message);
                complain.ComplainImagePath = FileUtil.SaveFile($"Uploads/Complain/Complain/{viewModel.Id}", fileInfo);
                _unitOfWork.RepositoryCRUD<Complain>().Update(complain);
                await _unitOfWork.CommitAsync();
                return JsonUtil.Success();
            }
            catch (Exception ex)
            {
                return JsonUtil.Error(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("GetImageByPath")]
        public JsonResult GetImageByPath(string imagePath)
        {
            return JsonUtil.Success(FileUtil.GetFile(imagePath));
        }

        [AllowAnonymous]
        [HttpGet("GetImagePickup")]
        public JsonResult GetImagePickup(string shipmentNumber)
        {
            var shipment = _unitOfWork.RepositoryR<Shipment>().GetSingle(f => f.ShipmentNumber == shipmentNumber.Trim()
            || f.ShipmentNumber == shipmentNumber.Trim().ToLower()
            || f.ShipmentNumber == shipmentNumber.Trim().ToUpper());
            return JsonUtil.Success(FileUtil.GetFile(shipment.PickupImagePath));
        }

        [AllowAnonymous]
        [HttpGet("GetImageShipments")]
        public JsonResult GetImageShipments(string shipmentNumber)
        {
            var shipment = _unitOfWork.RepositoryR<Shipment>().GetSingle(f => f.ShipmentNumber == shipmentNumber.Trim()
            || f.ShipmentNumber == shipmentNumber.Trim().ToLower()
            || f.ShipmentNumber == shipmentNumber.Trim().ToUpper());
            List<FileViewModel> images = new List<FileViewModel>();
            var shipmentImages = _unitOfWork.RepositoryR<ShipmentImage>().FindBy(f => f.ShipmentId == shipment.Id);
            foreach (var image in shipmentImages)
            {
                images.Add(FileUtil.GetFile(image.ImagePath));
            }
            return JsonUtil.Success(images);
        }

        [HttpPost("UploadShipmentImages")]
        public async Task<JsonResult> UploadShipmentImages([FromBody]ShipmentFileImages viewModel)
        {
            try
            {
                var userId = GetCurrentUserId();
                var shipment = _unitOfWork.RepositoryR<Shipment>().GetSingle(viewModel.ShipmentId);
                if (shipment == null) return JsonUtil.Error("Vận đơn không tồn tại");

                var fileInfo = new FileViewModel();
                var count = 0;
                foreach (var fileViewModel in viewModel.FileViewModels)
                {
                    count++;
                    var shipmentImage = new ShipmentImage();
                    shipmentImage.ShipmentId = shipment.Id;
                    fileInfo.FileName = fileViewModel.FileName;
                    fileInfo.FileExtension = fileViewModel.FileExtension;
                    fileInfo.FileBase64String = fileViewModel.FileBase64String;

                    var result = FileUtil.IsValidationImageBase64(fileInfo.FileBase64String);
                    if (!result.IsSuccess) return JsonUtil.Error(result.Message);
                    shipmentImage.ImagePath = FileUtil.SaveFile("Uploads/ShipmentImage", fileInfo);
                    shipmentImage.CreatedBy = userId;
                    shipmentImage.CreatedWhen = DateTime.Now;
                    shipmentImage.ImageType = viewModel.ImageType;
                    _unitOfWork.RepositoryCRUD<ShipmentImage>().Insert(shipmentImage);
                    if (count == 1)
                    {
                        if (viewModel.ImageType == 1)
                        {
                            shipment.PickupImagePath = shipmentImage.ImagePath;
                            _unitOfWork.RepositoryCRUD<Shipment>().Update(shipment);
                        }
                        else if (viewModel.ImageType == 2)
                        {
                            shipment.DeliveryImagePath = shipmentImage.ImagePath;
                            _unitOfWork.RepositoryCRUD<Shipment>().Update(shipment);
                        }
                    }
                }
                await _unitOfWork.CommitAsync();
                return JsonUtil.Success();
            }
            catch (Exception ex)
            {
                return JsonUtil.Error(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("GetImageByShipmentId")]
        public JsonResult GetImageByShipmentId(int shipmentId, int imageType, int? incidentsId)
        {
            var shipmentImages = _unitOfWork.RepositoryR<ShipmentImage>().FindBy(f => f.ShipmentId == shipmentId && f.ImageType == imageType && (f.IncidentsId == incidentsId || !incidentsId.HasValue));
            if (shipmentImages.Count() > 0)
            {
                List<FileViewModel> images = new List<FileViewModel>();
                foreach (var image in shipmentImages)
                {
                    images.Add(FileUtil.GetFile(image.ImagePath));
                }
                return JsonUtil.Success(images);
            }
            else return JsonUtil.Error("Không tìm thấy hình ảnh.");
        }

        [AllowAnonymous]
        [HttpGet("GetImageByShipmentNumber")]
        public JsonResult GetImageByShipmentNumber(string shipmentNumber, int imageType)
        {
            var shipment = _unitOfWork.Repository<Proc_CheckShipmentNumber>().ExecProcedureSingle(Proc_CheckShipmentNumber.GetEntityProc(shipmentNumber));
            if (Util.IsNull(shipment)) return JsonUtil.Error("Không tìm thông tin.");
            var shipmentImages = _unitOfWork.RepositoryR<ShipmentImage>().FindBy(f => f.ShipmentId == shipment.Id && f.ImageType == imageType);
            if (shipmentImages.Count() > 0)
            {
                List<FileViewModel> images = new List<FileViewModel>();
                foreach (var image in shipmentImages)
                {
                    images.Add(FileUtil.GetFile(image.ImagePath));
                }
                return JsonUtil.Success(images);
            }
            else return JsonUtil.Error("Không tìm thấy hình ảnh.");
        }

        [HttpPost("UploadTruckScheduleImages")]
        public async Task<JsonResult> UploadTruckScheduleImages([FromBody]TruckScheduleFileImage viewModel)
        {
            try
            {
                var userId = GetCurrentUserId();
                var truckSchedule = _unitOfWork.RepositoryR<TruckSchedule>().GetSingle(viewModel.TruckScheduleId);
                if (truckSchedule == null) return JsonUtil.Error("Chuyến xe không tồn tại");
                var truckScheduleDetail = _unitOfWork.RepositoryR<TruckScheduleDetail>().FindBy(f => f.TruckScheduleId == truckSchedule.Id)
                    .OrderByDescending(o => o.Id).FirstOrDefault();
                if (truckScheduleDetail == null) return JsonUtil.Error("Hành trình xe không tồn tại");
                var fileInfo = new FileViewModel();
                foreach (var fileViewModel in viewModel.FileViewModels)
                {
                    var truckScheduleImage = new TruckScheduleImage();
                    truckScheduleImage.TruckScheduleDetailId = truckScheduleDetail.Id;
                    fileInfo.FileName = fileViewModel.FileName;
                    fileInfo.FileExtension = fileViewModel.FileExtension;
                    fileInfo.FileBase64String = fileViewModel.FileBase64String;

                    var result = FileUtil.IsValidationImageBase64(fileInfo.FileBase64String);
                    if (!result.IsSuccess) return JsonUtil.Error(result.Message);
                    truckScheduleImage.ImagePath = FileUtil.SaveFile("Uploads/TruckScheduleImage", fileInfo);
                    truckScheduleImage.CreatedBy = userId;
                    truckScheduleImage.CreatedWhen = DateTime.Now;
                    _unitOfWork.RepositoryCRUD<TruckScheduleImage>().Insert(truckScheduleImage);
                }
                await _unitOfWork.CommitAsync();
                return JsonUtil.Success();
            }
            catch (Exception ex)
            {
                return JsonUtil.Error(ex.Message);
            }
        }

    }
}
