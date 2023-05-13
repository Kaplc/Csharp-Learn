using System;

namespace Rescue_Princess
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            #region 初始化控制台

            int windowHeight = 30;
            int windowWidth = 60;
            // 缓冲区大小
            Console.BufferHeight = windowHeight * 2;
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

                        #region 游戏场景

                        Console.Clear(); // 清屏

                        #region 画墙

                        // 横墙
                        for (int x = 0; x < windowWidth; x += 2)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;

                            Console.SetCursorPosition(x, 0);
                            Console.Write("■");
                            Console.SetCursorPosition(x, windowHeight - 1);
                            Console.Write("■");
                            Console.SetCursorPosition(x, windowHeight - 7);
                            Console.Write("■");
                        }

                        // 竖墙
                        for (int y = 0; y < windowHeight - 1; y++)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;

                            Console.SetCursorPosition(0, y);
                            Console.Write("■");
                            Console.SetCursorPosition(windowWidth - 2, y);
                            Console.Write("■");
                        }

                        #endregion

                        #region 绘制玩家和boss

                        Random r = new Random();
                        // boss
                        int bossX = 30;
                        int bossY = 20;
                        int bossHp = 100;
                        int bossAtkMax = 10;
                        int bossAtkMin = 8;
                        Console.SetCursorPosition(bossX, bossY);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("★");
                        // 玩家
                        int playerX = 4;
                        int playerY = 4;
                        int playerHp = 100;
                        int playerAtkMax = 12;
                        int playerAtkMin = 8;
                        bool atkState = false;
                        Console.SetCursorPosition(playerX, playerY);
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("●");
                        // 提示信息
                        Console.SetCursorPosition(2, windowHeight - 6);
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("●:玩家  ");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("★:Boss");

                        Console.SetCursorPosition(2, windowHeight - 5);
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine("请移动到boss身边按J进行攻击");

                        #endregion

                        #region 移动

                        while (true)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.SetCursorPosition(playerX, playerY);

                            switch (Console.ReadKey(true).KeyChar)
                            {
                                case 'w':
                                case 'W':
                                    Console.Write("  ");
                                    playerY--;
                                    // 判断碰撞
                                    if ((playerX == bossX && playerY == bossY && bossHp > 0) ||
                                        playerX < 2 || playerY < 1 || playerX > windowWidth - 4 ||
                                        playerY > windowHeight - 8 || atkState == true)
                                    {
                                        playerY++;
                                    }

                                    break;
                                case 'a':
                                case 'A':
                                    Console.Write("  ");
                                    playerX -= 2;
                                    if ((playerX == bossX && playerY == bossY && bossHp > 0) ||
                                        playerX < 2 || playerY < 1 || playerX > windowWidth - 4 ||
                                        playerY > windowHeight - 8 || atkState == true)
                                    {
                                        playerX += 2;
                                    }

                                    break;
                                case 's':
                                case 'S':
                                    Console.Write("  ");
                                    playerY++;
                                    if ((playerX == bossX && playerY == bossY && bossHp > 0) ||
                                        playerX < 2 || playerY < 1 || playerX > windowWidth - 4 ||
                                        playerY > windowHeight - 8 || atkState == true)
                                    {
                                        playerY--;
                                    }

                                    break;
                                case 'd':
                                case 'D':
                                    Console.Write("  ");
                                    playerX += 2;
                                    if ((playerX == bossX && playerY == bossY && bossHp > 0) ||
                                        playerX < 2 || playerY < 1 || playerX > windowWidth - 4 ||
                                        playerY > windowHeight - 8 || atkState == true)
                                    {
                                        playerX -= 2;
                                    }

                                    break;
                                case 'j':
                                case 'J':

                                    #region 战斗

                                    while (true)
                                    {
                                        if (bossHp > 0 &&
                                            ((playerX == bossX - 2 && playerY == bossY) ||
                                             (playerX == bossX + 2 && playerY == bossY) ||
                                             (playerX == bossX && playerY == bossY - 1) ||
                                             (playerX == bossX && playerY == bossY + 1))
                                           )
                                        {
                                            atkState = true;
                                            Console.SetCursorPosition(2, windowHeight - 5);
                                            Console.ForegroundColor = ConsoleColor.Cyan;
                                            Console.WriteLine("进入战斗状态,禁止移动!按J进行攻击");

                                            char key1 = Console.ReadKey(true).KeyChar;
                                            if (key1 == 'j' || key1 == 'J')
                                            {
                                                int playerAtk = r.Next(playerAtkMin, playerAtkMax + 1);
                                                bossHp -= playerAtk;
                                                // 血量为负值置零
                                                bossHp = bossHp < 0 ? 0 : bossHp;
                                                // 打印战斗信息
                                                Console.SetCursorPosition(2, windowHeight - 4);
                                                Console.ForegroundColor = ConsoleColor.Yellow;
                                                Console.WriteLine("你对Boss造成{0}点伤害, Boss剩余血量{1}     ", playerAtk,
                                                    bossHp);
                                            }
                                            
                                            char key2 = Console.ReadKey(true).KeyChar;
                                            if (key2 == 'j' || key2 == 'J')
                                            {
                                                int bossAtk = r.Next(bossAtkMin, bossAtkMax + 1);
                                                playerHp -= bossAtk;
                                                playerHp = playerHp < 0 ? 0 : playerHp;

                                                Console.SetCursorPosition(2, windowHeight - 3);
                                                Console.ForegroundColor = ConsoleColor.Green;
                                                Console.WriteLine("Boss对你造成{0}点伤害, 你的剩余血量{1}      ", bossAtk, playerHp);
                                            }

                                        }

                                        // 判断是否结束战斗
                                        if (playerHp == 0)
                                        {
                                            sceneNum = 3;
                                            break;
                                        }

                                        if (bossHp == 0)
                                        {
                                            // 删除boss
                                            Console.SetCursorPosition(bossX, bossY);
                                            Console.Write("  ");
                                            
                                            
                                        }
                                        
                                        
                                        
                                    }

                                    #endregion
                                    break;
                            }

                            if (sceneNum == 3)break;

                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.SetCursorPosition(playerX, playerY);
                            Console.Write("●");
                        }

                        #endregion

                        #endregion

                        break;
                    case 3:
                        Console.Clear();
                        Console.WriteLine("结束场景");
                        break;
                }
            }

            Console.ReadLine();
        }
    }
}