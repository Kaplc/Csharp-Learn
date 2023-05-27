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
        public static List<Wall> walls = new List<Wall>();
        public static List<Star> stars = new List<Star>();
        public static List<Box> boxes = new List<Box>();
        public MapInfo mapInfo;

        public GameMap(int index)
        {
            mapInfo = new MapInfos()[index]; // 取出第一个地图信息
            // 加载信息
            foreach (var wall in mapInfo.walls)
            {
                walls.Add(new Wall(EType.Wall, wall));
            }

            for (int i = 0; i < mapInfo.boxes.Count; i++)
            {
                boxes.Add(new Box(EType.Box, mapInfo.boxes[i], i));
            }


            foreach (var star in mapInfo.stars)
            {
                stars.Add(new Star(EType.Star, star));
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

        private (bool, Box) CheckBoxNear(EMoveDir dir)
        {

            foreach (var box in GameMap.boxes)
            {
                if (this.pos == box.up && dir == EMoveDir.Down)
                    return (true, box);
                if (this.pos == box.down && dir == EMoveDir.UP)
                    return (true, box);
                if (this.pos == box.left && dir == EMoveDir.Right)
                    return (true, box);
                if (this.pos == box.right && dir == EMoveDir.Left)
                    return (true, box);
            }

            return (false, null);
        }

        /// <summary>
        /// 碰撞检测
        /// </summary>
        /// <returns></returns>
        private bool ColCheck()
        {
            foreach (var item in GameMap.walls)
            {
                if (this.pos == item.pos)
                    return true; // true为有碰撞
            }


            return false;
        }

        public void Move(EMoveDir dir, GameMap map)
        {
            
            bool isBox;
            Box box;
            
            Clear();
            switch (dir)
            {
                case EMoveDir.UP:
                    var upRes = CheckBoxNear(EMoveDir.UP);
                    isBox = upRes.Item1;
                    box = upRes.Item2;
                    if (isBox)
                    {
                        if (box.Move(EMoveDir.UP, map))
                        {
                            pos.y--;
                            if (ColCheck()) // 先进行碰撞检测再确认移动
                                pos.y++;
                        }
                    }
                    else
                    {
                        pos.y--;
                        if (ColCheck()) // 先进行碰撞检测再确认移动
                            pos.y++;
                    }

                    break;
                case EMoveDir.Down:
                    var downRes = CheckBoxNear(EMoveDir.Down);
                    isBox = downRes.Item1;
                    box = downRes.Item2;
                    if (isBox)
                    {
                        if (box.Move(EMoveDir.Down, map))
                        {
                            pos.y++;
                            if (ColCheck())
                                pos.y--;
                        }
                    }
                    else
                    {
                        pos.y++;
                        if (ColCheck())
                            pos.y--;
                    }

                    break;
                case EMoveDir.Left:
                    var LeftRes = CheckBoxNear(EMoveDir.Left);
                    isBox = LeftRes.Item1;
                    box = LeftRes.Item2;
                    if (isBox)
                    {
                        if (box.Move(EMoveDir.Left, map))
                        {
                            pos.x -= 2;
                            if (ColCheck())
                                pos.x += 2;
                        }
                    }
                    else
                    {
                        pos.x -= 2;
                        if (ColCheck())
                            pos.x += 2;
                    }

                    break;
                case EMoveDir.Right:
                    var rightRes = CheckBoxNear(EMoveDir.Right);
                    isBox = rightRes.Item1;
                    box = rightRes.Item2;
                    if (isBox)
                    {
                        if (box.Move(EMoveDir.Right, map))
                        {
                            pos.x += 2;
                            if (ColCheck())
                                pos.x -= 2;
                        }
                    }
                    else
                    {
                        pos.x += 2;
                        if (ColCheck())
                            pos.x -= 2;
                    }

                    break;
            }
            
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
        // 箱子的四个方向位置
        public Position up;
        public Position down;
        public Position left;
        public Position right;
        public int index;

        public Box(EType type, Position pos, int index)
        {
            this.index = index;
            this.pos = pos;
            this.type = type;

            up = new Position(this.pos.x, this.pos.y - 1);
            down = new Position(this.pos.x, this.pos.y + 1);
            left = new Position(this.pos.x - 2, this.pos.y);
            right = new Position(this.pos.x + 2, this.pos.y);
        }

        /// <summary>
        /// 碰撞检测
        /// </summary>
        /// <returns></returns>
        private bool ColCheck()
        {
            // 禁止撞墙
            foreach (var wall in GameMap.walls)
            {
                if (this.pos == wall.pos)
                    return true; // true为有碰撞
            }
            // 禁止两个箱子移动
            foreach (var box in GameMap.boxes)
            {
                if (box.index == this.index)
                {
                    continue;
                }

                if (this.pos == box.pos)
                {
                    return true;
                }
            }

            return false;
        }

        private void UpdataPos()
        {
            up = new Position(this.pos.x, this.pos.y - 1);
            down = new Position(this.pos.x, this.pos.y + 1);
            left = new Position(this.pos.x - 2, this.pos.y);
            right = new Position(this.pos.x + 2, this.pos.y);
        }

        public bool Move(EMoveDir dir, GameMap map)
        {
            Clear();
            switch (dir)
            {
                case EMoveDir.UP:
                    pos.y--;
                    if (ColCheck()) // true为禁止碰撞归位
                    {
                        pos.y++;
                        return false; // 返回移动失败
                    }

                    UpdataPos();
                    return true;
                case EMoveDir.Down:
                    pos.y++;
                    if (ColCheck()) // true为禁止碰撞归位
                    {
                        pos.y--;
                        return false; // 返回移动失败
                    }

                    UpdataPos();
                    return true;
                case EMoveDir.Left:
                    pos.x -= 2;
                    if (ColCheck()) // true为禁止碰撞归位
                    {
                        pos.x += 2;
                        return false; // 返回移动失败
                    }

                    UpdataPos();
                    return true;
                case EMoveDir.Right:
                    pos.x += 2;
                    if (ColCheck()) // true为禁止碰撞归位
                    {
                        pos.x -= 2;
                        return false; // 返回移动失败
                    }

                    UpdataPos();
                    return true;
            }

            return false;
        }
    }

    public class Star : GameObject
    {
        public bool show;
        public Star(EType type, Position pos)
        {
            this.show = true;
            this.pos = pos;
            this.type = type;
        }
    }
}