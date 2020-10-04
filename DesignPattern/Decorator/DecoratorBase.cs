using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPattern.Decorator
{
    /// <summary>
    /// 装饰器基类
    /// 组合+继承
    /// </summary>
    public class DecoratorBase:StudentBase
    {
        private StudentBase StudentBase = null;

        public DecoratorBase(StudentBase studentBase)
        {
            this.StudentBase = studentBase;
        }

        public override void Study()
        {
            StudentBase.Study();
        }
    }
}
