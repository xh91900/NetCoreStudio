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
                new ApiResource("api1", "My API")
            };
        }

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
                    ClientId = "client",
                    //授权方式为客户端授权，类型可参考GrantTypes枚举
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    //授权密钥，客户端和服务端事先约定的一个Key
                    ClientSecrets =
                    {
                        new Secret("111111".Sha256())
                    },                     
                    //允许客户端访问的Scopes[作用域]
                    AllowedScopes = { "api1" }
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
                new IdentityResources.OpenId()
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
