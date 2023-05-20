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
        public E_SceneType sceneType;

        public Game(int windowWide, int windowHight)
        {
            this.windowWide = windowWide;
            this.windowHight = windowHight;
            sceneType = E_SceneType.Start;
            InitConsole();
            StartGame();
        }

        private void InitConsole()
        {
            Console.WindowWidth = windowWide;
            Console.WindowHeight = windowHight;
            Console.BufferWidth = windowWide;
            Console.BufferHeight = windowHight;
            Console.CursorVisible = false;
        }

        private void StartGame()
        {
            while (true)
            {
                switch (sceneType)
                {
                    case E_SceneType.Start:
                        Console.WriteLine("开始场景");
                        break;
                    case E_SceneType.Game:
                        Console.WriteLine("游戏场景");
                        break;
                    case E_SceneType.End:
                        Console.WriteLine("结束场景");
                        break;
                }
            }
        }

        private void SceneChange(E_SceneType changeSceneType)
        {
            this.sceneType = changeSceneType;
        }
    }
}