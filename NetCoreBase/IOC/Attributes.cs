using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreBase.IOC
{
    //标记需要注入的构造函数
    [AttributeUsage(AttributeTargets.Constructor)]
    public class ConstructorInjectionAttribute : Attribute
    {
    }


    //标记需要注入的属性
    [AttributeUsage(AttributeTargets.Property)]
    public class PropertyInjectionAttribute : Attribute
    {
    }

    //标记需要注入的方法
    [AttributeUsage(AttributeTargets.Method)]
    public class MethodInjectionAttribute : Attribute
    {
    }

    //标记需要按参数区分注入，用于一个接口多个实现的场景
    //标记在参数前面
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property)]
    public class ParameterNameInjectionAttribute : Attribute
    {
        public string InstanceName { get; private set; }
        public ParameterNameInjectionAttribute(string instanceName)
        {
            this.InstanceName = instanceName;
        }
    }
}
