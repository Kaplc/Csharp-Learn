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
                    blockInfos.Add(new[] { new Position(0, -1), new Position(-2, 0), new Position(-2, 1) });
                    blockInfos.Add(new[] { new Position(2, 0), new Position(0, 1), new Position(-2, 1) });
                    blockInfos.Add(new[] { new Position(-2, -1), new Position(-2, 0), new Position(0, 1) });
                    blockInfos.Add(new[] { new Position(-2, 0), new Position(0, -1), new Position(2, -1) });
                    break;
                case EBlockType.LStairs:
                    blockInfos.Add(new[] { new Position(0, -1), new Position(0, 1), new Position(0, 2) });
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
    public class smallBlock : GameObject
    {
        public smallBlock(EBlockType blockType, Position pos)
        {
            this.blockType = blockType;
            this.pos = pos;
        }
    }

    /// <summary>
    /// 大方块
    /// </summary>
    public class BigBlock : IDraw
    {
        public List<smallBlock> smallBlocks = new List<smallBlock>();
        public EBlockType bigBlockType;
        public int blockInfosIndex;

        public BigBlock()
        {
            CreateBigBlock(); // 创建剩余组成部分
        }

        public void CreateBigBlock()
        {
            Random r = new Random();
            int BlockTypeIndex = r.Next(1, 8);
            bigBlockType = (EBlockType)BlockTypeIndex; // 初始化大方块类型
            List<Position[]> smallBlockInfos = new BigBlockInfo(bigBlockType).blockInfos; // 下标获取枚举->获取某个类型的方块中包含小方块信息数组
            
            blockInfosIndex = r.Next(0, smallBlockInfos.Count);
            Position[] CreatedBlockInfo = smallBlockInfos[blockInfosIndex]; // 小方块信息数组中的其中一种变形
            
            smallBlocks.Add(new smallBlock(bigBlockType, new Position(Game.WindowWide / 2, 4))); // 初始化原点方块
            for (int i = 0; i < CreatedBlockInfo.Length; i++)
            {
                smallBlocks.Add(new smallBlock(bigBlockType, CreatedBlockInfo[i] + smallBlocks[0].pos));
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

        public Worker()
        {
            
        }

        public void NewBlock()
        {
            block = new BigBlock();
        }
        
        public void Draw()
        {
            block.Draw();
        }
    }
}