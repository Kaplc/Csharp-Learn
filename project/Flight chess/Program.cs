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

    /// <summary>
    /// 格子类型
    /// </summary>
    enum E_BlockType
    {
        Normal,
        Pause,
        Tunnel,
        Boom
    }

    struct Position
    {
        public int x;
        public int y;

        public Position(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    struct Block
    {
        public E_BlockType type;
        public Position position;

        public Block(int x, int y, E_BlockType type)
        {
            this.position = new Position(x, y);
            this.type = type;
        }

        public void Draw()
        {
            Console.SetCursorPosition(this.position.x, this.position.y);
            switch (type)
            {
                case E_BlockType.Normal:
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("□");
                    break;
                case E_BlockType.Boom:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("●");
                    break;
                case E_BlockType.Pause:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write("×");
                    break;
                case E_BlockType.Tunnel:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("¤");
                    break;
            }
        }
    }

    struct Map
    {
        public Block[] blocks; // 格子数组

        public Map(int a)
        {
            blocks = new Block[20 * 7 + 7];
            Random r = new Random();
            int x = 10;
            int y = 3;
            int index = 0; // 当前打印第几个的格子索引
            bool dir = true; // 打印方向为true是x轴正方向

            for (int i = 0; i < blocks.Length; i++)
            {
                // 第一格必须是普通格子
                if (i == 0)
                {
                    blocks[i] = new Block(x, y, E_BlockType.Normal);
                    x += 2;
                    index++;
                    continue;
                }

                // 随机格子类型
                int typeProbability = r.Next(0, 100);
                E_BlockType blockType = E_BlockType.Normal;
                if (typeProbability >= 70 && typeProbability < 80)
                {
                    blockType = E_BlockType.Boom;
                }
                else if (typeProbability >= 80 && typeProbability < 90)
                {
                    blockType = E_BlockType.Pause;
                }
                else if (typeProbability >= 90 && typeProbability < 100)
                {
                    blockType = E_BlockType.Tunnel;
                }

                // x正向打印
                if (dir == true && y < a)
                {
                    // 记录第几个
                    index++;
                    blocks[i] = new Block(x, y, blockType);
                    // 当生成到第20个时
                    if (index == 20)
                    {
                        // 竖向打印一个
                        y++;
                        blocks[++i] = new Block(x, y, blockType);
                        y++; // 再下一行
                        dir = false; // 改打印方向
                        index = 0; // 第几个的索引置零
                        continue;
                    }

                    // 下一个坐标
                    x += 2;
                }

                if (dir == false && y < a)
                {
                    index++;
                    blocks[i] = new Block(x, y, blockType);
                    if (index == 20)
                    {
                        y++;
                        blocks[++i] = new Block(x, y, blockType);
                        y++;
                        dir = true;
                        index = 0;
                        continue;
                    }

                    x -= 2;
                }
            }
        }

        public void Draw()
        {
            for (int i = 0; i < blocks.Length; i++)
            {
                blocks[i].Draw();
            }
        }
    }

    enum E_PlayerType // 玩家类型枚举
    {
        Player,
        AI,
    }

    struct Player
    {
        public E_PlayerType playerType; // 玩家类型
        public int blockIndex; // 所在格子索引
        public bool pause; // 判断暂停

        public Player(E_PlayerType playerType)
        {
            this.playerType = playerType;
            this.blockIndex = 0;
            this.pause = false;
        }
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
            Console.Clear();
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
            Console.Write("×:暂停，一回合");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(26, windowsHight - 9);
            Console.Write("●:炸弹，倒退5格");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(2, windowsHight - 8);
            Console.Write("¤:盲盒，随机倒退，暂停，换位置");

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
            Console.Write("按任意键开始扔骰子");
        }

        public static void DrawPlayer(Map map, Player player1, Player player2)
        {
            map.Draw();
            int player1X = map.blocks[player1.blockIndex].position.x;
            int player1Y = map.blocks[player1.blockIndex].position.y;
            int player2X = map.blocks[player2.blockIndex].position.x;
            int player2Y = map.blocks[player2.blockIndex].position.y;
            if (player1.blockIndex == player2.blockIndex)
            {
                Console.SetCursorPosition(player1X, player1Y);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("◎");
            }
            else
            {
                Console.SetCursorPosition(player1X, player1Y);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("★");
                Console.SetCursorPosition(player2X, player2Y);
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.Write("▲");
            }
        }

        public static int ThrowingDice(ref Player player1, ref Player player2, Map map)
        {
            if (player1.pause == true)
            {
                if (player1.playerType == E_PlayerType.Player)
                {
                    Console.SetCursorPosition(2, windowsHight - 5);
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("你暂停一回合禁止移动,按任意键继续               ");
                }
                else
                {
                    Console.SetCursorPosition(2, windowsHight - 5);
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.WriteLine("AI暂停一回合禁止移动,按任意键继续           ");
                }

                Console.SetCursorPosition(2, windowsHight - 4);
                Console.WriteLine("                                           ");
                player1.pause = false;
                return 1;
            }

            Random r = new Random();
            int num = r.Next(1, 7);
            if (player1.playerType == E_PlayerType.Player)
            {
                Console.SetCursorPosition(2, windowsHight - 5);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("你投出了{0}点, 向前移动{1}格                          ", num, num);
            }
            else
            {
                Console.SetCursorPosition(2, windowsHight - 5);
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine("AI投出了{0}点, 向前移动{1}格                        ", num, num);
            }

            player1.blockIndex += num; // 移动
            // 判断是否到终点
            if (player1.blockIndex >= map.blocks.Length)
            {
                return -1;
            }

            E_BlockType blockType = map.blocks[player1.blockIndex].type;
            switch (blockType)
            {
                case E_BlockType.Normal:
                    Console.SetCursorPosition(2, windowsHight - 4);
                    Console.WriteLine("到达安全位置, 按任意键继续                        ");
                    break;
                case E_BlockType.Boom:
                    player1.blockIndex -= 5;
                    // 倒退不能超过起点
                    if (player1.blockIndex < 0)
                    {
                        player1.blockIndex = 0;
                    }

                    Console.SetCursorPosition(2, windowsHight - 4);
                    Console.WriteLine("踩到炸弹:倒退5格, 按任意键继续                       ");
                    break;
                case E_BlockType.Pause:
                    player1.pause = true;
                    Console.SetCursorPosition(2, windowsHight - 4);
                    Console.WriteLine("踩到暂停:暂停1回合, 按任意键继续                      ");
                    break;
                case E_BlockType.Tunnel:
                    int tunnelNum = r.Next(1, 4);
                    if (tunnelNum == 1)
                    {
                        player1.blockIndex -= 5;
                        // 倒退不能超过起点
                        if (player1.blockIndex < 0)
                        {
                            player1.blockIndex = 0;
                        }

                        Console.SetCursorPosition(2, windowsHight - 4);
                        Console.WriteLine("踩中了盲盒:倒退5格, 按任意键继续                     ");
                    }
                    else if (tunnelNum == 2)
                    {
                        player1.pause = true;
                        Console.SetCursorPosition(2, windowsHight - 4);
                        Console.WriteLine("踩中了盲盒:暂停1回合, 按任意键继续                    ");
                    }
                    else
                    {
                        // 交换位置
                        int player2Index = player2.blockIndex;
                        player2.blockIndex = player1.blockIndex;
                        player1.blockIndex = player2Index;
                        Console.SetCursorPosition(2, windowsHight - 4);
                        Console.WriteLine("踩中了盲盒:交换双方位置, 按任意键继续               ");
                    }

                    break;
            }

            return 1;
        }

        public static bool GameScene()
        {
            Console.Clear();
            DrawWalls(); // 画墙
            Map map = new Map(16);
            Player player1 = new Player(E_PlayerType.Player);
            Player player2 = new Player(E_PlayerType.AI);
            DrawPlayer(map, player1, player2);
            while (true)
            {
                Console.ReadKey(true);
                // 玩家扔骰子
                if (ThrowingDice(ref player1, ref player2, map) == -1)
                {
                    return true; // 玩家胜利返回1
                }

                DrawPlayer(map, player1, player2);
                Console.ReadKey(true);
                // AI扔骰子
                if (ThrowingDice(ref player2, ref player1, map) == -1)
                {
                    return false; // AI胜利返回2
                }

                DrawPlayer(map, player1, player2);
            }
        }

        public static void EndScene(ref E_SceneType sceneType, bool winer)
        {
            Console.Clear();
            int selectNum = 1;
            // 打印标题
            string gameOverTitle = "";
            gameOverTitle = winer ? "你赢了, 你到达了终点" : "你输了,AI到达了终点";
            Console.SetCursorPosition(windowsWilde / 2 - 10, 8);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(gameOverTitle);

            ConsoleColor firstSelect = ConsoleColor.Red;
            ConsoleColor secondSelect = ConsoleColor.White;

            // 处理选择
            while (true)
            {
                Console.SetCursorPosition(windowsWilde / 2 - 5, 12);
                Console.ForegroundColor = firstSelect;
                if (selectNum == 1)
                {
                    Console.Write("返回主菜单 ");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("←");
                }
                else
                {
                    Console.WriteLine("返回主菜单      ");
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
                            sceneType = E_SceneType.StartScene;
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
            bool winer = true; // 胜利者true为玩家
            while (true)
            {
                switch (sceneType)
                {
                    case E_SceneType.StartScene:
                        StartScene(ref sceneType);
                        break;
                    case E_SceneType.GameScene:
                        winer = GameScene();
                        sceneType = E_SceneType.EndScene;
                        break;
                    case E_SceneType.EndScene:
                        EndScene(ref sceneType, winer);
                        break;
                }
            }
        }
    }
}