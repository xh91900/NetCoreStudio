using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace NetCoreBase.APO
{
    /// <summary>
    /// 拦截器的实现类
    /// 需要安装Castle.Core
    /// </summary>
    public class CustomInterceptor : StandardInterceptor
    {
        /// <summary>
        /// 调用前执行
        /// </summary>
        /// <param name="invocation"></param>
        protected override void PerformProceed(IInvocation invocation)
        {
            Console.WriteLine("调用前执行，目标方法的方法名是" + invocation.Method.Name);
        }

        /// <summary>
        /// 拦截的方法返回时调用的拦截器
        /// </summary>
        /// <param name="invocation"></param>
        protected override void PreProceed(IInvocation invocation)
        {
            var method = invocation.Method;

            //每个方法返回一个委托，用于组装一个委托连，然后逐个执行，
            //借鉴了.netcore中间件管道的思想。
            Action action = () => base.PerformProceed(invocation);

            //判断此方法有没有加LogBeforeAttribute特性
            if (method.IsDefined(typeof(BaseInterceptorAttribute), true))
            {
                //获取此特性，执行特性里的方法。
                //可以解决哪些方法不用aop的问题
                //可以把切面逻辑转移到特性里面去
                //理解这里为什么要Reverse一下
                foreach (var attribute in method.GetCustomAttributes<BaseInterceptorAttribute>().ToArray().Reverse())
                {
                    attribute.Do(invocation, action);
                }
            }

            Console.WriteLine("拦截的方法返回时调用，目标方法的方法名是" + invocation.Method.Name);

            //在这执行目标方法
            // invocation内包含目标方法的很多信息，比如函数参数，方法名等
            //base.PerformProceed(invocation);

            //执行委托连。
            action.Invoke();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="invocation"></param>
        protected override void PostProceed(IInvocation invocation)
        {
            Console.WriteLine("调用后执行，目标方法的方法名是" + invocation.Method.Name);
        }
    }
}
