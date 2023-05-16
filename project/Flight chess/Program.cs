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
        const int windowsHight = 40;

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
                Console.SetCursorPosition(windowsWilde / 2 - 4, 12);
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

                Console.SetCursorPosition(windowsWilde / 2 - 4, 14);
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

        public static void DrawWalls()
        {
            Console.ForegroundColor = ConsoleColor.Red;

            for (int x = 0; x < windowsWilde - 2; x += 2)
            {
                Console.SetCursorPosition(x, 0);
                Console.Write("■");

                Console.SetCursorPosition(x, windowsHight - 2);
                Console.Write("■");

                Console.SetCursorPosition(x, windowsHight - 11);
                Console.Write("■");
            }

            for (int y = 0; y < windowsHight - 1; y++)
            {
                Console.SetCursorPosition(0, y);
                Console.Write("■");

                Console.SetCursorPosition(windowsWilde - 2, y);
                Console.Write("■");
            }

            //文字信息
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(2, windowsHight - 10);
            Console.Write("□:普通格子");

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.SetCursorPosition(2, windowsHight - 9);
            Console.Write("‖:暂停，一回合");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(26, windowsHight - 9);
            Console.Write("●:炸弹，倒退5格");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(2, windowsHight - 8);
            Console.Write("¤:时空隧道，随机倒退，暂停，换位置");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.SetCursorPosition(2, windowsHight - 7);
            Console.Write("★:玩家");

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.SetCursorPosition(12, windowsHight - 7);
            Console.Write("▲:电脑");

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.SetCursorPosition(22, windowsHight - 7);
            Console.Write("◎:玩家和电脑重合");
            
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(2, windowsHight - 6);
            Console.Write("========================================================");
            Console.SetCursorPosition(2, windowsHight - 5);
            Console.Write("按任意键开始扔色子");
        }

        public static void GameScene()
        {
            Console.Clear();
            DrawWalls();
            Console.ReadLine();
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
                        GameScene();
                        break;
                    case E_SceneType.EndScene:
                        Console.WriteLine("结束场景");
                        break;
                }
            }
        }
    }
}