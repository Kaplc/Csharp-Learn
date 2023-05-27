using System.CodeDom.Compiler;
using System.Collections.Generic;

namespace Pushing_boxes
{
    /// <summary>
    /// 关卡信息
    /// </summary>
    public class MapInfos
    {
        public List<MapInfo> infos = new List<MapInfo>();

        public MapInfos()
        {
            infos.Add(new MapInfo(1));
            infos.Add(new MapInfo(2));
        }

        public MapInfo this[int index]
        {
            get => infos[index];
        }
    }

    public class MapInfo
    {
        public Position player;
        public List<Position> walls = new List<Position>();
        public List<Position> stars = new List<Position>();
        public List<Position> boxes = new List<Position>();

        public MapInfo(int index)
        {
            switch (index)
            {
                case 1:
                    Create1();
                    break;
                case 2:
                    Create2();
                    break;
            }
        }

        public void Create1()
        {
            // 地图原点
            int oX = Game.WindowWide / 3 + 1;
            int oY = Game.WindowHight / 3;

            #region 边界墙

            for (int x = 0; x < 8; x++)
            {
                walls.Add(new Position(oX + x * 2, oY + 0));

                if (x >= 1 && x <= 5)
                {
                    walls.Add(new Position(oX + x * 2, oY + 7));
                }
            }

            for (int y = 0; y < 7; y++)
            {
                if (y < 4)
                {
                    walls.Add(new Position(oX + 0, oY + y));
                    walls.Add(new Position(oX + 2 * 7, oY + y));
                    continue;
                }
                else if (y == 4)
                {
                    walls.Add(new Position(oX + 0 * 2, oY + y));
                    walls.Add(new Position(oX + 1 * 2, oY + y));
                    walls.Add(new Position(oX + 6 * 2, oY + y));
                    walls.Add(new Position(oX + 7 * 2, oY + y));
                }
                else
                {
                    walls.Add(new Position(oX + 2, oY + y));
                    walls.Add(new Position(oX + 12, oY + y));
                }
            }

            #endregion

            #region 中间墙

            walls.Add(new Position(oX + 5 * 2, oY + 2));

            walls.Add(new Position(oX + 2 * 2, oY + 3));
            walls.Add(new Position(oX + 4 * 2, oY + 3));

            walls.Add(new Position(oX + 2 * 2, oY + 4));
            walls.Add(new Position(oX + 4 * 2, oY + 4));

            walls.Add(new Position(oX + 4 * 2, oY + 6));
            walls.Add(new Position(oX + 5 * 2, oY + 6));

            #endregion

            #region 人物

            player = new Position(oX + 2 * 2, oY + 5);

            #endregion

            #region 箱子

            boxes.Add(new Position(oX + 2 * 2, oY + 2));
            boxes.Add(new Position(oX + 5 * 2, oY + 1));
            boxes.Add(new Position(oX + 3 * 2, oY + 5));

            #endregion

            #region 星星

            stars.Add(new Position(oX + 1 * 2, oY + 1));
            stars.Add(new Position(oX + 1 * 2, oY + 2));
            stars.Add(new Position(oX + 1 * 2, oY + 3));

            #endregion
        }

        public void Create2()
        {
        }
    }
}