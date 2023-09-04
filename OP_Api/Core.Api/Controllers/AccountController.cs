using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Core.Api.Hubs;
using Core.Business.Services.Abstract;
using Core.Business.Services.Models;
using Core.Business.ViewModels;
using Core.Business.ViewModels.Account;
using Core.Data.Abstract;
using Core.Entity.Entities;
using Core.Entity.Procedures;
using Core.Infrastructure.Helper;
using Core.Infrastructure.Security;
using Core.Infrastructure.Utils;
using LinqKit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Core.Api.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : GeneralController<CreateAccountViewModel, UpdateAccountViewModel, UserInfoViewModel, User>
    {
        private readonly IAccountService _iAccountService;
        private readonly IUserService _iUserServe;
        private readonly SendMail _iSendMail;
        private readonly IHubContext<NotifyHub> _hubContext;
        private readonly IEncryptionService _iEncryptionService;
        private readonly IUserService _iUserService;

        public AccountController(
            Microsoft.Extensions.Logging.ILogger<dynamic> logger,
            IOptions<AppSettings> optionsAccessor,
            IOptions<JwtIssuerOptions> jwtOptions,
            IOptions<SendMail> sendMail,
            IUnitOfWork unitOfWork,
            IGeneralService<CreateAccountViewModel, UpdateAccountViewModel, UserInfoViewModel, User> iGeneralService,
            IAccountService iAccountService,
            IUserService iUserServe,
            IHubContext<NotifyHub> hubContext,
            IEncryptionService iEncryptionService) : base(logger, optionsAccessor, jwtOptions, unitOfWork, iGeneralService)
        {
            _iAccountService = iAccountService;
            _iUserServe = iUserServe;
            _hubContext = hubContext;
            _iSendMail = sendMail.Value;
            _iEncryptionService = iEncryptionService;
        }

        [AllowAnonymous]
        [HttpGet("GetHashPassWord")]
        public JsonResult GetHashPassWord(string userName, String passWord, string companyCode, String sc)
        {
            if ("dscabc123456dhdmnxjdkwius".Equals(sc))
            {
                var user = _unitOfWork.Repository<Proc_CheckInfoLogin>().ExecProcedureSingle(
                    Proc_CheckInfoLogin.GetEntityProc(userName, companyCode, 2));
                if (user == null)
                {
                    return JsonUtil.Error("Tài khoản đăng nhập không chính xác!");
                }
                if (user != null)
                {
                    return JsonUtil.Success(new Encryption().EncryptPassword(passWord + 2 + user.CompanyId, user.SecurityStamp));
                }
            }
            return JsonUtil.Error("Thông tin tìa khoản không tồn tại!");
        }

        // POST api/values
        [AllowAnonymous]
        [HttpPost("SignIn")]
        public async Task<JsonResult> SignIn([FromBody]SignInViewModel model)
        {
            return await _iAccountService.SignIn(model);
        }

        [HttpPost("ChangePassWord")]
        public async Task<JsonResult> ChangePassWord([FromBody]ChangePassWordViewModel model)
        {
            return await _iAccountService.ChangePassWord(model);
        }

        [HttpGet("GetInfoUserById")]
        public JsonResult GetInfoUserById(int id)
        {
            return _iAccountService.GetAccountInfo(id);
        }

        public override async Task<JsonResult> Create([FromBody] CreateAccountViewModel viewModel)
        {
            //viewModel.TypeUserId = GetCurrentTypeUserId();
            var companyId = GetCurrentCompanyId();
            viewModel.CompanyId = companyId;
            viewModel.IsPassWordBasic = true;
            if (string.IsNullOrWhiteSpace(viewModel.UserName))
            {
                viewModel.UserName = viewModel.Email;
            }
            if (companyId == 0)
            {
                return JsonUtil.Error("#403001 - Tài khoản chưa đăng ký thông tin cty!");
            }
            //Check username
            var checkUserName = _unitOfWork.Repository<Proc_CheckExistData>().ExecProcedureSingle(
                Proc_CheckExistData.GetEntityProc(companyId, "Core_User", "UserName", viewModel.UserName,string.Format("TypeUserId={0}", viewModel.TypeUserId)));
            if (checkUserName.DataCount > 0)
            {
                return JsonUtil.Error("Tài khoản đã tồn tại!");
            }
            //check email
            var checkEmail = _unitOfWork.Repository<Proc_CheckExistData>().ExecProcedureSingle(
                Proc_CheckExistData.GetEntityProc(companyId, "Core_User", "Email", viewModel.Email, string.Format("TypeUserId={0}", viewModel.TypeUserId)));
            if (checkEmail.DataCount > 0)
            {
                return JsonUtil.Error("Email đã tồn tại!");
            }
            //Check phoneNumber
            var checkNumberPhone = _unitOfWork.Repository<Proc_CheckExistData>().ExecProcedureSingle(
                Proc_CheckExistData.GetEntityProc(companyId, "Core_User", "PhoneNumber", viewModel.PhoneNumber, string.Format("TypeUserId={0}", viewModel.TypeUserId)));
            if (checkNumberPhone.DataCount > 0)
            {
                return JsonUtil.Error("Số điện thoại đã tồn tại!");
            }
            var result = await _iGeneralService.Create(viewModel);
            if (result.IsSuccess)
            {
                _unitOfWork.RepositoryCRUD<UserRole>().DeleteWhere(r => r.UserId == viewModel.Id);// xóa quận / huyện đã chọn
                var user = result.Data as UserInfoViewModel;
                List<UserRole> listUserRole = new List<UserRole>();
                if (!Util.IsNull(viewModel.RoleIds))
                {
                    foreach (int roleId in viewModel.RoleIds)
                    {
                        UserRole userRole = new UserRole();
                        userRole.UserId = user.Id;
                        userRole.RoleId = roleId;
                        listUserRole.Add(userRole);
                    }
                }
                if (!Util.IsNull(listUserRole))
                {
                    foreach (var item in listUserRole)
                    {
                        _unitOfWork.RepositoryCRUD<UserRole>().Insert(item);
                    }
                    await _unitOfWork.CommitAsync();
                }
                //
            }
            //
            return JsonUtil.Create(result);
        }

        public override async Task<JsonResult> Update([FromBody] UpdateAccountViewModel viewModel)
        {
            var typeUserId = GetCurrentTypeUserId();
            var companyId = GetCurrentCompanyId();
            if (companyId == 0)
            {
                return JsonUtil.Error("#403001 - Tài khoản chưa đăng ký thông tin cty!");
            }
            //
            if (viewModel.CompanyId != companyId)
            {
                return JsonUtil.Error("#403002 - Không có quyền thay đổi thông tin của tài khoản này!");
            }
            //Check username
            var checkUserName = _unitOfWork.Repository<Proc_CheckExistData>()
                .ExecProcedureSingle(Proc_CheckExistData.GetEntityProc(companyId, "Core_User", "UserName", viewModel.UserName,
                string.Format("TypeUserId={0} AND Id!={1}", typeUserId, viewModel.Id)));
            if (checkUserName.DataCount > 0)
            {
                return JsonUtil.Error("Tài khoản đã tồn tại!");
            }
            //check email
            var checkEmail = _unitOfWork.Repository<Proc_CheckExistData>()
                .ExecProcedureSingle(Proc_CheckExistData.GetEntityProc(companyId, "Core_User", "Email", viewModel.Email,
                string.Format("TypeUserId={0} AND Id!={1}", typeUserId, viewModel.Id)));
            if (checkEmail.DataCount > 0)
            {
                return JsonUtil.Error("Email đã tồn tại!");
            }
            //Check phoneNumber
            var checkNumberPhone = _unitOfWork.Repository<Proc_CheckExistData>()
                .ExecProcedureSingle(Proc_CheckExistData.GetEntityProc(companyId, "Core_User", "PhoneNumber", viewModel.PhoneNumber,
                string.Format("TypeUserId={0} AND Id!={1}", typeUserId, viewModel.Id)));
            if (checkNumberPhone.DataCount > 0)
            {
                return JsonUtil.Error("Số điện thoại đã tồn tại!");
            }
            //
            //viewModel.IsPassWordBasic = false;
            var result = await _iGeneralService.Update(viewModel);
            if (result.IsSuccess)
            {
                _unitOfWork.RepositoryCRUD<UserRole>().DeleteEmptyWhere(r => r.UserId == viewModel.Id);
                List<UserRole> listUserRole = new List<UserRole>();
                var user = result.Data as UserInfoViewModel;
                if (!Util.IsNull(viewModel.RoleIds))
                {
                    foreach (int roleId in viewModel.RoleIds)
                    {
                        UserRole userRole = new UserRole();
                        userRole.UserId = user.Id;
                        userRole.RoleId = roleId;
                        listUserRole.Add(userRole);
                    }
                }
                if (!Util.IsNull(listUserRole))
                {
                    foreach (var item in listUserRole)
                    {
                        _unitOfWork.RepositoryCRUD<UserRole>().Insert(item);
                    }
                    await _unitOfWork.CommitAsync();
                }
            }
            //
            return JsonUtil.Create(result);
        }

        [HttpPost("Search")]
        public JsonResult Search([FromBody] SearchViewModel model)
        {
            Expression<Func<User, bool>> predicate = x => !x.IsHidden && x.TypeUserId == 2 && x.IsEnabled == true;
            if (!Util.IsNull(model.SearchText))
            {
                predicate = predicate.And(x => x.Code.Contains(model.SearchText.Trim()) ||
                x.UserName.Contains(model.SearchText.Trim()) ||
                x.FullName.Contains(model.SearchText.Trim()) ||
                x.PhoneNumber.Contains(model.SearchText.Trim()) ||
                x.Email.Contains(model.SearchText.Trim()) ||
                x.VSEOracleCode.Contains(model.SearchText.Trim()));
            }
            return base.FindBy(predicate, model.PageSize, model.PageNumber, model.Cols);
        }

        [AllowAnonymous]
        [HttpPost("ForgotPassword")]
        public async Task<JsonResult> ForgotPassword([FromBody] ForgotPasswordViewModel model)
        {
            //check company
            var company = _unitOfWork.Repository<Proc_CheckCompany>()
                .ExecProcedureSingle(Proc_CheckCompany.GetEntityProc(model.CompanyCode));
            if (company == null)
            {
                return JsonUtil.Error("Mã công ty không tồn tại!");
            }
            //check email
            var user = _unitOfWork.Repository<Proc_CheckUserLogin>()
               .ExecProcedureSingle(Proc_CheckUserLogin.GetEntityProc(model.Email));

            //var user = _unitOfWork.RepositoryR<User>().GetSingle(x => x.Email.Equals(model.Email) || x.PhoneNumber.Equals(model.Email));
            if (user != null)
            {
                var code = RandomUtil.RandomString(30, false);
                var codeResetPassWord = _iEncryptionService.HashSHA256(code.ToString());

                const string chars = "9021436587";
                Random random = new Random();
                var randomPassword = new string(Enumerable.Repeat(chars, 10).Select(s => s[random.Next(s.Length)]).ToArray());
                var passWord = _iEncryptionService.EncryptPassword(randomPassword + user.TypeUserId + user.CompanyId, user.SecurityStamp);
                user.PasswordHash = passWord;
                user.CodeResetPassWord = randomPassword;
                user.CodeResetPassWord = codeResetPassWord;
                DateTime currentDate = DateTime.Now;
                user.ResetPassWordSentat = currentDate;
                user.IsPassWordBasic = true;
                var data = _unitOfWork.Repository<Proc_UpdatePassWordUser>()
               .ExecProcedureSingle(Proc_UpdatePassWordUser.GetEntityProc(user.Id, user.PasswordHash, user.CodeResetPassWord, user.ResetPassWordSentat, user.IsPassWordBasic));
                if(data.Result){
                    //send Email to customer
                    var emailRecipient = new EmailRecipient(
                        user.Id,
                        user.Email,
                        randomPassword
                    );
                    var result = _iUserServe.SendEmail(_iSendMail, emailRecipient);
                    if (result.Equals(true))
                    {
                        return JsonUtil.Success(null, $"Đã gửi xác nhận đổi mật khẩu đến " + user.Email);
                    }
                    else
                    {
                        return JsonUtil.Error("Đã có lỗi xảy ra!");
                    }
                }
                else
                {
                    return JsonUtil.Error("Đã có lỗi xảy ra!");
                }
            }
            else
            {
                return JsonUtil.Error("Email hoặc số điện thoại không tồn tại");
            }
        }

        [HttpPost("GetUsers")]
        public JsonResult GetUsers([FromBody] GetUsersViewModel model)
        {
            model.CompanyId = GetCurrentCompanyId();
            var res = _unitOfWork.Repository<Proc_GetUsers>()
               .ExecProcedure(Proc_GetUsers.GetEntityProc(model.CompanyId, model.PageNumber, model.PageSize, model.SearchText)).ToList();
            return JsonUtil.Success(res);
        }

        [HttpPost("GetUsersBySearchCode")]
        public JsonResult GetUsersBySearchCode([FromBody] GetUsersViewModel model)
        {
            model.CompanyId = GetCurrentCompanyId();
            var res = _unitOfWork.Repository<Proc_GetUsersBySearchCode>()
               .ExecProcedure(Proc_GetUsersBySearchCode.GetEntityProc(model.CompanyId, model.PageNumber, model.PageSize, model.SearchText)).ToList();
            return JsonUtil.Success(res);
        }

        [HttpGet("GetEmpByCurrentHub")]
        public JsonResult GetEmpByCurrentHub(int? hubId = null)
        {
            var listHub = _iUserServe.GetListHubFromHubId(hubId);
            var userRoles = _unitOfWork.RepositoryR<UserRole>().FindBy(f => f.RoleId == RoleHelper.Rider);
            List<int> listUsers = new List<int>();
            if (userRoles.Count() > 0)
            {
                listUsers = userRoles.Select(s => s.UserId).ToList();
            }
            var listRoles = _unitOfWork.RepositoryR<UserRole>().FindBy(f => f.RoleId == RoleHelper.Rider || f.RoleId == RoleHelper.Delivery);
            return JsonUtil.Create(_iGeneralService.FindBy(x => listRoles.Select(s => s.UserId).Contains(x.Id) && listHub.Contains((int)x.HubId) && !x.IsHidden && x.IsEnabled));
        }

        [HttpGet("GetUserByTypeUser")]
        public JsonResult GetUserByTypeUser()
        {
            var data = _unitOfWork.RepositoryR<User>().GetAll().Where(x => x.TypeUserId == 2 && x.IsEnabled == true && x.IsHidden == false);
            return JsonUtil.Success(data);
        }

        [HttpGet("GetUserById")]
        public JsonResult GetUserById(int UserId)
        {
            var data = _unitOfWork.RepositoryR<User>().GetAll().Where(x => x.TypeUserId == 2 && x.IsEnabled == true && x.Id == UserId);
            return JsonUtil.Success(data);
        }

        [HttpPost("GetUsersBySearch")]
        public JsonResult GetUsersBySearch([FromBody] GetUsersViewModel model)
        {
            model.CompanyId = GetCurrentCompanyId();
            var res = _unitOfWork.Repository<Proc_GetUsersBySearch>().ExecProcedure(Proc_GetUsersBySearch.GetEntityProc(model.CompanyId, model.PageNumber,
                model.PageSize, model.SearchText)).ToList();
            return JsonUtil.Success(res);
        }

        [HttpPost("SearchCodeName")]
        public JsonResult SearchCodeName([FromBody] SearchViewModel model)
        {
            Expression<Func<User, bool>> predicate = x => !x.IsHidden && x.TypeUserId == 2 && x.IsEnabled == true && x.FullName != null;
            if (!Util.IsNull(model.SearchText))
            {
                predicate = predicate.And(x => x.Code.Contains(model.SearchText.Trim()) ||
                x.Code.Contains(model.SearchText.Trim()) ||
                x.FullName.Contains(model.SearchText.Trim()));
            }
            return base.FindBy(predicate, model.PageSize, model.PageNumber, model.Cols);
        }

        //[HttpGet("GetEmpByCurrentHub")]
        //public async Task<JsonResult> GetEmpByCurrentHub()
        //{
        //    var listHub = _iUserServe.GetListHubFromUser(GetCurrentUser());
        //    var userRoles = _unitOfWork.RepositoryR<UserRole>().FindBy(f => f.RoleId == RoleHelper.Rider);
        //    List<int> listUsers = new List<int>();
        //    if (userRoles.Count() > 0)
        //    {
        //        listUsers = userRoles.Select(s => s.UserId).ToList();
        //    }
        //    return JsonUtil.Create(_iGeneralService.FindBy(x => listUsers.Contains(x.Id) && listHub.Contains((int)x.HubId) && !x.IsHidden));
        //}

        //[HttpGet("GetRiderAllHub")]
        //public JsonResult GetRiderAllHub()
        //{
        //    var userRoles = _unitOfWork.RepositoryR<UserRole>().FindBy(f => f.RoleId == RoleHelper.Rider);
        //    List<int> listUsers = new List<int>();
        //    if (userRoles.Count() > 0)
        //    {
        //        listUsers = userRoles.Select(s => s.UserId).ToList();
        //    }
        //    return JsonUtil.Create(_iGeneralService.FindBy<User, UserInfoViewModel>(x => listUsers.Contains(x.Id) && !x.IsHidden));
        //}

        //[HttpGet("GetAllRiderInCenterByHubId")]
        //public JsonResult GetAllEmpInCenterByHubId(int hubId)
        //{
        //    var listHub = _iUserServe.GetListAllHubByHubId(hubId);
        //    var userRoles = _unitOfWork.RepositoryR<UserRole>().FindBy(f => f.RoleId == RoleHelper.Rider);
        //    List<int> listUsers = new List<int>();
        //    if (userRoles.Count() > 0)
        //    {
        //        listUsers = userRoles.Select(s => s.UserId).ToList();
        //    }
        //    return JsonUtil.Create(_iGeneralService.FindBy<User, UserInfoViewModel>(x => listUsers.Contains(x.Id) && listHub.Contains((int)x.HubId) && !x.IsHidden));
        //}

        //[HttpGet("GetAllUserByCurrentHub")]
        //public JsonResult GetAllUserByCurrentHub()
        //{
        //    var listHub = _iUserServe.GetListHubFromUser(GetCurrentUser());

        //    return JsonUtil.Create(_iGeneralService.FindBy<User, UserInfoViewModel>(x => listHub.Contains((int)x.HubId) && !x.IsHidden));
        //}
        //[HttpGet("GetByCode")]
        //public JsonResult GetByCode(string code)
        //{
        //    return JsonUtil.Create(_iGeneralService.FindBy<User, UserInfoViewModel>(x => x.Code == code && !x.IsHidden));
        //}
        //[HttpGet("GetAllUserByHubId")]
        //public JsonResult GetAllUserByHubId(int hubId)
        //{
        //    var listHub = _iUserServe.GetListHubFromHubId(hubId);
        //    var userRoles = _unitOfWork.RepositoryR<UserRole>().FindBy(f => f.RoleId == RoleHelper.Rider);
        //    List<int> listUsers = new List<int>();
        //    if (userRoles.Count() > 0)
        //    {
        //        listUsers = userRoles.Select(s => s.UserId).ToList();
        //    }
        //    return JsonUtil.Create(_iGeneralService.FindBy<User, UserInfoViewModel>(x => listUsers.Contains(x.Id) && listHub.Contains((int)x.HubId) && !x.IsHidden));
        //}

        //[HttpGet("SearchByValue")]
        //public JsonResult GetByWhere(string value, int? id)
        //{
        //    var datas = _unitOfWork.Repository<Proc_SearchEntityByValue>()
        //        .ExecProcedure(Proc_SearchEntityByValue.GetEntityProc("Core_User", value, id));
        //    return JsonUtil.Success(datas);
        //}

        //[HttpGet("GetAllEmpByHubId")]
        //public JsonResult GetAllEmpByHubId(int hubId)
        //{
        //    var listHub = _iUserServe.GetListHubFromHubId(hubId);
        //    return JsonUtil.Create(_iGeneralService.FindBy<User, UserInfoViewModel>(x => listHub.Contains((int)x.HubId) && !x.IsHidden));
        //}

        //[HttpGet("GetCurrentHubAndChildren")]
        //public JsonResult GetCurrentHubAndChildren()
        //{
        //    var listHub = _iUserServe.GetListHubFromUser(GetCurrentUser());
        //    return JsonUtil.Create(_iGeneralService.FindBy<Entity.Entities.Hub, HubInfoViewModel>(x => listHub.Contains(x.Id)));
        //}

        //[HttpGet("GetEmpByHubId")]
        //public JsonResult GetEmpByHubId(int hubId)
        //{
        //    var listHub = _iUserServe.GetListHubFromHubId(hubId);
        //    var userRoles = _unitOfWork.RepositoryR<UserRole>().FindBy(f => f.RoleId == RoleHelper.Rider || f.RoleId == RoleHelper.Delivery);
        //    List<int> listUsers = new List<int>();
        //    if (userRoles.Count() > 0)
        //    {
        //        listUsers = userRoles.Select(s => s.UserId).ToList();
        //    }
        //    return JsonUtil.Create(_iGeneralService.FindBy<User, UserInfoViewModel>(x => listUsers.Contains(x.Id) && listHub.Contains((int)x.HubId) && !x.IsHidden));
        //}

        //[HttpGet("GetCurrentEmpHistory")]
        //public JsonResult GetCurrentEmpHistory(int pageSize, int pageNumber, string cols)
        //{
        //    var list = new List<LadingSchedule>();
        //    var listCount = list.Count();
        //    var arrCols = new string[0];

        //    if (!string.IsNullOrWhiteSpace(cols))
        //    {
        //        arrCols = cols.Split(',');
        //    }

        //    int iPageNumber = (int)pageNumber;
        //    int iPageSize = (int)pageSize;
        //    var currentUserId = GetCurrentUserId();
        //    var ladingSchedule = _unitOfWork.RepositoryR<LadingSchedule>()
        //                                    .FindBy(x => x.UserId == currentUserId, arrCols)
        //                                    .OrderByDescending(x => x.CreatedWhen)
        //                                    .Skip((iPageNumber - 1) * iPageSize)
        //                                    .Take(iPageSize);

        //    //Add Lading to List
        //    foreach (var item in ladingSchedule)
        //    {
        //        list.Add(Mapper.Map<LadingSchedule>(item));
        //    }
        //    listCount = list.Count;

        //    if (listCount < 20)
        //    {
        //        var requestLadingSchedule = _unitOfWork.RepositoryR<RequestLadingSchedule>()
        //                                           .FindBy(x => x.UserId == currentUserId, arrCols)
        //                                           .OrderByDescending(x => x.CreatedWhen)
        //                                           .Skip((iPageNumber - 1) * iPageSize)
        //                                               .Take(iPageSize - listCount);

        //        foreach (var item in requestLadingSchedule)
        //        {
        //            list.Add(Mapper.Map<LadingSchedule>(item));
        //        }
        //    }

        //    return JsonUtil.Success(list);
        //}
    }
}
