using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreBase.APO
{
    public class CommonClass
    {
        public virtual void MethodInterceptor()
        {
            Console.WriteLine("This is Interceptor,这是拦截器");
        }

        public void MethodNoInterceptor()
        {
            Console.WriteLine("This is Interceptor,这是拦截器");
        }
    }

    interface IService
    {
        [LogBeforeAttribute]
        public void Show();

        [LogAfterAttribute]
        public void ShowWithOutAttribute();
    }

    public class Service : IService
    {
        public void Show()
        {
            Console.WriteLine("Service.Show()");
        }

        public void ShowWithOutAttribute()
        {
            Console.WriteLine("Service.ShowWithOutAttribute()");
        }
    }

    public class Main
    {
        public void Start()
        {
            IService service = new Service();
            service.Show();// 单纯执行show

            service = (IService)service.InitAop(typeof(IService));

            service.Show();
        }
    }
}
