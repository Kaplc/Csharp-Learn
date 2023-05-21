using System;
using System.Threading;

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
        public Food(Snake snake)
        {
            CreateFood(snake);
        }

        private void CreateFood(Snake snake)
        {
            Random random = new Random();
            position = new Position(random.Next(0 + 4, 90 - 4) / 2 * 2, random.Next(0 + 2, 30 - 2)); // (/ 2 * 2):确保x是偶数
            for (int i = 0; i < snake.size; i++)
            {
                if (snake.bodys[i].position == this.position)
                {
                    CreateFood(snake); // 刷在身体内则递归生成
                }
            }
            Draw();
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
        public E_SnakeBodyType snakeBodyType;

        public SnakeBody()
        {
        }

        public SnakeBody(E_SnakeBodyType snakeBodyType, int x, int y)
        {
            this.snakeBodyType = snakeBodyType;
            position = new Position(x, y);
        }

        public override void Draw()
        {
            Console.SetCursorPosition(position.x, position.y);
            Console.ForegroundColor = snakeBodyType == E_SnakeBodyType.Body ? ConsoleColor.Yellow : ConsoleColor.Green;
            Console.Write("●");
        }
    }

    /// <summary>
    /// 蛇对象
    /// </summary>
    class Snake : SnakeBody
    {
        public SnakeBody[] bodys;
        private E_MoveDir nowDir;
        public int size;
        private bool moving; // 控制线程执行
        public static int score;

        public Snake(int x, int y)
        {
            bodys = new SnakeBody[200];
            nowDir = E_MoveDir.Right; // 默认移动方向向右
            bodys[0] = new SnakeBody(E_SnakeBodyType.Header, x, y); // 初始化头
            // bodys[1] = new SnakeBody(E_SnakeBodyType.Body, x - 2, y); // 初始化身体
            // size = 2;
            size = 1;
            moving = true;
            score = 0;
        }

        private void Cleartail()
        {
            Console.SetCursorPosition(bodys[size - 1].position.x, bodys[size - 1].position.y);
            Console.Write("  ");
        }

        public void Move()
        {
            Thread.Sleep(100);
            Cleartail(); // 擦除尾部
            for (int i = size - 1; i > 0; i--)
            {
                // 身体移动
                bodys[i] = bodys[i - 1];
                bodys[i].snakeBodyType = E_SnakeBodyType.Body;
            }

            // 移动单位方向
            int dx = 0;
            int dy = 0;
            switch (nowDir)
            {
                case E_MoveDir.Up:
                    dx = 0;
                    dy = -1;
                    break;
                case E_MoveDir.Down:
                    dx = 0;
                    dy = +1;
                    break;
                case E_MoveDir.Left:
                    dx = -2;
                    dy = 0;
                    break;
                case E_MoveDir.Right:
                    dx = +2;
                    dy = 0;
                    break;
            }

            bodys[0] = new SnakeBody(E_SnakeBodyType.Header, bodys[0].position.x + dx, bodys[0].position.y + dy);
            Draw();
        }

        /// <summary>
        /// 多线程执行读取转向命令
        /// </summary>
        public void Turn()
        {
            while (moving)
            {
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.W:
                        if (nowDir == E_MoveDir.Down)
                        {
                            break;
                        }
                        nowDir = E_MoveDir.Up;
                        break;
                    case ConsoleKey.DownArrow:
                    case ConsoleKey.S:
                        if (nowDir == E_MoveDir.Up)
                        {
                            break;
                        }

                        nowDir = E_MoveDir.Down;
                        break;
                    case ConsoleKey.LeftArrow:
                    case ConsoleKey.A:
                        if (nowDir == E_MoveDir.Right)
                        {
                            break;
                        }

                        nowDir = E_MoveDir.Left;
                        break;
                    case ConsoleKey.RightArrow:
                    case ConsoleKey.D:
                        if (nowDir == E_MoveDir.Left)
                        {
                            break;
                        }

                        nowDir = E_MoveDir.Right;
                        break;
                }
            }
        }

        public void Eat(ref Food food)
        {
            for (int i = 0; i < size; i++)
            {
                if (bodys[i].position == food.position )
                {
                    food = new Food(this);
                    Add();
                    break;
                }
            }
        }

        private void Add()
        {
            bodys[size] = new SnakeBody(E_SnakeBodyType.Body, bodys[size - 1].position.x, bodys[size - 1].position.y);
            size++;
            CalScore();
        }
    
        public bool Die()
        {
            for (int i = 1; i < size; i++)
            {
                if (bodys[0].position == bodys[i].position && size >2)
                {
                    moving = false;
                    return true;
                }
            }

            for (int j = 0; j < Map.walls.Length; j++)
            {
                if (bodys[0].position == Map.walls[j].position)
                {
                    moving = false;
                    return true;
                }
            }
            return false;
        }

        private void CalScore()
        {
            score += size;
        }
        
        public override void Draw()
        {
            for (int i = 0; i < size; i++)
            {
                bodys[i].Draw();
            }
        }
    }
}