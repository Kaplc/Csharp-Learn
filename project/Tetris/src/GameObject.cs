using System;
using System.Collections.Generic;
using System.Linq;

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
        /// 右楼梯
        /// </summary>
        RStairs,

        /// <summary>
        /// 左楼梯
        /// </summary>
        LStairs,

        /// <summary>
        /// 左长拐
        /// </summary>
        LLongCrutch,

        /// <summary>
        /// 右长拐
        /// </summary>
        RLongCrutch,
    }

    public enum EDir
    {
        Left,
        Right,
        Down
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
            set { x = value; }
        }

        public int Y
        {
            get => y;
            set { y = value; }
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
            return new Position(p1.X + p2.X, p1.Y + p2.Y);
        }
    }

    #endregion

    /// <summary>
    /// 游戏方块对象
    /// </summary>
    public abstract class GameObject : IDraw
    {
        public Position pos;
        public EBlockType blockType;

        public virtual void Draw()
        {
            if (pos.Y < 0 || pos.X < 0 || pos.X > Game.WindowWide)
            {
                return;
            }

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
                case EBlockType.Tanker:
                    color = ConsoleColor.Magenta;
                    break;
                case EBlockType.LStairs:
                case EBlockType.RStairs:
                    color = ConsoleColor.Blue;
                    break;
                case EBlockType.LLongCrutch:
                case EBlockType.RLongCrutch:
                    color = ConsoleColor.Yellow;
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

    /// <summary>
    /// 地图
    /// </summary>
    public class Map : GameObject
    {
        public int wide = Game.WindowWide - 2;
        public int hight = Game.WindowHight - 6;
        public List<Wall> walls = new List<Wall>();
        public List<SmallBlock> dynamicWalls = new List<SmallBlock>();

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

        public void AddDynamicWalls(List<SmallBlock> smallBlocks)
        {
            for (int i = 0; i < smallBlocks.Count; i++)
            {
                smallBlocks[i].ChangeType(EBlockType.Wall);
                dynamicWalls.Add(smallBlocks[i]);
            }

            Draw();
        }

        public bool GameOver()
        {
            for (int i = 0; i < dynamicWalls.Count; i++)
            {
                if (dynamicWalls[i].pos.Y <= 0)
                    return true;
            }

            return false;
        }
    }

    /// <summary>
    /// 方块信息
    /// </summary>
    public class BigBlockInfo
    {
        // list索引代表该类型方块是哪个变形, Position数组代表最小单位方块的对原点相对坐标集合
        // public List<Position[]> blockInfos = new List<Position[]>();
        public List<Position[]> blockInfos = new List<Position[]>();

        public BigBlockInfo(EBlockType type)
        {
            switch (type)
            {
                case EBlockType.Square:
                    Position[] positions = new Position[3];
                    positions[0] = new Position(2, 0); // 相对原点坐标
                    positions[1] = new Position(2, 1);
                    positions[2] = new Position(0, 1);
                    blockInfos.Add(positions);
                    break;
                case EBlockType.Strip:
                    blockInfos.Add(new[] { new Position(0, -1), new Position(0, 1), new Position(0, 2) });
                    blockInfos.Add(new[] { new Position(-4, 0), new Position(-2, 0), new Position(2, 0) });
                    blockInfos.Add(new[] { new Position(0, -2), new Position(0, -1), new Position(0, 1) });
                    blockInfos.Add(new[] { new Position(-2, 0), new Position(2, 0), new Position(4, 0) });
                    break;
                case EBlockType.RStairs:
                    blockInfos.Add(new[] { new Position(0, -1), new Position(2, 0), new Position(2, 1) });
                    blockInfos.Add(new[] { new Position(2, 0), new Position(0, 1), new Position(-2, 1) });
                    blockInfos.Add(new[] { new Position(-2, -1), new Position(-2, 0), new Position(0, 1) });
                    blockInfos.Add(new[] { new Position(-2, 0), new Position(0, -1), new Position(2, -1) });
                    break;
                case EBlockType.LStairs:
                    blockInfos.Add(new[] { new Position(0, -1), new Position(-2, 0), new Position(-2, 1) });
                    blockInfos.Add(new[] { new Position(-2, -1), new Position(0, -1), new Position(2, 0) });
                    blockInfos.Add(new[] { new Position(2, -1), new Position(2, 0), new Position(0, 1) });
                    blockInfos.Add(new[] { new Position(-2, 0), new Position(0, 1), new Position(2, 1) });
                    break;
                case EBlockType.Tanker:
                    blockInfos.Add(new[] { new Position(-2, 0), new Position(0, 1), new Position(2, 0) });
                    blockInfos.Add(new[] { new Position(-2, 0), new Position(0, -1), new Position(0, 1) });
                    blockInfos.Add(new[] { new Position(-2, 0), new Position(0, -1), new Position(2, 0) });
                    blockInfos.Add(new[] { new Position(0, -1), new Position(2, 0), new Position(0, 1) });
                    break;
                case EBlockType.LLongCrutch:
                    blockInfos.Add(new[] { new Position(-2, -1), new Position(0, -1), new Position(0, 1) });
                    blockInfos.Add(new[] { new Position(2, -1), new Position(2, 0), new Position(-2, 0) });
                    blockInfos.Add(new[] { new Position(0, -1), new Position(0, 1), new Position(2, 1) });
                    blockInfos.Add(new[] { new Position(-2, 0), new Position(-2, 1), new Position(2, 0) });
                    break;
                case EBlockType.RLongCrutch:
                    blockInfos.Add(new[] { new Position(0, -1), new Position(2, -1), new Position(0, 1) });
                    blockInfos.Add(new[] { new Position(-2, 0), new Position(2, 1), new Position(2, 0) });
                    blockInfos.Add(new[] { new Position(0, -1), new Position(0, 1), new Position(-2, 1) });
                    blockInfos.Add(new[] { new Position(-2, -1), new Position(-2, 0), new Position(2, 0) });
                    break;
            }
        }
    }

    /// <summary>
    /// 组成大方块的小方块
    /// </summary>
    public class SmallBlock : GameObject
    {
        public SmallBlock(EBlockType blockType, Position pos)
        {
            this.blockType = blockType;
            this.pos = pos;
        }

        public void Clear()
        {
            if (pos.Y < 0 || pos.X < 0 || pos.X > Game.WindowWide) // 缓冲区外不打印
            {
                return;
            }

            Console.SetCursorPosition(pos.X, pos.Y);
            Console.Write("  ");
        }
    }

    /// <summary>
    /// 大方块
    /// </summary>
    public class BigBlock : IDraw
    {
        public List<SmallBlock> smallBlocks = new List<SmallBlock>();
        public List<Position[]> smallBlockInfos;
        public EBlockType bigBlockType;
        public int blockInfosIndex; // 当前变形索引

        public BigBlock(Map map)
        {
            Random r = new Random();
            // 随机方块类型
            int BlockTypeIndex = r.Next(1, 8);
            bigBlockType = (EBlockType)BlockTypeIndex; // 初始化大方块类型
            smallBlockInfos = new BigBlockInfo(bigBlockType).blockInfos; // 下标获取枚举->获取某个类型的方块中包含小方块信息数组

            smallBlocks.Add(new SmallBlock(bigBlockType, new Position(Game.WindowWide / 2 - 1, -4))); // 初始化原点方块

            // 随机生成方块变形
            blockInfosIndex = r.Next(0, smallBlockInfos.Count);
            Create(blockInfosIndex, map); // 创建剩余组成部分
        }

        public void Create(int newIndex, Map map)
        {
            // 预创建
            List<SmallBlock> newSmallBlocks = new List<SmallBlock>();
            Position[] CreatedBlockInfo = smallBlockInfos[newIndex]; // 小方块信息数组中的其中一种变形
            newSmallBlocks.Add(smallBlocks[0]);
            for (int i = 0; i < CreatedBlockInfo.Length; i++)
            {
                newSmallBlocks.Add(new SmallBlock(bigBlockType, CreatedBlockInfo[i] + newSmallBlocks[0].pos));
            }

            // 碰撞判断
            for (int i = 0; i < newSmallBlocks.Count; i++)
            {
                for (int j = 0; j < map.walls.Count; j++)
                {
                    if (newSmallBlocks[i].pos.X == map.walls[j].pos.X &&
                        newSmallBlocks[i].pos.Y == map.walls[j].pos.Y) // 固定墙判断
                    {
                        return;
                    }
                }

                for (int j = 0; j < map.dynamicWalls.Count; j++)
                {
                    if (newSmallBlocks[i].pos.X == map.dynamicWalls[j].pos.X &&
                        newSmallBlocks[i].pos.Y == map.dynamicWalls[j].pos.Y) // 动态墙判断
                    {
                        return;
                    }
                }
            }

            // 真实创建
            blockInfosIndex = newIndex;
            smallBlocks.Clear();
            smallBlocks = newSmallBlocks;
        }

        public void Change(EDir sign, Map map)
        {
            // 预变形索引
            int newIndex = blockInfosIndex;
            switch (sign)
            {
                case EDir.Left:
                    newIndex--;
                    break;
                case EDir.Right:
                    newIndex++;
                    break;
            }

            // 索引超界判断
            if (newIndex > smallBlockInfos.Count - 1)
            {
                newIndex = 0;
            }
            else if (newIndex < 0)
            {
                newIndex = smallBlockInfos.Count - 1;
            }

            Clear();
            Create(newIndex, map); // 创建新方块
        }

        public void Clear()
        {
            for (int i = 0; i < smallBlocks.Count; i++)
            {
                smallBlocks[i].Clear();
            }
        }

        public void Draw()
        {
            for (int i = 0; i < smallBlocks.Count; i++)
            {
                smallBlocks[i].Draw();
            }
        }
    }

    /// <summary>
    /// 搬砖工人
    /// </summary>
    public class Worker : IDraw
    {
        public BigBlock block;
        public Map map;

        public Worker(Map map)
        {
            this.map = map;
            NewBlock(); // 第一次初始化新方块
        }

        public void NewBlock()
        {
            if (block != null)
                block.Clear();

            block = new BigBlock(map);
            Draw();
        }

        public void ChangeBlock(EDir sign)
        {
            if (block == null) return;

            block.Change(sign, map);
            Draw();
        }

        public void KeyCheck()
        {
            if (Console.KeyAvailable)
            {
                lock (map)
                {
                    switch (Console.ReadKey(true).Key)
                    {
                        // QE旋转, S加速下降, AD左右移动
                        case ConsoleKey.Q:
                            ChangeBlock(EDir.Left);
                            break;
                        case ConsoleKey.E:
                            ChangeBlock(EDir.Right);
                            break;
                        case ConsoleKey.S:
                            Move(EDir.Down);
                            break;
                        case ConsoleKey.A:
                            Move(EDir.Left);
                            break;
                        case ConsoleKey.D:
                            Move(EDir.Right);
                            break;
                    }
                }
            }
        }

        public void Move(EDir dir)
        {
            List<SmallBlock> newSmallBlocks = new List<SmallBlock>();
            // 临时变量预移动
            int tempX = 0;
            int tempY = 0;
            for (int i = 0; i < block.smallBlocks.Count; i++)
            {
                tempX = block.smallBlocks[i].pos.X;
                tempY = block.smallBlocks[i].pos.Y;
                if (dir == EDir.Right)
                    tempX += 2;
                else if (dir == EDir.Left)
                    tempX -= 2;
                else
                    tempY += 1;

                newSmallBlocks.Add(new SmallBlock(block.smallBlocks[i].blockType,
                    new Position(tempX, tempY))); // 生成预移动的方块
            }

            // 碰撞判断
            for (int i = 0; i < newSmallBlocks.Count; i++)
            {
                for (int j = 0; j < map.walls.Count; j++)
                {
                    if (newSmallBlocks[i].pos.X == map.walls[j].pos.X &&
                        newSmallBlocks[i].pos.Y == map.walls[j].pos.Y) // 固定墙判断
                    {
                        return;
                    }
                }

                for (int j = 0; j < map.dynamicWalls.Count; j++)
                {
                    if (newSmallBlocks[i].pos.X == map.dynamicWalls[j].pos.X &&
                        newSmallBlocks[i].pos.Y == map.dynamicWalls[j].pos.Y) // 动态墙判断
                    {
                        return;
                    }
                }
            }

            block.Clear();
            block.smallBlocks = newSmallBlocks; // 预移动变成真实移动
            Draw();
        }

        public void FailingBottom(Map map)
        {
            for (int i = 0; i < block.smallBlocks.Count; i++)
            {
                if (block.smallBlocks[i].pos.Y == map.hight - 1)
                {
                    map.AddDynamicWalls(block.smallBlocks);
                    NewBlock();
                }

                for (int j = 0; j < map.dynamicWalls.Count; j++)
                {
                    if (block.smallBlocks[i].pos.Y == map.dynamicWalls[j].pos.Y - 1 &&
                        block.smallBlocks[i].pos.X == map.dynamicWalls[j].pos.X)
                    {
                        map.AddDynamicWalls(block.smallBlocks);
                        NewBlock();
                    }
                }
            }
        }

        public void Draw()
        {
            block.Draw();
        }
    }
}