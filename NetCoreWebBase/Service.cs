using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetCoreWebBase
{
    public interface IService
    {
        public DateTime Find();
    }

    public class Service : IService
    {
        public DateTime Find()
        {
            Thread.Sleep(300);
            return DateTime.Now;
        }
    }
}
