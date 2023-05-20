using System;

namespace Greedy_Snake.game
{
    /// <summary>
    /// 游戏类
    /// </summary>
    class Game
    {
        private int windowWide;
        private int windowHight;
        private E_SceneType sceneType;

        public Game(int windowWide, int windowHight)
        {
            this.windowWide = windowWide;
            this.windowHight = windowHight;
            sceneType = E_SceneType.Start;
            InitConsole();
        }

        private void InitConsole()
        {
            Console.WindowWidth = windowWide + 1;
            Console.WindowHeight = windowHight;
            Console.BufferWidth = windowWide + 1;
            Console.BufferHeight = windowHight;
            Console.CursorVisible = false;
        }

        public void StartGame()
        {
            while (true)
            {
                switch (sceneType)
                {
                    case E_SceneType.Start:
                        StartScene startScene = new StartScene();
                        startScene.UpdateGameImage(windowWide, windowHight, ref sceneType);
                        break;
                    case E_SceneType.Game:
                        GameScene gameScene = new GameScene(windowWide, windowHight);
                        gameScene.UpdateGameImage(windowWide, windowHight, ref sceneType);
                        break;
                    case E_SceneType.End:
                        EndScene endScene = new EndScene();
                        endScene.UpdateGameImage(windowWide, windowHight, ref sceneType);
                        break;
                }
            }
        }

        private void SceneChange(E_SceneType changeSceneType)
        {
            this.sceneType = changeSceneType;
        }
    }

    /// <summary>
    /// 开始结束场景基类
    /// </summary>
    abstract class StartEndSceneBase : I_UpdateGameImage
    {
        protected string title;
        protected string firstSelect;
        protected string secondSelect;
        int selectNum = 1; // 默认选择第一个选项

        protected StartEndSceneBase()
        {
            // this.currSceneType = currSceneType;
            title = "";
            firstSelect = "";
            secondSelect = "";
        }

        public void UpdateGameImage(int w, int h, ref E_SceneType currSceneType)
        {
            Console.Clear();
            Console.SetCursorPosition(w / 2 - 4, h / 4);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(title);


            ConsoleColor firstSelectColor = ConsoleColor.Red;
            ConsoleColor secondSelectColor = ConsoleColor.White;
            // 处理选择
            while (true)
            {
                Console.SetCursorPosition(w / 2 - 4, h / 3 + 2);
                Console.ForegroundColor = firstSelectColor;
                if (selectNum == 1)
                {
                    Console.Write(firstSelect);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("  ←");
                }
                else
                {
                    Console.WriteLine(firstSelect + "      ");
                }

                Console.SetCursorPosition(w / 2 - 4, h / 3 + 4);
                Console.ForegroundColor = secondSelectColor;
                if (selectNum == 2)
                {
                    Console.Write(secondSelect);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("  ←");
                }
                else
                {
                    Console.WriteLine(secondSelect + "      ");
                }

                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.W:
                        if (selectNum == 1)
                        {
                            break;
                        }

                        selectNum = 1;
                        firstSelectColor = ConsoleColor.Red;
                        secondSelectColor = ConsoleColor.White;
                        break;
                    case ConsoleKey.S:
                        if (selectNum == 2)
                        {
                            break;
                        }

                        selectNum = 2;
                        firstSelectColor = ConsoleColor.White;
                        secondSelectColor = ConsoleColor.Red;
                        break;
                    case ConsoleKey.J:
                        if (selectNum == 1)
                        {
                            currSceneType = (currSceneType == E_SceneType.Start) ? E_SceneType.Game : E_SceneType.Start;
                            return;
                        }

                        Environment.Exit(0);
                        break;
                }
            }
        }
    }

    /// <summary>
    /// 开始场景类
    /// </summary>
    class StartScene : StartEndSceneBase
    {
        public StartScene() // 子类自动调用父类构造函数
        {
            title = "贪 吃 蛇";
            firstSelect = "开始游戏";
            secondSelect = "退出游戏";
        }
    }

    /// <summary>
    /// 结束场景类
    /// </summary>
    class EndScene : StartEndSceneBase
    {
        public EndScene()
        {
            title = "游戏结束";
            firstSelect = "返回菜单";
            secondSelect = "退出游戏";
        }
    }

    /// <summary>
    /// 游戏场景类
    /// </summary>
    class GameScene : I_UpdateGameImage
    {
        public Map map;
        public Food food;
        public Snake snake;

        public GameScene(int w, int h)
        {
            map = new Map(w, h);
        }

        public void UpdateGameImage(int w, int h, ref E_SceneType currSceneType)
        {
            Console.Clear();
            map.Draw();
            Console.ReadLine();
        }
    }
}