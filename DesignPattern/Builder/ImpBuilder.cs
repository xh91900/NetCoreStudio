using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPattern.Builder
{
    public class RomanHouse : House
    { }

    public class RomanDoor : Door
    { }

    public class RomanWall : Wall
    { }

    public class RomanWindows : Windows
    { }

    public class RomanFloor : Floor
    { }

    public class RomanHouseCeiling : HouseCeiling
    { }

    /// <summary>
    /// concretebuilder
    /// </summary>
    public class RomanHouseBuilder : Builder
    {
        public override void BuildDoor()
        {
            throw new NotImplementedException();
        }

        public override void BuildWall()
        {
            throw new NotImplementedException();
        }

        public override void BuildWindows()
        {
            throw new NotImplementedException();
        }

        public override void BuildFloor()
        {
            throw new NotImplementedException();
        }

        public override void BuildHouseCeiling()
        {
            throw new NotImplementedException();
        }

        public override House GetHouse()
        {
            // 放具体的门窗组合方式
            throw new NotImplementedException();
        }
    }
}
