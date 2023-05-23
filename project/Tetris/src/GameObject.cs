namespace Tetris
{
    #region 枚举

    public enum BlockType
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

    public class GameObject
    {
    }
}