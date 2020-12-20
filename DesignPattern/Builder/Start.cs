using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPattern.Builder
{
    class Start
    {
        public static void Main1()
        {
            House house = GameManager.CreateHouse(new RomanHouseBuilder());
        }
    }
}
