using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPattern.Builder
{
    /// <summary>
    /// builder模式主要用于分步骤构建一个复杂的对象
    /// “分步骤”是一个稳定的算法，而复杂对象的各个部分是经常变化的
    /// 变化在哪里就封装哪里
    /// abstract factory模式解决“系列对象”的需求变化
    /// builder模式解决“对象部分”的需求变化，
    /// builder模式通常和composite模式组合使用
    /// </summary>
    class Start
    {
        public static void Main1()
        {
            House house = GameManager.CreateHouse(new RomanHouseBuilder());
        }
    }
}
