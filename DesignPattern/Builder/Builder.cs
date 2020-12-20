using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPattern.Builder
{
    /// <summary>
    /// builder
    /// </summary>
    public abstract class Builder
    {
        public abstract void BuildDoor();
        public abstract void BuildWall();
        public abstract void BuildWindows();
        public abstract void BuildFloor();
        public abstract void BuildHouseCeiling();

        public abstract House GetHouse();
    }

    public abstract class House
    { }

    public abstract class Door
    { }

    public abstract class Wall
    { }

    public abstract class Windows
    { }

    public abstract class Floor
    { }

    public abstract class HouseCeiling
    { }


    /// <summary>
    /// director
    /// 客户程序
    /// 稳定的，不需要改变
    /// </summary>
    public class GameManager
    {
        public static House CreateHouse(Builder builder)
        {
            builder.BuildDoor();
            builder.BuildWall();
            builder.BuildWindows();
            builder.BuildFloor();
            builder.BuildHouseCeiling();

            return builder.GetHouse();
        }
    }

}
