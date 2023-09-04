using System;
using Microsoft.Extensions.DependencyInjection;
using Core.Data.Abstract;
using Core.Business.Services;
using Core.Data.Core;
using Microsoft.AspNetCore.Http;
using Core.Api.Core;
using Microsoft.AspNetCore.Authorization;
using Core.Api.Core.Sercurity;
using Core.Business.Services.Abstract;
using Core.Data;
using Microsoft.EntityFrameworkCore;

namespace Core.Api
{
    public partial class Startup
    {
        private void MappingScopeService(IServiceCollection services)
        {
            ////register the repository and context
            //services.AddScoped<ApplicationContext>();
            //services.AddTransient<DbContext, ApplicationContext>();
            //services.AddTransient(typeof(ApplicationContext));
            //services.AddTransient(typeof(DbContextOptions<ApplicationContext>));
     
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IAuthorizationHandler, PermissionsHandler>();
            //UnitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUnitOfWorkRRP, UnitOfWorkRRP>();
            //Services
            services.AddScoped(typeof(IGeneralService), typeof(GeneralService));
            services.AddScoped(typeof(IGeneralService<,>), typeof(GeneralService<,>));
            services.AddScoped(typeof(IGeneralService<,,>), typeof(GeneralService<,,>));
            services.AddScoped(typeof(IGeneralService<,,,>), typeof(GeneralService<,,,>));
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IEncryptionService, EncryptionService>();
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IShipmentService, ShipmentService>();
            services.AddScoped<ITruckScheduleService, TruckScheduleService>();
            services.AddScoped<IRequestShipmentService, RequestShipmentService>();
            services.AddScoped<IListReceiptMoneyService, ListReceiptMoneyService>();
            services.AddScoped<IListGoodsService, ListGoodsService>();
            services.AddScoped<IListCustomerPaymentService, ListCustomerPaymentService>();
            services.AddScoped<IHubService, HubService>();
            services.AddScoped<IKPIShipmentDetailService, KPIShipmentDetailService>();
            services.AddScoped<IKPIShipmentCusService, KPIShipmentCusService>();

            //Repository
            services.AddScoped(typeof(IEntityCRUDRepository<>), typeof(EntityCRUDRepository<>));
            services.AddScoped(typeof(IEntityRRepository<>), typeof(EntityRRepository<>));
            services.AddScoped(typeof(IEntityVPRepository<>), typeof(EntityVPRepository<>));
            //RepositoryRRP
            services.AddScoped(typeof(IEntityVPRepository<>), typeof(EntityVPRepositoryRRP<>));
            services.AddScoped<IPromotionService, PromotionService>();
        }
    }
}
