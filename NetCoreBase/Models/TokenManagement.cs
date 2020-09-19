using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreBase.Models
{
    public class TokenManagement
    {
        public string Secret { get; set; }

        /// <summary>
        /// token 是给谁的
        /// </summary>
        public string Issuer { get; set; }

        public string Audience { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public double AccessExpiration { get; set; }

        public string RefreshExpiration { get; set; }
    }
}
