using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPattern.AbstractFactory
{
    #region abstract product
    /// <summary>
    /// 道路
    /// </summary>
    public abstract class Road
    {
        // 与房屋有交互
    }

    /// <summary>
    /// 房屋
    /// </summary>
    public abstract class Building
    {
        // 与地道有交互
    }

    /// <summary>
    /// 地道
    /// </summary>
    public abstract class Tunnel
    {
        // 与丛林有交互
    }

    /// <summary>
    /// 丛林
    /// </summary>
    public abstract class Jungle
    {
        // 与道路有交互
    }
    #endregion

    #region abstract factory
    /// <summary>
    /// 设施工厂
    /// </summary>
    public abstract class FacilitiesFactory
    {
        public abstract Road CreateRoad();
        public abstract Building CreateBuilding();
        public abstract Tunnel CreateTunnel();
        public abstract Jungle CreateJungle();
    }
    #endregion


}
