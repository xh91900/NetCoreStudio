using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreBase.ExtensionFunctions
{
    /// <summary>
    /// 用swagger action必须标记HTTP操作方法，get,post...
    /// </summary>
    public static class SwaggerExtension
    {
        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(p => {
                p.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo() {
                    Version = "v1",
                    Title = "Swagger WebAPI",
                    Description = "NetCoreBase项目API文档",
                    TermsOfService = new Uri("http://baidu.com"),
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                    {
                        Name = "NetCoreBase项目",
                        Email = "xh91900@126.com"
                        ,
                        Url = new Uri("https://www.baidu.com/")
                    }
                });
                
                p.SwaggerDoc("User", new Microsoft.OpenApi.Models.OpenApiInfo() { Title = "用户模块", Version = "User" });// 分组显示
                p.SwaggerDoc("Project", new Microsoft.OpenApi.Models.OpenApiInfo() { Title = "项目模块", Version = "Project" });// 分组显示

                var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);
                //添加接口XML的路径
                var xmlPath = Path.Combine(basePath, "NetCoreBase.xml");
                //如果需要显示控制器注释只需将第二个参数设置为true
                p.IncludeXmlComments(xmlPath, true);

            });
        }
    }
}
