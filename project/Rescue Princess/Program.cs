using System;

namespace Rescue_Princess
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            #region 初始化控制台

            int windowHeight = 30;
            int windowWidth = 50;
            // 缓冲区大小
            Console.BufferHeight = windowHeight;
            Console.BufferWidth = windowWidth * 3;
            // 控制台窗口
            Console.WindowHeight = windowHeight;
            Console.WindowWidth = windowWidth;
            // 隐藏光标
            Console.CursorVisible = false;

            #endregion

            int sceneNum = 1;
            // 游戏主循环
            while (true)
            {
                switch (sceneNum)
                {
                    case 1:

                        #region 开始场景

                        bool startIsTrue = false;
                        int selectNum = 1;
                        int curX = windowWidth / 2 - 4;
                        int curY = windowHeight - 22;

                        // 打印标题
                        Console.SetCursorPosition(curX, curY); // 移动光标
                        Console.ForegroundColor = ConsoleColor.White; // 文字颜色
                        Console.WriteLine("营救公主");

                        ConsoleColor startTitle = ConsoleColor.Red;
                        ConsoleColor endTitle = ConsoleColor.White;
                        while (true)
                        {
                            Console.SetCursorPosition(curX, curY + 3); // 移动光标
                            Console.ForegroundColor = startTitle; // 文字颜色
                            Console.WriteLine("开始游戏");
                            Console.SetCursorPosition(curX, curY + 5); // 移动光标
                            Console.ForegroundColor = endTitle; // 文字颜色
                            Console.WriteLine("退出游戏");

                            char key = Console.ReadKey(true).KeyChar;
                            switch (key)
                            {
                                case 'w':
                                case 'W':
                                    selectNum = 1;
                                    startTitle = ConsoleColor.Red; // 文字颜色
                                    endTitle = ConsoleColor.White;
                                    break;
                                case 's':
                                case 'S':
                                    selectNum = 2;
                                    startTitle = ConsoleColor.White; // 文字颜色
                                    endTitle = ConsoleColor.Red;
                                    break;
                                case 'j':
                                case 'J':
                                    if (selectNum == 1)
                                    {
                                        startIsTrue = true;
                                        
                                    }
                                    else
                                    {
                                        Environment.Exit(0);
                                    }
                                    
                                    break;
                            }

                            if (startIsTrue == true)
                            {
                                sceneNum = 2;
                                break;
                            }
                            
                        }

                        #endregion

                        break;
                    case 2:

                        #region 开始游戏

                        

                        #endregion
                        break;
                    case 3:

                        break;
                }
            }

            Console.ReadLine();
        }
    }
}