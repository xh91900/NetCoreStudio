using Microsoft.Extensions.DependencyInjection;
using NetCoreBase.IdentityServer4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreBase.ExtensionFunctions
{
    /// <summary>
    /// ids授权中心
    /// </summary>
    public static class IdentityServerCenterExtension
    {
        public static void AddIdentityServerCenter(this IServiceCollection services)
        {
            //客户端模式，
            services.AddIdentityServer()
                    .AddDeveloperSigningCredential()//默认的开发者证书--临时证书
                    .AddInMemoryClients(InitConfig.GetClients())//inmemory内存模式
                    .AddInMemoryApiResources(InitConfig.GetApiResources());//能访问啥资源
        }
    }
}
