using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPattern.AbstractFactory
{
    /// <summary>
    /// 应对多系列对象构建（就是要变的是一组对象的关系同时变，而不是添加某一种对象）
    /// “系列对象”指的是这些对象之间有相互依赖或关系，比如道路与房屋的依赖
    /// 应对“新系列”的需求变动，难以应对“新对象”的需求变动
    /// 抽象工厂模式和工厂方法模式共同组合来应对“对象创建”的需求变化
    /// </summary>
    public class Start
    {
        public static void Main1()
        {
            GameManager gameManager = new GameManager(new ModernFacilitiesFactory());
            gameManager.BuildGameFacilities();
            gameManager.Run();
        }
    }

    /// <summary>
    /// 客户程序
    /// 稳定的，不需要改变
    /// </summary>
    class GameManager
    {
        FacilitiesFactory _facilitiesFactory;
        Road road;
        Building building;
        Tunnel tunnel;
        Jungle jungle;

        public GameManager(FacilitiesFactory facilitiesFactory)
        {
            _facilitiesFactory = facilitiesFactory;
        }

        public void BuildGameFacilities()
        {
            road = _facilitiesFactory.CreateRoad();
            building = _facilitiesFactory.CreateBuilding();
            tunnel = _facilitiesFactory.CreateTunnel();
            jungle = _facilitiesFactory.CreateJungle();
        }

        public void Run()
        {
            //road.DoSomething(building);
            //building.DoSomething(tunnel);
            //tunnel.DoSomething();
        }
    }
}