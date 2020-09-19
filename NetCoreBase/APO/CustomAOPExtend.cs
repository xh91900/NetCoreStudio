using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace NetCoreBase.APO
{
    /// <summary>
    /// 初始化aop
    /// </summary>
    public static class CustomAOPExtend
    {
        public static object InitAop(this Object obj,Type type)
        {
            // 动态代理
            ProxyGenerator generator = new ProxyGenerator();
            CustomInterceptor interceptor = new CustomInterceptor();

            //通过类代理
            // CommonClass commonClass = generator.CreateClassProxy<CommonClass>(interceptor);

            //通过接口注入，接口里面的所有方法都会生效，可以通过给方法加特性规避这个问题
            obj = generator.CreateInterfaceProxyWithTargetInterface(type,obj, interceptor);

            return obj;
        }
    }
}
