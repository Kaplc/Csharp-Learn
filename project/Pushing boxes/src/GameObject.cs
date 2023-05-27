using System;
using System.Collections.Generic;

namespace Pushing_boxes
{
    #region 枚举

    public enum EType
    {
        Player,
        Wall,
        Box,
        Star
    }

    public enum EMoveDir
    {
        UP,
        Down,
        Left,
        Right
    }

    #endregion

    #region 接口

    public interface IDraw
    {
        void Draw();
    }

    #endregion

    #region 结构体

    public struct Position
    {
        public int x;
        public int y;

        public Position(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static bool operator ==(Position p1, Position p2)
        {
            return p1.x == p2.x && p1.y == p2.y;
        }

        public static bool operator !=(Position p1, Position p2)
        {
            return p1.x != p2.x && p1.y != p2.y;
        }
    }

    #endregion

    /// <summary>
    /// 关卡
    /// </summary>
    public class GameMap
    {
        public Player player;
        public List<Wall> walls = new List<Wall>();
        public List<Star> stars = new List<Star>();
        public List<Box> boxes = new List<Box>();
        public MapInfo mapInfo;

        public GameMap(int index)
        {
            mapInfo = new MapInfos()[index]; // 取出第一个地图信息
            // 加载信息
            foreach (var item in mapInfo.walls)
            {
                walls.Add(new Wall(EType.Wall, item));
            }

            foreach (var item in mapInfo.boxes)
            {
                boxes.Add(new Box(EType.Box, item));
            }

            foreach (var item in mapInfo.stars)
            {
                stars.Add(new Star(EType.Star, item));
            }

            player = new Player(EType.Player, mapInfo.player);
        }

        public void CheckKeyBroad()
        {
            if (Console.KeyAvailable)
            {
                lock (this)
                {
                    switch (Console.ReadKey(true).Key)
                    {
                        case ConsoleKey.W:
                            player.Move(EMoveDir.UP, this);
                            break;
                        case ConsoleKey.S:
                            player.Move(EMoveDir.Down, this);
                            break;
                        case ConsoleKey.A:
                            player.Move(EMoveDir.Left, this);
                            break;
                        case ConsoleKey.D:
                            player.Move(EMoveDir.Right, this);
                            break;
                        case ConsoleKey.Escape:
                            break;
                    }
                }
            }
        }

        public void Draw()
        {
            player.Draw();
            foreach (var item in boxes)
            {
                item.Draw();
            }

            foreach (var item in stars)
            {
                item.Draw();
            }

            foreach (var item in walls)
            {
                item.Draw();
            }
        }
    }

    public abstract class GameObject : IDraw
    {
        public Position pos;
        public EType type;


        public virtual void Draw()
        {
            Console.SetCursorPosition(pos.x, pos.y);
            switch (type)
            {
                case EType.Player:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write("◎");
                    break;
                case EType.Wall:
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("■");
                    break;
                case EType.Star:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("★");
                    break;
                case EType.Box:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("■");
                    break;
            }
        }

        public void Clear()
        {
            Console.SetCursorPosition(pos.x, pos.y);
            Console.Write("  ");
        }
    }

    public class Player : GameObject
    {
        public Player(EType type, Position pos)
        {
            this.pos = pos;
            this.type = type;
        }


        public void Move(EMoveDir dir, GameMap map)
        {
            Clear();
            switch (dir)
            {
                case EMoveDir.UP:
                    pos.y--;
                    if (ColCheck(map)) // 先进行碰撞检测再确认移动
                        pos.y++;
                    break;
                case EMoveDir.Down:
                    pos.y++;
                    if (ColCheck(map))
                        pos.y--;
                    break;
                case EMoveDir.Left:
                    pos.x -= 2;
                    if (ColCheck(map))
                        pos.x += 2;
                    break;
                case EMoveDir.Right:
                    pos.x += 2;
                    if (ColCheck(map))
                        pos.x -= 2;
                    break;
            }
        }

        /// <summary>
        /// 碰撞检测
        /// </summary>
        /// <returns></returns>
        public bool ColCheck(GameMap map)
        {
            foreach (var item in map.walls)
            {
                if (this.pos == item.pos)
                    return true; // true为有碰撞
            }

            foreach (var item in map.boxes)
            {
                if (this.pos == item.pos)
                {
                    return true;
                }
            }

            return false;
        }
    }

    public class Wall : GameObject
    {
        public Wall(EType type, Position pos)
        {
            this.pos = pos;
            this.type = type;
        }
    }

    public class Box : GameObject
    {
        public Box(EType type, Position pos)
        {
            this.pos = pos;
            this.type = type;
        }
    }

    public class Star : GameObject
    {
        public Star(EType type, Position pos)
        {
            this.pos = pos;
            this.type = type;
        }
    }
}