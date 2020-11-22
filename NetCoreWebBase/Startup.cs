using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extras.DynamicProxy;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using NetCoreWebBase.DapperHelper;

namespace NetCoreWebBase
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();

            services.AddSession();

            //本地内存缓存
            services.AddMemoryCache(p =>
            {
                p.Clock = new LocalClok();
            });

            //通过redis实现分布式缓存，这里会把session也存到redis里面
            services.AddDistributedRedisCache(p =>
            {
                p.Configuration = "127.0.0.1:6379";
                p.InstanceName = "DistributedRedisCache";
            });

            // 注入IOptions
            services.Configure<DBConnectionOption>(Configuration.GetSection("DBConnectionString"));

            //服务器端缓存
            services.AddResponseCaching();

            //支持跨域，允许任何域名，允许任何方法，允许任何头部请求。
            services.AddCors(p => p.AddPolicy("any", q => q.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

            //把jwt默认的claim映射关掉，如果不关闭就会修改从服务器返回的claim
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            }).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)//使用cookie作为验证用户的方式
            .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
            {
                options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.Authority = "http://localhost:5000";
                options.RequireHttpsMetadata = false;
                options.ClientId = "mvcclient";
                options.ClientSecret = "mvcsecret";
                options.SaveTokens = true;//把获取的token存到cookie里
                options.ResponseType = "code";//token的响应类型，id_token，token（accesstoken）

                options.Scope.Clear();
                options.Scope.Add("api1");
                options.Scope.Add(OidcConstants.StandardScopes.OpenId);
                options.Scope.Add(OidcConstants.StandardScopes.Profile);
                options.Scope.Add(OidcConstants.StandardScopes.OfflineAccess);//返回刷新token
            });
        }

        /// <summary>
        /// 使用autofac
        /// program里面增加UseServiceProviderFactory(new AutofacServiceProviderFactory())
        /// 和ConfigureServices不冲突，两边都可以注册
        /// </summary>
        /// <param name="containerBuilder"></param>
        public void ConfigureContainer(ContainerBuilder containerBuilder)
        {
            //通过Module注册，就不用一个一个的去注册了
            //containerBuilder.RegisterModule<CustomAutofacModule>();

            containerBuilder.RegisterType<Service>().As<IService>()
                .EnableInterfaceInterceptors()// 支持aop扩展
                .InterceptedBy(typeof(CustomAutofacCacheInterceptor));//要扩展的特性,也可以用特性写在action上

            containerBuilder.RegisterType<CustomAutofacCacheInterceptor>();
        }

        private class LocalClok : Microsoft.Extensions.Internal.ISystemClock
        {
            //以什么时间为过期时间的标准
            public DateTimeOffset UtcNow => DateTime.Now;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //中间件层面的缓存，也是搭配[ResponseCache(Duration =70)]特性使用，属于把html生成好之后的缓存，响应式缓存
            app.UseResponseCaching();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseAuthentication();

            app.UseHttpsRedirection();
            app.UseStaticFiles(new StaticFileOptions() { 
            FileProvider=new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(),@"wwwroot")),
            //OnPrepareResponse=p=>
            });

            app.UseSession();

            app.UseRouting();

            app.UseAuthorization();

            //支持跨域，搭配[EnableCors("any")]特性使用
            app.UseCors();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
