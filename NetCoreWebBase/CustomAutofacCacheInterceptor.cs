using Castle.DynamicProxy;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreWebBase
{
    /// <summary>
    /// 实现拦截
    /// </summary>
    public class CustomAutofacCacheInterceptor : IInterceptor
    {

        private IDistributedCache _distributedCache = null;

        public CustomAutofacCacheInterceptor(IDistributedCache distributedCache)
        {
            this._distributedCache = distributedCache;
        }

        public void Intercept(IInvocation invocation)
        {
            string key = "key";

            string result = this._distributedCache.GetString(key);

            if (string.IsNullOrEmpty(result))
            {
                invocation.Proceed();//继续执行原方法

                this._distributedCache.SetString(key, invocation.ReturnValue.ToString());
            }
            else
            {
                invocation.ReturnValue = Convert.ChangeType(result, invocation.Method.ReturnType);
            }
        }
    }
}
