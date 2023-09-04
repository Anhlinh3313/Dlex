using System;
using Core.Business.ViewModels.Mappings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Http;
using Core.Infrastructure.Extensions;
using System.Net;
using Core.Business.Services.Models;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Core.Data;
using Core.Infrastructure.Http;
using Newtonsoft.Json;
using Core.Api.Hubs;
using Core.Infrastructure;
using Core.Business.Core.Utils;
using System.Threading;

namespace Core.Api
{
    public partial class Startup
    {
        protected string connectionString;
        protected string connectionStringRRP;

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            connectionString = Configuration.GetConnectionString("DefaultConnection");
            connectionStringRRP = Configuration.GetConnectionString("DefaultConnectionRRP");
            //Store global connection string
            Connection.Create(connectionString);
            ConnectionRRP.Create(connectionStringRRP);
            Console.WriteLine("DefaultConnection: " + ConnectionRRP.Instance.GetConnectionString());

            // Add framework services.
            var companyInformationSection = Configuration.GetSection("CompanyInformation");
            int timeOutSql = 180;
            Int32.TryParse(companyInformationSection.GetSection("TimeOutSql").Value, out timeOutSql);
            // Add framework services.
            services.Configure<FormOptions>(options => options.BufferBody = true);
            services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(connectionString
                , sqlServerOptions => sqlServerOptions.CommandTimeout(timeOutSql)
                ));
            services.AddDbContext<ApplicationContextRRP>(options =>
                options.UseSqlServer(connectionStringRRP
                , sqlServerOptions => sqlServerOptions.CommandTimeout(timeOutSql)
                ));
            //

            // Add framework services.
            services.AddOptions();

            // Automapper Configuration
            AutoMapperConfiguration.Configure();

            // Enable Cors
            //services.AddCors();
            services.Configure<SendMail>(Configuration.GetSection("SendMail"));
            services.AddCors(options => options.AddPolicy("CorsPolicy", builder =>
           {
               builder.AllowAnyMethod().AllowAnyHeader()
                      .AllowAnyOrigin()
                      .AllowCredentials();
           }));

            // Add MVC services to the services container.
            services.AddMvc()
                .AddJsonOptions(opts =>
                {
                    // Force Camel Case to JSON
                    opts.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    opts.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });

            // Configure with Microsoft.Extensions.Options.ConfigurationExtensions
            // Binding the whole configuration should be rare, subsections are more typical.
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.Configure<CompanyInformation>(Configuration.GetSection("CompanyInformation"));

            //Add service Http context
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //

            StaticHttpContextExtensions.AddHttpContextAccessor(services);

            // Extend services
            JwtConfigService(services);
            MappingScopeService(services);
            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IApplicationLifetime lifetime, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseDefaultFiles();
            app.UseStaticFiles();
            //Configure app
            app.UseStaticHttpContext();

            // Add MVC to the request pipeline.
            app.UseCors(builder =>
                builder.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod());

            app.UseCors("CorsPolicy");

            app.UseExceptionHandler(
              builder =>
              {
                  builder.Run(
                    async context =>
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        context.Response.Headers.Add("Access-Control-Allow-Origin", "*");

                        var error = context.Features.Get<IExceptionHandlerFeature>();
                        if (error != null)
                        {
                            context.Response.AddApplicationError(error.Error.Message);
                            await context.Response.WriteAsync(error.Error.Message).ConfigureAwait(false);
                        }
                    });
              });

            // Use Authentication
            app.UseAuthentication();

            // Extend config
            JwtConfig(app, loggerFactory);
            MiddlewareConfig(app, loggerFactory);
            StaticHttpContextExtensions.UseStaticHttpContext(app);

            //
            app.UseSignalR(routes =>
            {
                routes.MapHub<NotifyHub>("/api/SignalR");
            });

            // Use MVC
            app.UseMvc();
            //
            lifetime.ApplicationStarted.Register(OnApplicationStarted);
        }

        public void OnApplicationStarted()
        {
            var companyInformationSection = Configuration.GetSection("CompanyInformation");
            string companyName = companyInformationSection.GetSection("Name").Value;
            //VSE
            if (companyName == "vietstar")
            {
                //Thread thread4 = new Thread(PushUtil.PushPickupInfo);
                //thread4.IsBackground = true;
                //thread4.Start();

                //Thread thread3 = new Thread(PushUtil.PushDeliveryInfo);
                //thread3.IsBackground = true;
                //thread3.Start();

                //Thread thread2 = new Thread(PushUtil.GetInfoBillVSE);
                //thread2.IsBackground = true;
                //thread2.Start();

                //Thread thread1 = new Thread(PushUtil.PushBillVSE);
                //thread1.IsBackground = true;
                //thread1.Start();
            }
            else if (companyName == "gsdp")
            {
                Thread thread4 = new Thread(PushUtil.GetInfoBillGSDP);
                thread4.IsBackground = true;
                thread4.Start();

                Thread thread5 = new Thread(PushUtil.GetInfoBillReturnGSDP);
                thread5.IsBackground = true;
                thread5.Start();
            }
            else if (companyName == "gsdp-staging")
            {
                //Thread thread4 = new Thread(PushUtil.GetInfoBillGSDP_STAGING);
                //thread4.IsBackground = true;
                //thread4.Start();

                //Thread thread5 = new Thread(PushUtil.GetInfoBillReturnGSDPStaging);
                //thread5.IsBackground = true;
                //thread5.Start();
            }
            else if (companyName == "be")
            {
                //Thread thread4 = new Thread(PushUtil.PushStatusLazada);
                //thread4.IsBackground = true;
                //thread4.Start();
                ////
                //Thread thread5 = new Thread(PushUtil.PushStatusADayRoi);
                //thread5.IsBackground = true;
                //thread5.Start();
            }
            else if (companyName == "flashship")
            {
                //Thread thread4 = new Thread(PushUtil.PushStatusShipNhanh);
                //thread4.IsBackground = true;
                //thread4.Start();
                //
            }
        }
    }
}
