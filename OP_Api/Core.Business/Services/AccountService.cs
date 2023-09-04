using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Core.Infrastructure.Helper;
using Core.Business.Services.Models;
using Core.Data.Abstract;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Linq;
using Core.Infrastructure.Utils;
using Core.Entity.Entities;
using Core.Business.ViewModels;
using Core.Infrastructure.Extensions;
using AutoMapper;
using System.Linq.Expressions;
using Core.Entity.Procedures;
using System.Reflection.Metadata;
using System.Data.SqlClient;
using Core.Business.Services.Abstract;
using System.Collections.Generic;

namespace Core.Business.Services
{
    public partial class AccountService : BaseService, IAccountService
    {
        private readonly JwtIssuerOptions _jwtOptions;
        private readonly IEncryptionService _iEncryptionService;
        private readonly CompanyInformation _icompanyInformation;

        public AccountService(
            Microsoft.Extensions.Logging.ILogger<dynamic> logger,
            IOptions<AppSettings> optionsAccessor,
            IUnitOfWork unitOfWork,
            IOptions<JwtIssuerOptions> jwtOptions,
            IOptions<CompanyInformation> companyInformation,
            IEncryptionService iEncryptionService) : base(logger, optionsAccessor, unitOfWork)
        {
            _jwtOptions = jwtOptions.Value;
            ThrowIfInvalidOptions(_jwtOptions);
            _iEncryptionService = iEncryptionService;
            _icompanyInformation = companyInformation.Value;
        }

        public async Task<dynamic> CreateAccount(CreateAccountViewModel model)
        {
            User user = Mapper.Map<User>(model);
            _unitOfWork.RepositoryCRUD<User>().Insert(user);
            await _unitOfWork.CommitAsync();
            return JsonUtil.Success(ConvertUserToUserInfoViewModel(user));
        }

        public async Task<dynamic> UpdateAccount(UpdateAccountViewModel model)
        {
            User user = GetUser(model.Id);
            user = Mapper.Map(model, user);
            _unitOfWork.RepositoryCRUD<User>().Update(user);
            await _unitOfWork.CommitAsync();
            return JsonUtil.Success(ConvertUserToUserInfoViewModel(user));
        }

        public async Task<dynamic> ChangePassWord(ChangePassWordViewModel model)
        {
            User user = GetUser(model.UserId);
            if (user == null)
            {
                return JsonUtil.Error("Tài khoản đăng nhập không chính xác!");
            }
            if (user.IsBlocked == true)
            {
                return JsonUtil.Error("Tài khoản đăng nhập đã bị khóa!");
            }
            var checkPass = _iEncryptionService.EncryptPassword(model.CurrentPassWord + user.TypeUserId + user.CompanyId, user.SecurityStamp);
            if (user.PasswordHash != checkPass)
            {
                return JsonUtil.Error("Mật khẩu đăng nhập không chính xác!");
            }
            if (!Util.IsNull(user.IsPassWordBasic))
            {
                user.IsPassWordBasic = false;
            }
            var info = _unitOfWork.Repository<Proc_CheckInfoInUserId>().ExecProcedure(Proc_CheckInfoInUserId.GetEntityProc(model.UserId)).FirstOrDefault();
    
            user = Mapper.Map(model, user);
            _unitOfWork.RepositoryCRUD<User>().Update(user);
            await _unitOfWork.CommitAsync();
            if (info != null)
            {
                if (!string.IsNullOrWhiteSpace(model.NameClient) && model.NameClient == "CORE_OP")
                {
                    await FireBaseUtil.SendNotification(_icompanyInformation.FireBaseBrowserAPIKey, user.FireBaseToken, string.Format("CORE_OP", info.CountUser), info.CountUser);
                }
            }
            await _unitOfWork.CommitAsync();
            return JsonUtil.Success(ConvertUserToUserInfoViewModel(user));
        }

        public async Task<dynamic> DeleteAccount(BasicViewModel model)
        {
            User user = GetUser(model.Id);

            if (user != null)
            {
                user.IsEnabled = false;
                _unitOfWork.RepositoryCRUD<User>().Update(user);
                await _unitOfWork.CommitAsync();
                return JsonUtil.Success();
            }

            return JsonUtil.Error(ValidatorMessage.Account.NotExist);

        }

        public dynamic GetAccountInfo(int id)
        {
            var user = GetUser(id);
            var userRole = GetUserRole(id);

            var viewModel = ConvertUserToUserInfoViewModel(user);
            viewModel.UserRoleIds = userRole;

            if (user != null)
            {
                return JsonUtil.Success(viewModel);
            }

            return JsonUtil.Error(ValidatorMessage.Account.NotExist);
        }

        public dynamic GetAccountList()
        {
            return JsonUtil.Success(Mapper.Map<IEnumerable<UserInfoViewModel>>(_unitOfWork.RepositoryR<User>().FindBy(x => !x.IsHidden && x.IsEnabled)));
        }

        public async Task<dynamic> SignIn(SignInViewModel model)
        {
            var user = _unitOfWork.Repository<Proc_CheckInfoLogin>().ExecProcedureSingle(
                Proc_CheckInfoLogin.GetEntityProc(model.UserName, model.CompanyCode, model.TypeUserId));
            if (user == null)
            {
                return JsonUtil.Error("Tài khoản đăng nhập không thể đăng nhập vui lòng kiểm tra lại!");
            }
            if (user.IsBlocked == true)
            {
                return JsonUtil.Error("Tài khoản đăng nhập đã bị khóa!");
            }
            var checkPass = _iEncryptionService.EncryptPassword(model.PassWord + user.TypeUserId + user.CompanyId, user.SecurityStamp);
            if (user.PasswordHash != checkPass)
            {
                return JsonUtil.Error("Mật khẩu đăng nhập không chính xác!");
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.CHash, _iEncryptionService.HashSHA256(user.SecurityStamp)),
                new Claim(JwtRegisteredClaimNames.Jti, await _jwtOptions.JtiGenerator()),
                new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(_jwtOptions.IssuedAt).ToString(), ClaimValueTypes.Integer64),
                new Claim(JwtRegisteredClaimNames.Sid, user.CompanyId.ToString()),
                new Claim(JwtRegisteredClaimNames.Website, user.CompanyCode),
                new Claim(JwtRegisteredClaimNames.GivenName, user.TypeUserId.ToString())
            };

            // Create the JWT security token and encode it.
            var jwt = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                notBefore: _jwtOptions.NotBefore,
                expires: _jwtOptions.Expiration,
                signingCredentials: _jwtOptions.SigningCredentials);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            // Serialize and return the response
            return JsonUtil.Success(new
            {
                userId = user.Id.ToString(),
                userName = user.UserName,
                userFullName = user.FullName,
                token = encodedJwt,
                expires = (int)_jwtOptions.ValidFor.TotalSeconds,
                expiresDate = DateTime.Now.AddDays(_jwtOptions.ValidFor.Days),
                isPassWordBasic = user.IsPassWordBasic,
                companyId = user.CompanyId,
                companyCode = user.CompanyCode,
                typeUserId = user.TypeUserId,
            });
        }

        private User GetUser(int id)
        {
            return _unitOfWork.RepositoryR<User>().GetSingle(x => x.Id == id);
        }
        private dynamic GetUserRole(int userId)
        {
            return _unitOfWork.RepositoryR<UserRole>().FindBy(x => x.UserId == userId);
        }

        private UserInfoViewModel ConvertUserToUserInfoViewModel(User user)
        {
            return Mapper.Map<UserInfoViewModel>(user);
        }
    }
}
