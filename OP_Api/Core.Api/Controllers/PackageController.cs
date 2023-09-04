using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Business.Services.Abstract;
using Core.Business.Services.Models;
using Core.Business.ViewModels;
using Core.Data.Abstract;
using Core.Entity.Entities;
using Core.Entity.Procedures;
using Core.Infrastructure.Helper;
using Core.Infrastructure.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Core.Api.Controllers
{
    [Route("api/[controller]")]
    public class PackageController : GeneralController<PackageViewModel, PackageInfoViewModel, Package>
    {
        private readonly IGeneralService _iGeneralServiceRaw;

        public PackageController(
            Microsoft.Extensions.Logging.ILogger<dynamic> logger, IOptions<AppSettings> optionsAccessor,
            IOptions<JwtIssuerOptions> jwtOptions, IUnitOfWork unitOfWork, IGeneralService<PackageViewModel, PackageInfoViewModel, Package> iGeneralService,
            IGeneralService iGeneralServiceRaw) : base(logger, optionsAccessor, jwtOptions, unitOfWork, iGeneralService)
        {
            _iGeneralServiceRaw = iGeneralServiceRaw;
        }

        public override async Task<JsonResult> Create([FromBody]PackageViewModel viewModel)
        {
            if (viewModel == null) return JsonUtil.Error("Thông tin tạo gói lỗi!");
            if (!viewModel.ToHubId.HasValue) return JsonUtil.Error("Vui lòng chọn thông tin đến!");
            if (!Util.IsNull(viewModel.SealNumber))
            {
                var checkSealNumber = _unitOfWork.Repository<Proc_CheckExistSealNumber>().ExecProcedureSingle(Proc_CheckExistSealNumber.GetEntityProc(viewModel.SealNumber));
                if (checkSealNumber.TotalCount > 0) return JsonUtil.Error("Số seal đã được sử dụng!");
                viewModel.Code = viewModel.SealNumber;
                viewModel.Name = viewModel.SealNumber;
            }
            var currentUser = this.GetCurrentUser();
            viewModel.StatusId = PackageStatusHelper.NewCreate;
            viewModel.CreatedHubId = currentUser.HubId;
            var data = await _iGeneralService.Create(viewModel);
            if (data.IsSuccess)
            {
                if (Util.IsNull(viewModel.SealNumber))
                {
                    var package = data.Data as PackageInfoViewModel;
                    package.Code = $"PA{package.Id.ToString("D5")}";
                    package.Name = package.Code;
                    package.SealNumber = package.Code;
                    data = await _iGeneralService.Update(package);
                }
            }
            return JsonUtil.Create(data);
        }

        [HttpPost("ClosedPackage")]
        public async Task<JsonResult> ClosedPackage([FromBody]PackageViewModel viewModel)
        {
            var package = _unitOfWork.RepositoryR<Package>().GetSingle(viewModel.Id);
            if (package == null) return JsonUtil.Error("Gói không tồn tại");
            var shipmentIds = _unitOfWork.RepositoryR<ShipmentPackage>().FindBy(x => x.PackageId == package.Id).Select(x => x.ShipmentId);
            if (Util.IsNull(shipmentIds) || shipmentIds.Count() == 0) return JsonUtil.Error("Gói trống, không cho phép đóng!");
            package.StatusId = PackageStatusHelper.IsClosed;

            //
            if (Util.IsNull(viewModel.SealNumber))
            {
                package.SealNumber = package.Code;
            }
            else
            {
                package.SealNumber = viewModel.SealNumber;
            }
            package.Content = viewModel.Content;
            package.Weight = viewModel.Weight;
            package.CalWeight = viewModel.CalWeight;
            package.Length = viewModel.Length;
            package.Width = viewModel.Width;
            package.Height = viewModel.Height;

            _unitOfWork.RepositoryCRUD<Package>().Update(package);
            await _unitOfWork.CommitAsync();
            return JsonUtil.Success(package);
        }

        [HttpPost("OpenUpdatePackage")]
        public async Task<JsonResult> OpenUpdatePackage([FromBody]PackageViewModel viewModel)
        {
            var package = _unitOfWork.RepositoryR<Package>().GetSingle(viewModel.Id);
            if (package == null) return JsonUtil.Error("Gói không tồn tại");
            var shipmentIds = _unitOfWork.RepositoryR<ShipmentPackage>().FindBy(x => x.PackageId == package.Id).Select(x => x.ShipmentId);
            if (package.StatusId == PackageStatusHelper.IsUpdating) return JsonUtil.Error("Gói đã được mở trước đó!");
            package.StatusId = PackageStatusHelper.IsUpdating;
            _unitOfWork.RepositoryCRUD<Package>().Update(package);
            await _unitOfWork.CommitAsync();
            return JsonUtil.Success(package);
        }

        [HttpPost("TrackingPackage")]
        public JsonResult TrackingPackage([FromBody]PackageViewModel viewModel)
        {
            var package = _iGeneralServiceRaw.GetSingle<Package, PackageInfoViewModel>(f => viewModel.SealNumber == f.SealNumber || viewModel.SealNumber == f.Code, "ToHub,CreatedHub");
            return JsonUtil.Success(package);
        }

        [HttpPost("OpenCheckedPackage")]
        public async Task<JsonResult> OpenCheckedPackage([FromBody]PackageViewModel viewModel)
        {
            var package = _unitOfWork.RepositoryR<Package>().GetSingle(viewModel.Id);
            if (package == null) return JsonUtil.Error("Số seal không tồn tại");
            var shipmentIds = _unitOfWork.RepositoryR<ShipmentPackage>().FindBy(x => x.PackageId == package.Id).Select(x => x.ShipmentId);
            if (package.StatusId == PackageStatusHelper.IsChecked) return JsonUtil.Error("Gói chưa đóng, không  mở!");
            package.StatusId = PackageStatusHelper.IsOpen;
            _unitOfWork.RepositoryCRUD<Package>().Update(package);
            await _unitOfWork.CommitAsync();
            return JsonUtil.Success(package);
        }

        [HttpPost("CompleteCheckedPackage")]
        public async Task<JsonResult> CompleteCheckedPackage([FromBody]PackageViewModel viewModel)
        {
            var package = _unitOfWork.RepositoryR<Package>().GetSingle(viewModel.Id);
            if (package == null) return JsonUtil.Error("Gói không tồn tại");
            var shipmentIds = _unitOfWork.RepositoryR<ShipmentPackage>().FindBy(x => x.PackageId == package.Id).Select(x => x.ShipmentId);
            if (package.StatusId != PackageStatusHelper.IsOpen) return JsonUtil.Error("Không được phép thao tác, vui lòng mở gói trước!");
            package.StatusId = PackageStatusHelper.IsChecked;
            _unitOfWork.RepositoryCRUD<Package>().Update(package);
            await _unitOfWork.CommitAsync();
            return JsonUtil.Success();
        }

        [HttpPost("ScanShipmentInPackage")]
        public JsonResult ScanShipmentInPackage([FromBody]PackageScanViewModel viewModel)
        {
            var currentUser = GetCurrentUser();
            var result = _unitOfWork.Repository<Proc_ScanShipmentInPackage>()
                .ExecProcedureSingle(Proc_ScanShipmentInPackage.GetEntityProc(viewModel.ShipmentNumber, viewModel.PackageId, currentUser.Id));
            if (!Util.IsNull(result))
            {
                if (result.IsSuccess == true) return JsonUtil.Success(result);
                else return JsonUtil.Error(result.Message);
            }
            else
            {
                return JsonUtil.Error("Mã vận đơn '{0}' không tìm thấy", viewModel.ShipmentNumber);
            }
        }


        [HttpPost("ScanShipmentOutPackage")]
        public JsonResult ScanShipmentOutPackage([FromBody]PackageScanViewModel viewModel)
        {
            var currentUser = GetCurrentUser();
            var result = _unitOfWork.Repository<Proc_ScanShipmentOutPackage>()
                .ExecProcedureSingle(Proc_ScanShipmentOutPackage.GetEntityProc(viewModel.ShipmentNumber, viewModel.PackageId, currentUser.Id));
            if (!Util.IsNull(result))
            {
                if (result.IsSuccess == true) return JsonUtil.Success();
                else return JsonUtil.Error(result.Message);
            }
            else
            {
                return JsonUtil.Error("Mã vận đơn '{0}' không tìm thấy", viewModel.ShipmentNumber);
            }
        }

        [HttpPost("OpenPackage")]
        public async Task<JsonResult> OpenPackage([FromBody]PackageViewModel viewModel)
        {
            var package = _unitOfWork.RepositoryR<Package>().GetSingle(viewModel.Id);

            if (package == null)
            {
                return JsonUtil.Error("Gói không tồn tại");
            }
            _unitOfWork.RepositoryCRUD<Package>().Update(package);

            var shipmentIds = _unitOfWork.RepositoryR<ShipmentPackage>().FindBy(x => x.PackageId == package.Id).Select(x => x.ShipmentId);
            var shipments = _unitOfWork.RepositoryR<Shipment>().FindBy(pack => shipmentIds.Contains(pack.Id));

            foreach (var item in shipments)
            {
                item.PackageId = null;
                _unitOfWork.RepositoryCRUD<Shipment>().Update(item);
            }
            await _unitOfWork.CommitAsync();

            return JsonUtil.Success();
        }

        [HttpGet("GetPackageNew")]
        public JsonResult GetPackageNew(int toHubId)
        {
            var currentUser = GetCurrentUser();
            var result = this.FindBy(f => f.CreatedHubId == currentUser.HubId && f.ToHubId == toHubId && f.StatusId == PackageStatusHelper.NewCreate);
            return result;
        }

        [HttpGet("GetShipmentByPackageId")]
        public JsonResult GetShipmentByPackageId(int packageId, int? statusId = null, int? pageNumber = null, int? pageSize = null)
        {
            var result = _unitOfWork.Repository<Proc_GetShipmentByPackageId>()
                .ExecProcedure(Proc_GetShipmentByPackageId.GetEntityProc(packageId, statusId, pageNumber, pageSize));
            return JsonUtil.Success(result);
        }

        [HttpGet("GetShipmentByPackageCodeOrSeal")]
        public JsonResult GetShipmentByPackageCodeOrSeal(string codeOrSeal, int? statusId = null, int? pageNumber = null, int? pageSize = null)
        {
            var pack = _unitOfWork.RepositoryR<Package>().GetSingle(f => f.Code == codeOrSeal || f.SealNumber == codeOrSeal);
            if (Util.IsNull(pack))
            {
                return JsonUtil.Error("Không tìm thấy gói");
            }
            if (pack.StatusId != PackageStatusHelper.IsClosed)
            {
                return JsonUtil.Error("Vui lòng xác nhận đóng gói trước");
            }
            var result = _unitOfWork.Repository<Proc_GetShipmentByPackageId>()
                .ExecProcedure(Proc_GetShipmentByPackageId.GetEntityProc(pack.Id, statusId, pageNumber, pageSize));
            return JsonUtil.Success(result);
        }

        [HttpGet("GetListPackage")]
        public JsonResult GetListPackage(int? createdHubId = null, string searchText = null, int? statusId = null,
            DateTime? dateFrom = null, DateTime? dateTo = null, int? pageNumber = null, int? pageSize = null)
        {
            var data = _unitOfWork.Repository<Proc_GetListPackage>().ExecProcedure(Proc_GetListPackage.GetEntityProc(createdHubId, searchText, statusId, dateFrom, dateTo, pageNumber, pageSize));
            return JsonUtil.Success(data);
        }

        [HttpGet("GetPackageStatus")]
        public JsonResult GetPackageStatus()
        {
            var data = _unitOfWork.RepositoryR<PackageStatus>().GetAll();
            return JsonUtil.Success(data);
        }


        [HttpGet("GetPackageToPrint")]
        public JsonResult GetPackageToPrint(int packageId)
        {
            var data = _unitOfWork.Repository<Proc_GetPackageToPrint>().ExecProcedureSingle(Proc_GetPackageToPrint.GetEntityProc(packageId));
            if (Util.IsNull(data)) return JsonUtil.Error("Không tìm thấy thông tin gói");
            else return JsonUtil.Success(data);
        }

        #region DANH SACH GOI BY SHIPMENT
        [HttpPost("GetListPackageBy")]
        public JsonResult GetListPackageBy([FromBody] ShipmentFilterViewModel filterViewModel)
        {
            var data = _unitOfWork.Repository<Proc_GetListPackageBy>()
                      .ExecProcedure(Proc_GetListPackageBy.GetEntityProc(
            filterViewModel.OrderDateFrom,
            filterViewModel.OrderDateTo,
            filterViewModel.SenderId,
            filterViewModel.PaymentTypeId,
            filterViewModel.FromProvinceId,
            filterViewModel.ToProvinceId,
            filterViewModel.WeightFrom,
            filterViewModel.WeightTo,
            filterViewModel.ServiceId,
            filterViewModel.ShipmentNumber,
            filterViewModel.ShopCode,
            filterViewModel.ReferencesCode,
            filterViewModel.ReShipmentNumber,
            filterViewModel.SearchText,
            filterViewModel.IsExistInfoDelivery,
            filterViewModel.IsExistImagePickup,
            filterViewModel.IsBox,
            filterViewModel.IsPrintBill,
            filterViewModel.UploadExcelHistoryId,
            filterViewModel.GroupStatusId,
            filterViewModel.ShipmentStatusId,
            filterViewModel.FromHubId,
            filterViewModel.ToHubId,
            filterViewModel.CurrentHubId,
            filterViewModel.CurrentEmpId,
            filterViewModel.DeadlineTypeId,
            filterViewModel.pageNumber,
            filterViewModel.pageSize,
            filterViewModel.DeliveryUserId,
            filterViewModel.NumIssueDelivery,
            filterViewModel.IsSuccess,
            filterViewModel.IsGroupEmp,
            filterViewModel.ListGoodsId));
            return JsonUtil.Success(data);
        }
        #endregion
    }
}
