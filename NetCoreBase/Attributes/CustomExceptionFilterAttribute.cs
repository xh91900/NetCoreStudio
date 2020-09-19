using log4net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreBase.Attributes
{
    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    {

        public override void OnException(ExceptionContext context)
        {
            base.OnException(context);

            LogManager.GetLogger(typeof(CustomExceptionFilterAttribute)).Error(context.Exception.ToString());

            context.HttpContext.Response.ContentType = "text/html; charset=utf-8";
            context.HttpContext.Response.WriteAsync("程序出错了");
        }
    }


    /// <summary>
    /// asp.net core 里面的过滤器有（按执行顺序排序）
    /// IAuthorizationFilter
    /// IResourceFilter
    /// IExceptionFilter
    /// IActionFilter
    /// IResultFilter
    /// </summary>
    public class CustomActionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            context.HttpContext.Response.Headers.Add(new KeyValuePair<string, Microsoft.Extensions.Primitives.StringValues>("1111", "1111"));
        }
    }
}
