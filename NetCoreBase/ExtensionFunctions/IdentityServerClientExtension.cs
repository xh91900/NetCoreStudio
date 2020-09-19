using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreBase.ExtensionFunctions
{
    /// <summary>
    /// ids客户端其实就是去授权中心拿解密的公钥，然后用公钥解密token，如果能解密则说明是特定私钥加密的，则通过身份认证
    /// </summary>
    public static class IdentityServerClientExtension
    {
        public static void AddIdentityServerClent(this IServiceCollection services)
        {
            var UserGatewayKey = "UserGatewayKey";
            services.AddAuthentication("Bearer")//Scheme：指定读信息的方式
                .AddIdentityServerAuthentication(UserGatewayKey, p =>
                {
                    p.Authority = "http://localhost:5000";//ids4授权中心的地址
                    p.ApiName = "UserApi";
                    p.RequireHttpsMetadata = false;
                });
        }
    }
}
