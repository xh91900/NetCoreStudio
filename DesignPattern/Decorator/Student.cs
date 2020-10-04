using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPattern.Decorator
{
    public class Student : StudentBase
    {
        public override void Study()
        {
            Console.WriteLine("上课了");
        }
    }

    /// <summary>
    /// 继承式，完成Student功能扩展，没有破坏封装
    /// </summary>
    public class StudentWithVideo: Student
    {
        public override void Study()
        {
            base.Study();
            Console.WriteLine("看视频了");
        }
    }

    /// <summary>
    ///  组合式，完成Student功能扩展，没有破坏封装
    /// </summary>
    public class StudentCombination
    {
        private StudentBase StudentBase = null;

        public StudentCombination(StudentBase studentBase)
        {
            this.StudentBase = studentBase;
        }

        public void Study()
        {
            this.StudentBase.Study();
            Console.WriteLine("看视频了");
        }
    }
}
