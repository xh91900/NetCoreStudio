using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServerProvider
{
    public class Config
    {
        /// <summary>
        /// 这个方法是来描述和阐述，提供外部选择的作用域
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("api1","api1")
                {
                    Scopes ={"api1"},//重要,不配置返回 invalid_scope
                }
            };
        }

        public static IEnumerable<ApiScope> ApiScopes => new List<ApiScope>
            {
                new ApiScope("api1", "My API")
        };

        /// <summary>
        /// 作用域的名称和验证规则
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "console client",
                    //授权方式为客户端授权，类型可参考GrantTypes枚举
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    //授权密钥，客户端和服务端事先约定的一个Key
                    ClientSecrets =
                    {
                        new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256())
                    },                     
                    //允许客户端访问的Scopes[作用域]
                    AllowedScopes = { "api1" }//需要额外添加 }
                },
                new Client
                {
                    ClientId="passwordclient",
                    AllowedGrantTypes=GrantTypes.ResourceOwnerPassword,
                    ClientSecrets=
                    {
                        new Secret("passwordsecrect".Sha256())
                    },
                    AllowedScopes={ "api1"}
                },
                new Client
                {
                    ClientId="mvcclient",
                    AllowedGrantTypes=GrantTypes.CodeAndClientCredentials,
                    ClientSecrets=
                    {
                        new Secret("mvcsecret".Sha256())
                    },
                    RedirectUris={ "http://localhost:5001/signin-oidc"},
                    FrontChannelLogoutUri="http://localhost:5001/signout-oidc",
                    PostLogoutRedirectUris={ "http://localhost:5001/signout-callback-oidc" },
                    AlwaysIncludeUserClaimsInIdToken=true,//返回idtoken的同时把所有UserClaims信息都返回
                    AllowOfflineAccess=true,//允许离线访问,是否可以申请刷新token
                    AccessTokenLifetime=60,//单位秒
                    AllowedScopes={ "api1",IdentityServerConstants.StandardScopes.OpenId}
                },
                new Client
                {
                    ClientId="hybridclient",
                    AllowedGrantTypes=GrantTypes.Hybrid,
                    ClientSecrets=
                    {
                        new Secret("hybridsecrect".Sha256())
                    },
                    RedirectUris={ "http://localhost:5001/signin-oidc" },
                    PostLogoutRedirectUris={ "http://localhost:5001/signout-callback-oidc" },
                    AllowOfflineAccess=true,//允许离线访问,是否可以申请刷新token
                    AllowedScopes={ "api1"}
                }
            };
        }

        /// <summary>
        /// 这个方法是来规范tooken生成的规则和方法的。一般不进行设置，直接采用默认的即可。
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),//如果要请求任何IdentityResources，必须要先添加OpenId
                new IdentityResources.Profile()
            };
        }

        /// <summary>
        /// 针对用户密码验证方式需要比对的账户
        /// </summary>
        /// <returns></returns>
        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>()
                   {
                       new TestUser()
                       {
                           SubjectId="1",
                           Username="YCZ",
                           Password="123456",
                           ProviderName="",        //获取或设置提供程序名称。
                           ProviderSubjectId="",   //获取或设置提供程序主题标识符。
                           IsActive=true,          //获取或设置用户是否处于活动状态。
                           Claims=new List<Claim>()     
        //身份  这是一个List 里面放的是包含的身份。 身份的概念就类似于(一个人是教师/父亲这种标识性的东西可以支持多个)
                         }

                   };
        }
    }
}
