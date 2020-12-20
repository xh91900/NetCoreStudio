using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPattern.AbstractFactory
{
    /// <summary>
    /// 现代道路
    /// </summary>
    public class ModernRoad : Road
    { }

    /// <summary>
    /// 现代建筑
    /// </summary>
    public class ModernBuilding : Building
    { }

    public class ModernTunnel : Tunnel
    { }

    public class ModernJungle : Jungle
    { }

    /// <summary>
    /// 
    /// </summary>
    public class ModernFacilitiesFactory : FacilitiesFactory
    {
        public override Road CreateRoad()
        {
            return new ModernRoad();
        }

        public override Building CreateBuilding()
        {
            return new ModernBuilding();
        }

        public override Tunnel CreateTunnel()
        {
            return new ModernTunnel();
        }

        public override Jungle CreateJungle()
        {
            return new ModernJungle();
        }
    }
}
