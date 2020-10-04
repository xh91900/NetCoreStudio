using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPattern.Decorator
{
    /// <summary>
    /// 学员基类
    /// </summary>
    public abstract class StudentBase
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public abstract void Study();

        public void Show()
        {
            Console.WriteLine($"This is {this.Name}");
        }
    }
}
