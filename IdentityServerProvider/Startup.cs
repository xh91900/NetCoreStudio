using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace IdentityServerProvider
{
    public class Startup
    {
        public IHostEnvironment Environment { get; }

        public IConfiguration Configuration { get; }

        public Startup(IHostEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //安装IdentityServer4包
            //查看endpoint端点地址.well-known/openid-configuration(发现文档)
            var builder = services.AddIdentityServer(IdsOptions =>
            {
                IdsOptions.Events.RaiseErrorEvents = true;
                IdsOptions.Events.RaiseInformationEvents = true;
                IdsOptions.Events.RaiseFailureEvents = true;
                IdsOptions.Events.RaiseSuccessEvents = true;
            })
               .AddTestUsers(Config.GetUsers());//哪些User可以被这个AuthrizationServer识别并授权

            builder.AddInMemoryIdentityResources(Config.GetIdentityResources());
            //builder.AddInMemoryApiResources(Config.GetApiResources());
            builder.AddInMemoryApiScopes(Config.ApiScopes);
            builder.AddInMemoryClients(Config.GetClients());

            if (Environment.IsDevelopment())
            {
                //对于Token签名需要一对公钥和私钥
                //IdentityServer为开发者提供了一个AddDeveloperSigningCredential()方法，它会帮我们搞定这个事，并默认存到硬盘中
                builder.AddDeveloperSigningCredential();
            }
            else
            {
                //正式环境需要用正式证书
                builder.AddSigningCredential(new System.Security.Cryptography.X509Certificates.X509Certificate2(System.IO.Path.Combine(AppContext.BaseDirectory,
                Configuration["Certificates:CerPath"]),
                Configuration["Certificates:Password"]));

                throw new Exception("配置key material");
            }


            //services.AddAuthentication()
            ////添加Google第三方身份认证服务（按需添加）
            ////.AddGoogle("Google", options =>
            ////{
            ////    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
            ////    options.ClientId = "434483408261-55tc8n0cs4ff1fe21ea8df2o443v2iuc.apps.googleusercontent.com";
            ////    options.ClientSecret = "3gcoTrEDPPJ0ukn_aYYT6PWo";
            ////})
            //.AddOpenIdConnect("oidc", "OpenID Connection", options =>
            //  {
            //      options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
            //      options.SignOutScheme = IdentityServerConstants.SignoutScheme;
            //      options.Authority = "https://demo.identityserver.io/";
            //      options.ClientId = "implicit";
            //      options.TokenValidationParameters = new TokenValidationParameters
            //      {
            //          NameClaimType = "name",
            //          RoleClaimType = "role"
            //      };

            //  });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //添加IdentityServer中间件
            app.UseIdentityServer();


            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });
        }
    }
}
