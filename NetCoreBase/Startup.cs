using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Consul;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NetCoreBase.Attributes;
using NetCoreBase.ExtensionFunctions;
using NetCoreBase.IdentityServer4;
using NetCoreBase.Models;

namespace NetCoreBase
{
    public class Startup
    {
        /// <summary>
        /// IConfiguration 是用来加载配置值的，可以加载内存键值对、JSON或XML配置文件，我们通常用来加载缺省的appsettings.json 
        /// 执行到Startup的时候，IConfiguration已经被注入到services了，不需要我们额外添加注入的代码，缺省就是读取appsettings.json文件
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        //默认返回void 可选地返回 IServiceProvider以使用自定义的容器

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // exceptionfilter的全局注册
            services.AddControllersWithViews(p =>
            {
                p.Filters.Add(typeof(CustomExceptionFilterAttribute));
                p.Filters.Add(typeof(CustomActionFilterAttribute));
            });

            // 注入IOptions
            services.Configure<TokenManagement>(Configuration.GetSection("tokenManagement"));

            //new ConfigurationBuilder().AddJsonFile("",true,true).Build().GetSection("").Get<>
            var token = Configuration.GetSection("tokenManagement").Get<TokenManagement>();
            services.AddJWTAuthentication(token);//jwt

            services.AddIdentityServerCenter();//ids授权中心

            services.AddIdentityServerClent();//ids客户端

            services.AddTransient<IConsulClient, ConsulClient>();
           
            Configuration.ConsulRegist();//方式一
           // services.AddSingleton<IConsulClient>(p =>
           // new ConsulClient(c => c.Address = new Uri("http://localhost:8900"))
           // );//方式二，consulClient.Agent.ServiceRegister在middleware中间件里面实现

            services.AddSwagger();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Map("/maptest", p => { p.Run(async (context) => { await context.Response.WriteAsync("任何基于路径/maptest的请求都会在这里面处理"); }); });
            // app.UseMiddleware<自定义的Middleware>();

            // 开启网络应用目录浏览
            app.UseDirectoryBrowser();

            // 使得 web root（默认为 wwwroot）下的文件可以被访问
            app.UseStaticFiles();

            app.UseIdentityServer();// ids4中间件

            app.UseLog4net();
            app.UseSwagger();
            app.UseSwaggerUI(p =>
            {
                p.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiDocument V1");//第二个参数v1和SwaggerDoc里面的Version一致
                p.SwaggerEndpoint("/swagger/User/swagger.json", "用户模块");  //分组显示
                p.SwaggerEndpoint("/swagger/Project/swagger.json", "项目模块");  //分组显示
            });

            app.UseAuthentication();//鉴权，注意添加这一句，启用验证(此处是jwt验证)

            app.UseRouting();

            app.UseAuthorization();//授权

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
