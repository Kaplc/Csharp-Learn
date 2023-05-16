using System;

namespace Flight_chess
{
    enum E_SceneType
    {
        /// <summary>
        /// 开始场景id
        /// </summary>
        StartScene,

        /// <summary>
        /// 游戏场景id
        /// </summary>
        GameScene,

        /// <summary>
        /// 结束场景id
        /// </summary>
        EndScene,
    }

    internal class Program
    {
        const int windowsWilde = 60;
        const int windowsHight = 30;

        public static void InitConsole()
        {
            Console.WindowWidth = windowsWilde;
            Console.WindowHeight = windowsHight;
            Console.BufferWidth = windowsWilde;
            Console.BufferHeight = windowsHight;
            Console.CursorVisible = false;
        }

        public static void StartScene(ref E_SceneType sceneType)
        {
            int selectNum = 1;
            // 打印标题
            Console.SetCursorPosition(windowsWilde / 2 - 3, 8);
            Console.WriteLine("飞行棋");
            
            ConsoleColor firstSelect = ConsoleColor.Red;
            ConsoleColor secondSelect = ConsoleColor.White;
            
            // 处理选择
            while (true)
            {
                Console.SetCursorPosition(windowsWilde/2 -4, 12);
                Console.ForegroundColor = firstSelect;
                if (selectNum == 1)
                {
                    Console.Write("开始游戏 ");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("←");
                }
                else
                {
                    Console.WriteLine("开始游戏      ");
                }

                Console.SetCursorPosition(windowsWilde/2 -4, 14);
                Console.ForegroundColor = secondSelect;
                if (selectNum == 2)
                {
                    Console.Write("退出游戏 ");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("←");
                }
                else
                {
                    Console.WriteLine("退出游戏      ");
                }
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.W:
                        if (selectNum == 1)
                        {
                            break;
                        }

                        selectNum = 1;
                        firstSelect = ConsoleColor.Red;
                        secondSelect = ConsoleColor.White;
                        break;
                    case ConsoleKey.S:
                        if (selectNum == 2)
                        {
                            break;
                        }
    
                        selectNum = 2;
                        firstSelect = ConsoleColor.White;
                        secondSelect = ConsoleColor.Red;
                        break;
                    case ConsoleKey.J:
                        if (selectNum == 1)
                        {
                            sceneType = E_SceneType.GameScene;
                            return;
                        }
                        Environment.Exit(0);    
                        break;
                        
                }
            }
        }

        public static void Main(string[] args)
        {
            InitConsole();
            E_SceneType sceneType = E_SceneType.StartScene;
            while (true)
            {
                switch (sceneType)
                {
                    case E_SceneType.StartScene:
                        StartScene(ref sceneType);
                        break;
                    case E_SceneType.GameScene:
                        Console.WriteLine("游戏场景");
                        break;
                    case E_SceneType.EndScene:
                        Console.WriteLine("结束场景");
                        break;
                }
            }
        }
    }
}