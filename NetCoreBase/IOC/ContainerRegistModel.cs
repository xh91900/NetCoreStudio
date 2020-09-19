using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreBase.IOC
{
    public class ContainerRegistModel
    {
        public Type TargetType { get; set; }

        public LifeTime LifeTime { get; set; }

        /// <summary>
        /// 仅限单例使用
        /// </summary>
        public object SingletonInstance { get; set; }
    }

    public enum LifeTime
    {
        Transient,
        Singleton,
        Scope
    }
}
