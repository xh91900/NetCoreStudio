using DesignPattern.Decorator;
using System;

namespace DesignPattern
{
    class Program
    {
        static void Main(string[] args)
        {
            // 装饰器
            StudentBase student = new Student()
            {
                Id=1,Name="altman"
            };

            // 给student包了一层，装饰了一下。
            student = new DecoratorBase(student);
            student = new StudentDecoratorVideo(student);//再装饰一层
            student = new StudentDecoratorHomeWork(student);
            student.Study();
        }
    }
}
