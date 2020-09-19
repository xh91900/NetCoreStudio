using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NetCoreBase.Interface;
using NetCoreBase.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreBase.Implement
{
    public class TokenAuthenticationService : IAuthenticateService
    {
        private readonly TokenManagement _tokenManagement;

        public TokenAuthenticationService(IOptions<TokenManagement> tokenManagement)
        {
            _tokenManagement = tokenManagement.Value;
        }

        public bool IsAuthenticated(string Username, string Password, out string token)
        {
            token = string.Empty;

            /* Claims (Payload)
               Claims 部分包含了一些跟这个 token 有关的重要信息。 JWT 标准规定了一些字段，下面节选一些字段:

               iss: The issuer of the token，token 是给谁的
               sub: The subject of the token，token 主题
               exp: Expiration Time。 token 过期时间，Unix 时间戳格式
               iat: Issued At。 token 创建时间， Unix 时间戳格式
               jti: JWT ID。针对当前 token 的唯一标识
               除了规定的字段外，可以包含其他任何 JSON 兼容的字段。
            */
            var claims = new[]
            {
                new Claim(ClaimTypes.Name,Username)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenManagement.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var jwtToken = new JwtSecurityToken(_tokenManagement.Issuer, _tokenManagement.Audience, claims, expires: DateTime.Now.AddMinutes(_tokenManagement.AccessExpiration), signingCredentials: credentials);

            token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            return true;
        }

    }
}
