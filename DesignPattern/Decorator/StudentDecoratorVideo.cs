using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPattern.Decorator
{
    public class StudentDecoratorVideo : DecoratorBase
    {
        public StudentDecoratorVideo(StudentBase studentBase)
            : base(studentBase)
        { }

        public override void Study()
        {
            base.Study();
            Console.WriteLine("看视频了");
        }
    }
}
