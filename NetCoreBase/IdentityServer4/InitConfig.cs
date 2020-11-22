using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreBase.IdentityServer4
{
    public class InitConfig
    {
        /// <summary>
        /// 定义验证条件的client
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Client> GetClients()
        {
            return new[]
            {
                new Client
                {
                    ClientId="xh91900",//客户端唯一标识,比如我是美团的客户端
                ClientSecrets=new[]{ new Secret("111111".Sha256())},
                AllowedGrantTypes=GrantTypes.ClientCredentials,//授权方式，客户端认证，只要ClientId和ClientSecrets
                AllowedScopes=new []{"UserApi" },//允许访问的资源
                Claims={
                new ClientClaim(IdentityModel.JwtClaimTypes.Role,"Admin"),
                new ClientClaim(IdentityModel.JwtClaimTypes.NickName,"altman"),
                new ClientClaim("email","xh91900@126.com")
                }
                }
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new[]
            {
                new ApiResource("UserApi","用户获取API")
            };
        }
    }
}
