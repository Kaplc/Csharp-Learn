using System;

namespace Greedy_Snake.game
{
    /// <summary>
    /// 抽象游戏对象类
    /// </summary>
    abstract class GameObject : I_Draw
    {
        public Position position;

        public virtual void Draw()
        {
        }
    }

    /// <summary>
    /// 食物对象
    /// </summary>
    class Food : GameObject
    {
        public Food()
        {
            CreateFood();
            // for (int i = 0; i < Map.walls.Length; i++)
            // {
            //     if (Map.walls[i].position == this.position)
            //     {
            //         CreateFood();
            //     }
            // }
        }

        public void CreateFood()
        {
            Random random = new Random();
            position = new Position(random.Next(0 + 4, 90 - 4) / 2 * 2, random.Next(0 + 2, 30 - 2)); // (/ 2 * 2):确保x是偶数
        }

        public override void Draw()
        {
            Console.SetCursorPosition(position.x, position.y);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("★");
        }
    }

    /// <summary>
    /// 墙体对象
    /// </summary>
    class Wall : GameObject
    {
        protected Wall()
        {
        }

        public Wall(int x, int y)
        {
            position = new Position(x, y);
        }

        public override void Draw()
        {
            Console.SetCursorPosition(position.x, position.y);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("■");
        }
    }

    /// <summary>
    /// 地图对象
    /// </summary>
    class Map : Wall
    {
        public static Wall[] walls;

        public Map(int w, int h)
        {
            walls = new Wall[2 * w / 2 + h * 2 - 4];

            int i = 0; // index

            // 横墙
            int x = 0;
            for (; i < walls.Length && x < w; i++)
            {
                walls[i] = new Wall(x, 0);
                walls[++i] = new Wall(x, h - 1);
                x += 2;
            }

            // 竖墙
            int y = 1;
            for (; i < walls.Length && y < h; i++)
            {
                walls[i] = new Wall(0, y);
                walls[++i] = new Wall(w - 2, y);
                y++;
            }
        }

        public override void Draw()
        {
            for (int i = 0; i < walls.Length; i++)
            {
                walls[i].Draw();
            }
        }
    }

    /// <summary>
    /// 蛇身体对象
    /// </summary>
    class SnakeBody : GameObject
    {
    }

    /// <summary>
    /// 蛇对象
    /// </summary>
    class Snake : SnakeBody
    {
    }
}