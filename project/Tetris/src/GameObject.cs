using System;
using System.Collections.Generic;

namespace Tetris
{
    #region 枚举

    public enum EBlockType
    {
        Wall,

        /// <summary>
        /// 方形
        /// </summary>
        Square,

        /// <summary>
        /// 长条
        /// </summary>
        Strip,

        /// <summary>
        /// 坦克
        /// </summary>
        Tanker,

        /// <summary>
        /// 楼梯
        /// </summary>
        Stairs,

        /// <summary>
        /// 长拐
        /// </summary>
        LongCrutch,
    }

    #endregion

    #region 结构体

    public struct Position
    {
        private int x;
        private int y;

        public int X
        {
            get => x;
        }

        public int Y
        {
            get => y;
        }

        public Position(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static bool operator ==(Position p1, Position p2)
        {
            return p1.X == p2.X && p1.Y == p2.Y;
        }

        public static bool operator !=(Position p1, Position p2)
        {
            return p1.X != p2.X || p1.Y != p2.Y;
        }

        public static Position operator +(Position p1, Position p2)
        {
            return new Position(p1.X + p2.X, p2.Y + p2.Y);
        }
    }

    #endregion

    public abstract class GameObject : IDraw
    {
        public Position pos;
        public EBlockType blockType;

        public virtual void Draw()
        {
            Console.SetCursorPosition(pos.X, pos.Y);
            ConsoleColor color = ConsoleColor.Red;
            switch (blockType)
            {
                case EBlockType.Wall:
                    color = ConsoleColor.Red;
                    break;
                case EBlockType.Strip:
                    color = ConsoleColor.Green;
                    break;
                case EBlockType.Square:
                    color = ConsoleColor.Cyan;
                    break;
                case EBlockType.Stairs:
                    color = ConsoleColor.Blue;
                    break;
                case EBlockType.LongCrutch:
                    color = ConsoleColor.Gray;
                    break;
            }

            Console.ForegroundColor = color;
            Console.Write("■");
        }

        public void ChangeType(EBlockType oldBlockType)
        {
            this.blockType = oldBlockType;
        }
    }

    /// <summary>
    /// 墙
    /// </summary>
    public class Wall : GameObject
    {
        public Wall(int x, int y)
        {
            pos = new Position(x, y);
            blockType = EBlockType.Wall;
        }
    }

    public class Map : GameObject
    {
        public int wide = Game.WindowWide - 2;
        public int hight = Game.WindowHight - 6;
        public List<Wall> walls = new List<Wall>();
        public List<Wall> dynamicWalls = new List<Wall>();

        public Map()
        {
            // 横墙
            for (int x = 0; x <= wide; x += 2)
            {
                walls.Add(new Wall(x, hight));
            }

            // 竖墙
            for (int y = 0; y < hight; y++)
            {
                walls.Add(new Wall(0, y));
                walls.Add(new Wall(wide, y));
            }
        }

        public override void Draw()
        {
            for (int i = 0; i < walls.Count; i++)
            {
                walls[i].Draw();
            }

            for (int j = 0; j < dynamicWalls.Count; j++)
            {
                dynamicWalls[j].Draw();
            }
        }

        public void AddDynamicWalls(List<Wall> srcWalls)
        {
            for (int i = 0; i < srcWalls.Count; i++)
            {
                dynamicWalls.Add(srcWalls[i]);
            }
        }
    }
}