using System;

namespace Rescue_Princess
{
    internal class Program
    {
        static void start_scene()
        {
        }

        static void game_scene()
        {
        }

        static void end_scene()
        {
        }

        public static void Main(string[] args)
        {
            #region 初始化控制台

            // 缓冲区大小
            Console.BufferHeight = 30;
            Console.BufferWidth = 150;
            // 控制台窗口
            Console.WindowHeight = 30;
            Console.WindowWidth = 50;
            // 隐藏光标
            Console.CursorVisible = false;

            #endregion

            int scene_num = 1;
            // 游戏主循环
            while (true)
            {
                switch (scene_num)
                {
                    case 1:
                        start_scene();
                        break;
                    case 2:
                        game_scene();
                        break;
                    case 3:
                        end_scene();
                        break;
                }
            }

            Console.ReadLine();
        }
    }
}