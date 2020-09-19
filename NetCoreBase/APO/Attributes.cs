using Castle.DynamicProxy;
using Castle.DynamicProxy.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreBase.APO
{
    public abstract class BaseInterceptorAttribute : Attribute
    {
        public abstract Action Do(IInvocation invocation,Action action);
    }

    public class LogBeforeAttribute : BaseInterceptorAttribute
    {
        /// <summary>
        /// 通过把参数invocation传进来，可以得到目标方法的很多信息
        /// 比如记录日志、参数检查
        /// </summary>
        /// <param name="invocation"></param>
        public override Action Do(IInvocation invocation, Action action)
        {

            //每个方法返回一个委托，用于组装一个委托连，然后逐个执行，
            //借鉴了.netcore中间件管道的思想。
            return () =>
            {
                //这里面执行上一个委托
                action.Invoke();
                Console.WriteLine("this is LogBeforeAttribute" + invocation.Method);
            };
        }
    }

    public class LogAfterAttribute : BaseInterceptorAttribute
    {
        public override Action Do(IInvocation invocation, Action action)
        {
            return () =>
            {
                action.Invoke();
                // 注意顺序，这是before和after原因
                Console.WriteLine("this is LogBeforeAttribute");
            };
        }
    }
}
