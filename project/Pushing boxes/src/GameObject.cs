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
        public bool reStart = false;
        public bool back = false;

        public GameMap(int index)
        {
            mapInfo = MapInfos.infos[index]; // 取出第一个地图信息
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
                        case ConsoleKey.N:
                            reStart = true;
                            break;
                        case ConsoleKey.Escape:
                            back = true;
                            break;
                    }
                }
            }
        }

        public bool GameOver()
        {
            
            foreach (var star in stars)
            {
                star.CheckPress(this);
            }

            int showCount = 3;
            foreach (var star in stars)
            {
                if (!star.show)
                {
                    showCount--;
                }
            }

            if (showCount == 0)
            {
                return true;
            }

            return false;
        }

        public void Draw()
        {
            player.Draw();
            foreach (var item in boxes)
            {
                item.CheckPress(this);
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

        /// <summary>
        /// 检测是否在箱子旁边
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        private (bool, Box) CheckBoxNear(EMoveDir dir, GameMap map)
        {
            foreach (var box in map.boxes)
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
        private bool ColCheck(GameMap map)
        {
            foreach (var item in map.walls)
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
                    var upRes = CheckBoxNear(EMoveDir.UP, map); // 检测是否在箱子旁边并且合法方向推动
                    isBox = upRes.Item1;
                    box = upRes.Item2;
                    if (isBox) // 方向合法
                    {
                        if (box.Move(EMoveDir.UP, map))
                        {
                            // 移动箱子成功
                            // 人也移动
                            pos.y--;
                            if (ColCheck(map)) // 先进行碰撞检测再确认移动
                                pos.y++;
                        }
                        // 箱子移动失败不做操作
                    }
                    else // 旁边无箱子或移动方向非法
                    {
                        // 移动人
                        pos.y--;
                        if (ColCheck(map)) 
                            pos.y++;
                    }

                    break;
                case EMoveDir.Down:
                    var downRes = CheckBoxNear(EMoveDir.Down, map);
                    isBox = downRes.Item1;
                    box = downRes.Item2;
                    if (isBox)
                    {
                        if (box.Move(EMoveDir.Down, map))
                        {
                            pos.y++;
                            if (ColCheck(map))
                                pos.y--;
                        }
                    }
                    else
                    {
                        pos.y++;
                        if (ColCheck(map))
                            pos.y--;
                    }

                    break;
                case EMoveDir.Left:
                    var LeftRes = CheckBoxNear(EMoveDir.Left, map);
                    isBox = LeftRes.Item1;
                    box = LeftRes.Item2;
                    if (isBox)
                    {
                        if (box.Move(EMoveDir.Left, map))
                        {
                            pos.x -= 2;
                            if (ColCheck(map))
                                pos.x += 2;
                        }
                    }
                    else
                    {
                        pos.x -= 2;
                        if (ColCheck(map))
                            pos.x += 2;
                    }

                    break;
                case EMoveDir.Right:
                    var rightRes = CheckBoxNear(EMoveDir.Right, map);
                    isBox = rightRes.Item1;
                    box = rightRes.Item2;
                    if (isBox)
                    {
                        if (box.Move(EMoveDir.Right, map))
                        {
                            pos.x += 2;
                            if (ColCheck(map))
                                pos.x -= 2;
                        }
                    }
                    else
                    {
                        pos.x += 2;
                        if (ColCheck(map))
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
        public int index; // 箱子编号
        public bool press = false;

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
        private bool ColCheck(GameMap map)
        {
            // 禁止撞墙
            foreach (var wall in map.walls)
            {
                if (this.pos == wall.pos)
                    return true; // true为有碰撞
            }

            // 禁止两个箱子移动
            foreach (var box in map.boxes)
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
                    if (ColCheck(map)) // true为禁止碰撞归位
                    {
                        pos.y++;
                        return false; // 返回移动失败
                    }

                    UpdataPos();
                    return true;
                case EMoveDir.Down:
                    pos.y++;
                    if (ColCheck(map)) // true为禁止碰撞归位
                    {
                        pos.y--;
                        return false; // 返回移动失败
                    }

                    UpdataPos();
                    return true;
                case EMoveDir.Left:
                    pos.x -= 2;
                    if (ColCheck(map)) // true为禁止碰撞归位
                    {
                        pos.x += 2;
                        return false; // 返回移动失败
                    }

                    UpdataPos();
                    return true;
                case EMoveDir.Right:
                    pos.x += 2;
                    if (ColCheck(map)) // true为禁止碰撞归位
                    {
                        pos.x -= 2;
                        return false; // 返回移动失败
                    }

                    UpdataPos();
                    return true;
            }

            return false;
        }

        public void CheckPress(GameMap map)
        {
            foreach (var star in map.stars)
            {
                if (star.pos == pos) // 相等就打标识绿色
                {
                    press = true;
                    return; // 直接返回不需要继续判断
                }
            }

            press = false; // 都不相等就红色
        }

        public override void Draw()
        {
            Console.SetCursorPosition(pos.x, pos.y);
            if (press)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            Console.Write("■");
        }
    }

    public class Star : GameObject
    {
        public bool show;
        public int boxIndex = -1;

        public Star(EType type, Position pos)
        {
            this.show = true;
            this.pos = pos;
            this.type = type;
        }

        public void CheckPress(GameMap map)
        {
            foreach (var box in map.boxes)
            {
                if (box.pos == pos || map.player.pos == pos) // 与人位置或箱子相等就打标识禁止显示
                {
                    show = false;
                    return; // 直接返回不需要继续判断
                }
            }

            show = true; // 都不相等就显示
        }

        public override void Draw()
        {
            if (show)
            {
                Console.SetCursorPosition(pos.x, pos.y);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("★");
            }
        }
    }
}