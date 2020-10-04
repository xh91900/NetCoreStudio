using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPattern.Decorator
{
    public class StudentDecoratorHomeWork : DecoratorBase
    {
        public StudentDecoratorHomeWork(StudentBase studentBase)
            : base(studentBase)
        { }

        public override void Study()
        {
            base.Study();
            Console.WriteLine("做作业了");
        }
    }
}
